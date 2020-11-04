namespace FluentAssertions.Equivalency.Tracing
{
    /// <summary>
    /// Defines a function that takes the full path from the root object until the current object
    /// in the equivalency operation separated by dots, and returns the trace message to log.
    /// </summary>
    public delegate string GetTraceMessage(INode node);
}
