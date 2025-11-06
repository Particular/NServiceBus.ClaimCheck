namespace NServiceBus;

using ClaimCheck;
using Features;
using Microsoft.Extensions.DependencyInjection;
using Settings;

/// <summary>
/// Base class for implementations of the claim check pattern definitions.
/// </summary>
public class FileShareClaimCheck : ClaimCheckDefinition
{
    internal string BasePath;

    /// <inheritdoc />
    protected internal override void EnableFeature(SettingsHolder settings) => settings.EnableFeature<FileShareClaimCheckFeature>();

    class FileShareClaimCheckFeature : Feature
    {
        public FileShareClaimCheckFeature()
        {
            DependsOn<Features.ClaimCheck>();
            EnableByDefault<Features.ClaimCheck>();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var basePath = context.Settings.Get<FileShareClaimCheck>().BasePath;
            if (string.IsNullOrWhiteSpace(basePath))
            {
                throw new InvalidOperationException("Specify the basepath for FileShareClaimCheck, eg endpointConfiguration.UseClaimCheck<FileShareClaimCheck>().BasePath(\"c:\\claimcheck\")");
            }

            context.Services.AddSingleton<IClaimCheck>(new FileShareClaimCheckImplementation(basePath));

            context.Settings.AddStartupDiagnosticsSection("FileShareClaimCheck", new
            {
                basePath
            });
        }
    }
}