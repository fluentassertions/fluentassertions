using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides information on a particular property during an assertion for structural equality of two object graphs.
    /// </summary>
    public interface IEquivalencyValidationContext : IMemberInfo
    {
        /// <summary>
        /// Gets the value of the <see cref="MatchingExpectationProperty"/>.
        /// </summary>
        object Expectation { get; }

        /// <summary>
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        string Because { get; }

        /// <summary>
        /// Zero or more objects to format using the placeholders in <see cref="Because"/>.
        /// </summary>
        object[] BecauseArgs { get; }

        /// <summary>
        /// Gets a value indicating whether the current context represents the root of the object graph.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Gets the value of the <see cref="ISelectionContext.PropertyInfo"/>
        /// </summary>
        object Subject { get; }

        /// <summary>
        /// Gets or sets a value indicating that the root of the graph is a collection so all type-specific options apply on
        /// the collection type and not on the root itself.
        /// </summary>
        bool RootIsCollection { get; set; }

        /// <summary>
        /// Gets or sets the current trace writer or <c>null</c> if no tracing has been enabled.
        /// </summary>
        ITraceWriter Tracer { get; set; }

        /// <summary>
        /// Starts a block that scopes an operation that will be written to the currently configured <see cref="Tracer"/>
        /// after the returned disposable is disposed..
        /// </summary>
        /// <remarks>
        /// If no tracer has been configured, the call will be ignored.
        /// </remarks>
        IDisposable TraceBlock(GetTraceMessage getMessage);

        /// <summary>
        /// Writes a single line to the currently configured <see cref="Tracer"/>.
        /// </summary>
        /// <remarks>
        /// If no tracer has been configured, the call will be ignored.
        /// </remarks>
        void TraceSingle(GetTraceMessage getMessage);
    }

    /// <summary>
    /// Defines a function that takes the current path and returns the trace message to log.
    /// </summary>
    public delegate string GetTraceMessage(string path);
}
