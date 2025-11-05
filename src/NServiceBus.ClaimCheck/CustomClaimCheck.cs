namespace NServiceBus;

using System;
using ClaimCheck;
using Microsoft.Extensions.DependencyInjection;
using Settings;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheckFactory) : ClaimCheckDefinition
{
    protected internal override void Configure(IReadOnlySettings settings, IServiceCollection services) => services.AddSingleton(claimCheckFactory);
}