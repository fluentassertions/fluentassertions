using System;
using System.Diagnostics;
using System.Linq.Expressions;

using FluentAssertions.Execution;

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
        /// Asserts that the current object has not been initialized yet.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:" + Context + "} to be <null>{reason}, but found {0}.", Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the current object has been initialized.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeNull(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:" + Context + "} not to be <null>{reason}.");

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<TAssertions> BeSameAs(TSubject expected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .UsingLineBreaks
                .ForCondition(ReferenceEquals(Subject, expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:" + Context + "} to refer to {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name="unexpected">The unexpected object</param>
        /// <param name="because">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<TAssertions> NotBeSameAs(TSubject unexpected, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .UsingLineBreaks
                .ForCondition(!ReferenceEquals(Subject, unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Did not expect reference to object {0}{reason}.", unexpected);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The expected type of the object.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<TAssertions, T> BeOfType<T>(string because = "", params object[] becauseArgs)
        {
            BeOfType(typeof(T), because, becauseArgs);

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, (T)(object)Subject);
        }

        /// <summary>
        /// Asserts that the object is of the specified type <paramref name="expectedType"/>.
        /// </summary>
        /// <param name="expectedType">
        /// The type that the subject is supposed to be of.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeOfType(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:type} to be {0}{reason}, but found <null>.", expectedType);

            Type subjectType = Subject.GetType(); 
            if (expectedType.IsGenericTypeDefinition() && subjectType.IsGenericType())
            {
                subjectType.GetGenericTypeDefinition().Should().Be(expectedType, because, becauseArgs);
            } 
            else
            {
                subjectType.Should().Be(expectedType, because, becauseArgs);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is not of the specified type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type that the subject is not supposed to be of.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeOfType<T>(string because = "", params object[] becauseArgs)
        {
            NotBeOfType(typeof(T), because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is not of the specified type <paramref name="expectedType"/>.
        /// </summary>
        /// <param name="expectedType">
        /// The type that the subject is not supposed to be of.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeOfType(Type expectedType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:type} not to be {0}{reason}, but found <null>.", expectedType);

            Subject.GetType().Should().NotBe(expectedType, because, becauseArgs);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
        /// <param name="because">The reason why the object should be assignable to the type.</param>
        /// <param name="becauseArgs">The parameters used when formatting the <paramref name="because"/>.</param>
        /// <returns>An <see cref="AndWhichConstraint{TAssertions, T}"/> which can be used to chain assertions.</returns>
        public AndWhichConstraint<TAssertions, T> BeAssignableTo<T>(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject is T)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:" + Context + "} to be assignable to {0}{reason}, but {1} is not",
                    typeof(T),
                    Subject.GetType());

            return new AndWhichConstraint<TAssertions, T>((TAssertions)this, (T)((object)Subject));
        }

        /// <summary>
        /// Asserts that the object is assignable to a variable of given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to which the object should be assignable.</param>
        /// <param name="because">The parameters used when formatting the <paramref name="because"/>.</param>
        /// <param name="becauseArgs"></param>
        /// <returns>An <see cref="AndWhichConstraint{TAssertions, T}"/> which can be used to chain assertions.</returns>
        public AndConstraint<TAssertions> BeAssignableTo(Type type, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:type} not to be {0}{reason}, but found <null>.", type);

            Execute.Assertion
                .ForCondition(type.IsAssignableFrom(Subject.GetType()))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:" + Context + "} to be assignable to {0}{reason}, but {1} is not",
                    type,
                    Subject.GetType());

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be satisfied by the <typeparamref name="TSubject" />.</param>
        /// <param name="because">The reason why the predicate should be satisfied.</param>
        /// <param name="becauseArgs">The parameters used when formatting the <paramref name="because" />.</param>
        /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
        public AndConstraint<TAssertions> Match(Expression<Func<TSubject, bool>> predicate,
            string because = "",
            params object[] becauseArgs)
        {
            return Match<TSubject>(predicate, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the <paramref name="predicate" /> is satisfied.
        /// </summary>
        /// <param name="predicate">The predicate which must be satisfied by the <typeparamref name="TSubject" />.</param>
        /// <param name="because">The reason why the predicate should be satisfied.</param>
        /// <param name="becauseArgs">The parameters used when formatting the <paramref name="because" />.</param>
        /// <returns>An <see cref="AndConstraint{T}" /> which can be used to chain assertions.</returns>
        public AndConstraint<TAssertions> Match<T>(Expression<Func<T, bool>> predicate,
            string because = "",
            params object[] becauseArgs)
            where T : TSubject
        {
            if (predicate == null)
            {
                throw new NullReferenceException("Cannot match an object against a <null> predicate.");
            }

            Execute.Assertion
                .ForCondition(predicate.Compile()((T)Subject))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to match {1}{reason}.", Subject, predicate.Body);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected abstract string Context { get; }
    }
}