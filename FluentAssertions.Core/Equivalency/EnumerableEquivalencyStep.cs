using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    internal class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return (context.Subject != null) && IsCollection(context.Subject);
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
            Type[] interfaces = GetIEnumerableInterfaces(context.Subject);
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

                Type typeOfEnumeration = GetTypeOfEnumeration(context);

                Expression subjectToArray = ToArray(context.Subject, typeOfEnumeration);

                MethodCallExpression executeExpression = Expression.Call(
                    Expression.Constant(validator),
                    "Execute",
                    new Type[] { typeOfEnumeration }, subjectToArray,
                    Expression.Constant(ToObjectArray(context.Expectation)));

                Expression.Lambda(executeExpression).Compile().DynamicInvoke();
            }

            return true;
        }

        private static bool AssertExpectationIsCollection(object expectation)
        {
            return AssertionScope.Current
                .ForCondition(IsCollection(expectation))
                .FailWith(
                    "{context:Subject} is a collection and cannot be compared with a non-collection type.");
        }

        private static bool IsCollection(object expectation)
        {
            return IsGenericCollection(expectation) ||
                   IsNonGenericCollection(expectation);
        }

        private static bool IsNonGenericCollection(object value)
        {
            return !(value is string) && (value is IEnumerable);
        }

        private static bool IsGenericCollection(object value)
        {
            var enumerableInterfaces = GetIEnumerableInterfaces(value);

            return !(value is string) && enumerableInterfaces.Any();
        }

        private static Type[] GetIEnumerableInterfaces(object value)
        {
            return value.GetType()
                .GetInterfaces()
                .Where(
                    type =>
                        (type.IsGenericType &&
                         (type.GetGenericTypeDefinition() ==
                          typeof (IEnumerable<>)))).ToArray();
        }

        private static Type GetTypeOfEnumeration(EquivalencyValidationContext context)
        {
            Type interfaceType =
                GetIEnumerableInterfaces(context.Subject).SingleOrDefault();

            return (interfaceType == null)
                ? typeof(object)
                : interfaceType.GetGenericArguments().Single();
        }

        private static Expression ToArray(object value, Type typeOfEnumeration)
        {
            if (IsGenericCollection(value))
            {
                return Expression.Call(
                    typeof (Enumerable),
                    "ToArray",
                    new Type[] {typeOfEnumeration},
                    Expression.Constant(value));
            }
            else
            {
                return Expression.Constant(ToObjectArray(value));
            }
        }

        private static object[] ToObjectArray(object value)
        {
            return ((IEnumerable)value).Cast<object>().ToArray();
        }
    }
}