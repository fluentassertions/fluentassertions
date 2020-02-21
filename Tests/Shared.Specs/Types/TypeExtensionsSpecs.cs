using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs
{
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

        private struct TypeWithFakeConversionOperators
        {
            private readonly int value;

            private TypeWithFakeConversionOperators(int value)
            {
                this.value = value;
            }

#pragma warning disable IDE1006 // Naming Styles
            public static int op_Implicit(TypeWithFakeConversionOperators typeWithFakeConversionOperators) => typeWithFakeConversionOperators.value;
            public static byte op_Explicit(TypeWithFakeConversionOperators typeWithFakeConversionOperators) => (byte)typeWithFakeConversionOperators.value;
#pragma warning restore IDE1006 // Naming Styles
        }
    }
}
