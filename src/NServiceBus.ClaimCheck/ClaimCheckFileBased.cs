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

        Prerequisite(c =>
        {
            fileShareClaimCheck = c.Settings.Get<ClaimCheckDefinition>() as FileShareClaimCheck;
            return fileShareClaimCheck is not null;
        }, "Checks whether the file-based claim check is active.");
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        if (string.IsNullOrEmpty(fileShareClaimCheck.BasePath))
        {
            throw new InvalidOperationException("Specify the basepath for FileShareClaimCheck, eg endpointConfiguration.UseClaimCheck<FileShareClaimCheck>().BasePath(\"c:\\claimcheck\")");
        }

        context.Services.AddSingleton<IClaimCheck>(new FileShareClaimCheckImplementation(fileShareClaimCheck.BasePath));
    }

    FileShareClaimCheck fileShareClaimCheck;
}