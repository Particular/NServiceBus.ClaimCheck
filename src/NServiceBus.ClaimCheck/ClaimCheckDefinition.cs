namespace NServiceBus.ClaimCheck;

using System;

/// <summary>
/// Defines an implementation of the claim check pattern that can be used by NServiceBus.
/// </summary>
public abstract partial class ClaimCheckDefinition
{
    /// <summary>
    /// The feature to enable when this implmentation of the claim check pattern is selected.
    /// </summary>
    /// TODO Obsolete
    protected internal abstract Type ProvidedByFeature();

    /// <summary>
    /// Applies the custom settings to the provided endpoint configuration.
    /// </summary>
    /// <param name="endpointConfiguration">The endpoint configuration to customize.</param>
    protected internal abstract void ApplyTo(EndpointConfiguration endpointConfiguration);
}