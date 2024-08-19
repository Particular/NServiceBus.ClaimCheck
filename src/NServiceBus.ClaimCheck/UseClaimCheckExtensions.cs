namespace NServiceBus.ClaimCheck.DataBus;

using System;
using System.Collections.Generic;
using Configuration.AdvancedExtensibility;

/// <summary>
/// Extension methods to configure data bus.
/// </summary>
public static class UseClaimCheckExtensions
{
    /// <summary>
    /// Configures NServiceBus to use the given data bus definition.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    public static ClaimCheckExtensions<TDataBusDefinition> UseClaimCheck<TDataBusDefinition, TDataBusSerializer>(this EndpointConfiguration config)
        where TDataBusDefinition : DataBusDefinition, new()
        where TDataBusSerializer : IClaimCheckSerializer, new()
    {
        ArgumentNullException.ThrowIfNull(config);

        return config.UseClaimCheck<TDataBusDefinition>(new TDataBusSerializer());
    }

    /// <summary>
    /// Configures NServiceBus to use the given data bus definition.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    /// <param name="dataBusSerializer">The <see cref="IClaimCheckSerializer" /> instance to use.</param>
    public static ClaimCheckExtensions<TDataBusDefinition> UseClaimCheck<TDataBusDefinition>(this EndpointConfiguration config, IClaimCheckSerializer dataBusSerializer)
        where TDataBusDefinition : DataBusDefinition, new()
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(dataBusSerializer);

        var dataBusExtensionType = typeof(ClaimCheckExtensions<>).MakeGenericType(typeof(TDataBusDefinition));
        var dataBusExtension = (ClaimCheckExtensions<TDataBusDefinition>)Activator.CreateInstance(dataBusExtensionType, config.GetSettings());
        var dataBusDefinition = new TDataBusDefinition();

        EnableDataBus(config, dataBusDefinition, dataBusSerializer);

        return dataBusExtension;
    }

    /// <summary>
    /// Configures NServiceBus to use a custom <see cref="IDataBus" /> implementation.
    /// </summary>
    /// <param name="config">The <see cref="EndpointConfiguration" /> instance to apply the settings to.</param>
    /// <param name="dataBusFactory">The factory to create the custom <see cref="IDataBus" /> to use.</param>
    /// <param name="dataBusSerializer">The <see cref="IClaimCheckSerializer" /> instance to use.</param>
    public static ClaimCheckExtensions UseClaimCheck(this EndpointConfiguration config, Func<IServiceProvider, IDataBus> dataBusFactory, IClaimCheckSerializer dataBusSerializer)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentNullException.ThrowIfNull(dataBusFactory);
        ArgumentNullException.ThrowIfNull(dataBusSerializer);

        EnableDataBus(config, new CustomDataBus(dataBusFactory), dataBusSerializer);

        return new ClaimCheckExtensions(config.GetSettings());
    }

    static void EnableDataBus(EndpointConfiguration config, DataBusDefinition selectedDataBus, IClaimCheckSerializer dataBusSerializer)
    {
        config.GetSettings().Set(Features.DataBusFeature.SelectedDataBusKey, selectedDataBus);
        config.GetSettings().Set(Features.DataBusFeature.DataBusSerializerKey, dataBusSerializer);
        config.GetSettings().Set(Features.DataBusFeature.AdditionalDataBusDeserializersKey, new List<IClaimCheckSerializer>());

        if (!config.GetSettings().HasSetting(Features.DataBusFeature.DataBusConventionsKey))
        {
            config.GetSettings().Set(Features.DataBusFeature.DataBusConventionsKey, new ClaimCheckConventions());
        }

        config.EnableFeature<Features.DataBusFeature>();
    }
}