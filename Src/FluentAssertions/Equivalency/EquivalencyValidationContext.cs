using FluentAssertions.Equivalency.Execution;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;
using static System.FormattableString;

namespace FluentAssertions.Equivalency;

/// <summary>
/// Provides information on a particular property during an assertion for structural equality of two object graphs.
/// </summary>
public class EquivalencyValidationContext : IEquivalencyValidationContext
{
    private Tracer tracer;

    public EquivalencyValidationContext(INode root, IEquivalencyOptions options)
    {
        Options = options;
        CurrentNode = root;
        CyclicReferenceDetector = new CyclicReferenceDetector();
    }

    public INode CurrentNode { get; }

    public Reason Reason { get; set; }

    public Tracer Tracer => tracer ??= new Tracer(CurrentNode, TraceWriter);

    public IEquivalencyOptions Options { get; }

    private CyclicReferenceDetector CyclicReferenceDetector { get; set; }

    public IEquivalencyValidationContext AsNestedMember(IMember expectationMember)
    {
        return new EquivalencyValidationContext(expectationMember, Options)
        {
            Reason = Reason,
            TraceWriter = TraceWriter,
            CyclicReferenceDetector = CyclicReferenceDetector
        };
    }

    public IEquivalencyValidationContext AsCollectionItem<TItem>(string index)
    {
        return new EquivalencyValidationContext(Node.FromCollectionItem<TItem>(index, CurrentNode), Options)
        {
            Reason = Reason,
            TraceWriter = TraceWriter,
            CyclicReferenceDetector = CyclicReferenceDetector
        };
    }

    public IEquivalencyValidationContext AsDictionaryItem<TKey, TExpectation>(TKey key)
    {
        return new EquivalencyValidationContext(Node.FromDictionaryItem<TExpectation>(key, CurrentNode), Options)
        {
            Reason = Reason,
            TraceWriter = TraceWriter,
            CyclicReferenceDetector = CyclicReferenceDetector
        };
    }

    public IEquivalencyValidationContext Clone()
    {
        return new EquivalencyValidationContext(CurrentNode, Options)
        {
            Reason = Reason,
            TraceWriter = TraceWriter,
            CyclicReferenceDetector = CyclicReferenceDetector
        };
    }

    public bool IsCyclicReference(object expectation)
    {
        EqualityStrategy strategy = expectation is not null
            ? Options.GetEqualityStrategy(expectation.GetType())
            : EqualityStrategy.Equals;

        var reference = new ObjectReference(expectation, CurrentNode.Subject.PathAndName, strategy);

        return CyclicReferenceDetector.IsCyclicReference(reference);
    }

    public ITraceWriter TraceWriter { get; set; }

    public override string ToString()
    {
        return Invariant($"{{Path=\"{CurrentNode.Subject.PathAndName}\"}}");
    }
}
