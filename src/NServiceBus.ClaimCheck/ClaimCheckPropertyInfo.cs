namespace NServiceBus.ClaimCheck;

using System;

class ClaimCheckPropertyInfo
{
    public Func<object, object> Getter;
    public string Name;
    public Type Type;
    public Action<object, object> Setter;
}