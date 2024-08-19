namespace NServiceBus.ClaimCheck.DataBus;

/// <summary>
/// Static class containing headers used by NServiceBus.ClaimCheck.DataBus.
/// </summary>
public static class DataBusHeaders
{
    /// <summary>
    /// The content type used to serialize the data bus properties in the message.
    /// </summary>
    public const string DataBusConfigContentType = "NServiceBus.DataBusConfig.ContentType"; // NOTE: .DataConfig required for compatibility with the Gateway BLOB matching behavior.
}