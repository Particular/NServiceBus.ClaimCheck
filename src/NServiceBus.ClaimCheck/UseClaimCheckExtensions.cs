namespace NServiceBus;

using System;
using System.Collections.Generic;
using ClaimCheck;
using Configuration.AdvancedExtensibility;
using Features;

/// <summary>
/// Extension methods to configure the implementation of the claim check pattern.
/// </summary>
public static class UseClaimCheckExtensions
{
    /// <summary>
    /// Configures NServiceBus to use the given implementation of the claim check pattern definition.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    public static ClaimCheckExtensions<TClaimCheckFeature> UseClaimCheck<TClaimCheckFeature, TClaimCheckSerializer>(this EndpointConfiguration config)
        where TClaimCheckFeature : Feature
        where TClaimCheckSerializer : IClaimCheckSerializer, new()
    {
        ArgumentNullException.ThrowIfNull(config);

        return config.UseClaimCheck<TClaimCheckFeature>(new TClaimCheckSerializer());
    }

    /// <summary>
    /// Configures NServiceBus to use the given implementation of the claim check pattern definition.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    /// <param name="claimCheckSerializer">The <see cref="IClaimCheckSerializer" /> instance to use.</param>
    public static ClaimCheckExtensions<TClaimCheckFeature> UseClaimCheck<TClaimCheckFeature>(this EndpointConfiguration config, IClaimCheckSerializer claimCheckSerializer)
        where TClaimCheckFeature : Feature
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(claimCheckSerializer);

        EnableClaimCheck<TClaimCheckFeature>(config, claimCheckSerializer);

        return new ClaimCheckExtensions<TClaimCheckFeature>(config.GetSettings());
    }

    /// <summary>
    /// Configures NServiceBus to use a custom <see cref="IClaimCheck" /> implementation.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    /// <param name="claimCheckFactory">The factory to create the custom <see cref="IClaimCheck" /> to use.</param>
    /// <param name="claimCheckSerializer">The <see cref="IClaimCheckSerializer" /> instance to use.</param>
    public static ClaimCheckExtensions UseClaimCheck(this EndpointConfiguration config, Func<IServiceProvider, IClaimCheck> claimCheckFactory, IClaimCheckSerializer claimCheckSerializer)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(claimCheckFactory);
        ArgumentNullException.ThrowIfNull(claimCheckSerializer);

        config.GetSettings().Set(claimCheckFactory);

        EnableClaimCheck<CustomClaimCheck>(config, claimCheckSerializer);

        return new ClaimCheckExtensions(config.GetSettings());
    }

    static void EnableClaimCheck<TClaimCheckFeature>(EndpointConfiguration config, IClaimCheckSerializer claimCheckSerializer) where TClaimCheckFeature : Features.Feature
    {
        var settings = config.GetSettings();
        settings.Set(Features.ClaimCheck.ClaimCheckSerializerKey, claimCheckSerializer);
        settings.Set(Features.ClaimCheck.AdditionalClaimCheckDeserializersKey, new List<IClaimCheckSerializer>());

        if (!settings.HasSetting(Features.ClaimCheck.ClaimCheckConventionsKey))
        {
            settings.Set(Features.ClaimCheck.ClaimCheckConventionsKey, new ClaimCheckConventions());
        }

        config.EnableFeature<TClaimCheckFeature>();
        config.EnableFeature<Features.ClaimCheck>();
    }
}