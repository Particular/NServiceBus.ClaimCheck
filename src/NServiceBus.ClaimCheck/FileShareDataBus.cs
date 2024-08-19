namespace NServiceBus.ClaimCheck.DataBus;

using System;
using Features;

/// <summary>
/// Base class for data bus definitions.
/// </summary>
public class FileShareDataBus : DataBusDefinition
{
    /// <summary>
    /// The feature to enable when this databus is selected.
    /// </summary>
    protected internal override Type ProvidedByFeature()
    {
        return typeof(DataBusFileBased);
    }
}