namespace FluentAssertions.Equivalency.Tracing;

internal static class ExtensionMethods
{
    internal static ITraceWriter NewInstance(this ITraceWriter _) => new StringBuilderTraceWriter();
}
