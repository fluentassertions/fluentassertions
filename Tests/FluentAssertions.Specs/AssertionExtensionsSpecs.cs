using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Numeric;
using FluentAssertions.Primitives;
using FluentAssertions.Specialized;
using FluentAssertions.Types;
using Xunit;

namespace FluentAssertions.Specs;

public class AssertionExtensionsSpecs
{
    [Fact]
    public void Assertions_classes_override_equals()
    {
        // Arrange / Act
        var equalsOverloads = AllTypes.From(typeof(FluentAssertions.AssertionExtensions).Assembly)
            .ThatAreClasses()
            .Where(t => t.IsPublic && t.Name.TrimEnd('`', '1', '2', '3').EndsWith("Assertions", StringComparison.Ordinal))
            .Select(e => GetMostParentType(e))
            .Distinct()
            .Select(t => (type: t, overridesEquals: OverridesEquals(t)))
            .ToList();

        // Assert
        equalsOverloads.Should().OnlyContain(e => e.overridesEquals);
    }

    private static bool OverridesEquals(Type t)
    {
        MethodInfo equals = t.GetMethod("Equals", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public,
            null, [typeof(object)], null);

        return equals is not null;
    }

    public static TheoryData<object> ClassesWithGuardEquals =>
    [
        new ObjectAssertions<object, ObjectAssertions>(default, AssertionChain.GetOrCreate()),
        new BooleanAssertions<BooleanAssertions>(default, AssertionChain.GetOrCreate()),
        new DateTimeAssertions<DateTimeAssertions>(default, AssertionChain.GetOrCreate()),
        new DateTimeRangeAssertions<DateTimeAssertions>(default, AssertionChain.GetOrCreate(),  default, default, default),
        new DateTimeOffsetAssertions<DateTimeOffsetAssertions>(default, AssertionChain.GetOrCreate()),
        new DateTimeOffsetRangeAssertions<DateTimeOffsetAssertions>(default, AssertionChain.GetOrCreate(), default, default, default),
        new ExecutionTimeAssertions(new ExecutionTime(() => { }, () => new StopwatchTimer()), AssertionChain.GetOrCreate()),
        new GuidAssertions<GuidAssertions>(default, AssertionChain.GetOrCreate()),
        new MethodInfoSelectorAssertions(AssertionChain.GetOrCreate()),
        new NumericAssertions<int, NumericAssertions<int>>(default, AssertionChain.GetOrCreate()),
        new PropertyInfoSelectorAssertions(AssertionChain.GetOrCreate()),
        new SimpleTimeSpanAssertions<SimpleTimeSpanAssertions>(default, AssertionChain.GetOrCreate()),
        new TaskCompletionSourceAssertions<int>(default, AssertionChain.GetOrCreate()),
        new TypeSelectorAssertions(AssertionChain.GetOrCreate()),
        new EnumAssertions<StringComparison, EnumAssertions<StringComparison>>(default, AssertionChain.GetOrCreate()),
#if NET6_0_OR_GREATER
        new DateOnlyAssertions<DateOnlyAssertions>(default, AssertionChain.GetOrCreate()),
        new TimeOnlyAssertions<TimeOnlyAssertions>(default, AssertionChain.GetOrCreate()),
#endif
    ];

    [Theory]
    [MemberData(nameof(ClassesWithGuardEquals))]
    public void Guarding_equals_throws(object obj)
    {
        // Act
        Action act = () => obj.Equals(null);

        // Assert
        act.Should().ThrowExactly<NotSupportedException>();
    }

    [Theory]
    [InlineData(typeof(ReferenceTypeAssertions<object, ObjectAssertions>))]
    [InlineData(typeof(BooleanAssertions<BooleanAssertions>))]
    [InlineData(typeof(DateTimeAssertions<DateTimeAssertions>))]
    [InlineData(typeof(DateTimeRangeAssertions<DateTimeAssertions>))]
    [InlineData(typeof(DateTimeOffsetAssertions<DateTimeOffsetAssertions>))]
    [InlineData(typeof(DateTimeOffsetRangeAssertions<DateTimeOffsetAssertions>))]
    [InlineData(typeof(ExecutionTimeAssertions))]
    [InlineData(typeof(GuidAssertions<GuidAssertions>))]
    [InlineData(typeof(MethodInfoSelectorAssertions))]
    [InlineData(typeof(PropertyInfoSelectorAssertions))]
    [InlineData(typeof(SimpleTimeSpanAssertions<SimpleTimeSpanAssertions>))]
    [InlineData(typeof(TaskCompletionSourceAssertionsBase))]
    [InlineData(typeof(TypeSelectorAssertions))]
    [InlineData(typeof(EnumAssertions<StringComparison, EnumAssertions<StringComparison>>))]
#if NET6_0_OR_GREATER
    [InlineData(typeof(DateOnlyAssertions<DateOnlyAssertions>))]
    [InlineData(typeof(TimeOnlyAssertions<TimeOnlyAssertions>))]
#endif
    public void Fake_should_method_throws(Type type)
    {
        // Arrange
        MethodInfo fakeOverload = AllTypes.From(typeof(FluentAssertions.AssertionExtensions).Assembly)
            .ThatAreClasses()
            .ThatAreStatic()
            .Where(t => t.IsPublic)
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .Single(m => m.Name == "Should" && IsGuardOverload(m)
                && m.GetParameters().Single().ParameterType.Name == type.Name);

        if (type.IsConstructedGenericType)
        {
            fakeOverload = fakeOverload.MakeGenericMethod(type.GenericTypeArguments);
        }

        // Act
        Action act = () => fakeOverload.Invoke(null, [null]);

        // Assert
        act.Should()
            .ThrowExactly<TargetInvocationException>()
            .WithInnerExceptionExactly<InvalidOperationException>()
            .WithMessage("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'.");
    }

    [Fact]
    public void Should_methods_have_a_matching_overload_to_guard_against_chaining_and_constraints()
    {
        // Arrange / Act
        List<MethodInfo> shouldOverloads = AllTypes.From(typeof(FluentAssertions.AssertionExtensions).Assembly)
            .ThatAreClasses()
            .ThatAreStatic()
            .Where(t => t.IsPublic)
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .Where(m => m.Name == "Should")
            .ToList();

        List<Type> realOverloads =
        [
            ..shouldOverloads
                .Where(m => !IsGuardOverload(m))
                .Select(t => GetMostParentType(t.ReturnType))
                .Distinct(),

            // @jnyrup: DateTimeRangeAssertions and DateTimeOffsetRangeAssertions are manually added here,
            // because they expose AndConstraints,
            // and hence should have a guarding Should(DateTimeRangeAssertions _) overloads,
            // but they do not have a regular Should() overload,
            // as they are always constructed through the fluent API.
            typeof(DateTimeRangeAssertions<>),
            typeof(DateTimeOffsetRangeAssertions<>)
        ];

        List<Type> fakeOverloads = shouldOverloads
            .Where(m => IsGuardOverload(m))
            .Select(e => e.GetParameters()[0].ParameterType)
            .ToList();

        // Assert
        fakeOverloads.Should().BeEquivalentTo(realOverloads, opt => opt
                .Using<Type>(ctx => ctx.Subject.Name.Should().Be(ctx.Expectation.Name))
                .WhenTypeIs<Type>(),
            "AssertionExtensions.cs should have a guard overload of Should calling InvalidShouldCall()");
    }

    [Theory]
    [MemberData(nameof(GetShouldMethods), true)]
    public void Should_methods_returning_reference_or_nullable_type_assertions_are_annotated_with_not_null_attribute(MethodInfo method)
    {
        var notNullAttribute = method.GetParameters().Single().GetCustomAttribute<NotNullAttribute>();
        notNullAttribute.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(GetShouldMethods), false)]
    public void Should_methods_not_returning_reference_or_nullable_type_assertions_are_not_annotated_with_not_null_attribute(MethodInfo method)
    {
        var notNullAttribute = method.GetParameters().Single().GetCustomAttribute<NotNullAttribute>();
        notNullAttribute.Should().BeNull();
    }

    public static IEnumerable<object[]> GetShouldMethods(bool referenceOrNullableTypes)
    {
        return AllTypes.From(typeof(FluentAssertions.AssertionExtensions).Assembly)
            .ThatAreClasses()
            .ThatAreStatic()
            .Where(t => t.IsPublic)
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .Where(m => m.Name == "Should"
                && !IsGuardOverload(m)
                && m.GetParameters().Length == 1
                && (referenceOrNullableTypes ? IsReferenceOrNullableTypeAssertion(m) : !IsReferenceOrNullableTypeAssertion(m)))
            .Select(m => new object[] { m });
    }

    private static bool ReturnsReferenceTypeAssertions(MethodInfo m) =>
        m.ReturnType.IsAssignableToOpenGeneric(typeof(ReferenceTypeAssertions<,>));

    private static bool IsNullableTypeAssertion(MethodInfo m) =>
        m.GetParameters()[0].ParameterType.IsAssignableToOpenGeneric(typeof(Nullable<>));

    private static bool IsReferenceOrNullableTypeAssertion(MethodInfo m) =>
        ReturnsReferenceTypeAssertions(m) || IsNullableTypeAssertion(m);

    private static bool IsGuardOverload(MethodInfo m) =>
        m.ReturnType == typeof(void) && m.IsDefined(typeof(ObsoleteAttribute));

    private static Type GetMostParentType(Type type)
    {
        while (type.BaseType != typeof(object))
        {
            type = type.BaseType;
        }

        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }

        return type;
    }
}
