using System;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a nullable <typeparamref name="TEnum"/> is in the expected state.
    /// </summary>
    public class NullableEnumAssertions<TEnum> : NullableEnumAssertions<TEnum, NullableEnumAssertions<TEnum>>
        where TEnum : struct, Enum
    {
        public NullableEnumAssertions(TEnum? subject)
            : base(subject)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a nullable <typeparamref name="TEnum"/> is in the expected state.
    /// </summary>
    public class NullableEnumAssertions<TEnum, TAssertions> : EnumAssertions<TEnum, TAssertions>
        where TEnum : struct, Enum
        where TAssertions : NullableEnumAssertions<TEnum, TAssertions>
    {
        public NullableEnumAssertions(TEnum? subject)
            : base(subject)
        {
        }

        /// <summary>
        /// Asserts that a nullable <typeparamref name="TEnum"/> value is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, TEnum> HaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:nullable enum} to have a value{reason}, but found {0}.", Subject);

            return new AndWhichConstraint<TAssertions, TEnum>((TAssertions)this, Subject.GetValueOrDefault());
        }

        /// <summary>
        /// Asserts that a nullable <typeparamref name="TEnum"/> is not <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, TEnum> NotBeNull(string because = "", params object[] becauseArgs)
        {
            return HaveValue(because, becauseArgs);
        }

        /// <summary>
        /// Asserts that a nullable <typeparamref name="TEnum"/> value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveValue(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.HasValue)
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:nullable enum} to have a value{reason}, but found {0}.", Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that a nullable <typeparamref name="TEnum"/> value is <c>null</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeNull(string because = "", params object[] becauseArgs)
        {
            return NotHaveValue(because, becauseArgs);
        }
    }
}
