using System;
using System.Diagnostics;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class ComparableTypeAssertions<T>
    {
        protected internal ComparableTypeAssertions(IComparable<T> value)
        {
            Subject = value;
        }

        public IComparable<T> Subject { get; private set; }

        /// <summary>
        /// Asserts that the subject is considered equal to another object according to the implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeEqualTo(T expected)
        {
            return BeEqualTo(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that the subject is considered equal to another object according to the implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeEqualTo(T expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, expected) || (Subject.CompareTo(expected) == 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {1}{0}, but found {2}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        public AndConstraint<ComparableTypeAssertions<T>> NotBe(T expected)
        {
            return NotBe(expected, String.Empty);
        }

        public AndConstraint<ComparableTypeAssertions<T>> NotBe(T expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) != 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect {1}{0}.", expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeLessThan(T expected)
        {
            return BeLessThan(expected, String.Empty);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeLessThan(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) < 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less than {1}{0}, but found {2}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeLessOrEqualTo(T expected)
        {
            return BeLessOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeLessOrEqualTo(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) <= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value less or equal to {1}{0}, but found {2}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterThan(T expected)
        {
            return BeGreaterThan(expected, String.Empty);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterThan(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) > 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater than {1}{0}, but found {2}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterOrEqualTo(T expected)
        {
            return BeGreaterOrEqualTo(expected, String.Empty);
        }

        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterOrEqualTo(T expected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.CompareTo(expected) >= 0)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value greater or equal to {1}{0}, but found {2}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a value is within a range.
        /// </summary>
        /// <remarks>
        /// Where the range is continuous or incremental depends on the actual type of the value. 
        /// </remarks>
        /// <param name="minimumValue">
        /// The minimum valid value of the range.
        /// </param>
        /// <param name="maximumValue">
        /// The maximum valid value of the range.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeInRange(T minimumValue, T maximumValue)
        {
            return BeInRange(minimumValue, maximumValue, "");
        }

        /// <summary>
        /// Asserts that a value is within a range.
        /// </summary>
        /// <remarks>
        /// Where the range is continuous or incremental depends on the actual type of the value. 
        /// </remarks>
        /// <param name="minimumValue">
        /// The minimum valid value of the range.
        /// </param>
        /// <param name="maximumValue">
        /// The maximum valid value of the range.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeInRange(T minimumValue, T maximumValue, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition((Subject.CompareTo(minimumValue) >= 0) && (Subject.CompareTo(maximumValue) <= 0))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected value {1} to be between {2} and {3}{0}, but it was not.",
                    Subject, minimumValue, maximumValue);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is not <c>null</c>.
        /// </summary>
        public AndConstraint<ComparableTypeAssertions<T>> HaveValue()
        {
            return HaveValue(String.Empty);
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
        public AndConstraint<ComparableTypeAssertions<T>> HaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected a value{0}.");

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a nullable numeric value is <c>null</c>.
        /// </summary>
        public AndConstraint<ComparableTypeAssertions<T>> NotHaveValue()
        {
            return NotHaveValue(String.Empty);
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
        public AndConstraint<ComparableTypeAssertions<T>> NotHaveValue(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect a value{0}, but found {1}.", Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }
    }
}