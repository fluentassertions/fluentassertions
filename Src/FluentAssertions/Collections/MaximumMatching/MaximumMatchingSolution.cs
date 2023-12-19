using System.Collections.Generic;
using System.Linq;

namespace FluentAssertionsAsync.Collections.MaximumMatching;

/// <summary>
/// The <see cref="MaximumMatchingSolution{TElement}"/> class defines the solution (output) for the maximum matching problem.
/// See documentation of <see cref="MaximumMatchingProblem{TElement}"/> for more details.
/// </summary>
/// <typeparam name="TValue">The type of elements which must be matched with predicates.</typeparam>
internal class MaximumMatchingSolution<TValue>
{
    private readonly Dictionary<Predicate<TValue>, Element<TValue>> elementsByMatchedPredicate;
    private readonly MaximumMatchingProblem<TValue> problem;

    public MaximumMatchingSolution(
        MaximumMatchingProblem<TValue> problem,
        Dictionary<Predicate<TValue>, Element<TValue>> elementsByMatchedPredicate)
    {
        this.problem = problem;
        this.elementsByMatchedPredicate = elementsByMatchedPredicate;
    }

    public bool UnmatchedPredicatesExist => problem.Predicates.Count != elementsByMatchedPredicate.Count;

    public bool UnmatchedElementsExist => problem.Elements.Count != elementsByMatchedPredicate.Count;

    public List<Predicate<TValue>> GetUnmatchedPredicates()
    {
        return problem.Predicates.Except(elementsByMatchedPredicate.Keys).ToList();
    }

    public List<Element<TValue>> GetUnmatchedElements()
    {
        return problem.Elements.Except(elementsByMatchedPredicate.Values).ToList();
    }
}
