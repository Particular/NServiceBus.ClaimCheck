namespace NServiceBus.ClaimCheck;

using System;
using Features;

class CustomClaimCheck : ClaimCheckDefinition
{
    public CustomClaimCheck(Func<IServiceProvider, IClaimCheck> dataBusFactory)
    {
        ClaimCheckFactory = dataBusFactory;
    }

    protected internal override Type ProvidedByFeature()
    {
        return typeof(CustomIClaimCheck);
    }

    public Func<IServiceProvider, IClaimCheck> ClaimCheckFactory { get; }
}