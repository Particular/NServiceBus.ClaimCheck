namespace NServiceBus.ClaimCheck;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Defines an implementation of the claim check pattern that can be used by NServiceBus.
/// </summary>
public abstract class ClaimCheckDefinition
{
    /// <summary>
    /// Called when the claim check implementation should register itself in the container.
    /// </summary>
    /// <param name="services">The service collection to register in.</param>
    protected internal abstract void ConfigureServices(IServiceCollection services);
}