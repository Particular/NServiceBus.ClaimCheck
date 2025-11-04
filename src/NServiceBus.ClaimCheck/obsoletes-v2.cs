#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NServiceBus.Features
{
    using Particular.Obsoletes;

    [ObsoleteMetadata(Message = "The ClaimCheck feature should not be referenced explicitly.", RemoveInVersion = "3", TreatAsErrorFromVersion = "2")]
    [Obsolete("The ClaimCheck feature should not be referenced explicitly.. Will be removed in version 3.0.0.", true)]
    public sealed class ClaimCheck : Feature
    {
        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member