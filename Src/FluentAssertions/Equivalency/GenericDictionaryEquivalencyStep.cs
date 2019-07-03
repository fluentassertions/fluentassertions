using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Equivalency
{
    /// <remarks>
    /// I think (but did not try) this would have been easier using 'dynamic' but that is
    /// precluded by some of the PCL targets.
    /// </remarks>
    public class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
        private static readonly MethodInfo AssertSameLengthMethod = new Func<IDictionary<object, object>, IDictionary<object, object>, bool>
            (AssertSameLength).GetMethodInfo().GetGenericMethodDefinition();

        private static readonly MethodInfo AssertDictionaryEquivalenceMethod = new Action<EquivalencyValidationContext, IEquivalencyValidator, IEquivalencyAssertionOptions, IDictionary<object, object>, IDictionary<object, object>>
            (AssertDictionaryEquivalence).GetMethodInfo().GetGenericMethodDefinition();

        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

            return context.Expectation != null && GetIDictionaryInterfaces(expectationType).Any();
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            if (PreconditionsAreMet(context, config))
            {
                AssertDictionaryEquivalence(context, parent, config);
            }

            return true;
        }

        private static Type[] GetIDictionaryInterfaces(Type type)
        {
            return Common.TypeExtensions.GetClosedGenericInterfaces(
                type,
                typeof(IDictionary<,>));
        }

        private static bool PreconditionsAreMet(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

            return AssertImplementsOnlyOneDictionaryInterface(context.Expectation)
                   && AssertSubjectIsNotNull(context.Subject)
                   && AssertExpectationIsNotNull(context.Subject, context.Expectation)
                   && AssertIsCompatiblyTypedDictionary(expectationType, context.Subject)
                   && AssertSameLength(context.Subject, expectationType, context.Expectation);
        }

        private static bool AssertSubjectIsNotNull(object subject)
        {
            return AssertionScope.Current
                .ForCondition(!(subject is null))
                .FailWith(Resources.Collection_ExpectedSubjectNotToBeXFormat, new object[] { null });
        }

        private static bool AssertExpectationIsNotNull(object subject, object expectation)
        {
            return AssertionScope.Current
                .ForCondition(!(expectation is null))
                .FailWith(Resources.Dictionary_ExpectedSubjectToBeXFormat + Resources.Common_CommaButFoundYFormat, null, subject);
        }

        private static bool AssertImplementsOnlyOneDictionaryInterface(object expectation)
        {
            Type[] interfaces = GetIDictionaryInterfaces(expectation.GetType());
            bool multipleInterfaces = interfaces.Length > 1;
            if (!multipleInterfaces)
            {
                return true;
            }

            AssertionScope.Current.FailWith(
                string.Format(Resources.Dictionary_ExpectationImplementsMultipleDictionaryTypesFormat,
                    Environment.NewLine,
                    string.Join(", ", (IEnumerable<Type>)interfaces)));

            return false;
        }

        private static bool AssertIsCompatiblyTypedDictionary(Type expectedType, object subject)
        {
            Type expectedDictionaryType = GetIDictionaryInterface(expectedType);
            Type expectedKeyType = GetDictionaryKeyType(expectedDictionaryType);

            Type subjectType = subject.GetType();
            Type[] subjectDictionaryInterfaces = GetIDictionaryInterfaces(subjectType);
            if (!subjectDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(Resources.Dictionary_ExpectedSubjectToBeAXFormat + Resources.Common_CommaButFoundYFormat,
                    expectedDictionaryType, subjectType);

                return false;
            }

            Type[] suitableDictionaryInterfaces = subjectDictionaryInterfaces.Where(
                @interface => GetDictionaryKeyType(@interface).IsAssignableFrom(expectedKeyType)).ToArray();

            if (suitableDictionaryInterfaces.Length > 1)
            {
                // SMELL: Code could be written to handle this better, but is it really worth the effort?
                AssertionScope.Current.FailWith(Resources.Dictionary_SubjectImplementsMultipleIDictionaryInterfaces);

                return false;
            }

            if (!suitableDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    string.Format(Resources.Dictionary_SubjectDictionaryHasKeysOfTypeXButExpectationIncompatibleTypesYSubjectImplementsZFormat,
                        expectedKeyType,
                        Environment.NewLine,
                        string.Join(",", (IEnumerable<Type>)subjectDictionaryInterfaces)));

                return false;
            }

            return true;
        }

        private static Type GetDictionaryKeyType(Type expectedType)
        {
            return expectedType.GetGenericArguments()[0];
        }

        private static bool AssertSameLength(object subject, Type expectationType, object expectation)
        {
            if (subject is ICollection subjectCollection
                && expectation is ICollection expectationCollection
                && subjectCollection.Count == expectationCollection.Count)
                return true;

            Type subjectType = subject.GetType();
            Type[] subjectTypeArguments = GetDictionaryTypeArguments(subjectType);
            Type[] expectationTypeArguments = GetDictionaryTypeArguments(expectationType);
            Type[] typeArguments = subjectTypeArguments.Concat(expectationTypeArguments).ToArray();

            return (bool)AssertSameLengthMethod.MakeGenericMethod(typeArguments).Invoke(null, new[] { subject, expectation });
        }

        private static Type[] GetDictionaryTypeArguments(Type type)
        {
            Type dictionaryType = GetIDictionaryInterface(type);

            return dictionaryType.GetGenericArguments();
        }

        private static Type GetIDictionaryInterface(Type expectedType)
        {
            return GetIDictionaryInterfaces(expectedType).Single();
        }

        private static bool AssertSameLength<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            IDictionary<TSubjectKey, TSubjectValue> subject, IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey
            // Type constraint of TExpectedKey is asymmetric in regards to TSubjectKey
            // but it is valid. This constraint is implicitly enforced by the
            // AssertIsCompatiblyTypedDictionary method which is called before
            // the AssertSameLength method.
        {
            if (expectation.Count == subject.Count)
            {
                return true;
            }

            KeyDifference<TSubjectKey, TExpectedKey> keyDifference = CalculateKeyDifference(subject, expectation);

            bool hasMissingKeys = keyDifference.MissingKeys.Count > 0;
            bool hasAdditionalKeys = keyDifference.AdditionalKeys.Any();

            return Execute.Assertion
                .WithExpectation(Resources.Dictionary_ExpectedSubjectToBeDictionaryWithXItemsFormat, expectation.Count)
                .ForCondition(!hasMissingKeys || hasAdditionalKeys)
                .FailWith(Resources.Dictionary_ButItMissesKeysXFormat, keyDifference.MissingKeys)
                .Then
                .ForCondition(hasMissingKeys || !hasAdditionalKeys)
                .FailWith(Resources.Dictionary_ButHasAdditionalKeysXFormat, keyDifference.AdditionalKeys)
                .Then
                .ForCondition(!hasMissingKeys || !hasAdditionalKeys)
                .FailWith(Resources.Dictionary_ButItMissesKeysXAndHasAdditionalKeysYFormat, keyDifference.MissingKeys, keyDifference.AdditionalKeys)
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

        private static void AssertDictionaryEquivalence(IEquivalencyValidationContext context,
            IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);
            Type subjectType = context.Subject.GetType();
            Type[] subjectTypeArguments = GetDictionaryTypeArguments(subjectType);
            Type[] expectationTypeArguments = GetDictionaryTypeArguments(expectationType);
            Type[] typeArguments = subjectTypeArguments.Concat(expectationTypeArguments).ToArray();

            AssertDictionaryEquivalenceMethod.MakeGenericMethod(typeArguments).Invoke(null, new[] { context, parent, config, context.Subject, context.Expectation });
        }

        private static void AssertDictionaryEquivalence<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            EquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config,
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey
        {
            foreach (TExpectedKey key in expectation.Keys)
            {
                if (subject.TryGetValue(key, out TSubjectValue subjectValue))
                {
                    if (config.IsRecursive)
                    {
                        parent.AssertEqualityUsing(context.CreateForDictionaryItem(key, subjectValue, expectation[key]));
                    }
                    else
                    {
                        subjectValue.Should().Be(expectation[key], context.Because, context.BecauseArgs);
                    }
                }
                else
                {
                    AssertionScope.Current.FailWith(Resources.Dictionary_ExpectedSubjectToContainKeyXFormat, key);
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
