using System;
using System.Linq.Expressions;

namespace FluentAssertions
{
    public abstract class Assertions<TSubject, TAssertions>
        where TAssertions : Assertions<TSubject, TAssertions>
    {
        protected TSubject Subject
        {
            get; set;
        }

        /// <summary>
        ///   Asserts that the <paramref name = "predicate" /> is statisfied.
        /// </summary>
        /// <param name = "predicate">The predicate which must be satisfied by the <typeparamref name = "TSubject" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public AndConstraint<Assertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate)
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
        public AndConstraint<Assertions<TSubject, TAssertions>> Match(Expression<Func<TSubject, bool>> predicate, string reason,
            params object[] reasonParameters)
        {
            return Match<TSubject>(predicate, reason, reasonParameters);
        }

        /// <summary>
        ///   Asserts that the <paramref name = "predicate" /> is satisfied.
        /// </summary>
        /// <param name = "predicate">The predicate which must be statisfied by the <typeparamref name = "TSubject" />.</param>
        /// <returns>An <see cref = "AndConstraint" /> which can be used to chain assertions.</returns>
        public AndConstraint<Assertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate)
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
        public AndConstraint<Assertions<TSubject, TAssertions>> Match<T>(Expression<Func<T, bool>> predicate, string reason,
            params object[] reasonParameters)
            where T : TSubject
        {
            if (predicate == null)
            {
                throw new NullReferenceException("Cannot match an object against a <null> predicate.");
            }

            Verification.Verify(() => predicate.Compile()((T) Subject),
                "Expected {1} to match {0}{2}.",
                predicate.Body, Subject, reason, reasonParameters);

            return new AndConstraint<Assertions<TSubject, TAssertions>>(this);
        }
    }
}