namespace NServiceBus.ClaimCheck.Tests;

using Features;
using NUnit.Framework;
using Particular.Approvals;
using PublicApiGenerator;

[TestFixture]
public class APIApprovals
{
    [Test]
    public void ApproveClaimCheck()
    {
        var publicApi = typeof(ClaimCheckFeature).Assembly.GeneratePublicApi(new ApiGeneratorOptions
        {
            ExcludeAttributes = ["System.Runtime.Versioning.TargetFrameworkAttribute", "System.Reflection.AssemblyMetadataAttribute"]
        });
        Approver.Verify(publicApi);
    }
}