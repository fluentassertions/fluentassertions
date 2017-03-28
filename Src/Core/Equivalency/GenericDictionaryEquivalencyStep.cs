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
            Type subjectType = config.GetSubjectType(context);

            return ((context.Subject != null) && GetIDictionaryInterfaces(subjectType).Any());
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
            Type subjectType = config.GetSubjectType(context);

            return (AssertImplementsOnlyOneDictionaryInterface(context.Subject)
                    && AssertExpectationIsNotNull(context.Subject, context.Expectation)
                    && AssertIsCompatiblyTypedDictionary(subjectType, context.Expectation)
                    && AssertSameLength(context.Subject, subjectType, context.Expectation));
        }

        private static bool AssertExpectationIsNotNull(object subject, object expectation)
        {
            return AssertionScope.Current
                .ForCondition(!ReferenceEquals(expectation, null))
                .FailWith("Expected {context:Subject} to be {0}, but found {1}.", null, subject);
        }

        private static bool AssertImplementsOnlyOneDictionaryInterface(object subject)
        {
            Type[] interfaces = GetIDictionaryInterfaces(subject.GetType());
            bool multipleInterfaces = (interfaces.Count() > 1);

            if (multipleInterfaces)
            {
                AssertionScope.Current.FailWith(
                    string.Format(
                        "{{context:Subject}} implements multiple dictionary types.  "
                        + "It is not known which type should be use for equivalence.{0}"
                        + "The following IDictionary interfaces are implemented: {1}",
                        Environment.NewLine,
                        String.Join(", ", (IEnumerable<Type>)interfaces)));
                return false;
            }

            return true;
        }

        private static bool AssertIsCompatiblyTypedDictionary(Type subjectType, object expectation)
        {
            Type subjectDictionaryType = GetIDictionaryInterface(subjectType);
            Type subjectKeyType = GetDictionaryKeyType(subjectDictionaryType);

            Type[] expectationDictionaryInterfaces = GetIDictionaryInterfaces(expectation.GetType());

            if (!expectationDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    "{context:subject} is a dictionary and cannot be compared with a non-dictionary type.");
                return false;
            }

            Type[] suitableDictionaryInterfaces = expectationDictionaryInterfaces.Where(
                @interface => GetDictionaryKeyType(@interface).IsAssignableFrom(subjectKeyType)).ToArray();

            if (suitableDictionaryInterfaces.Count() > 1)
            {
                // Code could be written to handle this better, but is it really worth the effort?
                AssertionScope.Current.FailWith(
                    "The expected object implements multiple IDictionary interfaces.  "
                    + "If you need to use ShouldBeEquivalentTo in this case please file "
                    + "a bug with the FluentAssertions devlopment team");
                return false;
            }

            if (!suitableDictionaryInterfaces.Any())
            {
                AssertionScope.Current.FailWith(
                    string.Format(
                        "The {{context:subject}} dictionary has keys of type {0}; "
                        + "however, the expected dictionary is not keyed with any compatible types.{1}"
                        + "The expected dictionary implements: {2}",
                        subjectKeyType,
                        Environment.NewLine,
                        string.Join(",", (IEnumerable<Type>)expectationDictionaryInterfaces)));
                return false;
            }

            return true;
        }

        private static Type GetDictionaryKeyType(Type subjectType)
        {
            return subjectType.GetGenericArguments()[0];
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

        private static Type GetIDictionaryInterface(Type subjectType)
        {
            return GetIDictionaryInterfaces(subjectType).Single();
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
            Type subjectType = config.GetSubjectType(context);

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

        private static void AssertDictionaryEquivalence<TSubjectKey, TSubjectValue, TExpectKey, TExpectedValue>(
            EquivalencyValidationContext context,
            IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config,
            IDictionary<TSubjectKey, TSubjectValue> subject,
            IDictionary<TExpectKey, TExpectedValue> expectation) where TSubjectKey : TExpectKey
        {
            foreach (TSubjectKey key in subject.Keys)
            {
                TExpectedValue expectedValue;

                if (expectation.TryGetValue(key, out expectedValue))
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
                    AssertionScope.Current.FailWith("{context:subject} contains unexpected key {0}", key);
                }
            }
        }
    }
}