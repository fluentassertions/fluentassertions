using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Numeric;
using FluentAssertionsAsync.Primitives;
using FluentAssertionsAsync.Specialized;
using FluentAssertionsAsync.Types;
using Xunit;

namespace FluentAssertionsAsync.Specs;

public class AssertionExtensionsSpecs
{
    [Fact]
    public void Assertions_classes_override_equals()
    {
        // Arrange / Act
        var equalsOverloads = AllTypes.From(typeof(FluentAssertionsAsync.AssertionExtensions).Assembly)
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
            null, new[] { typeof(object) }, null);

        return equals is not null;
    }

    public static TheoryData<object> ClassesWithGuardEquals => new()
    {
        new ObjectAssertions<object, ObjectAssertions>(default),
        new BooleanAssertions<BooleanAssertions>(default),
        new DateTimeAssertions<DateTimeAssertions>(default),
        new DateTimeRangeAssertions<DateTimeAssertions>(default, default, default, default),
        new DateTimeOffsetAssertions<DateTimeOffsetAssertions>(default),
        new DateTimeOffsetRangeAssertions<DateTimeOffsetAssertions>(default, default, default, default),
        new ExecutionTimeAssertions(new ExecutionTime(() => { }, () => new StopwatchTimer())),
        new GuidAssertions<GuidAssertions>(default),
        new MethodInfoSelectorAssertions(),
        new NumericAssertions<int, NumericAssertions<int>>(default),
        new PropertyInfoSelectorAssertions(),
        new SimpleTimeSpanAssertions<SimpleTimeSpanAssertions>(default),
        new TaskCompletionSourceAssertions<int>(default),
        new TypeSelectorAssertions(),
        new EnumAssertions<StringComparison, EnumAssertions<StringComparison>>(default),
#if NET6_0_OR_GREATER
        new DateOnlyAssertions<DateOnlyAssertions>(default),
        new TimeOnlyAssertions<TimeOnlyAssertions>(default),
#endif
    };

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
    [InlineData(typeof(NumericAssertions<int, NumericAssertions<int>>))]
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
        MethodInfo fakeOverload = AllTypes.From(typeof(FluentAssertionsAsync.AssertionExtensions).Assembly)
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
        Action act = () => fakeOverload.Invoke(null, new object[] { null });

        // Assert
        act.Should()
            .ThrowExactly<TargetInvocationException>()
            .WithInnerExceptionExactly<InvalidOperationException>()
            .WithMessage("You are asserting the 'AndConstraint' itself. Remove the 'Should()' method directly following 'And'.");
    }

    [Fact]
    public async Task Should_methods_have_a_matching_overload_to_guard_against_chaining_and_constraints()
    {
        // Arrange / Act
        List<MethodInfo> shouldOverloads = AllTypes.From(typeof(FluentAssertionsAsync.AssertionExtensions).Assembly)
            .ThatAreClasses()
            .ThatAreStatic()
            .Where(t => t.IsPublic)
            .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public))
            .Where(m => m.Name == "Should")
            .ToList();

        List<Type> realOverloads = shouldOverloads
            .Where(m => !IsGuardOverload(m))
            .Select(t => GetMostParentType(t.ReturnType))
            .Distinct()
            .Concat(new[]
            {
                // @jnyrup: DateTimeRangeAssertions and DateTimeOffsetRangeAssertions are manually added here,
                // because they expose AndConstraints,
                // and hence should have a guarding Should(DateTimeRangeAssertions _) overloads,
                // but they do not have a regular Should() overload,
                // as they are always constructed through the fluent API.
                typeof(DateTimeRangeAssertions<>),
                typeof(DateTimeOffsetRangeAssertions<>),
            })
            .ToList();

        List<Type> fakeOverloads = shouldOverloads
            .Where(m => IsGuardOverload(m))
            .Select(e => e.GetParameters()[0].ParameterType)
            .ToList();

        // Assert
        await fakeOverloads.Should().BeEquivalentToAsync(realOverloads, opt => opt
                .Using<Type>(ctx => ctx.Subject.Name.Should().Be(ctx.Expectation.Name))
                .WhenTypeIs<Type>(),
            "AssertionExtensions.cs should have a guard overload of Should calling InvalidShouldCall()");
    }

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
