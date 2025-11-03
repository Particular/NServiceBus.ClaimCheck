namespace NServiceBus.ClaimCheck;

/// <summary>
/// Defines an implementation of the claim check pattern that can be used by NServiceBus.
/// </summary>
public abstract class ClaimCheckDefinition
{
    /// <summary>
    /// Applies the custom settings to the provided endpoint configuration.
    /// </summary>
    /// <param name="endpointConfiguration">The endpoint configuration to customize.</param>
    protected internal abstract void ApplyTo(EndpointConfiguration endpointConfiguration);
}