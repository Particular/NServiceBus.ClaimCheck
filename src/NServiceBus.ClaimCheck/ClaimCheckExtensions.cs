namespace NServiceBus.ClaimCheck;

using System;
using System.Collections.Generic;
using System.Linq;
using Configuration.AdvancedExtensibility;
using Settings;

/// <summary>
/// This class provides implementers of the claim check pattern with an extension mechanism for custom settings via extension methods.
/// </summary>
/// <typeparam name="T">The implementation of the claim check pattern definition eg <see cref="FileShareClaimCheck" />.</typeparam>
/// <remarks>
/// Default constructor.
/// </remarks>
public class ClaimCheckExtensions<T>(SettingsHolder settings) : ClaimCheckExtensions(settings) where T : ClaimCheckDefinition;

/// <summary>
/// This class provides implementers of the claim check pattern with an extension mechanism for custom settings via extension methods.
/// </summary>
/// <remarks>
/// Default constructor.
/// </remarks>
public class ClaimCheckExtensions(SettingsHolder settings) : ExposeSettings(settings)
{

    /// <summary>
    /// Configures additional deserializers to be considered when processing claim check properties. Can be called multiple times.
    /// </summary>
    public ClaimCheckExtensions AddDeserializer<TSerializer>() where TSerializer : IClaimCheckSerializer, new()
        => AddDeserializer(new TSerializer());

    /// <summary>
    /// Configures additional deserializers to be considered when processing claim check properties. Can be called multiple times.
    /// </summary>
    public ClaimCheckExtensions AddDeserializer<TSerializer>(TSerializer serializer) where TSerializer : IClaimCheckSerializer
    {
        ArgumentNullException.ThrowIfNull(serializer);

        var deserializers = this.GetSettings().Get<List<IClaimCheckSerializer>>();

        if (deserializers.Any(d => d.ContentType == serializer.ContentType))
        {
            throw new ArgumentException($"Deserializer for content type '{serializer.ContentType}' is already registered.");
        }

        var mainSerializer = this.GetSettings().Get<IClaimCheckSerializer>();

        if (mainSerializer.ContentType == serializer.ContentType)
        {
            throw new ArgumentException($"Main serializer already handles content type '{serializer.ContentType}'.");
        }

        deserializers.Add(serializer);

        return this;
    }
}