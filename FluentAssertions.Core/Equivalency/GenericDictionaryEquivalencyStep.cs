using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <remarks>
    /// I think (but did not try) this would have been easier using 'dynamic' but that is
    /// precluded by some of the PCL targets.
    /// </remarks>
    internal class GenericDictionaryEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type subjectType = EnumerableEquivalencyStep.GetSubjectType(context, config);

            return context.Subject != null
                   && GetIDictionaryInterfaces(subjectType).Any();
        }

        private static Type[] GetIDictionaryInterfaces(Type type)
        {
            return GenericEnumerableEquivalencyStep.GetClosedGenericInterfaces(
                type,
                typeof(IDictionary<,>));
        }

        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type[] interfaces = GetIDictionaryInterfaces(context.Subject.GetType());
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
            }

            Type subjectType = EnumerableEquivalencyStep.GetSubjectType(context, config);

            if (PreconditionsAreMet(context.Subject, subjectType, context.Expectation))
            {
                return false;
            }

            return true;
        }

        private static bool PreconditionsAreMet(object subject, Type subjectType, object expectation)
        {
            return AssertIsDictionary(subjectType, expectation) && AssertSameLength(subject, subjectType, expectation);
        }

        private static bool AssertIsDictionary(Type subjectType, object expectation)
        {
            Type subjectDictionaryType = GetIDictionaryInterfaces(subjectType).Single();
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
                        "The subject dictionary has keys of type {0}; "
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
            MethodCallExpression assertSameLength = Expression.Call(
                typeof(GenericDictionaryEquivalencyStep),
                "AssertSameLength",
                GetDictionaryRelatedTypeArgumentsArray(subjectType, expectation),
                Expression.Constant(subject, subjectType),
                Expression.Constant(expectation));

            return (bool)Expression.Lambda(assertSameLength).Compile().DynamicInvoke();
        }

        private static Type[] GetDictionaryRelatedTypeArgumentsArray(Type subjectType, object expectation)
        {
            IEnumerable<Type> subjectTypes = GetDictionaryTypeAndTypeArguments(subjectType);
            IEnumerable<Type> expectationTypes = GetDictionaryTypeAndTypeArguments(expectation.GetType());

            return subjectTypes.Concat(expectationTypes).ToArray();
        }

        private static IEnumerable<Type> GetDictionaryTypeAndTypeArguments(Type subjectType)
        {
            var dictionaryType = GetIDictionaryInterfaces(subjectType).Single();

            return new[] { subjectType }.Concat(dictionaryType.GetGenericArguments());
        }

        private static bool AssertSameLength
            <TSubject, TSubjectKey, TSubjectValue, TExpected, TExpectKey, TExpectedValue>(
            TSubject subject,
            TExpected expectation) where TExpected : IDictionary<TExpectKey, TExpectedValue>
            where TSubject : IDictionary<TSubjectKey, TSubjectValue>
        {
            return
                AssertionScope.Current.ForCondition(subject.Count == expectation.Count)
                    .FailWith(
                        "Expected {context:subject} to be a dictionary with {0} item(s), but found {1} item(s).",
                        expectation.Count,
                        subject.Count);
        }
    }
}