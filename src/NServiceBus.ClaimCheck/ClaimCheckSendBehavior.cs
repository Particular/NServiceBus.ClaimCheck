namespace NServiceBus;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Pipeline;
using ClaimCheck;
using Transport;

class ClaimCheckSendBehavior : IBehavior<IOutgoingLogicalMessageContext, IOutgoingLogicalMessageContext>
{
    public ClaimCheckSendBehavior(IClaimCheck claimCheck, IClaimCheckSerializer serializer, ClaimCheckConventions conventions)
    {
        this.conventions = conventions;
        claimCheckSerializer = serializer;
        this.claimCheck = claimCheck;
    }

    public async Task Invoke(IOutgoingLogicalMessageContext context, Func<IOutgoingLogicalMessageContext, Task> next)
    {
        var timeToBeReceived = TimeSpan.MaxValue;

        if (context.Extensions.TryGet<DispatchProperties>(out var properties) && properties.DiscardIfNotReceivedBefore != null)
        {
            timeToBeReceived = properties.DiscardIfNotReceivedBefore.MaxTime;
        }

        var message = context.Message.Instance;

        foreach (var property in conventions.GetClaimCheckProperties(message))
        {
            var propertyValue = property.Getter(message);

            if (propertyValue == null)
            {
                continue;
            }

            using (var stream = new MemoryStream())
            {
                var claimCheckProperty = propertyValue as IClaimCheckProperty;

                if (claimCheckProperty != null)
                {
                    propertyValue = claimCheckProperty.GetValue();
                }

                claimCheckSerializer.Serialize(propertyValue, stream);
                stream.Position = 0;

                string headerValue;

                using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
                {
                    headerValue = await claimCheck.Put(stream, timeToBeReceived, context.CancellationToken).ConfigureAwait(false);
                }

                string headerKey;

                if (claimCheckProperty != null)
                {
                    claimCheckProperty.Key = headerValue;
                    //we use the headers to in order to allow the infrastructure (eg. the gateway) to modify the actual key
                    headerKey = headerValue;
                }
                else
                {
                    property.Setter(message, null);
                    headerKey = $"{message.GetType().FullName}.{property.Name}";
                }

                //we use the headers to in order to allow the infrastructure (eg. the gateway) to modify the actual key
                context.Headers["NServiceBus.DataBus." + headerKey] = headerValue;
                context.Headers[ClaimCheckHeaders.ClaimCheckConfigContentType] = claimCheckSerializer.ContentType;
            }
        }

        await next(context).ConfigureAwait(false);
    }

    readonly ClaimCheckConventions conventions;
    readonly IClaimCheck claimCheck;
    readonly IClaimCheckSerializer claimCheckSerializer;

    public class Registration : RegisterStep
    {
        public Registration(ClaimCheckConventions conventions, IClaimCheckSerializer serializer) : base(
            "ClaimCheckSend",
            typeof(ClaimCheckSendBehavior),
            "Saves the payload into the shared location",
            b => new ClaimCheckSendBehavior(b.GetRequiredService<IClaimCheck>(), serializer, conventions))
        {
            InsertAfter("MutateOutgoingMessages");
            InsertAfter("ApplyTimeToBeReceived");
        }
    }
}