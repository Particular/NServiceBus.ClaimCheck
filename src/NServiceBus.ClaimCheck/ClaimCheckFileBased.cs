namespace NServiceBus.Features;

using System;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus.ClaimCheck;

class ClaimCheckFileBased : Feature
{
    public ClaimCheckFileBased()
    {
        DependsOn<ClaimCheckFeature>();
    }

    /// <summary>
    /// See <see cref="Feature.Setup" />
    /// </summary>
    protected override void Setup(FeatureConfigurationContext context)
    {
        if (!context.Settings.TryGet("FileShareDataBusPath", out string basePath))
        {
            throw new InvalidOperationException("Specify the basepath for FileShareDataBus, eg endpointConfiguration.UseDataBus<FileShareDataBus>().BasePath(\"c:\\databus\")");
        }
        var dataBus = new FileShareClaimCheckImplementation(basePath);

        context.Services.AddSingleton(typeof(IClaimCheck), dataBus);
    }
}