using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <remarks>
    /// I think (but did not try) this would have been easier using 'dynamic' but that is
    /// precluded by some of the PCL targets.
    /// </remarks>
    public class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

            return ((context.Expectation != null) && GetIDictionaryInterfaces(expectationType).Any());
        }

        private static Type[] GetIDictionaryInterfaces(Type type)
        {
            return Common.TypeExtensions.GetClosedGenericInterfaces(
                type,
                typeof(IDictionary<,>));
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            if (PreconditionsAreMet(context, config))
            {
                AssertDictionaryEquivalence(context, parent, config);
            }

            return true;
        }

        private static bool PreconditionsAreMet(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

            return (AssertImplementsOnlyOneDictionaryInterface(context.Expectation)
                    && AssertExpectationIsNotNull(context.Subject, context.Expectation)
                    && AssertIsCompatiblyTypedDictionary(expectationType, context.Subject)
                    && AssertSameLength(context.Subject, expectationType, context.Expectation));
        }

        private static bool AssertExpectationIsNotNull(object subject, object expectation)
        {
            return AssertionScope.Current
                .ForCondition(!ReferenceEquals(expectation, null))
                .FailWith("Expected {context:Subject} to be {0}, but found {1}.", null, subject);
        }

        private static bool AssertImplementsOnlyOneDictionaryInterface(object expectation)
        {
            Type[] interfaces = GetIDictionaryInterfaces(expectation.GetType());
            bool multipleInterfaces = (interfaces.Length > 1);
            if (!multipleInterfaces)
            {
                return true;
            }
            
            AssertionScope.Current.FailWith(
                string.Format(
                    "{{context:Expectation}} implements multiple dictionary types.  "
                    + "It is not known which type should be use for equivalence.{0}"
                    + "The following IDictionary interfaces are implemented: {1}",
                    Environment.NewLine,
                    String.Join(", ", (IEnumerable<Type>)interfaces)));
                
            return false;
        }

        private static bool AssertIsCompatiblyTypedDictionary(Type expectedType, object subject)
        {
            Type expectedDictionaryType = GetIDictionaryInterface(expectedType);
            Type expectedKeyType = GetDictionaryKeyType(expectedDictionaryType);

            Type[] subjectDictionaryInterfaces = GetIDictionaryInterfaces(subject.GetType());
            if (!subjectDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    "Expected {context:subject} to be a {0}, but found a {1}.", expectedDictionaryType, subject.GetType());
                
                return false;
            }

            Type[] suitableDictionaryInterfaces = subjectDictionaryInterfaces.Where(
                @interface => GetDictionaryKeyType(@interface).IsAssignableFrom(expectedKeyType)).ToArray();

            if (suitableDictionaryInterfaces.Length > 1)
            {
                // SMELL: Code could be written to handle this better, but is it really worth the effort?
                AssertionScope.Current.FailWith(
                    "The subject implements multiple IDictionary interfaces. ");

                    return false;
            }

            if (!suitableDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    string.Format(
                        "The {{context:subject}} dictionary has keys of type {0}; "
                        + "however, the expectation is not keyed with any compatible types.{1}"
                        + "The subject implements: {2}",
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
            string methodName =
                ExpressionExtensions.GetMethodName(() => AssertSameLength<object, object, object, object>(null, null));

            Type[] typeArguments = GetDictionaryTypeArguments(subject.GetType())
                .Concat(GetDictionaryTypeArguments(expectationType))
                .ToArray();
                
            MethodCallExpression assertSameLength = Expression.Call(
                typeof(GenericDictionaryEquivalencyStep),
                methodName,
                typeArguments,
                Expression.Constant(subject, GetIDictionaryInterface(subject.GetType())),
                Expression.Constant(expectation, GetIDictionaryInterface(expectationType)));

            return (bool)Expression.Lambda(assertSameLength).Compile().DynamicInvoke();
        }

        private static IEnumerable<Type> GetDictionaryTypeArguments(Type type)
        {
            Type dictionaryType = GetIDictionaryInterface(type);

            return dictionaryType.GetGenericArguments();
        }

        private static Type GetIDictionaryInterface(Type expectedType)
        {
            return GetIDictionaryInterfaces(expectedType).Single();
        }
        
        private static bool AssertSameLength<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectedKey, TExpectedValue> expectation)
            where TExpectedKey : TSubjectKey
            // Type constraint of TExpectedKey is asymetric in regards to TSubjectKey
            // but it is valid. This constraint is implicitly enforced by the
            // AssertIsCompatiblyTypedDictionary method which is called before
            // the AssertSameLength method.
        {
            const string messageCore = "Expected {context:subject} to be a dictionary with {0} item(s), but found {1} item(s).";

            if(expectation.Count != subject.Count)
            {
                // First, we gather missing and additional keys
                List<TExpectedKey> missingKeys = new List<TExpectedKey>();
                HashSet<TSubjectKey> presentKeys = new HashSet<TSubjectKey>();
                
                foreach (TExpectedKey expectationKey in expectation.Keys)
                {
                    if(subject.ContainsKey(expectationKey))
                    {
                        presentKeys.Add(expectationKey);
                    }
                    else
                    {
                        missingKeys.Add(expectationKey);
                    }
                }

                IEnumerable<TSubjectKey> additionalKeys = subject.Keys.Except(presentKeys);

                bool thereAreSomeMissingKeys = missingKeys.Count > 0;
                bool thereAreSomeAdditionalKeys = additionalKeys.Any();

                // Just missing keys
                if(thereAreSomeMissingKeys && !thereAreSomeAdditionalKeys)
                {
                    return AssertionScope.Current
                        .FailWith(
                            $"{messageCore} Missing key(s): {{2}}",
                            expectation.Count,
                            subject.Count,
                            missingKeys);
                }

                // Just additional keys
                else if(!thereAreSomeMissingKeys && thereAreSomeAdditionalKeys)
                {
                    return AssertionScope.Current
                        .FailWith(
                            $"{messageCore} Additional key(s): {{2}}",
                            expectation.Count,
                            subject.Count,
                            additionalKeys);
                }

                // Both missing and additional keys
                else if(thereAreSomeMissingKeys && thereAreSomeAdditionalKeys)
                {
                    return AssertionScope.Current
                        .FailWith(
                            $"{messageCore} Missing key(s): {{2}}. Additional key(s): {{3}}",
                            expectation.Count,
                            subject.Count,
                            missingKeys,
                            additionalKeys);
                }
            }

            // Should not happen, if there is mismatch in count, there must be
            // either some missing or some additional keys. However, for security,
            // this fallback assertion is left.
            return
                AssertionScope.Current
                    .ForCondition(subject.Count == expectation.Count)
                    .FailWith(
                        messageCore,
                        expectation.Count,
                        subject.Count);
        }

        private static void AssertDictionaryEquivalence(IEquivalencyValidationContext context,
            IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

            string methodName =
                ExpressionExtensions.GetMethodName(
                    () => AssertDictionaryEquivalence<object, object, object, object>(null, null, null, null, null));

            var assertDictionaryEquivalence = Expression.Call(
                typeof(GenericDictionaryEquivalencyStep),
                methodName,
                GetDictionaryTypeArguments(context.Subject.GetType())
                    .Concat(GetDictionaryTypeArguments(expectationType))
                    .ToArray(),
                Expression.Constant(context),
                Expression.Constant(parent),
                Expression.Constant(config),
                Expression.Constant(context.Subject, GetIDictionaryInterface(context.Subject.GetType())),
                Expression.Constant(context.Expectation, GetIDictionaryInterface(expectationType)));

            Expression.Lambda(assertDictionaryEquivalence).Compile().DynamicInvoke();
        }

        private static void AssertDictionaryEquivalence<TSubjectKey, TSubjectValue, TExpectedKey, TExpectedValue>(
            EquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config,
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectedKey, TExpectedValue> expectation) where TExpectedKey : TSubjectKey
        {
            foreach (TExpectedKey key in expectation.Keys)
            {
                TSubjectValue subjectValue;

                if (subject.TryGetValue(key, out subjectValue))
                {
                    if (config.IsRecursive)
                    {
                        parent.AssertEqualityUsing(context.CreateForDictionaryItem(key, subject[key], expectation[key]));

                    }
                    else
                    {
                        subject[key].Should().Be(expectation[key], context.Because, context.BecauseArgs);
                    }
                }
                else
                {
                    AssertionScope.Current.FailWith("Expected {context:subject} to contain key {0}.", key);
                }
            }
        }
    }
}
