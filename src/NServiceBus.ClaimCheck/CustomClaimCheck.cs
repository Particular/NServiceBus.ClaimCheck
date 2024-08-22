namespace NServiceBus;

using System;
using Features;
using ClaimCheck;

class CustomClaimCheck : ClaimCheckDefinition
{
    public CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheck)
    {
        ClaimCheckFactory = claimCheck;
    }

    protected internal override Type ProvidedByFeature()
    {
        return typeof(CustomIClaimCheck);
    }

    public Func<IServiceProvider, IClaimCheck> ClaimCheckFactory { get; }
}