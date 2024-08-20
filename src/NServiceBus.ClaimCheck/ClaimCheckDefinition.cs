namespace NServiceBus.ClaimCheck;

using System;

/// <summary>
/// Defines an implementation of the claim check pattern that can be used by NServiceBus.
/// </summary>
public abstract class ClaimCheckDefinition
{
    /// <summary>
    /// The feature to enable when this implmentation of the claim check pattern is selected.
    /// </summary>
    protected internal abstract Type ProvidedByFeature();
}