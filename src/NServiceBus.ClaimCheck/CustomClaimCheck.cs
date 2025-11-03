namespace NServiceBus;

using System;
using Features;
using ClaimCheck;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheck) : ClaimCheckDefinition
{
    protected internal override Type ProvidedByFeature() => typeof(CustomIClaimCheck);

    public Func<IServiceProvider, IClaimCheck> ClaimCheckFactory { get; } = claimCheck;
}