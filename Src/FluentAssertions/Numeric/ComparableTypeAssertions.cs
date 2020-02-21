using System;
using System.Diagnostics;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Numeric
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="IComparable{T}"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ComparableTypeAssertions<T> : ReferenceTypeAssertions<IComparable<T>, ComparableTypeAssertions<T>>
    {
        private const int Equal = 0;

        public ComparableTypeAssertions(IComparable<T> value) : base(value)
        {
        }

        /// <summary>
        /// Asserts that an object equals another object using its <see cref="object.Equals(object)" /> implementation.<br/>
        /// Verification whether <see cref="IComparable{T}.CompareTo(T)"/> returns 0 is not done here, you should use
        /// <see cref="BeRankedEquallyTo(T, string, object[])"/> to verify this. 
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> Be(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.IsSameOrEqualTo(expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} to be equal to {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value,
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
        /// items in the collection are structurally equal.
        /// Notice that actual behavior is determined by the global defaults managed by <see cref="AssertionOptions"/>.
        /// </remarks>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeEquivalentTo<TExpectation>(TExpectation expectation, string because = "",
            params object[] becauseArgs)
        {
            return BeEquivalentTo(expectation, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that an object is equivalent to another object.
        /// </summary>
        /// <remarks>
        /// Objects are equivalent when both object graphs have equally named properties with the same value,
        /// irrespective of the type of those objects. Two properties are also equal if one type can be converted to another and the result is equal.
        /// The type of a collection property is ignored as long as the collection implements <see cref="IEnumerable{T}"/> and all
        /// items in the collection are structurally equal.
        /// </remarks>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// An optional formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the
        /// assertion is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeEquivalentTo<TExpectation>(TExpectation expectation,
            Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(config, nameof(config));

            EquivalencyAssertionOptions<TExpectation> options = config(AssertionOptions.CloneDefaults<TExpectation>());

            var context = new EquivalencyValidationContext
            {
                Subject = Subject,
                Expectation = expectation,
                CompileTimeType = typeof(TExpectation),
                Because = because,
                BecauseArgs = becauseArgs,
                Tracer = options.TraceWriter
            };

            var equivalencyValidator = new EquivalencyValidator(options);
            equivalencyValidator.AssertEquality(context);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using its <see cref="object.Equals(object)" /> method.<br/>
        /// Verification whether <see cref="IComparable{T}.CompareTo(T)"/> returns non-zero is not done here, you should use
        /// <see cref="NotBeRankedEquallyTo(T, string, object[])"/> to verify this.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> NotBe(T unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.IsSameOrEqualTo(unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect {context:object} to be equal to {0}{reason}.", unexpected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is ranked equal to another object. I.e. the result of <see cref="IComparable{T}.CompareTo"/> returns 0.
        /// To verify whether the objects are equal you must use <see cref="Be(T, string, object[])"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeRankedEquallyTo(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CompareTo(expected) == Equal)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} {0} to be ranked as equal to {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is not ranked equal to another object. I.e. the result of <see cref="IComparable{T}.CompareTo"/>returns non-zero.
        /// To verify whether the objects are not equal according to <see cref="object.Equals(object)"/> you must use <see cref="NotBe(T, string, object[])"/>.
        /// </summary>
        /// <param name="unexpected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> NotBeRankedEquallyTo(T unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CompareTo(unexpected) != Equal)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} {0} not to be ranked as equal to {1}{reason}.", Subject, unexpected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is less than another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeLessThan(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CompareTo(expected) < Equal)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} {0} to be less than {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is less than or equal to another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeLessOrEqualTo(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CompareTo(expected) <= Equal)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} {0} to be less or equal to {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is greater than another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterThan(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CompareTo(expected) > Equal)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} {0} to be greater than {1}{reason}.", Subject, expected);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that the subject is greater than or equal to another object according to its implementation of <see cref="IComparable{T}"/>.
        /// </summary>
        /// <param name="expected">
        /// The object to pass to the subject's <see cref="IComparable{T}.CompareTo"/> method.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeGreaterOrEqualTo(T expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CompareTo(expected) >= Equal)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} {0} to be greater or equal to {1}{reason}.", Subject, expected);

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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> BeInRange(T minimumValue, T maximumValue, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition((Subject.CompareTo(minimumValue) >= Equal) && (Subject.CompareTo(maximumValue) <= Equal))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} to be between {0} and {1}{reason}, but found {2}.",
                    minimumValue, maximumValue, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Asserts that a value is not within a range.
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
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </param>
        public AndConstraint<ComparableTypeAssertions<T>> NotBeInRange(T minimumValue, T maximumValue, string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!((Subject.CompareTo(minimumValue) >= Equal) && (Subject.CompareTo(maximumValue) <= Equal)))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:object} to not be between {0} and {1}{reason}, but found {2}.",
                    minimumValue, maximumValue, Subject);

            return new AndConstraint<ComparableTypeAssertions<T>>(this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "object";
    }
}
