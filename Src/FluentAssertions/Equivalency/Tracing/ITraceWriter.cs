using System;

namespace FluentAssertionsAsync.Equivalency.Tracing;

/// <summary>
/// Represents an object that is used by the <see cref="Tracer"/> class to receive tracing statements on what is
/// happening during a structural equivalency comparison.
/// </summary>
public interface ITraceWriter
{
    /// <summary>
    /// Writes a single line to the trace.
    /// </summary>
    void AddSingle(string trace);

    /// <summary>
    /// Starts a block that scopes an operation that should be written to the trace after the returned <see cref="IDisposable"/>
    /// is disposed.
    /// </summary>
    IDisposable AddBlock(string trace);

    /// <summary>
    /// Returns a copy of the trace.
    /// </summary>
    string ToString();
}
