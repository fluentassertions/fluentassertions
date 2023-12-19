using System;

namespace FluentAssertionsAsync.Equivalency.Tracing;

/// <summary>
/// Exposes tracing capabilities that can be used by the implementation of the equivalency algorithm
/// when an <see cref="ITraceWriter"/> is provided.
/// </summary>
public class Tracer
{
    private readonly INode currentNode;
    private readonly ITraceWriter traceWriter;

    internal Tracer(INode currentNode, ITraceWriter traceWriter)
    {
        this.currentNode = currentNode;
        this.traceWriter = traceWriter;
    }

    /// <summary>
    /// Writes a single line to the currently configured <see cref="ITraceWriter"/>.
    /// </summary>
    /// <remarks>
    /// If no tracer has been configured, the call will be ignored.
    /// </remarks>
    public void WriteLine(GetTraceMessage getTraceMessage)
    {
        traceWriter?.AddSingle(getTraceMessage(currentNode));
    }

    /// <summary>
    /// Starts a block that scopes an operation that will be written to the currently configured <see cref="ITraceWriter"/>
    /// after the returned disposable is disposed.
    /// </summary>
    /// <remarks>
    /// If no tracer has been configured for the <see cref="IEquivalencyValidationContext"/>, the call will be ignored.
    /// </remarks>
    public IDisposable WriteBlock(GetTraceMessage getTraceMessage)
    {
        if (traceWriter is not null)
        {
            return traceWriter.AddBlock(getTraceMessage(currentNode));
        }

        return new Disposable(() => { });
    }

    public override string ToString() => traceWriter is not null ? traceWriter.ToString() : string.Empty;
}
