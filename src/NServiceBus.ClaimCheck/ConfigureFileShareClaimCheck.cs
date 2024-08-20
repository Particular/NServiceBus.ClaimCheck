namespace NServiceBus.ClaimCheck;

using System;
using Configuration.AdvancedExtensibility;

/// <summary>
/// Contains extension methods to <see cref="EndpointConfiguration" /> for the file share implementation of the claim check pattern.
/// </summary>
public static class ConfigureFileShareClaimCheck
{
    /// <summary>
    /// Sets the location to which to write/read serialized properties for the file share claim check.
    /// </summary>
    /// <param name="config">The configuration object.</param>
    /// <param name="basePath">The location to which to write/read serialized properties for the file share claim check.</param>
    /// <returns>The configuration.</returns>
    public static ClaimCheckExtensions<FileShareClaimCheck> BasePath(this ClaimCheckExtensions<FileShareClaimCheck> config, string basePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(basePath);
        config.GetSettings().Set("FileShareClaimCheckPath", basePath);

        return config;
    }
}