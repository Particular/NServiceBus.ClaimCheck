namespace NServiceBus.ClaimCheck.AcceptanceTests.EndpointTemplates;

using Configuration.AdvancedExtensibility;

public static class ConfigureExtensions
{
    public static RoutingSettings ConfigureRouting(this EndpointConfiguration configuration) => new(configuration.GetSettings());
}