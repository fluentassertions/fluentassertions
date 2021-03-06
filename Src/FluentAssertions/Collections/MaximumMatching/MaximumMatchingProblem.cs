using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAssertions.Collections.MaximumMatching
{
    /// <summary>
    /// The <see cref="MaximumMatchingProblem{TElement}"/> class defines input for the maximum matching problem.
    /// The input is a list of predicates and a list of elements.
    /// The goal of the problem is to find such mapping between predicates and elements that would maximize number of matches.
    /// A predicate can be mapped with only one element.
    /// An element can be mapped with only one predicate.
    /// </summary>
    /// <typeparam name="TValue">The type of elements which must be matched with predicates.</typeparam>
    internal class MaximumMatchingProblem<TValue>
    {
        public MaximumMatchingProblem(
            IEnumerable<Expression<Func<TValue, bool>>> predicates,
            IEnumerable<TValue> elements)
        {
            Predicates.AddRange(predicates.Select((predicate, index) => new Predicate<TValue>(predicate, index)));
            Elements.AddRange(elements.Select((element, index) => new Element<TValue>(element, index)));
        }

        public List<Predicate<TValue>> Predicates { get; } = new();

        public List<Element<TValue>> Elements { get; } = new();

        public MaximumMatchingSolution<TValue> Solve() => new MaximumMatchingSolver<TValue>(this).Solve();
    }
}
