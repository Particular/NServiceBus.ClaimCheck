namespace NServiceBus.ClaimCheck.Tests;

public class MessageWithDataBusProperty : IMessage
{
    public ClaimCheckProperty<string> DataBusProperty { get; set; }
}

public class MessageWithNullDataBusProperty : IMessage
{
    public ClaimCheckProperty<string> DataBusProperty { get; set; }
}