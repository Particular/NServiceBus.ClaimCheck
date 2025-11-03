namespace NServiceBus;

using System;
using Features;
using ClaimCheck;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheck) : ClaimCheckDefinition
{
    protected internal override Type ProvidedByFeature()
    {
        return typeof(CustomIClaimCheck);
    }

    protected internal override void ApplyTo(EndpointConfiguration endpointConfiguration)
        => endpointConfiguration.EnableFeature<CustomIClaimCheck>();

    public Func<IServiceProvider, IClaimCheck> ClaimCheckFactory { get; } = claimCheck;
}