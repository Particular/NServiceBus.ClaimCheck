namespace NServiceBus.Features;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;

sealed class CustomIClaimCheck : Feature
{
    public CustomIClaimCheck()
    {
        EnableByDefault<ClaimCheck>();

        DependsOn<ClaimCheck>();

        Prerequisite(c =>
        {
            customClaimCheck = c.Settings.Get<ClaimCheckDefinition>() as CustomClaimCheck;
            return customClaimCheck is not null;
        }, "Checks whether the custom claim check is active.");
    }

    protected override void Setup(FeatureConfigurationContext context) => context.Services.AddSingleton(sp => customClaimCheck.Factory(sp));

    CustomClaimCheck customClaimCheck;
}