namespace NServiceBus;

using System;
using ClaimCheck;
using Microsoft.Extensions.DependencyInjection;

class CustomClaimCheck(Func<IServiceProvider, IClaimCheck> claimCheckFactory) : ClaimCheckDefinition
{
    protected internal override void ConfigureServices(IServiceCollection services) => services.AddSingleton(claimCheckFactory);
}