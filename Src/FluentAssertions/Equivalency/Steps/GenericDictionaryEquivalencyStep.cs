using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class GenericDictionaryEquivalencyStep : IEquivalencyStep
{
#pragma warning disable SA1110 // Allow opening parenthesis on new line to reduce line length
    private static readonly MethodInfo AssertDictionaryEquivalenceMethod =
        new Func<EquivalencyValidationContext, IEquivalencyValidator, IEquivalencyOptions,
                IDictionary<object, object>, IDictionary<object, object>, Task>
            (AssertDictionaryEquivalenceAsync).GetMethodInfo().GetGenericMethodDefinition();
#pragma warning restore SA1110

    public async Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (comparands.Expectation is null)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        Type expectationType = comparands.GetExpectedType(context.Options);

        if (DictionaryInterfaceInfo.FindFrom(expectationType, "expectation") is not { } expectedDictionary)
        {
            return EquivalencyResult.ContinueWithNext;
        }

        if (IsNonGenericDictionary(comparands.Subject))
        {
            // Because we handle non-generic dictionaries later
            return EquivalencyResult.ContinueWithNext;
        }

        if (IsNotNull(comparands.Subject)
            && EnsureSubjectIsOfTheExpectedDictionaryType(comparands, expectedDictionary) is { } actualDictionary)
        {
            await AssertDictionaryEquivalenceAsync(comparands, context, nestedValidator, actualDictionary, expectedDictionary);
        }

        return EquivalencyResult.AssertionCompleted;
    }

    private static bool IsNonGenericDictionary(object subject)
    {
        if (subject is not IDictionary)
        {
            return false;
        }

        return !subject.GetType().GetInterfaces().Any(@interface =>
            @interface.IsGenericType && @interface.GetGenericTypeDefinition() == typeof(IDictionary<,>));
    }

    private static bool IsNotNull(object subject)
    {
        return AssertionScope.Current
            .ForCondition(subject is not null)
            .FailWith("Expected {context:Subject} not to be {0}{reason}.", new object[] { null });
    }

    private static DictionaryInterfaceInfo EnsureSubjectIsOfTheExpectedDictionaryType(Comparands comparands,
        DictionaryInterfaceInfo expectedDictionary)
    {
        var actualDictionary = DictionaryInterfaceInfo.FindFromWithKey(comparands.Subject.GetType(), "subject",
            expectedDictionary.Key);

        if (actualDictionary is null && expectedDictionary.ConvertFrom(comparands.Subject) is { } convertedSubject)
        {
            comparands.Subject = convertedSubject;
            actualDictionary = DictionaryInterfaceInfo.FindFrom(comparands.Subject.GetType(), "subject");
        }

        if (actualDictionary is null)
        {
            AssertionScope.Current.FailWith(
                "Expected {context:subject} to be a dictionary or collection of key-value pairs that is keyed to " +
                $"type {expectedDictionary.Key}.");
        }

        return actualDictionary;
    }

    private static void FailWithLengthDifference<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            IDictionary<TSubjectKey, TSubjectValue> subject, IDictionary<TExpectedKey, TExpectedValue> expectation)

        // Type constraint of TExpectedKey is asymmetric in regards to TSubjectKey
        // but it is valid. This constraint is implicitly enforced by the dictionary interface info which is called before
        // the AssertSameLength method.
        where TExpectedKey : TSubjectKey
    {
        KeyDifference<TSubjectKey, TExpectedKey> keyDifference = CalculateKeyDifference(subject, expectation);

        bool hasMissingKeys = keyDifference.MissingKeys.Count > 0;
        bool hasAdditionalKeys = keyDifference.AdditionalKeys.Count > 0;

        Execute.Assertion
            .WithExpectation("Expected {context:subject} to be a dictionary with {0} item(s){reason}, ", expectation.Count)
            .ForCondition(!hasMissingKeys || hasAdditionalKeys)
            .FailWith("but it misses key(s) {0}", keyDifference.MissingKeys)
            .Then
            .ForCondition(hasMissingKeys || !hasAdditionalKeys)
            .FailWith("but has additional key(s) {0}", keyDifference.AdditionalKeys)
            .Then
            .ForCondition(!hasMissingKeys || !hasAdditionalKeys)
            .FailWith("but it misses key(s) {0} and has additional key(s) {1}", keyDifference.MissingKeys,
                keyDifference.AdditionalKeys)
            .Then
            .ClearExpectation();
    }

    private static KeyDifference<TSubjectKey, TExpectedKey> CalculateKeyDifference<TSubjectKey, TSubjectValue, TExpectedKey,
        TExpectedValue>(IDictionary<TSubjectKey, TSubjectValue> subject,
        IDictionary<TExpectedKey, TExpectedValue> expectation)
        where TExpectedKey : TSubjectKey
    {
        var missingKeys = new List<TExpectedKey>();
        var presentKeys = new HashSet<TSubjectKey>();

        foreach (TExpectedKey expectationKey in expectation.Keys)
        {
            if (subject.ContainsKey(expectationKey))
            {
                presentKeys.Add(expectationKey);
            }
            else
            {
                missingKeys.Add(expectationKey);
            }
        }

        var additionalKeys = new List<TSubjectKey>();

        foreach (TSubjectKey subjectKey in subject.Keys)
        {
            if (!presentKeys.Contains(subjectKey))
            {
                additionalKeys.Add(subjectKey);
            }
        }

        return new KeyDifference<TSubjectKey, TExpectedKey>(missingKeys, additionalKeys);
    }

    private static async Task AssertDictionaryEquivalenceAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator parent, DictionaryInterfaceInfo actualDictionary, DictionaryInterfaceInfo expectedDictionary)
    {
        await AssertDictionaryEquivalenceMethod
            .MakeGenericMethod(actualDictionary.Key, actualDictionary.Value, expectedDictionary.Key, expectedDictionary.Value)
            .InvokeAsync(null, new[] { context, parent, context.Options, comparands.Subject, comparands.Expectation });
    }

    private static async Task AssertDictionaryEquivalenceAsync<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
        EquivalencyValidationContext context,
        IEquivalencyValidator parent,
        IEquivalencyOptions options,
        IDictionary<TSubjectKey, TSubjectValue> subject,
        IDictionary<TExpectedKey, TExpectedValue> expectation)
        where TExpectedKey : TSubjectKey
    {
        if (subject.Count != expectation.Count)
        {
            FailWithLengthDifference(subject, expectation);
        }
        else
        {
            foreach (TExpectedKey key in expectation.Keys)
            {
                if (subject.TryGetValue(key, out TSubjectValue subjectValue))
                {
                    if (options.IsRecursive)
                    {
                        // Run the child assertion without affecting the current context
                        using (new AssertionScope())
                        {
                            var nestedComparands = new Comparands(subject[key], expectation[key], typeof(TExpectedValue));

                            await parent.RecursivelyAssertEqualityAsync(nestedComparands,
                                context.AsDictionaryItem<TExpectedKey, TExpectedValue>(key));
                        }
                    }
                    else
                    {
                        subjectValue.Should().Be(expectation[key], context.Reason.FormattedMessage, context.Reason.Arguments);
                    }
                }
                else
                {
                    AssertionScope.Current
                        .BecauseOf(context.Reason)
                        .FailWith("Expected {context:subject} to contain key {0}{reason}.", key);
                }
            }
        }
    }

    private sealed class KeyDifference<TSubjectKey, TExpectedKey>
    {
        public KeyDifference(List<TExpectedKey> missingKeys, List<TSubjectKey> additionalKeys)
        {
            MissingKeys = missingKeys;
            AdditionalKeys = additionalKeys;
        }

        public List<TExpectedKey> MissingKeys { get; }

        public List<TSubjectKey> AdditionalKeys { get; }
    }
}
