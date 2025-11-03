namespace NServiceBus.Features;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;

class CustomIClaimCheck : Feature
{
    public CustomIClaimCheck()
    {
        EnableByDefault<ClaimCheck>();

        DependsOn<ClaimCheck>();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        if (context.Settings.Get<ClaimCheckDefinition>(ClaimCheck.SelectedClaimCheckKey) is CustomClaimCheck customClaimCheckDefinition)
        {
            context.Services.AddSingleton(sp => customClaimCheckDefinition.ClaimCheckFactory(sp));
        }
    }
}