using System;
using System.Collections.Generic;
using System.Reflection;

using Internal.Main.Test;
using Xunit;

namespace FluentAssertions.Specs
{
    public class MethodInfoSelectorSpecs
    {
        [Fact]
        public void When_selecting_methods_from_types_in_an_assembly_it_should_return_the_applicable_methods()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

            // Act
            IEnumerable<MethodInfo> methods = assembly.Types()
                .ThatAreDecoratedWith<SomeAttribute>()
                .Methods();

            // Assert
            methods.Should()
                .HaveCount(2)
                .And.Contain(m => m.Name == "Method1")
                .And.Contain(m => m.Name == "Method2");
        }

        [Fact]
        public void When_selecting_methods_that_are_public_or_internal_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatArePublicOrInternal;

            // Assert
            const int PublicMethodCount = 2;
            const int InternalMethodCount = 1;
            methods.Should().HaveCount(PublicMethodCount + InternalMethodCount);
        }

        [Fact]
        public void When_selecting_methods_decorated_with_specific_attribute_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWith<DummyMethodAttribute>().ToArray();

            // Assert
            methods.Should().HaveCount(2);
        }

        [Fact]
        public void When_selecting_methods_not_decorated_with_specific_attribute_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWith<DummyMethodAttribute>().ToArray();

            // Assert
            methods.Should()
                .NotBeEmpty()
                .And.NotContain(m => m.Name == "PublicVirtualVoidMethodWithAttribute")
                .And.NotContain(m => m.Name == "ProtectedVirtualVoidMethodWithAttribute");
        }

        [Fact]
        public void When_selecting_methods_that_return_a_specific_type_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatReturn<string>().ToArray();

            // Assert
            methods.Should().HaveCount(2);
        }

        [Fact]
        public void When_selecting_methods_that_do_not_return_a_specific_type_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatDoNotReturn<string>().ToArray();

            // Assert
            methods.Should().HaveCount(5);
        }

        [Fact]
        public void When_selecting_methods_without_return_value_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatReturnVoid.ToArray();

            // Assert
            methods.Should().HaveCount(4);
        }

        [Fact]
        public void When_selecting_methods_with_return_value_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatDoNotReturnVoid.ToArray();

            // Assert
            methods.Should().HaveCount(3);
        }

        [Fact]
        public void When_combining_filters_to_filter_methods_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelector);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods()
                .ThatArePublicOrInternal
                .ThatReturnVoid
                .ToArray();

            // Assert
            methods.Should().HaveCount(2);
        }

        [Fact]
        public void When_selecting_methods_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWith<DummyMethodAttribute>().ToArray();

            // Assert
            methods.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_methods_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWithOrInherit<DummyMethodAttribute>().ToArray();

            // Assert
            methods.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_methods_not_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWith<DummyMethodAttribute>().ToArray();

            // Assert
            methods.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_methods_not_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWithOrInherit<DummyMethodAttribute>().ToArray();

            // Assert
            methods.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_methods_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWith<DummyMethodNonInheritableAttributeAttribute>().ToArray();

            // Assert
            methods.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_methods_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreDecoratedWithOrInherit<DummyMethodNonInheritableAttributeAttribute>().ToArray();

            // Assert
            methods.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_methods_not_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWith<DummyMethodNonInheritableAttributeAttribute>().ToArray();

            // Assert
            methods.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_methods_not_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForMethodSelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<MethodInfo> methods = type.Methods().ThatAreNotDecoratedWithOrInherit<DummyMethodNonInheritableAttributeAttribute>().ToArray();

            // Assert
            methods.Should().ContainSingle();
        }
    }

    #region Internal classes used in unit tests

    internal class TestClassForMethodSelector
    {
#pragma warning disable 67 // "event is never used"
        public event EventHandler SomethingChanged = delegate { };
#pragma warning restore 67

        public virtual void PublicVirtualVoidMethod()
        {
        }

        [DummyMethod]
        public virtual void PublicVirtualVoidMethodWithAttribute()
        {
        }

        internal virtual int InternalVirtualIntMethod()
        {
            return 0;
        }

        [DummyMethod]
        protected virtual void ProtectedVirtualVoidMethodWithAttribute()
        {
        }

        private void PrivateVoidDoNothing()
        {
        }

        protected virtual string ProtectedVirtualStringMethod()
        {
            return "";
        }

        private string PrivateStringMethod()
        {
            return "";
        }
    }

    internal class TestClassForMethodSelectorWithInheritableAttribute
    {
        [DummyMethod]
        public virtual void PublicVirtualVoidMethodWithAttribute() { }
    }

    internal class TestClassForMethodSelectorWithNonInheritableAttribute
    {
        [DummyMethodNonInheritableAttribute]
        public virtual void PublicVirtualVoidMethodWithAttribute() { }
    }

    internal class TestClassForMethodSelectorWithInheritableAttributeDerived : TestClassForMethodSelectorWithInheritableAttribute
    {
        public override void PublicVirtualVoidMethodWithAttribute() { }
    }

    internal class TestClassForMethodSelectorWithNonInheritableAttributeDerived : TestClassForMethodSelectorWithNonInheritableAttribute
    {
        public override void PublicVirtualVoidMethodWithAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class DummyMethodNonInheritableAttributeAttribute : Attribute
    {
        public bool Filter { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class DummyMethodAttribute : Attribute
    {
        public bool Filter { get; set; }
    }

    #endregion
}
