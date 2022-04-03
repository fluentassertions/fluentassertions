using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a <typeparamref name="TEnum"/> is in the expected state.
    /// </summary>
    public class EnumAssertions<TEnum> : EnumAssertions<TEnum, EnumAssertions<TEnum>>
        where TEnum : struct, Enum
    {
        public EnumAssertions(TEnum subject)
            : base(subject)
        {
        }
    }

#pragma warning disable CS0659 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
    /// <summary>
    /// Contains a number of methods to assert that a <typeparamref name="TEnum"/> is in the expected state.
    /// </summary>
    public class EnumAssertions<TEnum, TAssertions>
        where TEnum : struct, Enum
        where TAssertions : EnumAssertions<TEnum, TAssertions>
    {
        public EnumAssertions(TEnum subject)
            : this((TEnum?)subject)
        {
        }

        private protected EnumAssertions(TEnum? value)
        {
            Subject = value;
        }

        public TEnum? Subject { get; }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Be(TEnum expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject?.Equals(expected) == true)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to be {0}{reason}, but found {1}.",
                    expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> Be(TEnum? expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Nullable.Equals(Subject, expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to be {0}{reason}, but found {1}.",
                    expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> or <typeparamref name="TEnum"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBe(TEnum unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject?.Equals(unexpected) != true)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} not to be {0}{reason}, but it is.", unexpected);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> or <typeparamref name="TEnum"/> is not equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBe(TEnum? unexpected, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Nullable.Equals(Subject, unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} not to be {0}{reason}, but it is.", unexpected);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> is exactly equal to the <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveValue(decimal expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (GetValue(Subject.Value) == expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to have value {0}{reason}, but found {1}.",
                    expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> is exactly equal to the <paramref name="unexpected"/> value.
        /// </summary>
        /// <param name="unexpected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveValue(decimal unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!(Subject.HasValue && (GetValue(Subject.Value) == unexpected)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to not have value {0}{reason}, but found {1}.",
                    unexpected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> has the same numeric value as <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveSameValueAs<T>(T expected, string because = "", params object[] becauseArgs)
            where T : struct, Enum
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (GetValue(Subject.Value) == GetValue(expected)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to have same value as {0}{reason}, but found {1}.",
                    expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> does not have the same numeric value as <paramref name="unexpected"/>.
        /// </summary>
        /// <param name="unexpected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveSameValueAs<T>(T unexpected, string because = "", params object[] becauseArgs)
            where T : struct, Enum
        {
            Execute.Assertion
                .ForCondition(!(Subject.HasValue && (GetValue(Subject.Value) == GetValue(unexpected))))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to not have same value as {0}{reason}, but found {1}.",
                    unexpected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> has the same name as <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveSameNameAs<T>(T expected, string because = "", params object[] becauseArgs)
            where T : struct, Enum
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue && (GetName(Subject.Value) == GetName(expected)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to have same name as {0}{reason}, but found {1}.",
                    expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current <typeparamref name="TEnum"/> does not have the same name as <paramref name="unexpected"/>.
        /// </summary>
        /// <param name="unexpected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveSameNameAs<T>(T unexpected, string because = "", params object[] becauseArgs)
            where T : struct, Enum
        {
            Execute.Assertion
                .ForCondition(!(Subject.HasValue && (GetName(Subject.Value) == GetName(unexpected))))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to not have same name as {0}{reason}, but found {1}.",
                    unexpected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that an enum has a specified flag
        /// </summary>
        /// <param name="expectedFlag">The expected flag.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> HaveFlag(TEnum expectedFlag, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject?.HasFlag(expectedFlag) == true)
                .FailWith("Expected {context:the enum} to have flag {0}{reason}, but found {1}.", expectedFlag, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that an enum does not have a specified flag
        /// </summary>
        /// <param name="unexpectedFlag">The unexpected flag.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public AndConstraint<TAssertions> NotHaveFlag(TEnum unexpectedFlag, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject?.HasFlag(unexpectedFlag) != true)
                .FailWith("Expected {context:the enum} to not have flag {0}{reason}.", unexpectedFlag);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the <see cref="Enum"/> matches the <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">
        /// The predicate which must be satisfied by the <typeparamref name="TEnum" />.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <c>null</c>.</exception>
        public AndConstraint<TAssertions> Match(Expression<Func<TEnum?, bool>> predicate,
            string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(predicate, nameof(predicate), "Cannot match an enum against a <null> predicate.");

            Execute.Assertion
                .ForCondition(predicate.Compile()(Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to match {1}{reason}, but found {0}.", Subject, predicate.Body);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the <see cref="Enum"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        public AndConstraint<TAssertions> BeOneOf(params TEnum[] validValues)
        {
            return BeOneOf(validValues, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="Enum"/> is one of the specified <paramref name="validValues"/>.
        /// </summary>
        /// <param name="validValues">
        /// The values that are valid.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeOneOf(IEnumerable<TEnum> validValues, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(validValues, nameof(validValues), "Cannot assert that an enum is one of a null list of enums");
            Guard.ThrowIfArgumentIsEmpty(validValues, nameof(validValues), "Cannot assert that an enum is one of an empty list of enums");

            Execute.Assertion
                .ForCondition(Subject is not null)
                .FailWith("Expected {context:the enum} to be one of {0}{reason}, but found <null>", validValues)
                .Then
                .ForCondition(validValues.Contains(Subject.Value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:the enum} to be one of {0}{reason}, but found {1}.", validValues, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private static decimal GetValue<T>(T @enum)
            where T : struct, Enum
        {
            return Convert.ToDecimal(@enum, CultureInfo.InvariantCulture);
        }

        private static string GetName<T>(T @enum)
            where T : struct, Enum
        {
            return @enum.ToString();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) =>
            throw new NotSupportedException("Calling Equals on Assertion classes is not supported.");
    }
}
