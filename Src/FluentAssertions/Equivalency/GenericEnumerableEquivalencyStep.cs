using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class GenericEnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            var expectationType = config.GetExpectationType(context);

            return (context.Expectation != null) && IsGenericCollection(expectationType);
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
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            Type expectedType = config.GetExpectationType(context);

            var interfaceTypes = GetIEnumerableInterfaces(expectedType)
                .Select(type => "IEnumerable<" + type.GetGenericArguments().Single() + ">")
                .ToList();

            AssertionScope.Current
                .ForCondition(interfaceTypes.Count == 1)
                .FailWith("{context:Expectation} implements {0}, so cannot determine which one " +
                          "to use for asserting the equivalency of the collection. ", interfaceTypes);

            if (AssertSubjectIsCollection(context.Expectation, context.Subject))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || config.IsRecursive,
                    OrderingRules = config.OrderingRules
                };

                Type typeOfEnumeration = GetTypeOfEnumeration(expectedType);

                MethodCallExpression expectationAsArray = ToArray(context.Expectation, typeOfEnumeration);
                ConstantExpression subjectAsArray =
                    Expression.Constant(EnumerableEquivalencyStep.ToArray(context.Subject));

                MethodCallExpression executeExpression = Expression.Call(
                    Expression.Constant(validator),
                    ExpressionExtensions.GetMethodName(() => validator.Execute<object>(null, null)),
                    new[] { typeOfEnumeration },
                    subjectAsArray,
                    expectationAsArray);

                try
                {
                    Expression.Lambda(executeExpression).Compile().DynamicInvoke();
                }
                catch (TargetInvocationException e)
                {
                    throw e.Unwrap();
                }
            }

            return true;
        }

        private static bool AssertSubjectIsCollection(object expectation, object subject)
        {
            bool conditionMet = AssertionScope.Current
                .ForCondition(!ReferenceEquals(subject, null))
                .FailWith("Expected {context:Subject} not to be {0}.", new object[] { null });

            if (conditionMet)
            {
                conditionMet = AssertionScope.Current
                    .ForCondition(IsGenericCollection(subject.GetType()))
                    .FailWith("Expected {context:Subject} to be {0}, but found {1}.", expectation, subject);
            }

            return conditionMet;
        }

        private static bool IsGenericCollection(Type type)
        {
            var enumerableInterfaces = GetIEnumerableInterfaces(type);

            return (!typeof(string).IsAssignableFrom(type)) && enumerableInterfaces.Any();
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
                new[] { typeOfEnumeration },
                Expression.Constant(value, typeof(IEnumerable<>).MakeGenericType(typeOfEnumeration)));
        }
    }
}
