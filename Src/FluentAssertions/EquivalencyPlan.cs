#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Equivalency.Steps;

#endregion

namespace FluentAssertionsAsync;

/// <summary>
/// Represents a mutable collection of equivalency steps that can be reordered and/or amended with additional
/// custom equivalency steps.
/// </summary>
public class EquivalencyPlan : IEnumerable<IEquivalencyStep>
{
    private List<IEquivalencyStep> steps = GetDefaultSteps();

    public IEnumerator<IEquivalencyStep> GetEnumerator()
    {
        return steps.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Adds a new <see cref="IEquivalencyStep"/> after any of the built-in steps, with the exception of the final
    /// <see cref="SimpleEqualityEquivalencyStep"/>.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void Add<TStep>()
        where TStep : IEquivalencyStep, new()
    {
        InsertBefore<SimpleEqualityEquivalencyStep, TStep>();
    }

    /// <summary>
    /// Adds a new <see cref="IEquivalencyStep"/> right after the specified <typeparamref name="TPredecessor"/>.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void AddAfter<TPredecessor, TStep>()
        where TStep : IEquivalencyStep, new()
    {
        int insertIndex = Math.Max(steps.Count - 1, 0);

        IEquivalencyStep predecessor = steps.LastOrDefault(s => s is TPredecessor);

        if (predecessor is not null)
        {
            insertIndex = Math.Min(insertIndex, steps.LastIndexOf(predecessor) + 1);
        }

        steps.Insert(insertIndex, new TStep());
    }

    /// <summary>
    /// Inserts a new <see cref="IEquivalencyStep"/> before any of the built-in steps.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void Insert<TStep>()
        where TStep : IEquivalencyStep, new()
    {
        steps.Insert(0, new TStep());
    }

    /// <summary>
    /// Inserts a new <see cref="IEquivalencyStep"/> just before the <typeparamref name="TSuccessor"/>.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void InsertBefore<TSuccessor, TStep>()
        where TStep : IEquivalencyStep, new()
    {
        int insertIndex = Math.Max(steps.Count - 1, 0);

        IEquivalencyStep equalityStep = steps.LastOrDefault(s => s is TSuccessor);

        if (equalityStep is not null)
        {
            insertIndex = steps.LastIndexOf(equalityStep);
        }

        steps.Insert(insertIndex, new TStep());
    }

    /// <summary>
    /// Removes all instances of the specified <typeparamref name="TStep"/> from the current step.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void Remove<TStep>()
        where TStep : IEquivalencyStep
    {
        steps.RemoveAll(s => s is TStep);
    }

    /// <summary>
    /// Removes each and every built-in <see cref="IEquivalencyStep"/>.
    /// </summary>
    /// <remarks>
    /// This method is not thread-safe and should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void Clear()
    {
        steps.Clear();
    }

    /// <summary>
    /// Removes all custom <see cref="IEquivalencyStep"/>s.
    /// </summary>
    /// <remarks>
    /// This method should not be invoked on <see cref="AssertionOptions.EquivalencyPlan"/> from within a unit test.
    /// See the <see href="https://fluentassertions.com/extensibility/#thread-safety">docs</see> on how to safely use it.
    /// </remarks>
    public void Reset()
    {
        steps = GetDefaultSteps();
    }

    private static List<IEquivalencyStep> GetDefaultSteps()
    {
        return new List<IEquivalencyStep>(18)
        {
            new RunAllUserStepsEquivalencyStep(),
            new AutoConversionStep(),
            new ReferenceEqualityEquivalencyStep(),
            new GenericDictionaryEquivalencyStep(),
            new XDocumentEquivalencyStep(),
            new XElementEquivalencyStep(),
            new XAttributeEquivalencyStep(),
            new DictionaryEquivalencyStep(),
            new MultiDimensionalArrayEquivalencyStep(),
            new GenericEnumerableEquivalencyStep(),
            new EnumerableEquivalencyStep(),
            new AsyncEnumerableEquivalencyStep(),
            new StringEqualityEquivalencyStep(),
            new EnumEqualityStep(),
            new ValueTypeEquivalencyStep(),
            new StructuralEqualityEquivalencyStep(),
            new SimpleEqualityEquivalencyStep(),
        };
    }
}
