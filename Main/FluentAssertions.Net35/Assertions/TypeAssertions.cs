using System;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Type"/> meets certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        protected internal TypeAssertions(Type type)
        {
            Subject = type;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public Type Subject { get; private set; }

        /// <summary>
        /// Asserts that the <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>()
        {
            return BeDecoratedWith<TAttribute>(string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(IsDecoratedWith<TAttribute>(Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected type {0} to be decorated with {1}{reason}, but the attribute was not found.",
                    Subject, typeof (TAttribute));

            return new AndConstraint<TypeAssertions>(this);
        }

        private static bool IsDecoratedWith<TAttribute>(Type type)
        {
            return type.GetCustomAttributes(false).OfType<TAttribute>().Any();
        }
    }
}