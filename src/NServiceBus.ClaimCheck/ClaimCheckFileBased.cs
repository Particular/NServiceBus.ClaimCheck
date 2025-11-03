namespace NServiceBus.Features;

using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;

sealed class ClaimCheckFileBased : Feature
{
    public ClaimCheckFileBased()
    {
        EnableByDefault<ClaimCheck>();

        DependsOn<ClaimCheck>();

        Prerequisite(c => c.Settings.Get<ClaimCheckDefinition>() is FileShareClaimCheck, "Checks whether the file-based claim check is active.");
    }

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