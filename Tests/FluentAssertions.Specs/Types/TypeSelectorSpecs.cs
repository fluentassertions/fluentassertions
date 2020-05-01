using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions.Types;

using Internal.Main.Test;
using Internal.NotOnlyClasses.Test;
using Internal.Other.Test;
using Internal.Other.Test.Common;
using Internal.StaticAndNonStaticClasses.Test;
using Internal.UnwrapSelectorTestTypes.Test;
using Xunit;
using ISomeInterface = Internal.Main.Test.ISomeInterface;

namespace FluentAssertions.Specs
{
    public class TypeSelectorSpecs
    {
        [Fact]
        public void When_selecting_types_that_derive_from_a_specific_class_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(ClassDerivedFromSomeBaseClass).Assembly;

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
            Assembly assembly = typeof(ClassDerivedFromSomeGenericBaseClass).Assembly;

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
            Assembly assembly = typeof(ClassDerivedFromSomeBaseClass).Assembly;

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
            Assembly assembly = typeof(ClassDerivedFromSomeGenericBaseClass).Assembly;

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
            Assembly assembly = typeof(ClassImplementingSomeInterface).Assembly;

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
            Assembly assembly = typeof(ClassImplementingSomeInterface).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

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

        [Fact]
        public void When_selecting_global_types_from_global_namespace_it_should_succeed()
        {
            // Arrange
            TypeSelector types = new[] { typeof(ClassInGlobalNamespace) }.Types();

            // Act
            TypeSelector filteredTypes = types.ThatAreUnderNamespace(null);

            // Assert
            filteredTypes.As<IEnumerable<Type>>().Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_global_types_not_from_global_namespace_it_should_succeed()
        {
            // Arrange
            TypeSelector types = new[] { typeof(ClassInGlobalNamespace) }.Types();

            // Act
            TypeSelector filteredTypes = types.ThatAreNotUnderNamespace(null);

            // Assert
            filteredTypes.As<IEnumerable<Type>>().Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_local_types_from_global_namespace_it_should_succeed()
        {
            // Arrange
            TypeSelector types = new[] { typeof(SomeBaseClass) }.Types();

            // Act
            TypeSelector filteredTypes = types.ThatAreUnderNamespace(null);

            // Assert
            filteredTypes.As<IEnumerable<Type>>().Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_local_types_not_from_global_namespace_it_should_succeed()
        {
            // Arrange
            TypeSelector types = new[] { typeof(SomeBaseClass) }.Types();

            // Act
            TypeSelector filteredTypes = types.ThatAreNotUnderNamespace(null);

            // Assert
            filteredTypes.As<IEnumerable<Type>>().Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_a_prefix_of_a_namespace_it_should_not_match()
        {
            // Arrange
            TypeSelector types = new[] { typeof(SomeBaseClass) }.Types();

            // Act
            TypeSelector filteredTypes = types.ThatAreUnderNamespace("Internal.Main.Tes");

            // Assert
            filteredTypes.As<IEnumerable<Type>>().Should().BeEmpty();
        }

        [Fact]
        public void When_deselecting_a_prefix_of_a_namespace_it_should_not_match()
        {
            // Arrange
            TypeSelector types = new[] { typeof(SomeBaseClass) }.Types();

            // Act
            TypeSelector filteredTypes = types.ThatAreNotUnderNamespace("Internal.Main.Tes");

            // Assert
            filteredTypes.As<IEnumerable<Type>>().Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_types_that_are_classes_it_should_return_the_correct_types()
        {
            // Arrange
            TypeSelector types = new[] { typeof(NotOnlyClassesClass), typeof(NotOnlyClassesEnumeration), typeof(INotOnlyClassesInterface) }.Types();

            // Act
            IEnumerable<Type> filteredTypes = types.ThatAreClasses();

            // Assert
            filteredTypes.Should()
                .ContainSingle()
                .Which.Should().Be(typeof(NotOnlyClassesClass));
        }

        [Fact]
        public void When_selecting_types_that_are_not_classes_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(NotOnlyClassesClass).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreInNamespace("Internal.NotOnlyClasses.Test")
                .ThatAreNotClasses();

            // Assert
            types.Should()
                .HaveCount(2)
                .And.Contain(typeof(INotOnlyClassesInterface))
                .And.Contain(typeof(NotOnlyClassesEnumeration));
        }

        [Fact]
        public void When_selecting_types_that_are_static_classes_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(StaticClass).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreInNamespace("Internal.StaticAndNonStaticClasses.Test")
                .ThatAreStatic();

            // Assert
            types.Should()
                .ContainSingle()
                .Which.Should().Be(typeof(StaticClass));
        }

        [Fact]
        public void When_selecting_types_that_are_not_static_classes_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(StaticClass).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatAreInNamespace("Internal.StaticAndNonStaticClasses.Test")
                .ThatAreNotStatic();

            // Assert
            types.Should()
                .ContainSingle()
                .Which.Should().Be(typeof(NotAStaticClass));
        }

        [Fact]
        public void When_selecting_types_with_predicate_it_should_return_the_correct_types()
        {
            // Arrange
            Assembly assembly = typeof(SomeBaseClass).GetTypeInfo().Assembly;

            // Act
            IEnumerable<Type> types = AllTypes.From(assembly)
                .ThatSatisfy(t => t.GetCustomAttribute<SomeAttribute>() != null);

            // Assert
            types.Should()
                .HaveCount(3)
                .And.Contain(typeof(ClassWithSomeAttribute))
                .And.Contain(typeof(ClassWithSomeAttributeDerived))
                .And.Contain(typeof(ClassWithSomeAttributeThatImplementsSomeInterface));
        }

        [Fact]
        public void When_unwrap_task_types_it_should_return_the_correct_types()
        {
            IEnumerable<Type> types = typeof(ClassToExploreUnwrappedTaskTypes)
                .Methods()
                .ReturnTypes()
                .UnwrapTaskTypes();

            types.Should()
                .BeEquivalentTo(typeof(int), typeof(void), typeof(void), typeof(string), typeof(bool));
        }

        [Fact]
        public void When_unwrap_enumerable_types_it_should_return_the_correct_types()
        {
            IEnumerable<Type> types = typeof(ClassToExploreUnwrappedEnumerableTypes)
                .Methods()
                .ReturnTypes()
                .UnwrapEnumerableTypes();

            types.Should()
                .HaveCount(4)
                .And.Contain(typeof(IEnumerable))
                .And.Contain(typeof(bool))
                .And.Contain(typeof(int))
                .And.Contain(typeof(string));
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

namespace Internal.NotOnlyClasses.Test
{
    internal class NotOnlyClassesClass
    {
    }

    internal enum NotOnlyClassesEnumeration
    {
    }

    internal interface INotOnlyClassesInterface
    {
    }
}

namespace Internal.StaticAndNonStaticClasses.Test
{
    internal static class StaticClass
    {
    }

    internal class NotAStaticClass
    {
    }
}

namespace Internal.UnwrapSelectorTestTypes.Test
{
    internal class ClassToExploreUnwrappedTaskTypes
    {
        internal int DoWithInt() { return default; }

        internal Task DoWithTask() { return Task.CompletedTask; }

        internal ValueTask DoWithValueTask() { return new ValueTask(); }

        internal Task<string> DoWithIntTask() { return Task.FromResult(string.Empty); }

        internal ValueTask<bool> DoWithBoolValueTask() { return new ValueTask<bool>(false); }
    }

    internal class ClassToExploreUnwrappedEnumerableTypes
    {
        internal IEnumerable DoWithTask() { return default; }

        internal List<bool> DoWithIntTask() { return default; }

        internal ClassImplementingMultipleEnumerable DoWithBoolValueTask() { return default; }
    }

    internal class ClassImplementingMultipleEnumerable : IEnumerable<int>, IEnumerable<string>
    {
        private readonly IEnumerable<int> integers = new int[0];
        private readonly IEnumerable<string> strings = new string[0];

        public IEnumerator<int> GetEnumerator() => integers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)integers).GetEnumerator();

        IEnumerator<string> IEnumerable<string>.GetEnumerator() => strings.GetEnumerator();
    }
}

#pragma warning disable RCS1110 // Declare type inside namespace.
internal class ClassInGlobalNamespace { }
#pragma warning restore RCS1110

#endregion
