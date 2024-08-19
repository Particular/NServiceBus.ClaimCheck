namespace NServiceBus.ClaimCheck.DataBus;

using System;
using Features;

class CustomDataBus : DataBusDefinition
{
    public CustomDataBus(Func<IServiceProvider, IDataBus> dataBusFactory)
    {
        DataBusFactory = dataBusFactory;
    }

    protected internal override Type ProvidedByFeature()
    {
        return typeof(CustomIDataBus);
    }

    public Func<IServiceProvider, IDataBus> DataBusFactory { get; }
}