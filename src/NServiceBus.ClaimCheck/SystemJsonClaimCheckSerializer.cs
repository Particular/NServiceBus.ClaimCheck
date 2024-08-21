namespace NServiceBus;

using System;
using System.IO;
using System.Text.Json;
using NServiceBus.ClaimCheck;

/// <summary>
/// Claim Check serialization using the <see cref="JsonSerializer"/> serializer.
/// </summary>
public class SystemJsonClaimCheckSerializer : IClaimCheckSerializer
{
    /// <summary>
    /// Serializes the property.
    /// </summary>
    public void Serialize(object claimCheckProperty, Stream stream)
    {
        JsonSerializer.Serialize(stream, claimCheckProperty);
    }

    /// <summary>
    /// Deserializes the property.
    /// </summary>
    public object Deserialize(Type propertyType, Stream stream)
    {
        return JsonSerializer.Deserialize(stream, propertyType);
    }

    /// <summary>
    /// The content type this serializer handles. Used to populate the <see cref="ClaimCheckHeaders.ClaimCheckConfigContentType"/> header.
    /// </summary>
    public string ContentType { get; } = "application/json";
}
