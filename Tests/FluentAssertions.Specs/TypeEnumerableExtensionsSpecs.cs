using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TypeEnumerableExtensionsSpecs.BaseNamespace;
using TypeEnumerableExtensionsSpecs.BaseNamespace.Nested;
using TypeEnumerableExtensionsSpecs.Internal;
using Xunit;

namespace FluentAssertionsAsync.Specs
{
    public class TypeEnumerableExtensionsSpecs
    {
        [Fact]
        public void When_selecting_types_that_decorated_with_attribute_it_should_return_the_correct_type()
        {
            var types = new[]
            {
                typeof(JustAClass), typeof(ClassWithSomeAttribute), typeof(ClassDerivedFromClassWithSomeAttribute)
            };

            types.ThatAreDecoratedWith<SomeAttribute>()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(ClassWithSomeAttribute));
        }

        [Fact]
        public void When_selecting_types_that_decorated_with_attribute_or_inherit_it_should_return_the_correct_type()
        {
            var types = new[]
            {
                typeof(JustAClass), typeof(ClassWithSomeAttribute), typeof(ClassDerivedFromClassWithSomeAttribute)
            };

            types.ThatAreDecoratedWithOrInherit<SomeAttribute>()
                .Should()
                .HaveCount(2)
                .And.Contain(typeof(ClassWithSomeAttribute))
                .And.Contain(typeof(ClassDerivedFromClassWithSomeAttribute));
        }

        [Fact]
        public void When_selecting_types_that_not_decorated_with_attribute_it_should_return_the_correct_type()
        {
            var types = new[]
            {
                typeof(JustAClass), typeof(ClassWithSomeAttribute), typeof(ClassDerivedFromClassWithSomeAttribute)
            };

            types.ThatAreNotDecoratedWith<SomeAttribute>()
                .Should()
                .HaveCount(2)
                .And.Contain(typeof(JustAClass))
                .And.Contain(typeof(ClassDerivedFromClassWithSomeAttribute));
        }

        [Fact]
        public void When_selecting_types_that_not_decorated_with_attribute_or_inherit_it_should_return_the_correct_type()
        {
            var types = new[]
            {
                typeof(JustAClass), typeof(ClassWithSomeAttribute), typeof(ClassDerivedFromClassWithSomeAttribute)
            };

            types.ThatAreNotDecoratedWithOrInherit<SomeAttribute>()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(JustAClass));
        }

        [Fact]
        public void When_selecting_types_in_namespace_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(BaseNamespaceClass), typeof(NestedNamespaceClass) };

            types.ThatAreInNamespace(typeof(BaseNamespaceClass).Namespace)
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(BaseNamespaceClass));
        }

        [Fact]
        public void When_selecting_types_under_namespace_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(BaseNamespaceClass), typeof(NestedNamespaceClass) };

            types.ThatAreUnderNamespace(typeof(BaseNamespaceClass).Namespace)
                .Should()
                .HaveCount(2)
                .And.Contain(typeof(BaseNamespaceClass))
                .And.Contain(typeof(NestedNamespaceClass));
        }

        [Fact]
        public void When_selecting_derived_classes_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(SomeBaseClass), typeof(SomeClassDerivedFromSomeBaseClass) };

            types.ThatDeriveFrom<SomeBaseClass>()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(SomeClassDerivedFromSomeBaseClass));
        }

        [Fact]
        public void When_selecting_types_that_implement_interface_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(ClassImplementingJustAnInterface), typeof(IJustAnInterface) };

            types.ThatImplement<IJustAnInterface>()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(ClassImplementingJustAnInterface));
        }

        [Fact]
        public void When_selecting_only_the_classes_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(IJustAnInterface) };

            types.ThatAreClasses()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(JustAClass));
        }

        [Fact]
        public void When_selecting_not_a_classes_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(IJustAnInterface) };

            types.ThatAreNotClasses()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(IJustAnInterface));
        }

        [Fact]
        public void When_selecting_static_classes_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(AStaticClass) };

            types.ThatAreStatic()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(AStaticClass));
        }

        [Fact]
        public void When_selecting_not_a_static_classes_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(AStaticClass) };

            types.ThatAreNotStatic()
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(JustAClass));
        }

        [Fact]
        public void When_selecting_types_with_predicate_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(JustAClass), typeof(AStaticClass) };

            types.ThatSatisfy(t => t.IsSealed && t.IsAbstract)
                .Should()
                .ContainSingle()
                .Which.Should().Be(typeof(AStaticClass));
        }

        [Fact]
        public void When_unwrap_task_types_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(Task<JustAClass>), typeof(List<IJustAnInterface>) };

            types.UnwrapTaskTypes()
                .Should()
                .HaveCount(2)
                .And.Contain(typeof(JustAClass))
                .And.Contain(typeof(List<IJustAnInterface>));
        }

        [Fact]
        public void When_unwrap_enumerable_types_it_should_return_the_correct_type()
        {
            var types = new[] { typeof(Task<JustAClass>), typeof(List<IJustAnInterface>) };

            types.UnwrapEnumerableTypes()
                .Should()
                .HaveCount(2)
                .And.Contain(typeof(Task<JustAClass>))
                .And.Contain(typeof(IJustAnInterface));
        }
    }
}

#region Internal classes used in unit tests

namespace TypeEnumerableExtensionsSpecs.BaseNamespace
{
    internal class BaseNamespaceClass
    {
    }
}

namespace TypeEnumerableExtensionsSpecs.BaseNamespace.Nested
{
    internal class NestedNamespaceClass
    {
    }
}

namespace TypeEnumerableExtensionsSpecs.Internal
{
    internal interface IJustAnInterface
    {
    }

    internal class JustAClass
    {
    }

    internal static class AStaticClass
    {
    }

    internal class SomeBaseClass
    {
    }

    internal class SomeClassDerivedFromSomeBaseClass : SomeBaseClass
    {
    }

    internal class ClassImplementingJustAnInterface : IJustAnInterface
    {
    }

    [Some]
    internal class ClassWithSomeAttribute
    {
    }

    internal class ClassDerivedFromClassWithSomeAttribute : ClassWithSomeAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal class SomeAttribute : Attribute
    {
    }
}

#endregion
