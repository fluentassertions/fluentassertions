using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class GenericEnumerableEquivalencyStep : IEquivalencyStep
    {
        private static readonly MethodInfo HandleMethod = new Action<EnumerableEquivalencyValidator, object[], IEnumerable<object>>
            (HandleImpl).GetMethodInfo().GetGenericMethodDefinition();

        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type expectationType = config.GetExpectationType(context);

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

            Type[] interfaceTypes = GetIEnumerableInterfaces(expectedType);

            AssertionScope.Current
                .ForCondition(interfaceTypes.Length == 1)
                .FailWith(() => new FailReason("{context:Expectation} implements {0}, so cannot determine which one " +
                    "to use for asserting the equivalency of the collection. ",
                    interfaceTypes.Select(type => "IEnumerable<" + type.GetGenericArguments().Single() + ">")));

            if (AssertSubjectIsCollection(context.Subject))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || config.IsRecursive,
                    OrderingRules = config.OrderingRules
                };

                Type typeOfEnumeration = GetTypeOfEnumeration(expectedType);

                var subjectAsArray = EnumerableEquivalencyStep.ToArray(context.Subject);

                try
                {
                    HandleMethod.MakeGenericMethod(typeOfEnumeration).Invoke(null, new[] { validator, subjectAsArray, context.Expectation });
                }
                catch (TargetInvocationException e)
                {
                    throw e.Unwrap();
                }
            }

            return true;
        }

        private static void HandleImpl<T>(EnumerableEquivalencyValidator validator, object[] subject, IEnumerable<T> expectation)
            => validator.Execute(subject, expectation?.ToArray());

        private static bool AssertSubjectIsCollection(object subject)
        {
            bool conditionMet = AssertionScope.Current
                .ForCondition(!(subject is null))
                .FailWith("Expected {context:subject} not to be {0}.", new object[] { null });

            if (conditionMet)
            {
                conditionMet = AssertionScope.Current
                    .ForCondition(IsCollection(subject.GetType()))
                    .FailWith("Expected {context:subject} to be a collection, but it was a {0}", subject.GetType());
            }

            return conditionMet;
        }

        private static bool IsCollection(Type type)
        {
            return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        private static bool IsGenericCollection(Type type)
        {
            Type[] enumerableInterfaces = GetIEnumerableInterfaces(type);

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
    }
}
