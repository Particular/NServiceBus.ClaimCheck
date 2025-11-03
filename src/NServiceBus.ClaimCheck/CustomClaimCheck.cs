namespace NServiceBus;

using ClaimCheck;
using Features;
using Microsoft.Extensions.DependencyInjection;

class CustomClaimCheck : Feature
{
    public CustomClaimCheck() => DependsOn<Features.ClaimCheck>();

    protected override void Setup(FeatureConfigurationContext context)
    {
        var claimCheckFactory = context.Settings.Get<Func<IServiceProvider, IClaimCheck>>();

        context.Services.AddSingleton(sp => claimCheckFactory(sp));
    }
}