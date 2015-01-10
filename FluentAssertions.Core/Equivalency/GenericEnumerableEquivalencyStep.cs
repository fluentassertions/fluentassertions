using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal class GenericEnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            var subjectType = config.GetSubjectType(context);

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
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            Type subjectType = config.GetSubjectType(context);

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

                Expression subjectToArray = ToArray(context.Subject, typeOfEnumeration);
                Expression expectationToArray =
                    Expression.Constant(EnumerableEquivalencyStep.ToArray(context.Expectation));

                MethodCallExpression executeExpression = Expression.Call(
                    Expression.Constant(validator),
                    ExpressionExtensions.GetMethodName(() => validator.Execute<object>(null,null)),
                    new Type[] { typeOfEnumeration },
                    subjectToArray,
                    expectationToArray);

                Expression.Lambda(executeExpression).Compile().DynamicInvoke();
            }

            return true;
        }

        private static bool AssertExpectationIsCollection(object expectation)
        {
            bool conditionMet = AssertionScope.Current
                .ForCondition(!ReferenceEquals(expectation, null))
                .FailWith("{context:Subject} is a collection and cannot be compared to <null>.");

            if (conditionMet)
            {
                return AssertionScope.Current
                    .ForCondition(IsGenericCollection(expectation.GetType()))
                    .FailWith(
                        "{context:Subject} is a collection and cannot be compared with a non-collection type.");
            }

            return conditionMet;
        }

        private static bool IsGenericCollection(Type type)
        {
            var enumerableInterfaces = GetIEnumerableInterfaces(type);

            return (!typeof (string).IsAssignableFrom(type)) && enumerableInterfaces.Any();
        }

        private static Type[] GetIEnumerableInterfaces(Type type)
        {
            Type soughtType = typeof(IEnumerable<>);

            return Common.TypeExtensions.GetClosedGenericInterfaces(type, soughtType);
        }

        private static Type GetTypeOfEnumeration(Type enumerableType)
        {
            Type interfaceType = GetIEnumerableInterfaces(enumerableType).Single();

            return interfaceType.GetGenericArguments().Single();
        }

        private static MethodCallExpression ToArray(object value, Type typeOfEnumeration)
        {
            return Expression.Call(
                typeof(Enumerable),
                "ToArray",
                new Type[] { typeOfEnumeration },
                Expression.Constant(value, typeof(IEnumerable<>).MakeGenericType(typeOfEnumeration)));
        }
    }
}