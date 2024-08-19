namespace NServiceBus.ClaimCheck.DataBus.Utils.Reflection;

using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

static class DelegateFactory
{
    public static Func<object, object> CreateGet(PropertyInfo property)
    {
        if (!propertyInfoToLateBoundProperty.TryGetValue(property, out var lateBoundPropertyGet))
        {
            var instanceParameter = Expression.Parameter(typeof(object), "target");

            var member = Expression.Property(Expression.Convert(instanceParameter, property.DeclaringType), property);

            var lambda = Expression.Lambda<Func<object, object>>(
                Expression.Convert(member, typeof(object)),
                instanceParameter
            );

            lateBoundPropertyGet = lambda.Compile();
            propertyInfoToLateBoundProperty[property] = lateBoundPropertyGet;
        }

        return lateBoundPropertyGet;
    }

    public static Action<object, object> CreateSet(PropertyInfo property)
    {
        if (!propertyInfoToLateBoundPropertySet.TryGetValue(property, out var result))
        {
            var method = new DynamicMethod("Set" + property.Name, null, new[] { typeof(object), typeof(object) }, true);
            var gen = method.GetILGenerator();

            var sourceType = property.DeclaringType;
            var setter = property.GetSetMethod(true);

            gen.Emit(OpCodes.Ldarg_0); // Load input to stack

            if (sourceType.IsValueType)
            {
                gen.Emit(OpCodes.Unbox, sourceType); //this is a struct so unbox
            }
            else
            {
                gen.Emit(OpCodes.Castclass, sourceType); // Cast to source type
            }

            gen.Emit(OpCodes.Ldarg_1); // Load value to stack
            gen.Emit(OpCodes.Unbox_Any, property.PropertyType); // Unbox the value to its proper value type

            if (sourceType.IsValueType && !IsSimpleType(sourceType))
            {
                gen.Emit(OpCodes.Call, setter); // structs don't have virtual setters
            }
            else
            {
                gen.Emit(OpCodes.Callvirt, setter); // Call the setter method
            }

            gen.Emit(OpCodes.Ret);

            result = (Action<object, object>)method.CreateDelegate(typeof(Action<object, object>));
            propertyInfoToLateBoundPropertySet[property] = result;
        }

        return result;
    }

    /// <summary>
    /// Returns true if the type can be serialized as is.
    /// </summary>
    static bool IsSimpleType(Type type) =>
        type == typeof(string) ||
        type.IsPrimitive ||
        type == typeof(decimal) ||
        type == typeof(Guid) ||
        type == typeof(DateTime) ||
        type == typeof(TimeSpan) ||
        type == typeof(DateTimeOffset) ||
        type.IsEnum;

    static readonly ConcurrentDictionary<PropertyInfo, Func<object, object>> propertyInfoToLateBoundProperty = new();
    static readonly ConcurrentDictionary<PropertyInfo, Action<object, object>> propertyInfoToLateBoundPropertySet = new();
}