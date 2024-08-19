namespace NServiceBus.ClaimCheck;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// The main interface for interactions with the databus.
/// </summary>
public interface IClaimCheck
{
    /// <summary>
    /// Gets a data item from the bus.
    /// </summary>
    /// <param name="key">The key to look for.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    /// <returns>The data <see cref="Stream" />.</returns>
    Task<Stream> Get(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a data item to the bus and returns the assigned key.
    /// </summary>
    /// <param name="stream">A create containing the data to be sent on the databus.</param>
    /// <param name="timeToBeReceived">The time to be received specified on the message type. TimeSpan.MaxValue is the default.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe.</param>
    Task<string> Put(Stream stream, TimeSpan timeToBeReceived, CancellationToken cancellationToken = default);

    /// <summary>
    /// Called when the bus starts up to allow the data bus to activate background tasks.
    /// </summary>
    Task Start(CancellationToken cancellationToken = default);
}