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
public class ClaimCheckExtensions<T> : ClaimCheckExtensions where T : ClaimCheckDefinition
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public ClaimCheckExtensions(SettingsHolder settings)
        : base(settings)
    {
    }
}

/// <summary>
/// This class provides implementers of the claim check pattern with an extension mechanism for custom settings via extension methods.
/// </summary>
public class ClaimCheckExtensions : ExposeSettings
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public ClaimCheckExtensions(SettingsHolder settings)
        : base(settings)
    {
    }

    /// <summary>
    /// Configures additional deserializers to be considered when processing claim check properties. Can be called multiple times.
    /// </summary>
    public ClaimCheckExtensions AddDeserializer<TSerializer>() where TSerializer : IClaimCheckSerializer, new()
    {
        var serializer = (TSerializer)Activator.CreateInstance(typeof(TSerializer));

        return AddDeserializer(serializer);
    }

    /// <summary>
    /// Configures additional deserializers to be considered when processing claim check properties. Can be called multiple times.
    /// </summary>
    public ClaimCheckExtensions AddDeserializer<TSerializer>(TSerializer serializer) where TSerializer : IClaimCheckSerializer
    {
        ArgumentNullException.ThrowIfNull(serializer);

        var deserializers = this.GetSettings().Get<List<IClaimCheckSerializer>>(Features.ClaimCheck.AdditionalClaimCheckDeserializersKey);

        if (deserializers.Any(d => d.ContentType == serializer.ContentType))
        {
            throw new ArgumentException($"Deserializer for content type '{serializer.ContentType}' is already registered.");
        }

        var mainSerializer = this.GetSettings().Get<IClaimCheckSerializer>(Features.ClaimCheck.ClaimCheckSerializerKey);

        if (mainSerializer.ContentType == serializer.ContentType)
        {
            throw new ArgumentException($"Main serializer already handles content type '{serializer.ContentType}'.");
        }

        deserializers.Add(serializer);

        return this;
    }
}