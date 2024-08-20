namespace NServiceBus.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClaimCheck;
using Microsoft.Extensions.DependencyInjection;
using Settings;

/// <summary>
/// Used to configure the claim check implementation.
/// </summary>
public class ClaimCheckFeature : Feature
{
    internal ClaimCheckFeature()
    {
        Defaults(s => s.EnableFeatureByDefault(GetSelectedFeatureForClaimCheck(s)));
    }

    static Type GetSelectedFeatureForClaimCheck(SettingsHolder settings)
    {
        return settings.Get<ClaimCheckDefinition>(SelectedClaimCheckKey)
            .ProvidedByFeature();
    }

    /// <summary>
    /// Called when the feature is activated.
    /// </summary>
    protected override void Setup(FeatureConfigurationContext context)
    {
        if (context.Services.Any(sd => sd.ServiceType == typeof(IClaimCheckSerializer)))
        {
            throw new Exception("Providing claimcheck serializer via dependency injection is no longer supported.");
        }

        var serializer = context.Settings.Get<IClaimCheckSerializer>(ClaimCheckSerializerKey);
        var additionalDeserializers = context.Settings.Get<List<IClaimCheckSerializer>>(AdditionalClaimCheckDeserializersKey);
        var conventions = context.Settings.Get<ClaimCheckConventions>(ClaimCheckConventionsKey);

        context.RegisterStartupTask(b => new ClaimCheckInitializer(b.GetRequiredService<IClaimCheck>()));
        context.Pipeline.Register(new ClaimCheckSendBehavior.Registration(conventions, serializer));
        context.Pipeline.Register(new ClaimCheckReceiveBehavior.Registration(b =>
        {
            return new ClaimCheckReceiveBehavior(
                b.GetRequiredService<IClaimCheck>(),
                new ClaimCheckDeserializer(serializer, additionalDeserializers),
                conventions);
        }));
    }

    internal static string SelectedClaimCheckKey = "SelectedClaimCheck";
    internal static string ClaimCheckSerializerKey = "ClaimCheckSerializer";
    internal static string AdditionalClaimCheckDeserializersKey = "AdditionalClaimCheckDeserializers";
    internal static string ClaimCheckConventionsKey = "ClaimCheckConventions";

    class ClaimCheckInitializer : FeatureStartupTask
    {
        readonly IClaimCheck claimcheck;

        public ClaimCheckInitializer(IClaimCheck claimcheck)
        {
            this.claimcheck = claimcheck;
        }

        protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return claimcheck.Start(cancellationToken);
        }

        protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}