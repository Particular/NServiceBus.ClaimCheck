namespace NServiceBus;

using System;
using ClaimCheck;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheckFactory) : ClaimCheckDefinition
{
    public Func<IServiceProvider, IClaimCheck> Factory { get; } = claimCheckFactory;

    protected internal override void ApplyTo(EndpointConfiguration endpointConfiguration) => endpointConfiguration.EnableFeature<Features.CustomIClaimCheck>();
}