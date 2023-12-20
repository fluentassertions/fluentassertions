using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertionsAsync.Types;
using Internal.Main.Test;
using Xunit;

namespace FluentAssertionsAsync.Specs.Types;

public class PropertyInfoSelectorSpecs
{
    [Fact]
    public void When_property_info_selector_is_created_with_a_null_type_it_should_throw()
    {
        // Arrange
        PropertyInfoSelector propertyInfoSelector;

        // Act
        Action act = () => propertyInfoSelector = new PropertyInfoSelector((Type)null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("types");
    }

    [Fact]
    public void When_property_info_selector_is_created_with_a_null_type_list_it_should_throw()
    {
        // Arrange
        PropertyInfoSelector propertyInfoSelector;

        // Act
        Action act = () => propertyInfoSelector = new PropertyInfoSelector((Type[])null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("types");
    }

    [Fact]
    public void When_property_info_selector_is_null_then_should_should_throw()
    {
        // Arrange
        PropertyInfoSelector propertyInfoSelector = null;

        // Act
        Action act = () => propertyInfoSelector.Should();

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>()
            .WithParameterName("propertyInfoSelector");
    }

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
        Type type = typeof(TestClassForPropertySelectorWithInternalAndPublicProperties);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatArePublicOrInternal.ToArray();

        // Assert
        properties.Should().HaveCount(2);
    }

    [Fact]
    public void When_selecting_properties_that_are_abstract_it_should_return_only_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreAbstract.ToArray();

        // Assert
        properties.Should().HaveCount(2);
    }

    [Fact]
    public void When_selecting_properties_that_are_not_abstract_it_should_return_only_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotAbstract.ToArray();

        // Assert
        properties.Should().HaveCount(10);
    }

    [Fact]
    public void When_selecting_properties_that_are_static_it_should_return_only_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreStatic.ToArray();

        // Assert
        properties.Should().HaveCount(4);
    }

    [Fact]
    public void When_selecting_properties_that_are_not_static_it_should_return_only_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotStatic.ToArray();

        // Assert
        properties.Should().HaveCount(8);
    }

    [Fact]
    public void When_selecting_properties_that_are_virtual_it_should_return_only_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreVirtual.ToArray();

        // Assert
        properties.Should().HaveCount(7);
    }

    [Fact]
    public void When_selecting_properties_that_are_not_virtual_it_should_return_only_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotVirtual.ToArray();

        // Assert
        properties.Should().HaveCount(5);
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
        properties.Should().HaveCount(8);
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
    public void
        When_selecting_properties_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreDecoratedWith<DummyPropertyAttribute>().ToArray();

        // Assert
        properties.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_properties_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties =
            type.Properties().ThatAreDecoratedWithOrInherit<DummyPropertyAttribute>().ToArray();

        // Assert
        properties.Should().ContainSingle();
    }

    [Fact]
    public void
        When_selecting_properties_not_decorated_with_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties().ThatAreNotDecoratedWith<DummyPropertyAttribute>().ToArray();

        // Assert
        properties.Should().ContainSingle();
    }

    [Fact]
    public void
        When_selecting_properties_not_decorated_with_or_inheriting_an_inheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties =
            type.Properties().ThatAreNotDecoratedWithOrInherit<DummyPropertyAttribute>().ToArray();

        // Assert
        properties.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_properties_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties =
            type.Properties().ThatAreDecoratedWith<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

        // Assert
        properties.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_properties_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties()
            .ThatAreDecoratedWithOrInherit<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

        // Assert
        properties.Should().BeEmpty();
    }

    [Fact]
    public void
        When_selecting_properties_not_decorated_with_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties =
            type.Properties().ThatAreNotDecoratedWith<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

        // Assert
        properties.Should().ContainSingle();
    }

    [Fact]
    public void
        When_selecting_properties_not_decorated_with_or_inheriting_a_noninheritable_attribute_it_should_only_return_the_applicable_properties()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelectorWithNonInheritableAttributeDerived);

        // Act
        IEnumerable<PropertyInfo> properties = type.Properties()
            .ThatAreNotDecoratedWithOrInherit<DummyPropertyNonInheritableAttributeAttribute>().ToArray();

        // Assert
        properties.Should().ContainSingle();
    }

    [Fact]
    public async Task When_selecting_properties_return_types_it_should_return_the_correct_types()
    {
        // Arrange
        Type type = typeof(TestClassForPropertySelector);

        // Act
        IEnumerable<Type> returnTypes = type.Properties().ReturnTypes().ToArray();

        // Assert
        await returnTypes.Should()
            .BeEquivalentToAsync(new[]
            {
                typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string),
                typeof(string), typeof(int), typeof(int), typeof(int), typeof(int)
            });
    }

    public class ThatArePublicOrInternal
    {
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
        public void When_a_property_only_has_a_public_setter_it_should_be_included_in_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPublicSetter);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatArePublicOrInternal.ToArray();

            // Assert
            properties.Should().HaveCount(3);
        }

        private class TestClassForPublicSetter
        {
            private static string myPrivateStaticStringField;

            public static string PublicStaticStringProperty { set => myPrivateStaticStringField = value; }

            public static string InternalStaticStringProperty { get; set; }

            public int PublicIntProperty { get; init; }
        }

        [Fact]
        public void When_selecting_properties_with_at_least_one_accessor_being_private_should_return_the_applicable_properties()
        {
            // Arrange
            Type type = typeof(TestClassForPrivateAccessors);

            // Act
            IEnumerable<PropertyInfo> properties = type.Properties().ThatArePublicOrInternal.ToArray();

            // Assert
            properties.Should().HaveCount(4);
        }

        private class TestClassForPrivateAccessors
        {
            public bool PublicBoolPrivateGet { private get; set; }

            public bool PublicBoolPrivateSet { get; private set; }

            internal bool InternalBoolPrivateGet { private get; set; }

            internal bool InternalBoolPrivateSet { get; private set; }
        }
    }
}

#region Internal classes used in unit tests

internal class TestClassForPropertySelectorWithInternalAndPublicProperties
{
    public static string PublicStaticStringProperty { get; }

    internal static string InternalStaticStringProperty { get; set; }

    protected static string ProtectedStaticStringProperty { get; set; }

    private static string PrivateStaticStringProperty { get; set; }
}

internal abstract class TestClassForPropertySelector
{
    private static string myPrivateStaticStringField;

    public static string PublicStaticStringProperty { set => myPrivateStaticStringField = value; }

    internal static string InternalStaticStringProperty { get; set; }

    protected static string ProtectedStaticStringProperty { get; set; }

    private static string PrivateStaticStringProperty { get; set; }

    // An abstract method/property is implicitly a virtual method/property.
    public abstract string PublicAbstractStringProperty { get; set; }

    public abstract string PublicAbstractStringPropertyWithSetterOnly { set; }

    private string myPrivateStringField;

    public virtual string PublicVirtualStringProperty { set => myPrivateStringField = value; }

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
