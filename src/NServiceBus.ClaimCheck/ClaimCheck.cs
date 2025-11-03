namespace NServiceBus.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NServiceBus.ClaimCheck;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Used to configure the claim check implementation.
/// </summary>
public class ClaimCheck : Feature
{
    internal ClaimCheck()
    {
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

    internal const string SelectedClaimCheckKey = "SelectedClaimCheck";
    internal const string ClaimCheckSerializerKey = "ClaimCheckSerializer";
    internal const string AdditionalClaimCheckDeserializersKey = "AdditionalClaimCheckDeserializers";
    internal const string ClaimCheckConventionsKey = "ClaimCheckConventions";

    class ClaimCheckInitializer(IClaimCheck claimcheck) : FeatureStartupTask
    {
        protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default) =>
            claimcheck.Start(cancellationToken);

        protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}