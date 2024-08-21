namespace NServiceBus;

using System;
using Features;
using ClaimCheck;

/// <summary>
/// Base class for implementations of the claim check pattern definitions.
/// </summary>
public class FileShareClaimCheck : ClaimCheckDefinition
{
    /// <summary>
    /// The feature to enable when this claim check is selected.
    /// </summary>
    protected internal override Type ProvidedByFeature()
    {
        return typeof(ClaimCheckFileBased);
    }
}