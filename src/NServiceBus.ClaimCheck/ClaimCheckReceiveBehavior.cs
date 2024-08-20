namespace NServiceBus.ClaimCheck;

using System;
using System.Threading.Tasks;
using System.Transactions;
using Pipeline;

class ClaimCheckReceiveBehavior : IBehavior<IIncomingLogicalMessageContext, IIncomingLogicalMessageContext>
{
    public ClaimCheckReceiveBehavior(
        IClaimCheck claimCheck,
        ClaimCheckDeserializer deserializer,
        ClaimCheckConventions conventions)
    {
        this.conventions = conventions;
        this.deserializer = deserializer;
        this.claimCheck = claimCheck;
    }

    public async Task Invoke(IIncomingLogicalMessageContext context, Func<IIncomingLogicalMessageContext, Task> next)
    {
        var message = context.Message.Instance;

        foreach (var property in conventions.GetClaimCheckProperties(message))
        {
            var propertyValue = property.Getter(message);

            var claimCheckProperty = propertyValue as IClaimCheckProperty;
            string headerKey;

            if (claimCheckProperty != null)
            {
                headerKey = claimCheckProperty.Key;
            }
            else
            {
                headerKey = $"{message.GetType().FullName}.{property.Name}";
            }

            if (!context.Headers.TryGetValue("NServiceBus.DataBus." + headerKey, out var claimCheckKey))
            {
                continue;
            }

            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var stream = await claimCheck.Get(claimCheckKey, context.CancellationToken).ConfigureAwait(false))
                {
                    context.Headers.TryGetValue(ClaimCheckHeaders.ClaimCheckConfigContentType, out var serializerUsed);

                    if (claimCheckProperty != null)
                    {
                        var value = deserializer.Deserialize(serializerUsed, claimCheckProperty.Type, stream);

                        claimCheckProperty.SetValue(value);
                    }
                    else
                    {
                        var value = deserializer.Deserialize(serializerUsed, property.Type, stream);

                        property.Setter(message, value);
                    }
                }
            }
        }

        await next(context).ConfigureAwait(false);
    }

    readonly ClaimCheckConventions conventions;
    readonly IClaimCheck claimCheck;
    readonly ClaimCheckDeserializer deserializer;

    public class Registration : RegisterStep
    {
        public Registration(Func<IServiceProvider, ClaimCheckReceiveBehavior> factory) : base("ClaimCheckReceive", typeof(ClaimCheckReceiveBehavior), "Copies the shared claim check data back to the logical message", b => factory(b))
        {
            InsertAfter("MutateIncomingMessages");
        }
    }
}