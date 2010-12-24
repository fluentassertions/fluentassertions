using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public class AndConstraint<T>
    {
        private readonly T parentConstraint;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public AndConstraint(T parentConstraint)
        {
            this.parentConstraint = parentConstraint;
        }

        public T And
        {
            get { return parentConstraint; }
        }
    }
}