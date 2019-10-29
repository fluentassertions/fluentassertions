using System;
using System.Collections.Generic;
using System.Reflection;

using FluentAssertions.Types;

using Internal.Main.Test;
using Internal.Other.Test;
using Internal.Other.Test.Common;
using Xunit;

namespace FluentAssertions.Specs
{
    public class TypeSelectorSpecs
    {
        [Fact]
        public void When_selecting_types_that_derive_from_a_specific_class_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassDerivedFromSomeBaseClass).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly).ThatDeriveFrom<SomeBaseClass>();

            // Assert
            types.Should().ContainSingle()
                .Which.Should().Be(typeof(ClassDerivedFromSomeBaseClass));
        }

        [Fact]
        public void When_selecting_types_that_derive_from_a_specific_generic_class_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassDerivedFromSomeGenericBaseClass).GetTypeInfo().Assembly;

            // Act
            TypeSelector types = AllTypes.From(assembly).ThatDeriveFrom<SomeGenericBaseClass<int>>();

            // Assert
            types.ToArray().Should().ContainSingle()
                .Which.Should().Be(typeof(ClassDerivedFromSomeGenericBaseClass));
        }

        [Fact]
        public void When_selecting_types_that_do_not_derive_from_a_specific_class_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassDerivedFromSomeBaseClass).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreInNamespace("Internal.Main.Test")
                .ThatDoNotDeriveFrom<SomeBaseClass>();

            // Assert
            types.Should()
                .HaveCount(12);
        }

        [Fact]
        public void When_selecting_types_that_do_not_derive_from_a_specific_generic_class_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassDerivedFromSomeGenericBaseClass).GetTypeInfo().Assembly;

            // Act
            TypeSelector types = AllTypes.From(assembly)
                .ThatAreInNamespace("Internal.Main.Test")
                .ThatDoNotDeriveFrom<SomeGenericBaseClass<int>>();

            // Assert
            types.ToArray().Should()
                .HaveCount(12);
        }

        [Fact]
        public void When_selecting_types_that_implement_a_specific_interface_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassImplementingSomeInterface).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly).ThatImplement<ISomeInterface>();

            // Assert
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(ClassImplementingSomeInterface))
                .And.Contain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [Fact]
        public void When_selecting_types_that_do_not_implement_a_specific_interface_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassImplementingSomeInterface).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreInNamespace("Internal.Main.Test")
                .ThatDoNotImplement<ISomeInterface>();

            // Assert
            types.Should()
                .HaveCount(10);
        }

        [Fact]
        public void When_selecting_types_that_are_decorated_with_a_specific_attribute_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreDecoratedWith<SomeAttribute>();

            // Assert
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(ClassWithSomeAttribute))
                .And.Contain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [Fact]
        public void When_selecting_types_that_are_not_decorated_with_a_specific_attribute_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreNotDecoratedWith<SomeAttribute>();

            // Assert
            types.Should()
                .NotBeEmpty()
                .And.NotContain(typeof(ClassWithSomeAttribute))
                .And.NotContain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [Fact]
        public void When_selecting_types_from_specific_namespace_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreInNamespace("Internal.Other.Test");

            // Assert
            types.Should().ContainSingle()
                .Which.Should().Be(typeof(SomeOtherClass));
        }

        [Fact]
        public void When_selecting_types_other_than_from_specific_namespace_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreUnderNamespace("Internal.Other")
                .ThatAreNotInNamespace("Internal.Other.Test");

            // Assert
            types.Should()
                .ContainSingle()
                .Which.Should().Be(typeof(SomeCommonClass));
        }

        [Fact]
        public void When_selecting_types_from_specific_namespace_or_sub_namespaces_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly).ThatAreUnderNamespace("Internal.Other.Test");

            // Assert
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(SomeOtherClass))
                .And.Contain(typeof(SomeCommonClass));
        }

        [Fact]
        public void When_selecting_types_other_than_from_specific_namespace_or_sub_namespaces_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreUnderNamespace("Internal.Other")
                .ThatAreNotUnderNamespace("Internal.Other.Test");

            // Assert
            types.Should()
                .BeEmpty();
        }

        [Fact]
        public void When_combining_type_selection_filters_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreDecoratedWith<SomeAttribute>()
                .ThatImplement<ISomeInterface>()
                .ThatAreInNamespace("Internal.Main.Test");

            // Assert
            types.Should().ContainSingle()
                .Which.Should().Be(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [Fact]
        public void When_using_the_single_type_ctor_of_TypeSelector_it_should_contain_that_singe_type()
        {
            // Arrange
            Type type = typeof(ClassWithSomeAttribute);

            // Act
            var typeSelector = new TypeSelector(type);

            // Assert
            typeSelector
                .ToArray()
                .Should()
                .ContainSingle()
                    .Which.Should().Be(type);
        }

        [Fact]
        public void When_selecting_types_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreDecoratedWith<SomeAttribute>();

            // Assert
            types.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_types_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreDecoratedWithOrInherit<SomeAttribute>();

            // Assert
            types.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_types_not_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreNotDecoratedWith<SomeAttribute>();

            // Assert
            types.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_types_not_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreNotDecoratedWithOrInherit<SomeAttribute>();

            // Assert
            types.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_types_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeNonInheritableAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreDecoratedWith<SomeAttribute>();

            // Assert
            types.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_types_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeNonInheritableAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreDecoratedWithOrInherit<SomeAttribute>();

            // Assert
            types.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_types_not_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeNonInheritableAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreNotDecoratedWith<SomeAttribute>();

            // Assert
            types.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_types_not_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_types()
        {
            // Arrange
            Type type = typeof(ClassWithSomeNonInheritableAttributeDerived);

            // Act
            IEnumerable<Type> types = type.Types().ThatAreNotDecoratedWithOrInherit<SomeAttribute>();

            // Assert
            types.Should().ContainSingle();
        }
    }
}

#region Internal classes used in unit tests

namespace Internal.Main.Test
{
    internal class SomeBaseClass
    {
    }

    internal class ClassDerivedFromSomeBaseClass : SomeBaseClass
    {
    }

    internal class SomeGenericBaseClass<T>
    {
        public T Value { get; set; }
    }

    internal class ClassDerivedFromSomeGenericBaseClass : SomeGenericBaseClass<int>
    {
    }

    internal interface ISomeInterface
    {
    }

    internal class ClassImplementingSomeInterface : ISomeInterface
    {
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    internal class SomeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal class SomeNonInheritableAttribute : Attribute
    {
    }

    [Some]
    internal class ClassWithSomeAttribute
    {
        public string Property1 { get; set; }

        public void Method1()
        {
        }
    }

    internal class ClassWithSomeAttributeDerived : ClassWithSomeAttribute
    {
    }

    [SomeNonInheritable]
    internal class ClassWithSomeNonInheritableAttribute
    {
        public string Property1 { get; set; }

        public void Method1()
        {
        }
    }

    internal class ClassWithSomeNonInheritableAttributeDerived : ClassWithSomeNonInheritableAttribute
    {
    }

    [Some]
    internal class ClassWithSomeAttributeThatImplementsSomeInterface : ISomeInterface
    {
        public string Property2 { get; set; }

        public void Method2()
        {
        }
    }
}

namespace Internal.Other.Test
{
    internal class SomeOtherClass
    {
    }
}

namespace Internal.Other.Test.Common
{
    internal class SomeCommonClass
    {
    }
}

#endregion
