namespace NServiceBus;

using System;
using ClaimCheck;
using Features;
using Microsoft.Extensions.DependencyInjection;
using Settings;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheckFactory) : ClaimCheckDefinition
{
    protected internal override void EnableFeature(SettingsHolder settings)
    {
        settings.Set(claimCheckFactory);
        settings.EnableFeature<CustomClaimCheckFeature>();
    }

    class CustomClaimCheckFeature : Feature
    {
        protected override void Setup(FeatureConfigurationContext context) => context.Services.AddSingleton(context.Settings.Get<Func<IServiceProvider, IClaimCheck>>());
    }
}