using System;
using System.Linq;
using FluentAssertions.Common;
using static System.FormattableString;

namespace FluentAssertions.Equivalency
{
    public class Comparands
    {
        private Type compileTimeType;

        public Comparands()
        {
        }

        public Comparands(object subject, object expectation, Type compileTimeType)
        {
            this.compileTimeType = compileTimeType;
            Subject = subject;
            Expectation = expectation;
        }

        /// <summary>
        /// Gets the value of the subject object graph.
        /// </summary>
        public object Subject { get; set; }

        /// <summary>
        /// Gets the value of the expected object graph.
        /// </summary>
        public object Expectation { get; set; }

        public Type CompileTimeType
        {
            get
            {
                return ((compileTimeType != typeof(object)) || Expectation is null) ? compileTimeType : RuntimeType;
            }

            // SMELL: Do we really need this? Can we replace it by making Comparands generic or take a constructor parameter?
            set => compileTimeType = value;
        }

        /// <summary>
        /// Gets the run-time type of the current expectation object.
        /// </summary>
        public Type RuntimeType
        {
            get
            {
                if (Expectation is not null)
                {
                    return Expectation.GetType();
                }

                return CompileTimeType;
            }
        }

        /// <summary>
        /// Returns either the run-time or compile-time type of the expectation based on the options provided by the caller.
        /// </summary>
        /// <remarks>
        /// If the expectation is a nullable type, it should return the type of the wrapped object.
        /// </remarks>
        public Type GetExpectedType(IEquivalencyAssertionOptions options)
        {
            Type type = options.UseRuntimeTyping ? RuntimeType : CompileTimeType;

            return type.NullableOrActualType();
        }

        public override string ToString()
        {
            return Invariant($"{{Subject={Subject}, Expectation={Expectation}}}");
        }
    }
}
