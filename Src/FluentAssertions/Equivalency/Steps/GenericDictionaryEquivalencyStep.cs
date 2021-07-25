using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
#pragma warning disable SA1110 // Allow opening parenthesis on new line to reduce line length
        private static readonly MethodInfo AssertSameLengthMethod =
            new Func<IDictionary<object, object>, IDictionary<object, object>, bool>(AssertSameLength).GetMethodInfo()
                .GetGenericMethodDefinition();

        private static readonly MethodInfo AssertDictionaryEquivalenceMethod =
            new Action<EquivalencyValidationContext, IEquivalencyValidator, IEquivalencyAssertionOptions,
                    IDictionary<object, object>, IDictionary<object, object>>
                (AssertDictionaryEquivalence).GetMethodInfo().GetGenericMethodDefinition();
#pragma warning restore SA1110

        public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            if (comparands.Expectation != null)
            {
                Type expectationType = comparands.GetExpectedType(context.Options);
                bool isDictionary = DictionaryInterfaceInfo.TryGetFrom(expectationType, "expectation", out var expectedDictionary);
                if (isDictionary)
                {
                    Handle(comparands, expectedDictionary, context, nestedValidator);

                    return EquivalencyResult.AssertionCompleted;
                }
            }

            return EquivalencyResult.ContinueWithNext;
        }

        private static void Handle(Comparands comparands, DictionaryInterfaceInfo expectedDictionary,
            IEquivalencyValidationContext context,
            IEquivalencyValidator nestedValidator)
        {
            if (AssertSubjectIsNotNull(comparands.Subject)
                && AssertExpectationIsNotNull(comparands.Subject, comparands.Expectation))
            {
                var (isDictionary, actualDictionary) = EnsureSubjectIsDictionary(comparands, expectedDictionary);
                if (isDictionary)
                {
                    if (AssertSameLength(comparands, actualDictionary, expectedDictionary))
                    {
                        AssertDictionaryEquivalence(comparands, context, nestedValidator, actualDictionary, expectedDictionary);
                    }
                }
            }
        }

        private static bool AssertSubjectIsNotNull(object subject)
        {
            return AssertionScope.Current
                .ForCondition(subject is not null)
                .FailWith("Expected {context:Subject} not to be {0}{reason}.", new object[] { null });
        }

        private static bool AssertExpectationIsNotNull(object subject, object expectation)
        {
            return AssertionScope.Current
                .ForCondition(expectation is not null)
                .FailWith("Expected {context:Subject} to be {0}{reason}, but found {1}.", null, subject);
        }

        private static (bool isDictionary, DictionaryInterfaceInfo info) EnsureSubjectIsDictionary(Comparands comparands,
            DictionaryInterfaceInfo expectedDictionary)
        {
            bool isDictionary = DictionaryInterfaceInfo.TryGetFromWithKey(comparands.Subject.GetType(), "subject",
                expectedDictionary.Key, out var actualDictionary);

            if (!isDictionary)
            {
                if (expectedDictionary.TryConvertFrom(comparands.Subject, out var convertedSubject))
                {
                    comparands.Subject = convertedSubject;
                    isDictionary = DictionaryInterfaceInfo.TryGetFrom(comparands.Subject.GetType(), "subject", out actualDictionary);
                }
            }

            if (!isDictionary)
            {
                AssertionScope.Current.FailWith(
                    $"Expected {{context:subject}} to be a dictionary or collection of key-value pairs that is keyed to type {expectedDictionary.Key}. " +
                    $"It implements {actualDictionary}.");
            }

            return (isDictionary, actualDictionary);
        }

        private static bool AssertSameLength(Comparands comparands, DictionaryInterfaceInfo actualDictionary,
            DictionaryInterfaceInfo expectedDictionary)
        {
            if (comparands.Subject is ICollection subjectCollection
                && comparands.Expectation is ICollection expectationCollection
                && subjectCollection.Count == expectationCollection.Count)
            {
                return true;
            }

            return (bool)AssertSameLengthMethod
                .MakeGenericMethod(actualDictionary.Key, actualDictionary.Value, expectedDictionary.Key, expectedDictionary.Value)
                .Invoke(null, new[] { comparands.Subject, comparands.Expectation });
        }

        private static bool AssertSameLength<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            IDictionary<TSubjectKey, TSubjectValue> subject, IDictionary<TExpectedKey, TExpectedValue> expectation)

            // Type constraint of TExpectedKey is asymmetric in regards to TSubjectKey
            // but it is valid. This constraint is implicitly enforced by the dictionary interface info which is called before
            // the AssertSameLength method.
            where TExpectedKey : TSubjectKey
        {
            if (expectation.Count == subject.Count)
            {
                return true;
            }

            KeyDifference<TSubjectKey, TExpectedKey> keyDifference = CalculateKeyDifference(subject, expectation);

            bool hasMissingKeys = keyDifference.MissingKeys.Count > 0;
            bool hasAdditionalKeys = keyDifference.AdditionalKeys.Any();

            return Execute.Assertion
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

        private static void AssertDictionaryEquivalence(Comparands comparands, IEquivalencyValidationContext context,
            IEquivalencyValidator parent, DictionaryInterfaceInfo actualDictionary, DictionaryInterfaceInfo expectedDictionary)
        {
            AssertDictionaryEquivalenceMethod
                .MakeGenericMethod(actualDictionary.Key, actualDictionary.Value, expectedDictionary.Key, expectedDictionary.Value)
                .Invoke(null, new[] { context, parent, context.Options, comparands.Subject, comparands.Expectation });
        }

        private static void AssertDictionaryEquivalence<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            EquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions options,
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey
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

                            parent.RecursivelyAssertEquality(nestedComparands,
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

        private class KeyDifference<TSubjectKey, TExpectedKey>
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
}
