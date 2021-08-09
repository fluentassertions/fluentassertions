using System;
using System.Globalization;
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
                .FailWith("Expected the enum to be {0}{reason}, but found {1}.",
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
                .FailWith("Expected the enum not to be {0}{reason}, but it is.", unexpected);

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
                .FailWith("Expected the enum to not have flag {0}{reason}.", unexpectedFlag);

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
    }
}
