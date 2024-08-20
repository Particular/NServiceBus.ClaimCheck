namespace NServiceBus.ClaimCheck.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Configuration.AdvancedExtensibility;
using NUnit.Framework;

[TestFixture]
public class DataBusConfigurationTests
{
    [Test]
    public void Should_allow_multiple_deserializers_to_be_used()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

        endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>()
            .AddDeserializer(new FakeDataBusSerializer("content-type-1"))
            .AddDeserializer(new FakeDataBusSerializer("content-type-2"));

        Assert.That(endpointConfiguration.GetSettings().Get<List<IClaimCheckSerializer>>(NServiceBus.Features.ClaimCheckFeature.AdditionalClaimCheckDeserializersKey).Count, Is.EqualTo(2));
    }

    [Test]
    public void Should_not_allow_duplicate_deserializers()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        var config = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>()
            .AddDeserializer(new FakeDataBusSerializer("duplicate"));

        Assert.Throws<ArgumentException>(() => config.AddDeserializer(new FakeDataBusSerializer("duplicate")));
    }

    [Test]
    public void Should_not_allow_duplicate_deserializer_with_same_content_type_as_main_serializer()
    {
        var endpointConfiguration = new EndpointConfiguration("MyEndpoint");
        var config = endpointConfiguration.UseClaimCheck<FileShareClaimCheck, SystemJsonClaimCheckSerializer>();

        Assert.Throws<ArgumentException>(() => config.AddDeserializer<SystemJsonClaimCheckSerializer>());
    }

    class MySerializer : IClaimCheckSerializer
    {
        public string ContentType => throw new NotImplementedException();

        public object Deserialize(Type propertyType, Stream stream) => throw new NotImplementedException();
        public void Serialize(object databusProperty, Stream stream) => throw new NotImplementedException();
    }

    class MyDataBus : IClaimCheck
    {
        public Task<Stream> Get(string key, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task Start(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
