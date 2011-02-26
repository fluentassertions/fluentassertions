using System;
using System.Linq.Expressions;

namespace FluentAssertions.Assertions
{
    public abstract class ReferenceTypeAssertions<TSubject, TAssertions> where TAssertions : ReferenceTypeAssertions<TSubject, TAssertions>
    {
        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public TSubject Subject
        {
            get; protected set;
        }

        public AndConstraint<TAssertions> BeOfType<T>()
        {
            return BeOfType<T>(String.Empty);
        }

        public AndConstraint<TAssertions> BeOfType<T>(string reason, params object[] reasonParameters)
        {
            Execute.Verify(typeof(T) == Subject.GetType(),
                "Expected type {0}{2}, but found {1}.", typeof(T), Subject.GetType(), reason,
                reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
        /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
        public AndConstraint<TAssertions> BeAssignableTo<T>()
        {
            return BeAssignableTo<T>(String.Empty);
        }

        /// <summary>
        /// Asserts that the object is assignable to a variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which the object should be assignable.</typeparam>
        /// <param name="reason">The reason why the object should be assignable to the type.</param>
        /// <param name="reasonParameters">The parameters used when formatting the <paramref name="reason"/>.</param>
        /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
        public AndConstraint<TAssertions> BeAssignableTo<T>(string reason, params object[] reasonParameters)
        {
            Execute.Verify(() => typeof(T).IsAssignableFrom(Subject.GetType()),
                "Expected to be assignable to {0}{2}, but {1} does not implement {0}", typeof(T),
                Subject.GetType(), reason, reasonParameters);

            return new AndConstraint<TAssertions>((TAssertions) this);
        }

        /// <summary>
        ///   Asserts that the <paramref name = "predicate" /> is statisfied.
        /// </summary>
        /// <param name = "predicate">The predicate which must be satisfied by the <typeparamref name = "TSubject" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate)
        {
            return Match<TSubject>(predicate, String.Empty);
        }

        /// <summary>
        ///   Asserts that the <paramref name = "predicate" /> is satisfied.
        /// </summary>
        /// <param name = "predicate">The predicate which must be statisfied by the <typeparamref name = "TSubject" />.</param>
        /// <param name = "reason">The reason why the predicate should be satisfied.</param>
        /// <param name = "reasonParameters">The parameters used when formatting the <paramref name = "reason" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate, string reason,
            params object[] reasonParameters)
        {
            return Match<TSubject>(predicate, reason, reasonParameters);
        }

        /// <summary>
        ///   Asserts that the <paramref name = "predicate" /> is satisfied.
        /// </summary>
        /// <param name = "predicate">The predicate which must be statisfied by the <typeparamref name = "TSubject" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate)
            where T : TSubject
        {
            return Match(predicate, string.Empty);
        }

        /// <summary>
        ///   Asserts that the <paramref name = "predicate" /> is satisfied.
        /// </summary>
        /// <param name = "predicate">The predicate which must be statisfied by the <typeparamref name = "TSubject" />.</param>
        /// <param name = "reason">The reason why the predicate should be satisfied.</param>
        /// <param name = "reasonParameters">The parameters used when formatting the <paramref name = "reason" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate, string reason,
            params object[] reasonParameters)
            where T : TSubject
        {
            if (predicate == null)
            {
                throw new NullReferenceException("Cannot match an object against a <null> predicate.");
            }

            Execute.Verify(() => predicate.Compile()((T) Subject),
                "Expected {1} to match {0}{2}.",
                predicate.Body, Subject, reason, reasonParameters);

            return new AndConstraint<ReferenceTypeAssertions<TSubject, TAssertions>>(this);
        }
    }
}