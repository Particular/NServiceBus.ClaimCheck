namespace NServiceBus.ClaimCheck;

using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

/// <summary>
/// Default implementation for <see cref="IClaimCheckProperty" />.
/// </summary>
/// <typeparam name="T">Type of data to store in <see cref="IClaimCheckProperty" />.</typeparam>
public class ClaimCheckProperty<T> : IClaimCheckProperty where T : class
{
    /// <summary>
    /// Initializes a <see cref="ClaimCheckProperty{T}" /> with no value set.
    /// </summary>
    public ClaimCheckProperty() { }

    /// <summary>
    /// Initializes a <see cref="ClaimCheckProperty{T}" /> with the <paramref name="value" />.
    /// </summary>
    /// <param name="value">The value to initialize with.</param>
    public ClaimCheckProperty(T value) => SetValue(value);

    /// <summary>
    /// The value.
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public T Value { get; private set; }

    /// <summary>
    /// The property <see cref="Type" />.
    /// </summary>
    [JsonIgnore]
    public Type Type { get; } = typeof(T);

    /// <summary>
    /// The <see cref="IClaimCheckProperty" /> key.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// <code>true</code> if <see cref="IClaimCheckProperty" /> has a value.
    /// </summary>
    public bool HasValue { get; set; }

    /// <summary>
    /// Sets the value for <see cref="IClaimCheckProperty" />.
    /// </summary>
    /// <param name="valueToSet">The value to set.</param>
    public void SetValue(object valueToSet)
    {
        Value = valueToSet as T;
        HasValue = Value != null;
    }

    /// <summary>
    /// Gets the value of the <see cref="IClaimCheckProperty" />.
    /// </summary>
    /// <returns>The value.</returns>
    public object GetValue() => Value;
}