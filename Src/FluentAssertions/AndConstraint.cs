using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class AndConstraint<T>
    {
        public T And { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AndConstraint(T parentConstraint)
        {
            And = parentConstraint;
        }
    }
}
