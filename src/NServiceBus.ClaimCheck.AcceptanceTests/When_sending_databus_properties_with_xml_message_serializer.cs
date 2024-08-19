﻿namespace NServiceBus.ClaimCheck.DataBus.AcceptanceTests;

using System;
using System.IO;
using System.Threading.Tasks;
using AcceptanceTesting;
using AcceptanceTesting.Customization;
using ClaimCheck.AcceptanceTests.EndpointTemplates;
using MessageMutator;
using NUnit.Framework;

[TestFixture]
public class When_sending_databus_properties_with_xml_message_serializer
{
    [Test]
    public async Task Should_receive_messages_with_largepayload_correctly()
    {
        var payloadToSend = new byte[PayloadSize];

        var context = await Scenario.Define<Context>()
            .WithEndpoint<Sender>(b => b.When(session => session.Send(new MyMessageWithLargePayload
            {
                Payload = new ClaimCheckProperty<byte[]>(payloadToSend)
            })))
            .WithEndpoint<Receiver>()
            .Done(c => c.ReceivedPayload != null)
            .Run();

        Assert.That(context.ReceivedPayload, Is.EqualTo(payloadToSend), "The large payload should be marshalled correctly using the databus");
    }

    const int PayloadSize = 500;

    public class Context : ScenarioContext
    {
        public byte[] ReceivedPayload { get; set; }
    }

    public class Sender : EndpointConfigurationBuilder
    {
        public Sender()
        {
            EndpointSetup<DefaultServer>(builder =>
            {
                var basePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "databus", "sender");
                builder.UseClaimCheck<FileShareDataBus, SystemJsonClaimCheckSerializer>().BasePath(basePath);
                builder.UseSerialization<XmlSerializer>();
                builder.ConfigureRouting().RouteToEndpoint(typeof(MyMessageWithLargePayload), typeof(Receiver));
            });
        }
    }

    public class Receiver : EndpointConfigurationBuilder
    {
        public Receiver()
        {
            EndpointSetup<DefaultServer>(builder =>
            {
                var basePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "databus", "sender");
                builder.UseClaimCheck<FileShareDataBus, SystemJsonClaimCheckSerializer>().BasePath(basePath);
                builder.UseSerialization<XmlSerializer>();
                builder.RegisterMessageMutator(new Mutator());
            });
        }

        public class MyMessageHandler : IHandleMessages<MyMessageWithLargePayload>
        {
            public MyMessageHandler(Context context)
            {
                testContext = context;
            }

            public Task Handle(MyMessageWithLargePayload messageWithLargePayload, IMessageHandlerContext context)
            {
                testContext.ReceivedPayload = messageWithLargePayload.Payload.Value;

                return Task.CompletedTask;
            }

            Context testContext;
        }

        public class Mutator : IMutateIncomingTransportMessages
        {
            public Task MutateIncoming(MutateIncomingTransportMessageContext context)
            {
                if (context.Body.Length > PayloadSize)
                {
                    throw new Exception("The message body is too large, which means the DataBus was not used to transfer the payload.");
                }
                return Task.CompletedTask;
            }
        }
    }

    public class MyMessageWithLargePayload : ICommand
    {
        public ClaimCheckProperty<byte[]> Payload { get; set; }
    }
}