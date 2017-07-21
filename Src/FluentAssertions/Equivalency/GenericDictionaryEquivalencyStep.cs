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
                    && AssertSameLength(context.Expectation, expectationType, context.Subject));
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
            bool multipleInterfaces = (interfaces.Count() > 1);

            if (multipleInterfaces)
            {
                AssertionScope.Current.FailWith(
                    string.Format(
                        "{{context:Expectation}} implements multiple dictionary types.  "
                        + "It is not known which type should be use for equivalence.{0}"
                        + "The following IDictionary interfaces are implemented: {1}",
                        Environment.NewLine,
                        String.Join(", ", (IEnumerable<Type>)interfaces)));
                return false;
            }

            return true;
        }

        private static bool AssertIsCompatiblyTypedDictionary(Type expectedType, object subject)
        {
            Type expectedDictionaryType = GetIDictionaryInterface(expectedType);
            Type subjectKeyType = GetDictionaryKeyType(expectedDictionaryType);

            Type[] subjectDictionaryInterfaces = GetIDictionaryInterfaces(subject.GetType());
            if (!subjectDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    "{context:subject} is a dictionary and cannot be compared with a non-dictionary type.");
                return false;
            }

            Type[] suitableDictionaryInterfaces = subjectDictionaryInterfaces.Where(
                @interface => GetDictionaryKeyType(@interface).IsAssignableFrom(subjectKeyType)).ToArray();

            if (suitableDictionaryInterfaces.Count() > 1)
            {
                // Code could be written to handle this better, but is it really worth the effort?
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
                        subjectKeyType,
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

        private static bool AssertSameLength(object subject, Type subjectType, object expectation)
        {
            string methodName =
                ExpressionExtensions.GetMethodName(() => AssertSameLength<object, object, object, object>(null, null));

            MethodCallExpression assertSameLength = Expression.Call(
                typeof(GenericDictionaryEquivalencyStep),
                methodName,
                GetDictionaryTypeArguments(subjectType)
                    .Concat(GetDictionaryTypeArguments(expectation.GetType()))
                    .ToArray(),
                Expression.Constant(subject, GetIDictionaryInterface(subjectType)),
                Expression.Constant(expectation, GetIDictionaryInterface(expectation.GetType())));

            return (bool)Expression.Lambda(assertSameLength).Compile().DynamicInvoke();
        }

        private static IEnumerable<Type> GetDictionaryTypeArguments(Type subjectType)
        {
            var dictionaryType = GetIDictionaryInterface(subjectType);

            return dictionaryType.GetGenericArguments();
        }

        private static Type GetIDictionaryInterface(Type expectedType)
        {
            return GetIDictionaryInterfaces(expectedType).Single();
        }

        private static bool AssertSameLength<TSubjectKey, TSubjectValue, TExpectKey, TExpectedValue>(
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectKey, TExpectedValue> expectation)
        {
            return
                AssertionScope.Current.ForCondition(subject.Count == expectation.Count)
                    .FailWith(
                        "Expected {context:subject} to be a dictionary with {0} item(s), but found {1} item(s).",
                        expectation.Count,
                        subject.Count);
        }

        private static void AssertDictionaryEquivalence(
            IEquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            Type subjectType = config.GetExpectationType(context);

            string methodName =
                ExpressionExtensions.GetMethodName(
                    () => AssertDictionaryEquivalence<object, object, object, object>(null, null, null, null, null));

            var assertDictionaryEquivalence = Expression.Call(
                typeof(GenericDictionaryEquivalencyStep),
                methodName,
                GetDictionaryTypeArguments(subjectType)
                    .Concat(GetDictionaryTypeArguments(context.Expectation.GetType()))
                    .ToArray(),
                Expression.Constant(context),
                Expression.Constant(parent),
                Expression.Constant(config),
                Expression.Constant(context.Subject, GetIDictionaryInterface(subjectType)),
                Expression.Constant(context.Expectation, GetIDictionaryInterface(context.Expectation.GetType())));

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
                    AssertionScope.Current.FailWith("{context:expectation} contains key {0} that subject doesn't have", key);
                }
            }
        }
    }
}