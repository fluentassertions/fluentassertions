using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs.Types;

public class TypeExtensionsSpecs
{
    [Fact]
    public void When_comparing_types_and_types_are_same_it_should_return_true()
    {
        // Arrange
        var type1 = typeof(InheritedType);
        var type2 = typeof(InheritedType);

        // Act
        bool result = type1.IsSameOrInherits(type2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void When_comparing_types_and_first_type_inherits_second_it_should_return_true()
    {
        // Arrange
        var type1 = typeof(InheritingType);
        var type2 = typeof(InheritedType);

        // Act
        bool result = type1.IsSameOrInherits(type2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void When_comparing_types_and_second_type_inherits_first_it_should_return_false()
    {
        // Arrange
        var type1 = typeof(InheritedType);
        var type2 = typeof(InheritingType);

        // Act
        bool result = type1.IsSameOrInherits(type2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void When_comparing_types_and_types_are_different_it_should_return_false()
    {
        // Arrange
        var type1 = typeof(string);
        var type2 = typeof(InheritedType);

        // Act
        bool result = type1.IsSameOrInherits(type2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void When_getting_explicit_conversion_operator_from_a_type_with_fake_conversion_operators_it_should_not_return_any()
    {
        // Arrange
        var type1 = typeof(TypeWithFakeConversionOperators);
        var type2 = typeof(byte);

        // Act
        MethodInfo result = type1.GetExplicitConversionOperator(type1, type2);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void When_getting_implicit_conversion_operator_from_a_type_with_fake_conversion_operators_it_should_not_return_any()
    {
        // Arrange
        var type1 = typeof(TypeWithFakeConversionOperators);
        var type2 = typeof(int);

        // Act
        MethodInfo result = type1.GetImplicitConversionOperator(type1, type2);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void When_getting_fake_explicit_conversion_operator_from_a_type_with_fake_conversion_operators_it_should_return_one()
    {
        // Arrange
        var type = typeof(TypeWithFakeConversionOperators);
        string name = "op_Explicit";
        var bindingAttr = BindingFlags.Public | BindingFlags.Static;
        var returnType = typeof(byte);

        // Act
        MethodInfo result = GetFakeConversionOperator(type, name, bindingAttr, returnType);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void When_getting_fake_implicit_conversion_operator_from_a_type_with_fake_conversion_operators_it_should_return_one()
    {
        // Arrange
        var type = typeof(TypeWithFakeConversionOperators);
        string name = "op_Implicit";
        var bindingAttr = BindingFlags.Public | BindingFlags.Static;
        var returnType = typeof(int);

        // Act
        MethodInfo result = GetFakeConversionOperator(type, name, bindingAttr, returnType);

        // Assert
        result.Should().NotBeNull();
    }

    [Theory]
    [InlineData(typeof(MyRecord), true)]
    [InlineData(typeof(MyRecordStruct), true)]
    [InlineData(typeof(MyRecordStructWithOverriddenEquality), true)]
    [InlineData(typeof(MyReadonlyRecordStruct), true)]
    [InlineData(typeof(MyStruct), false)]
    [InlineData(typeof(MyStructWithOverriddenEquality), false)]
    [InlineData(typeof(MyClass), false)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(string), false)]
    public void IsRecord_should_detect_records_correctly(Type type, bool expected)
    {
        type.IsRecord().Should().Be(expected);
    }

    [Fact]
    public void When_checking_if_anonymous_type_is_record_it_should_return_false()
    {
        new { Value = 42 }.GetType().IsRecord().Should().Be(false);
    }

    [Fact]
    public void When_checking_if_class_with_multiple_equality_methods_is_record_it_should_return_false()
    {
        typeof(ImmutableArray<int>).IsRecord().Should().Be(false);
    }

    private static MethodInfo GetFakeConversionOperator(Type type, string name, BindingFlags bindingAttr, Type returnType)
    {
        MethodInfo[] methods = type.GetMethods(bindingAttr);
        return methods.SingleOrDefault(m =>
            m.Name == name
            && m.ReturnType == returnType
            && m.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { type })
            );
    }

    private class InheritedType { }

    private class InheritingType : InheritedType { }

    private readonly struct TypeWithFakeConversionOperators
    {
        private readonly int value;

        private TypeWithFakeConversionOperators(int value)
        {
            this.value = value;
        }

#pragma warning disable IDE1006, SA1300 // These two functions mimic the compiler generated conversion operators
        public static int op_Implicit(TypeWithFakeConversionOperators typeWithFakeConversionOperators) => typeWithFakeConversionOperators.value;

        public static byte op_Explicit(TypeWithFakeConversionOperators typeWithFakeConversionOperators) => (byte)typeWithFakeConversionOperators.value;
#pragma warning restore SA1300, IDE1006
    }

    private record MyRecord(int Value);

    private record struct MyRecordStruct(int Value);

    private record struct MyRecordStructWithOverriddenEquality(int Value)
    {
        public bool Equals(MyRecordStructWithOverriddenEquality other) => Value == other.Value;

        public override int GetHashCode() => Value;
    }

    private readonly record struct MyReadonlyRecordStruct(int Value);

    private struct MyStruct
    {
        public int Value { get; set; }
    }

    private struct MyStructWithOverriddenEquality : IEquatable<MyStructWithOverriddenEquality>
    {
        public int Value { get; set; }

        public bool Equals(MyStructWithOverriddenEquality other) => Value == other.Value;

        public override bool Equals(object obj) => obj is MyStructWithOverriddenEquality other && Equals(other);

        public override int GetHashCode() => Value;

        public static bool operator ==(MyStructWithOverriddenEquality left, MyStructWithOverriddenEquality right) => left.Equals(right);

        public static bool operator !=(MyStructWithOverriddenEquality left, MyStructWithOverriddenEquality right) => !left.Equals(right);
    }

    private class MyClass
    {
        public int Value { get; set; }
    }
}
