namespace NServiceBus.ClaimCheck;

using System;
using System.Reflection;
using Configuration.AdvancedExtensibility;

/// <summary>
/// A set of extension methods for configuring unobtrusive DataBus properties.
/// </summary>
public static class ConventionsBuilderExtensions
{
    /// <summary>
    /// Sets the function to be used to evaluate whether a property should be sent via the DataBus or not.
    /// </summary>
    public static ConventionsBuilder DefiningDataBusPropertiesAs(this ConventionsBuilder builder, Func<PropertyInfo, bool> definesDataBusProperty)
    {
        ArgumentNullException.ThrowIfNull(definesDataBusProperty);

        var dataBusConventions = builder.GetSettings().GetOrDefault<ClaimCheckConventions>(Features.DataBusFeature.DataBusConventionsKey);

        if (dataBusConventions == null)
        {
            dataBusConventions = new ClaimCheckConventions();
            builder.GetSettings().Set(Features.DataBusFeature.DataBusConventionsKey, dataBusConventions);
        }

        dataBusConventions.IsClaimCheckPropertyAction = definesDataBusProperty;

        return builder;
    }
}