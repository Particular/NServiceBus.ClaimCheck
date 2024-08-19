namespace NServiceBus.Features;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;

class CustomIClaimCheck : Feature
{
    public CustomIClaimCheck()
    {
        DependsOn<ClaimCheckFeature>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var customDataBusDefinition = context.Settings.Get<ClaimCheckDefinition>(ClaimCheckFeature.SelectedDataBusKey) as CustomClaimCheck;

        if (customDataBusDefinition is not null)
        {
            context.Services.AddSingleton(sp => customDataBusDefinition.ClaimCheckFactory(sp));
        }
    }
}