namespace NServiceBus.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck.DataBus;
using Settings;

/// <summary>
/// Used to configure the DataBus.
/// </summary>
public class DataBusFeature : Feature
{
    internal DataBusFeature()
    {
        Defaults(s => s.EnableFeatureByDefault(GetSelectedFeatureForDataBus(s)));
    }

    static Type GetSelectedFeatureForDataBus(SettingsHolder settings)
    {
        return settings.Get<DataBusDefinition>(SelectedDataBusKey)
            .ProvidedByFeature();
    }

    /// <summary>
    /// Called when the features is activated.
    /// </summary>
    protected override void Setup(FeatureConfigurationContext context)
    {
        if (context.Services.Any(sd => sd.ServiceType == typeof(IClaimCheckSerializer)))
        {
            throw new Exception("Providing data bus serializer via dependency injection is no longer supported.");
        }

        var serializer = context.Settings.Get<IClaimCheckSerializer>(DataBusSerializerKey);
        var additionalDeserializers = context.Settings.Get<List<IClaimCheckSerializer>>(AdditionalDataBusDeserializersKey);
        var conventions = context.Settings.Get<ClaimCheckConventions>(DataBusConventionsKey);

        context.RegisterStartupTask(b => new DataBusInitializer(b.GetRequiredService<IDataBus>()));
        context.Pipeline.Register(new DataBusSendBehavior.Registration(conventions, serializer));
        context.Pipeline.Register(new DataBusReceiveBehavior.Registration(b =>
        {
            return new DataBusReceiveBehavior(
                b.GetRequiredService<IDataBus>(),
                new DataBusDeserializer(serializer, additionalDeserializers),
                conventions);
        }));
    }

    internal static string SelectedDataBusKey = "SelectedDataBus";
    internal static string DataBusSerializerKey = "DataBusSerializer";
    internal static string AdditionalDataBusDeserializersKey = "AdditionalDataBusDeserializers";
    internal static string DataBusConventionsKey = "DataBusConventions";

    class DataBusInitializer : FeatureStartupTask
    {
        readonly IDataBus dataBus;

        public DataBusInitializer(IDataBus dataBus)
        {
            this.dataBus = dataBus;
        }

        protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return dataBus.Start(cancellationToken);
        }

        protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}