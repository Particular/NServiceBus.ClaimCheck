namespace NServiceBus;

using ClaimCheck;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Base class for implementations of the claim check pattern definitions.
/// </summary>
public class FileShareClaimCheck : ClaimCheckDefinition
{
    internal string BasePath { get; set; }

    /// <inheritdoc />
    protected internal override void ConfigureServices(IServiceCollection services)
    {
        if (string.IsNullOrEmpty(BasePath))
        {
            throw new InvalidOperationException("Specify the basepath for FileShareClaimCheck, eg endpointConfiguration.UseClaimCheck<FileShareClaimCheck>().BasePath(\"c:\\claimcheck\")");
        }

        services.AddSingleton<IClaimCheck>(new FileShareClaimCheckImplementation(BasePath));
    }
}