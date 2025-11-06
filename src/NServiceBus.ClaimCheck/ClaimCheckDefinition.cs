namespace NServiceBus.ClaimCheck;

using Settings;

/// <summary>
/// Defines an implementation of the claim check pattern that can be used by NServiceBus.
/// </summary>
public abstract class ClaimCheckDefinition
{
    /// <summary>
    /// Called when the claim check implementation should enable its feature.
    /// </summary>
    protected internal abstract void EnableFeature(SettingsHolder settings);
}