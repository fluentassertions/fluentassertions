using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class GenericEnumerableEquivalencyStep : IEquivalencyStep
{
#pragma warning disable SA1110 // Allow opening parenthesis on new line to reduce line length
    private static readonly MethodInfo HandleMethod = new Action<EnumerableEquivalencyValidator, object[], IEnumerable<object>>
        (HandleImpl).GetMethodInfo().GetGenericMethodDefinition();
#pragma warning restore SA1110

    public EquivalencyResult Handle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency valueChildNodes)
    {
        Type expectedType = comparands.GetExpectedType();

        if (comparands.Expectation is null || !IsGenericCollection(expectedType))
        {
            return EquivalencyResult.ContinueWithNext;
        }

        Type[] interfaceTypes = GetIEnumerableInterfaces(expectedType);

        var assertionChain = AssertionChain.GetOrCreate().For(context);

        assertionChain
            .ForCondition(interfaceTypes.Length == 1)
            .FailWith(() => new FailReason("{context:Expectation} implements {0}, so cannot determine which one " +
                "to use for asserting the equivalency of the collection. ",
                interfaceTypes.Select(type => "IEnumerable<" + type.GetGenericArguments().Single() + ">")));

        if (AssertSubjectIsCollection(assertionChain, comparands.Subject))
        {
            var validator = new EnumerableEquivalencyValidator(assertionChain, valueChildNodes, context)
            {
                Recursive = context.CurrentNode.IsRoot || context.Options.IsRecursive,
                OrderingRules = context.Options.OrderingRules
            };

            Type typeOfEnumeration = GetTypeOfEnumeration(expectedType);

            var subjectAsArray = EnumerableEquivalencyStep.ToArray(comparands.Subject);

            try
            {
                HandleMethod.MakeGenericMethod(typeOfEnumeration)
                    .Invoke(null, [validator, subjectAsArray, comparands.Expectation]);
            }
            catch (TargetInvocationException e)
            {
                e.Unwrap().Throw();
            }
        }

        return EquivalencyResult.EquivalencyProven;
    }

    private static void HandleImpl<T>(EnumerableEquivalencyValidator validator, object[] subject, IEnumerable<T> expectation) =>
        validator.Execute(subject, ToArray(expectation));

    private static bool AssertSubjectIsCollection(AssertionChain assertionChain, object subject)
    {
        assertionChain
            .ForCondition(subject is not null)
            .FailWith("Expected {context:subject} not to be {0}.", new object[] { null });

        if (assertionChain.Succeeded)
        {
            assertionChain
                .ForCondition(IsCollection(subject.GetType()))
                .FailWith("Expected {context:subject} to be a collection, but it was a {0}", subject.GetType());
        }

        return assertionChain.Succeeded;
    }

    private static bool IsCollection(Type type)
    {
        return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    private static bool IsGenericCollection(Type type)
    {
        Type[] enumerableInterfaces = GetIEnumerableInterfaces(type);

        return !typeof(string).IsAssignableFrom(type) && enumerableInterfaces.Length > 0;
    }

    private static Type[] GetIEnumerableInterfaces(Type type)
    {
        // Avoid expensive calculation when the type in question can't possibly implement IEnumerable<>.
        if (Type.GetTypeCode(type) != TypeCode.Object)
        {
            return [];
        }

        Type soughtType = typeof(IEnumerable<>);

        return type.GetClosedGenericInterfaces(soughtType);
    }

    private static Type GetTypeOfEnumeration(Type enumerableType)
    {
        Type interfaceType = GetIEnumerableInterfaces(enumerableType).Single();

        return interfaceType.GetGenericArguments().Single();
    }

    private static T[] ToArray<T>(IEnumerable<T> value)
    {
        try
        {
            return value?.ToArray();
        }
        catch (InvalidOperationException) when (value.GetType().Name.Equals("ImmutableArray`1", StringComparison.Ordinal))
        {
            // This is probably a default ImmutableArray<T>
            return [];
        }
    }
}
