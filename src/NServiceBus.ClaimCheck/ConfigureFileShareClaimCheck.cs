namespace NServiceBus.ClaimCheck;

using System;
using Configuration.AdvancedExtensibility;

/// <summary>
/// Contains extension methods to <see cref="EndpointConfiguration" /> for the file share data bus.
/// </summary>
public static class ConfigureFileShareClaimCheck
{
    /// <summary>
    /// Sets the location to which to write/read serialized properties for the databus.
    /// </summary>
    /// <param name="config">The configuration object.</param>
    /// <param name="basePath">The location to which to write/read serialized properties for the databus.</param>
    /// <returns>The configuration.</returns>
    public static ClaimCheckExtensions<FileShareClaimCheck> BasePath(this ClaimCheckExtensions<FileShareClaimCheck> config, string basePath)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(basePath);
        config.GetSettings().Set("FileShareDataBusPath", basePath);

        return config;
    }
}