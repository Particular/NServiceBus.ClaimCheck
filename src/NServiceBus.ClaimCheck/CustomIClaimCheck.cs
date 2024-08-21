namespace NServiceBus.Features;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;

class CustomIClaimCheck : Feature
{
    public CustomIClaimCheck()
    {
        DependsOn<ClaimCheck>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var customClaimCheckDefinition = context.Settings.Get<ClaimCheckDefinition>(ClaimCheck.SelectedClaimCheckKey) as CustomClaimCheck;

        if (customClaimCheckDefinition is not null)
        {
            context.Services.AddSingleton(sp => customClaimCheckDefinition.ClaimCheckFactory(sp));
        }
    }
}