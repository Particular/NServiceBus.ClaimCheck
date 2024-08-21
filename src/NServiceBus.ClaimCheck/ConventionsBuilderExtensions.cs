namespace NServiceBus.ClaimCheck;

using System;
using System.Reflection;
using Configuration.AdvancedExtensibility;

/// <summary>
/// A set of extension methods for configuring unobtrusive claim check properties.
/// </summary>
public static class ConventionsBuilderExtensions
{
    /// <summary>
    /// Sets the function to be used to evaluate whether a property should be sent via the implementation of the claim check pattern or not.
    /// </summary>
    public static ConventionsBuilder DefiningClaimCheckPropertiesAs(this ConventionsBuilder builder, Func<PropertyInfo, bool> definesClaimCheckProperty)
    {
        ArgumentNullException.ThrowIfNull(definesClaimCheckProperty);

        var claimCheckConventions = builder.GetSettings().GetOrDefault<ClaimCheckConventions>(Features.ClaimCheck.ClaimCheckConventionsKey);

        if (claimCheckConventions == null)
        {
            claimCheckConventions = new ClaimCheckConventions();
            builder.GetSettings().Set(Features.ClaimCheck.ClaimCheckConventionsKey, claimCheckConventions);
        }

        claimCheckConventions.IsClaimCheckPropertyAction = definesClaimCheckProperty;

        return builder;
    }
}