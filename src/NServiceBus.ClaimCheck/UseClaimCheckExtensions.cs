namespace NServiceBus;

using System;
using System.Collections.Generic;
using System.Reflection;
using ClaimCheck;
using Configuration.AdvancedExtensibility;

/// <summary>
/// Extension methods to configure the implementation of the claim check pattern.
/// </summary>
public static class UseClaimCheckExtensions
{
    /// <summary>
    /// Configures NServiceBus to use the given implementation of the claim check pattern definition.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    public static ClaimCheckExtensions<TClaimCheckDefinition> UseClaimCheck<TClaimCheckDefinition, TClaimCheckSerializer>(this EndpointConfiguration config)
        where TClaimCheckDefinition : ClaimCheckDefinition, new()
        where TClaimCheckSerializer : IClaimCheckSerializer, new()
    {
        ArgumentNullException.ThrowIfNull(config);

        return config.UseClaimCheck<TClaimCheckDefinition>(new TClaimCheckSerializer());
    }

    /// <summary>
    /// Configures NServiceBus to use the given implementation of the claim check pattern definition.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    /// <param name="claimCheckSerializer">The <see cref="IClaimCheckSerializer" /> instance to use.</param>
    public static ClaimCheckExtensions<TClaimCheckDefinition> UseClaimCheck<TClaimCheckDefinition>(this EndpointConfiguration config, IClaimCheckSerializer claimCheckSerializer)
        where TClaimCheckDefinition : ClaimCheckDefinition, new()
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(claimCheckSerializer);

        var claimCheckExtensionType = typeof(ClaimCheckExtensions<>).MakeGenericType(typeof(TClaimCheckDefinition));
        var claimCheckExtension = (ClaimCheckExtensions<TClaimCheckDefinition>)Activator.CreateInstance(claimCheckExtensionType, config.GetSettings());
        var claimCheckDefinition = new TClaimCheckDefinition();

        EnableClaimCheck(config, claimCheckDefinition, claimCheckSerializer);

        return claimCheckExtension;
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

        EnableClaimCheck(config, new CustomClaimCheck(claimCheckFactory), claimCheckSerializer);

        return new ClaimCheckExtensions(config.GetSettings());
    }

    static void EnableClaimCheck(EndpointConfiguration config, ClaimCheckDefinition selectedClaimCheck, IClaimCheckSerializer claimCheckSerializer)
    {
        var settings = config.GetSettings();
        settings.Set(Features.ClaimCheck.SelectedClaimCheckKey, selectedClaimCheck);
        settings.Set(Features.ClaimCheck.ClaimCheckSerializerKey, claimCheckSerializer);
        settings.Set(Features.ClaimCheck.AdditionalClaimCheckDeserializersKey, new List<IClaimCheckSerializer>());

        if (!settings.HasSetting(Features.ClaimCheck.ClaimCheckConventionsKey))
        {
            settings.Set(Features.ClaimCheck.ClaimCheckConventionsKey, new ClaimCheckConventions());
        }

        var methods = typeof(EndpointConfigurationExtensions).GetMethods();
        var enableFeatureGenericMethod = methods.Single(m => m.Name == "EnableFeature" && m.IsGenericMethod);
        var method = enableFeatureGenericMethod!.MakeGenericMethod(selectedClaimCheck.ProvidedByFeature());
        method.Invoke(null, [config]);
        config.EnableFeature<Features.ClaimCheck>();
    }
}