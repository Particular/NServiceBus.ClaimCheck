namespace NServiceBus.ClaimCheck.AcceptanceTests.EndpointTemplates;

using System;
using System.Threading.Tasks;
using AcceptanceTesting.Customization;
using AcceptanceTesting.Support;

public class DefaultServer : IEndpointSetupTemplate
{
    public async Task<EndpointConfiguration> GetConfiguration(RunDescriptor runDescriptor, EndpointCustomizationConfiguration endpointConfiguration, Func<EndpointConfiguration, Task> configurationBuilderCustomization)
    {
        var builder = new EndpointConfiguration(endpointConfiguration.EndpointName);
        builder.EnableInstallers();

        builder.Recoverability()
            .Delayed(delayed => delayed.NumberOfRetries(0))
            .Immediate(immediate => immediate.NumberOfRetries(0));
        builder.SendFailedMessagesTo("error");

        var endpointCustomizationConfiguration = new ConfigureEndpointAcceptanceTestingTransport(true, true);

        await endpointCustomizationConfiguration.Configure(endpointConfiguration.EndpointName, builder, runDescriptor.Settings, endpointConfiguration.PublisherMetadata);
        runDescriptor.OnTestCompleted(_ => endpointCustomizationConfiguration.Cleanup());

        builder.UseSerialization<SystemJsonSerializer>();

        await configurationBuilderCustomization(builder).ConfigureAwait(false);

        // scan types at the end so that all types used by the configuration have been loaded into the AppDomain
        builder.ScanTypesForTest(endpointConfiguration);

        return builder;
    }
}
