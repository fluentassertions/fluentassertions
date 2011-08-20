using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable numeric value has the expected value.
    /// </summary>
    [DebuggerNonUserCode]
    public static class NullableNumericExtensions
    {
        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        public static AndConstraint<NumericAssertions<T?>> HaveValue<T>(this NumericAssertions<T?> parent) where T : struct
        {
            return HaveValue(parent, String.Empty);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>      
        public static AndConstraint<NumericAssertions<T?>> HaveValue<T>(this NumericAssertions<T?> parent, string reason,
            params object [] reasonArgs) where T : struct
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(parent.Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{reason}.");

            return new AndConstraint<NumericAssertions<T?>>(parent);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        public static AndConstraint<NumericAssertions<T?>> NotHaveValue<T>(this NumericAssertions<T?> parent) where T : struct
        {
            return NotHaveValue(parent, String.Empty);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>  
        public static AndConstraint<NumericAssertions<T?>> NotHaveValue<T>(this NumericAssertions<T?> parent, string reason,
            params object [] reasonArgs) where T : struct
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(parent.Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{reason}, but found {0}.", parent.Subject);

            return new AndConstraint<NumericAssertions<T?>>(parent);
        }
    }
}