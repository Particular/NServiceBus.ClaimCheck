namespace NServiceBus;

using System;
using ClaimCheck;
using Microsoft.Extensions.DependencyInjection;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheckFactory) : ClaimCheckDefinition
{
    protected internal override void ApplyTo(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.EnableFeature<Features.ClaimCheck>();
        endpointConfiguration.RegisterComponents(s => s.AddSingleton(claimCheckFactory));
    }
}