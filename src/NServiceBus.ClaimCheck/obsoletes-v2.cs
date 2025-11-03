#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace NServiceBus.ClaimCheck
{
    using System;
    using Particular.Obsoletes;

    [ObsoleteMetadata(Message = "Custom claim checks are now implemented by inheriting from NServiceBus.Features.Feature", RemoveInVersion = "3", TreatAsErrorFromVersion = "2")]
    [Obsolete("Custom claim checks are now implemented by inheriting from NServiceBus.Features.Feature. Will be removed in version 3.0.0.", true)]
    public abstract class ClaimCheckDefinition
    {
        protected internal abstract Type ProvidedByFeature();
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member