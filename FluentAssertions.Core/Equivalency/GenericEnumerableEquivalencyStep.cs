using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal class GenericEnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            var subjectType = EnumerableEquivalencyStep.GetSubjectType(context, config);

            return (context.Subject != null) && IsGenericCollection(subjectType);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type subjectType = EnumerableEquivalencyStep.GetSubjectType(context, config);

            Type[] interfaces = GetIEnumerableInterfaces(subjectType);
            bool multipleInterfaces = (interfaces.Count() > 1);

            if (multipleInterfaces)
            {
                IEnumerable<Type> enumerableTypes = interfaces.Select(
                    type => type.GetGenericArguments().Single());

                AssertionScope.Current.FailWith(
                    String.Format(
                        "{{context:Subject}} is enumerable for more than one type.  " +
                        "It is not known which type should be use for equivalence.{0}" +
                        "IEnumerable is implemented for the following types: {1}",
                        Environment.NewLine,
                        String.Join(", ", enumerableTypes)));
            }

            if (AssertExpectationIsCollection(context.Expectation))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || config.IsRecursive,
                    OrderingRules = config.OrderingRules
                };

                Type typeOfEnumeration = GetTypeOfEnumeration(subjectType);

                object subjectToArray = ToArray(context.Subject, typeOfEnumeration);
                object[] expectationToArray =
                    EnumerableEquivalencyStep.ToArray(context.Expectation);

                MethodCallExpression executeExpression = Expression.Call(
                    Expression.Constant(validator),
                    "Execute",
                    new Type[] { typeOfEnumeration },
                    Expression.Constant(subjectToArray),
                    Expression.Constant(expectationToArray));

                Expression.Lambda(executeExpression).Compile().DynamicInvoke();
            }

            return true;
        }

        private static bool AssertExpectationIsCollection(object expectation)
        {
            return AssertionScope.Current
                .ForCondition(IsGenericCollection(expectation.GetType()))
                .FailWith(
                    "{context:Subject} is a collection and cannot be compared with a non-collection type.");
        }

        private static bool IsGenericCollection(Type type)
        {
            var enumerableInterfaces = GetIEnumerableInterfaces(type);

            return (!typeof(string).IsAssignableFrom(type)) && enumerableInterfaces.Any();
        }

        private static Type[] GetIEnumerableInterfaces(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return new[] { type };
            }

            return
                type.GetInterfaces()
                    .Where(t => (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
                    .ToArray();
        }

        private static Type GetTypeOfEnumeration(Type enumerableType)
        {
            Type interfaceType = GetIEnumerableInterfaces(enumerableType).Single();

            return interfaceType.GetGenericArguments().Single();
        }

        private static object ToArray(object value, Type typeOfEnumeration)
        {
            MethodInfo toArray = typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(typeOfEnumeration);

            return toArray.Invoke(null, new [] {value});
        }
    }
}