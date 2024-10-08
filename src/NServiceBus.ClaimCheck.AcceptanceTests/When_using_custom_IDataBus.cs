﻿namespace NServiceBus.ClaimCheck.DataBus.AcceptanceTests;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AcceptanceTesting;
using AcceptanceTesting.Customization;
using ClaimCheck.AcceptanceTests.EndpointTemplates;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

[TestFixture]
public class When_using_custom_IDataBus
{
    [Test]
    public async Task Should_be_able_to_register_via_fluent()
    {
        var context = await Scenario.Define<Context>(c => { c.TempPath = Path.GetTempFileName(); })
            .WithEndpoint<SenderViaFluent>(b => b.When(session => session.Send(new MyMessageWithLargePayload
            {
                Payload = new ClaimCheckProperty<byte[]>(PayloadToSend)
            })))
            .WithEndpoint<ReceiverViaFluent>()
            .Done(c => c.ReceivedPayload != null)
            .Run();

        Assert.That(context.ReceivedPayload, Is.EqualTo(PayloadToSend), "The large payload should be marshalled correctly using the databus");
    }

    static byte[] PayloadToSend = new byte[1024 * 10];

    public class Context : ScenarioContext
    {
        public string TempPath { get; set; }
        public byte[] ReceivedPayload { get; set; }
    }

    public class SenderViaFluent : EndpointConfigurationBuilder
    {
        public SenderViaFluent()
        {
            EndpointSetup<DefaultServer>(b =>
            {
                b.UseClaimCheck(sp => new MyDataBus(sp.GetRequiredService<Context>()), new SystemJsonClaimCheckSerializer());
                b.ConfigureRouting().RouteToEndpoint(typeof(MyMessageWithLargePayload), typeof(ReceiverViaFluent));
            });
        }
    }

    public class ReceiverViaFluent : EndpointConfigurationBuilder
    {
        public ReceiverViaFluent()
        {
            EndpointSetup<DefaultServer>(b => b.UseClaimCheck(sp => new MyDataBus(sp.GetRequiredService<Context>()), new SystemJsonClaimCheckSerializer()));
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
    }

    public class MyDataBus : IClaimCheck
    {
        Context context;
        public MyDataBus(Context context)
        {
            this.context = context;
        }

        public Task<Stream> Get(string key, CancellationToken cancellationToken = default)
        {
            var fileStream = new FileStream(context.TempPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
            return Task.FromResult((Stream)fileStream);
        }

        public Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken = default)
        {
            using (var destination = File.OpenWrite(context.TempPath))
            {
                stream.CopyTo(destination);
            }
            return Task.FromResult("key");
        }

        public Task Start(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }


    public class MyMessageWithLargePayload : ICommand
    {
        public ClaimCheckProperty<byte[]> Payload { get; set; }
    }
}