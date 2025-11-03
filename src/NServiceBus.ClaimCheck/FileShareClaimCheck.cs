namespace NServiceBus;

using Features;
using ClaimCheck;

/// <summary>
/// Base class for implementations of the claim check pattern definitions.
/// </summary>
public class FileShareClaimCheck : ClaimCheckDefinition
{
    /// <inheritdoc />
    protected internal override void ApplyTo(EndpointConfiguration endpointConfiguration)
        => endpointConfiguration.EnableFeature<ClaimCheckFileBased>();

    internal string BasePath { get; set; }
}