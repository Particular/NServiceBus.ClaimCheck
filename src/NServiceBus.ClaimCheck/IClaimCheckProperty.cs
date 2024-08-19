namespace NServiceBus.ClaimCheck;

using System;

/// <summary>
/// The contract to implement a <see cref="IClaimCheckProperty" />.
/// </summary>
public interface IClaimCheckProperty
{
    /// <summary>
    /// The <see cref="IClaimCheckProperty" /> key.
    /// </summary>
    string Key { get; set; }

    /// <summary>
    /// <code>true</code> if <see cref="IClaimCheckProperty" /> has a value.
    /// </summary>
    bool HasValue { get; set; }

    /// <summary>
    /// Gets the value of the <see cref="IClaimCheckProperty" />.
    /// </summary>
    object GetValue();

    /// <summary>
    /// Sets the value for <see cref="IClaimCheckProperty" />.
    /// </summary>
    void SetValue(object value);

    /// <summary>
    /// The property <see cref="Type" />.
    /// </summary>
    Type Type { get; }
}