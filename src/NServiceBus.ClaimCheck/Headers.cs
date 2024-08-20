namespace NServiceBus.ClaimCheck;

/// <summary>
/// Static class containing headers used by NServiceBus.ClaimCheck
/// </summary>
public static class ClaimCheckHeaders
{
    /// <summary>
    /// The content type used to serialize the claim check properties in the message.
    /// </summary>
    public const string ClaimCheckConfigContentType = "NServiceBus.DataBusConfig.ContentType"; // NOTE: .DataConfig required for compatibility with the Gateway BLOB matching behavior.
}