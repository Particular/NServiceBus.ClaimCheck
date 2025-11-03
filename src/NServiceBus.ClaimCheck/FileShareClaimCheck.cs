namespace NServiceBus;

using System;
using Features;
using ClaimCheck;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Base class for implementations of the claim check pattern definitions.
/// </summary>
public class FileShareClaimCheck : Feature
{
    internal FileShareClaimCheck() => DependsOn<Features.ClaimCheck>();

    /// <summary>
    /// See <see cref="Feature.Setup" />
    /// </summary>
    protected override void Setup(FeatureConfigurationContext context)
    {
        if (!context.Settings.TryGet("FileShareClaimCheckPath", out string basePath))
        {
            throw new InvalidOperationException("Specify the basepath for FileShareClaimCheck, eg endpointConfiguration.UseClaimCheck<FileShareClaimCheck>().BasePath(\"c:\\claimcheck\")");
        }

        var claimCheck = new FileShareClaimCheckImplementation(basePath);

        context.Services.AddSingleton(typeof(IClaimCheck), claimCheck);
    }
}