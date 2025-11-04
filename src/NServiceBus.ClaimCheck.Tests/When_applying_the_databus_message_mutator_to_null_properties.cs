namespace NServiceBus.ClaimCheck.Tests;

using System.IO;
using System.Threading.Tasks;
using Pipeline;
using NUnit.Framework;
using Testing;

[TestFixture]
class When_applying_the_databus_message_mutator_to_null_properties
{
    [Test]
    public async Task Should_not_blow_up()
    {
        var context = new TestableOutgoingLogicalMessageContext
        {
            Message = new OutgoingLogicalMessage(typeof(MessageWithNullDataBusProperty), new MessageWithNullDataBusProperty())
        };

        var serializer = new SystemJsonClaimCheckSerializer();
        var sendBehavior = new ClaimCheckSendBehavior(null, serializer, new ClaimCheckConventions());

        using var stream = new MemoryStream();
        serializer.Serialize("test", stream);
        stream.Position = 0;

        await sendBehavior.Invoke(context, ctx => Task.CompletedTask);
    }
}