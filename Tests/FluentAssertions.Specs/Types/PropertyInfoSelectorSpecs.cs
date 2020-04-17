using System;
using System.Collections.Generic;
using System.Reflection;

using Internal.Main.Test;
using Xunit;

namespace FluentAssertions.Specs
{
    public class PropertyInfoSelectorSpecs
    {
        [Fact]
        public void When_selecting_properties_from_types_in_an_assembly_it_should_return_the_applicable_properties()
        {
            // Arrange
            Assembly assembly = typeof(ClassWithSomeAttribute).Assembly;

            // Act
            IEnumerable<PropertyInfo> properties = assembly.Types()
                .ThatAreDecoratedWith<SomeAttribute>()
                .Properties();

            // Assert
            properties.Should()
                .HaveCount(2)
                .And.Contain(m => m.Name == "Property1")
                .And.Contain(m => m.Name == "Property2");
        }

        [Fact]
        public void When_selecting_properties_that_are_public_or_internal_it_should_return_only_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelector);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatArePublicOrInternal.ToArray();

            // Assert
            const int PublicPropertyCount = 3;
            const int InternalPropertyCount = 1;
            properties.Should().HaveCount(PublicPropertyCount + InternalPropertyCount);
        }

        [Fact]
        public void When_selecting_properties_decorated_with_specific_attribute_it_should_return_only_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelector);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWith<DummyPropertyAttribute>().ToArray();

            // Assert
            properties.Should().HaveCount(2);
        }

        [Fact]
        public void When_selecting_properties_not_decorated_with_specific_attribute_it_should_return_only_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelector);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotDecoratedWith<DummyPropertyAttribute>().ToArray();

            // Assert
            properties.Should()
                .NotBeEmpty()
                .And.NotContain(p => p.Name == "PublicVirtualStringPropertyWithAttribute")
                .And.NotContain(p => p.Name == "ProtectedVirtualIntPropertyWithAttribute");
        }

        [Fact]
        public void When_selecting_methods_that_return_a_specific_type_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelector);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().OfType<string>().ToArray();

            // Assert
            properties.Should().HaveCount(2);
        }

        [Fact]
        public void When_selecting_methods_that_do_not_return_a_specific_type_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelector);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().NotOfType<string>().ToArray();

            // Assert
            properties.Should().HaveCount(4);
        }

        [Fact]
        public void When_combining_filters_to_filter_methods_it_should_return_only_the_applicable_methods()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelector);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties()
                .ThatArePublicOrInternal
                .OfType<string>()
                .ThatAreDecoratedWith<DummyPropertyAttribute>()
                .ToArray();

            // Assert
            properties.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_properties_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWith<DummyPropertyAttribute>().ToArray();

            // Assert
            properties.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_properties_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWithOrInherit<DummyPropertyAttribute>().ToArray();

            // Assert
            properties.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_properties_not_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotDecoratedWith<DummyPropertyAttribute>().ToArray();

            // Assert
            properties.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_properties_not_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotDecoratedWithOrInherit<DummyPropertyAttribute>().ToArray();

            // Assert
            properties.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_properties_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWith<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

            // Assert
            properties.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_properties_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWithOrInherit<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

            // Assert
            properties.Should().BeEmpty();
        }

        [Fact]
        public void When_selecting_properties_not_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotDecoratedWith<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

            // Assert
            properties.Should().ContainSingle();
        }

        [Fact]
        public void When_selecting_properties_not_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotDecoratedWithOrInherit<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

            // Assert
            properties.Should().ContainSingle();
        }
    }

    #region Internal classes used in unit tests

    internal class TestClassForPropertySelector
    {
        public virtual string PublicVirtualStringProperty { get; set; }

        [DummyProperty]
        public virtual string PublicVirtualStringPropertyWithAttribute { get; set; }

        public virtual int PublicVirtualIntPropertyWithPrivateSetter { get; private set; }

        internal virtual int InternalVirtualIntPropertyWithPrivateSetter { get; private set; }

        [DummyProperty]
        protected virtual int ProtectedVirtualIntPropertyWithAttribute { get; set; }

        private int PrivateIntProperty { get; set; }
    }

    internal class TestClassForPropertySelectorWithInheritableAttribute
    {
        [DummyProperty]
        public virtual string PublicVirtualStringPropertyWithAttribute { get; set; }
    }

    internal class TestClassForPropertySelectorWithNonInheritableAttribute
    {
        [DummyPropertyNonInheritableAttribute]
        public virtual string PublicVirtualStringPropertyWithAttribute { get; set; }
    }

    internal class TestClassForPropertySelectorWithInheritableAttributeDerived : TestClassForPropertySelectorWithInheritableAttribute
    {
        public override string PublicVirtualStringPropertyWithAttribute { get; set; }
    }

    internal class TestClassForPropertySelectorWithNonInheritableAttributeDerived : TestClassForPropertySelectorWithNonInheritableAttribute
    {
        public override string PublicVirtualStringPropertyWithAttribute { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class DummyPropertyNonInheritableAttributeAttribute : Attribute
    {
        public DummyPropertyNonInheritableAttributeAttribute()
        {
        }

        public DummyPropertyNonInheritableAttributeAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DummyPropertyAttribute : Attribute
    {
        public DummyPropertyAttribute()
        {
        }

        public DummyPropertyAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    #endregion
}
