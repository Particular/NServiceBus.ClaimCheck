namespace NServiceBus.ClaimCheck.Tests;

[TimeToBeReceived("00:01:00")]
public class MessageWithExplicitTimeToLive : IMessage
{
    public ClaimCheckProperty<string> DataBusProperty { get; set; }
}