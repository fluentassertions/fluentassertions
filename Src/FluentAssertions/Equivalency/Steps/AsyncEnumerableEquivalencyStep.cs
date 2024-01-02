using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class AsyncEnumerableEquivalencyStep : IEquivalencyStep
{
#pragma warning disable SA1110 // Allow opening parenthesis on new line to reduce line length
    private static readonly MethodInfo HandleMethod = new Action<EnumerableEquivalencyValidator, object[], IAsyncEnumerable<object>>
        (HandleImpl).GetMethodInfo().GetGenericMethodDefinition();

    private static readonly MethodInfo ToObjectArrayAsyncMethod = new Func<IAsyncEnumerable<object>, object[]>
            (ToObjectArray).GetMethodInfo().GetGenericMethodDefinition();
#pragma warning restore SA1110

    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        Type expectedType = comparands.GetExpectedType(context.Options);

        if (comparands.Expectation is null || !IsGenericCollection(expectedType))
        {
            return EquivalencyResult.ContinueWithNext;
        }

        Type[] interfaceTypes = GetIAsyncEnumerableInterfaces(expectedType);

        AssertionScope.Current
            .ForCondition(interfaceTypes.Length == 1)
            .FailWith(() => new FailReason("{context:Expectation} implements {0}, so cannot determine which one " +
                "to use for asserting the equivalency of the collection. ",
                interfaceTypes.Select(type => "IAsyncEnumerable<" + type.GetGenericArguments().Single() + ">")));

        if (AssertSubjectIsCollection(comparands.Subject))
        {
            var validator = new EnumerableEquivalencyValidator(nestedValidator, context)
            {
                Recursive = context.CurrentNode.IsRoot || context.Options.IsRecursive,
                OrderingRules = context.Options.OrderingRules
            };

            Type typeOfEnumeration = GetTypeOfEnumeration(expectedType);

            object subjectAsArray = null;

            try
            {
                subjectAsArray = ToObjectArrayAsyncMethod.MakeGenericMethod(typeOfEnumeration)
                    .Invoke(null, new[] { comparands.Subject });
            }
            catch (TargetInvocationException e)
            {
                e.Unwrap().Throw();
            }

            try
            {
                HandleMethod.MakeGenericMethod(typeOfEnumeration)
                    .Invoke(null, new[] { validator, subjectAsArray, comparands.Expectation });
            }
            catch (TargetInvocationException e)
            {
                e.Unwrap().Throw();
            }
        }

        return EquivalencyResult.AssertionCompleted;
    }

    private static void HandleImpl<T>(EnumerableEquivalencyValidator validator, object[] subject, IAsyncEnumerable<T> expectation) =>
        validator.Execute(subject, ToArray(expectation));

    private static bool AssertSubjectIsCollection(object subject)
    {
        bool conditionMet = AssertionScope.Current
            .ForCondition(subject is not null)
            .FailWith("Expected {context:subject} not to be {0}.", new object[] { null });

        if (conditionMet)
        {
            conditionMet = AssertionScope.Current
                .ForCondition(IsGenericCollection(subject.GetType()))
                .FailWith("Expected {context:subject} to be a collection, but it was a {0}", subject.GetType());
        }

        return conditionMet;
    }

    private static bool IsGenericCollection(Type type)
    {
        Type[] enumerableInterfaces = GetIAsyncEnumerableInterfaces(type);

        return enumerableInterfaces.Length > 0;
    }

    private static Type[] GetIAsyncEnumerableInterfaces(Type type)
    {
        // Avoid expensive calculation when the type in question can't possibly implement IEnumerable<>.
        if (Type.GetTypeCode(type) != TypeCode.Object)
        {
            return Array.Empty<Type>();
        }

        Type soughtType = typeof(IAsyncEnumerable<>);

        return type.GetClosedGenericInterfaces(soughtType);
    }

    private static Type GetTypeOfEnumeration(Type enumerableType)
    {
        Type interfaceType = GetIAsyncEnumerableInterfaces(enumerableType).Single();

        return interfaceType.GetGenericArguments().Single();
    }

    private static T[] ToArray<T>(IAsyncEnumerable<T> value)
    {
        return value?.ToBlockingEnumerable().ToArray();
    }

    private static object[] ToObjectArray<T>(IAsyncEnumerable<T> value)
    {
        return ToArray(value).Cast<object>().ToArray();
    }
}
