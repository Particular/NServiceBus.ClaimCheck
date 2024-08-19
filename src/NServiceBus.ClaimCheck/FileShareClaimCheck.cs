namespace NServiceBus.ClaimCheck;

using System;
using Features;

/// <summary>
/// Base class for data bus definitions.
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