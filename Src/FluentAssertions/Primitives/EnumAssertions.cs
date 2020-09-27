using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Enum"/> is in the expected state.
    /// </summary>
    public class EnumAssertions : EnumAssertions<Enum, EnumAssertions>
    {
        public EnumAssertions(Enum subject)
            : base(subject)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <typeparamref name="TEnum"/> is in the expected state.
    /// </summary>
    public class EnumAssertions<TEnum, TAssertions> : ObjectAssertions<TEnum, TAssertions>
        where TEnum : Enum
        where TAssertions : EnumAssertions<TEnum, TAssertions>
    {
        protected override string Identifier => "enum";

        public EnumAssertions(TEnum subject)
            : base(subject)
        {
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
        public AndConstraint<TAssertions> HaveFlag<T>(T expectedFlag, string because = "",
            params object[] becauseArgs)
            where T : struct, TEnum
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!(Subject is null))
                .FailWith("Expected type to be {0}{reason}, but found <null>.", expectedFlag.GetType())
                .Then
                .ForCondition(Subject.GetType() == expectedFlag.GetType())
                .FailWith("Expected the enum to be of type {0} type but found {1}{reason}.", expectedFlag.GetType(), Subject.GetType())
                .Then
                .ForCondition(Subject.HasFlag(expectedFlag))
                .FailWith("The enum was expected to have flag {0} but found {1}{reason}.", expectedFlag, Subject);

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
        public AndConstraint<TAssertions> NotHaveFlag<T>(T unexpectedFlag, string because = "",
            params object[] becauseArgs)
            where T : struct, TEnum
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!(Subject is null))
                .FailWith("Expected type to be {0}{reason}, but found <null>.", unexpectedFlag.GetType())
                .Then
                .ForCondition(Subject.GetType() == unexpectedFlag.GetType())
                .FailWith("Expected the enum to be of type {0} type but found {1}{reason}.", unexpectedFlag.GetType(), Subject.GetType())
                .Then
                .ForCondition(!Subject.HasFlag(unexpectedFlag))
                .FailWith("Did not expect the enum to have flag {0}{reason}.", unexpectedFlag);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }
}
