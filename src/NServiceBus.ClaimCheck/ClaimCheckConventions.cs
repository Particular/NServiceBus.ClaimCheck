namespace NServiceBus.ClaimCheck;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Utils.Reflection;

/// <summary>
/// This class contains helper methods to extract and cache claim check properties from messages.
/// </summary>
public class ClaimCheckConventions
{
    /// <summary>
    /// Returns true if the given property should be sent via the selected implementation of the claim check pattern.
    /// </summary>
    public bool IsClaimCheckProperty(PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);
        try
        {
            return IsClaimCheckPropertyAction(property);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to evaluate if the Property matches the provided claim check convention. See inner exception for details.", ex);
        }
    }

    internal List<ClaimCheckPropertyInfo> GetClaimCheckProperties(object message)
    {
        return cache.GetOrAdd(message.GetType(), messageType =>
        {
            var properties = new List<ClaimCheckPropertyInfo>();
            foreach (var propertyInfo in messageType.GetProperties())
            {
                if (IsClaimCheckProperty(propertyInfo))
                {
                    properties.Add(new ClaimCheckPropertyInfo
                    {
                        Name = propertyInfo.Name,
                        Type = propertyInfo.PropertyType,
                        Getter = DelegateFactory.CreateGet(propertyInfo),
                        Setter = DelegateFactory.CreateSet(propertyInfo)
                    });
                }
            }

            return properties;
        });
    }

    internal Func<PropertyInfo, bool> IsClaimCheckPropertyAction = p => typeof(IClaimCheckProperty).IsAssignableFrom(p.PropertyType) && typeof(IClaimCheckProperty) != p.PropertyType;

    readonly ConcurrentDictionary<Type, List<ClaimCheckPropertyInfo>> cache = new();
}