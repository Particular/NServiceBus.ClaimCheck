namespace NServiceBus.ClaimCheck.Tests;

using System;
using System.IO;
using System.Runtime.Serialization;

public class FakeDataBusSerializer(string contentType = "some-content-type", bool throwOnDeserialize = false)
    : IClaimCheckSerializer
{
    public string ContentType { get; } = contentType;

    public object Deserialize(Type propertyType, Stream stream)
        => throwOnDeserialize ? throw new SerializationException() : "test";

    public void Serialize(object databusProperty, Stream stream)
    {
    }
}