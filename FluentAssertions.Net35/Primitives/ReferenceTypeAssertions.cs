using System;
using System.Diagnostics;
using System.Linq.Expressions;

using FluentAssertions.Execution;
#if WINRT
using System.Reflection;

#endif

namespace FluentAssertions.Primitives
{
    /// <summary>
    /// Contains a number of methods to assert that a reference type object is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public abstract class ReferenceTypeAssertions<TSubject, TAssertions>
        where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions>
    {
        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TSubject Subject { get; protected set; }

        /// <summary>
        /// Asserts that the current dictionary has not been initialized yet with an actual dictionary.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeNull(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:" + Context + "} to be <null>{reason}, but found {0}.", Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current dictionary has been initialized with an actual dictionary.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeNull(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:" + Context + "} not to be <null>{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the object.</typeparam>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeOfType<T>(string reason = "", params object[] reasonArgs)
        {
            BeOfType(typeof(T), reason, reasonArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is of the specified type <paramref name="expectedType"/>.
        /// </summary>
        /// <param name="expectedType">
        /// The type that the subject is supposed to be of.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<TAssertions> BeOfType(Type expectedType, string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected type to be {0}{reason}, but found <null>.", expectedType);

            Subject.GetType().Should().Be(expectedType, reason, reasonArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
        /// <param name="reason">The reason why the object should be assignable to the type.</param>
        /// <param name="reasonArgs">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
        public AndConstraint<TAssertions> BeAssignableTo<T>(string reason = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(
#if !WINRT
                typeof(T).IsAssignableFrom(Subject.GetType())
#else
                typeof(T).GetTypeInfo().IsAssignableFrom(Subject.GetType().GetTypeInfo())
#endif
                )
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {context:" + Context + "} to be assignable to {0}{reason}, but {1} does not implement {0}",
                    typeof(T),
                    Subject.GetType());

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be statisfied by the <typeparamref name="TSubject" />.</param>
        /// <param name="reason">The reason why the predicate should be satisfied.</param>
        /// <param name="reasonArgs">The parameters used when formatting the <paramref name="reason" />.</param>
        /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
        public AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate,
            string reason = "",
            params object[] reasonArgs)
        {
            return Match<TSubject>(predicate, reason, reasonArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be statisfied by the <typeparamref name="TSubject" />.</param>
        /// <param name="reason">The reason why the predicate should be satisfied.</param>
        /// <param name="reasonArgs">The parameters used when formatting the <paramref name="reason" />.</param>
        /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
        public AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate,
            string reason = "",
            params object[] reasonArgs)
            where T : TSubject
        {
            if (predicate == null)
            {
                throw new NullReferenceException("Cannot match an object against a <null> predicate.");
            }

            Execute.Assertion
                .ForCondition(predicate.Compile()((T)Subject))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected {0} to match {1}{reason}.", Subject, predicate.Body);

            return new AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>>(this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected abstract string Context { get; }
    }
}