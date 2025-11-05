namespace NServiceBus.ClaimCheck;

using Microsoft.Extensions.DependencyInjection;
using Settings;

/// <summary>
/// Defines an implementation of the claim check pattern that can be used by NServiceBus.
/// </summary>
public abstract class ClaimCheckDefinition
{
    /// <summary>
    /// Called when the claim check implementation should perform its configuration.
    /// </summary>
    /// <param name="settings">The endpoint settings.</param>
    /// <param name="services">The service collection to register in.</param>
    protected internal abstract void Configure(IReadOnlySettings settings, IServiceCollection services);
}