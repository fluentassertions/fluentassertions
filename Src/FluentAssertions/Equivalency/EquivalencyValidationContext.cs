using System;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides information on a particular property during an assertion for structural equality of two object graphs.
    /// </summary>
    public class EquivalencyValidationContext : IEquivalencyValidationContext
    {
        private Type compileTimeType;
        private Tracer tracer;

        public EquivalencyValidationContext(INode root)
        {
            CurrentNode = root;
        }

        public INode CurrentNode { get; }

        /// <summary>
        /// Gets the value of the subject object graph.
        /// </summary>
        public object Subject { get; set; }

        /// <summary>
        /// Gets the value of the expected object graph..
        /// </summary>
        public object Expectation { get; set; }

        public Reason Reason { get; set; }

        public Type CompileTimeType
        {
            get
            {
                return ((compileTimeType != typeof(object)) || Expectation is null) ? compileTimeType : RuntimeType;
            }
            set => compileTimeType = value;
        }

        /// <summary>
        /// Gets the run-time type of the current expectation object.
        /// </summary>
        public Type RuntimeType
        {
            get
            {
                if (Expectation != null)
                {
                    return Expectation.GetType();
                }

                if (CurrentNode != null)
                {
                    return CurrentNode.Type;
                }

                return CompileTimeType;
            }
        }

        public Tracer Tracer => tracer ??= new Tracer(CurrentNode, TraceWriter);

        public IEquivalencyValidationContext AsNestedMember(IMember expectationMember, IMember matchingSubjectMember)
        {
            return new EquivalencyValidationContext(expectationMember)
            {
                Subject = matchingSubjectMember.GetValue(Subject),
                Expectation = expectationMember.GetValue(Expectation),
                Reason = Reason,
                CompileTimeType = expectationMember.Type,
                TraceWriter = TraceWriter
            };
        }

        public IEquivalencyValidationContext AsCollectionItem<T>(string index, object subject, T expectation)
        {
            return new EquivalencyValidationContext(Node.FromCollectionItem<T>(index, CurrentNode))
            {
                Subject = subject,
                Expectation = expectation,
                Reason = Reason,
                CompileTimeType = typeof(T),
                TraceWriter = TraceWriter
            };
        }

        public IEquivalencyValidationContext AsDictionaryItem<TKey, TExpectation>(
            TKey key,
            object subject,
            TExpectation expectation)
        {
            return new EquivalencyValidationContext(Node.FromDictionaryItem<TExpectation>(key, CurrentNode))
            {
                Subject = subject,
                Expectation = expectation,
                Reason = Reason,
                CompileTimeType = typeof(TExpectation),
                TraceWriter = TraceWriter
            };
        }

        public IEquivalencyValidationContext Clone()
        {
            return new EquivalencyValidationContext(CurrentNode)
            {
                CompileTimeType = CompileTimeType,
                Expectation = Expectation,
                Reason = Reason,
                Subject = Subject,
                TraceWriter = TraceWriter
            };
        }

        public ITraceWriter TraceWriter { get; set; }

        public override string ToString()
        {
            return $"{{Path=\"{CurrentNode.Description}\", Subject={Subject}, Expectation={Expectation}}}";
        }
    }
}
