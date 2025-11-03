namespace NServiceBus;

using Features;
using ClaimCheck;

/// <summary>
/// Base class for implementations of the claim check pattern definitions.
/// </summary>
public class FileShareClaimCheck : ClaimCheckDefinition
{
    protected internal override void ApplyTo(EndpointConfiguration endpointConfiguration)
        => endpointConfiguration.EnableFeature<ClaimCheckFileBased>();
}