#if NETCOREAPP3_0_OR_GREATER
using System.Globalization;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using JetBrains.Annotations;
using Xunit;

namespace FluentAssertions.Specs.Common;

public class TypeMemberReflectorSpecs
{
    public class GetProperties
    {
        [Fact]
        public void Can_get_all_public_explicit_and_default_interface_properties()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass),
                MemberVisibility.Public | MemberVisibility.ExplicitlyImplemented | MemberVisibility.DefaultInterfaceProperties);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new { Name = "NormalProperty", PropertyType = typeof(string) },
                new { Name = "NewProperty", PropertyType = typeof(int) },
                new { Name = "InterfaceProperty", PropertyType = typeof(string) },
                new
                {
                    Name = $"{typeof(IInterfaceWithSingleProperty).FullName!.Replace("+", ".")}.ExplicitlyImplementedProperty",
                    PropertyType = typeof(string)
                },
                new { Name = "DefaultProperty", PropertyType = typeof(string) }
            });
        }

        [Fact]
        public void Can_get_all_properties_from_an_interface()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(IInterfaceWithDefaultProperty),
                MemberVisibility.Public);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new { Name = "InterfaceProperty", PropertyType = typeof(string) },
                new { Name = "ExplicitlyImplementedProperty", PropertyType = typeof(string), },
                new { Name = "DefaultProperty", PropertyType = typeof(string) }
            });
        }

        [Fact]
        public void Can_get_normal_public_properties()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.Public);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new { Name = "NormalProperty", PropertyType = typeof(string) },
                new { Name = "NewProperty", PropertyType = typeof(int) },
                new { Name = "InterfaceProperty", PropertyType = typeof(string) },
            });
        }

        [Fact]
        public void Can_get_explicit_properties_only()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.ExplicitlyImplemented);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Name = $"{typeof(IInterfaceWithSingleProperty).FullName!.Replace("+", ".")}.ExplicitlyImplementedProperty",
                    PropertyType = typeof(string)
                },
            });
        }

        [Fact]
        public void Prefers_normal_property_over_explicitly_implemented_one()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(ClassWithExplicitAndNormalProperty),
                MemberVisibility.Public | MemberVisibility.ExplicitlyImplemented);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Name = "ExplicitlyImplementedProperty",
                    PropertyType = typeof(int)
                },
            });
        }

        [Fact]
        public void Can_get_default_interface_properties_only()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.DefaultInterfaceProperties);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new { Name = "DefaultProperty", PropertyType = typeof(string) },
            });
        }

        [Fact]
        public void Can_get_internal_properties()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.Internal);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new { Name = "InternalProperty", PropertyType = typeof(bool) },
                new { Name = "InternalProtectedProperty", PropertyType = typeof(bool) }
            });
        }

        [Fact]
        public void Will_ignore_indexers()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(ClassWithIndexer), MemberVisibility.Public);

            // Assert
            reflector.Properties.Should().BeEquivalentTo(new[]
            {
                new { Name = "Foo", PropertyType = typeof(object) }
            });
        }
    }

    public class GetFields
    {
        [Fact]
        public void Can_find_public_fields()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.Public);

            // Assert
            reflector.Fields.Should().BeEquivalentTo(new[]
            {
                new { Name = "NormalField", FieldType = typeof(string) },
            });
        }

        [Fact]
        public void Can_find_internal_fields()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.Internal);

            // Assert
            reflector.Fields.Should().BeEquivalentTo(new[]
            {
                new { Name = "InternalField", FieldType = typeof(string) },
                new { Name = "ProtectedInternalField", FieldType = typeof(string) },
            });
        }

        [Fact]
        public void Can_find_all_fields()
        {
            // Act
            var reflector = new TypeMemberReflector(typeof(SuperClass), MemberVisibility.Internal | MemberVisibility.Public);

            // Assert
            reflector.Fields.Should().BeEquivalentTo(new[]
            {
                new { Name = "NormalField", FieldType = typeof(string) },
                new { Name = "InternalField", FieldType = typeof(string) },
                new { Name = "ProtectedInternalField", FieldType = typeof(string) }
            });
        }
    }

    private class SuperClass : BaseClass, IInterfaceWithDefaultProperty
    {
        public string NormalProperty { get; set; }

        public new int NewProperty { get; set; }

        internal bool InternalProperty { get; set; }

        protected internal bool InternalProtectedProperty { get; set; }

        string IInterfaceWithSingleProperty.ExplicitlyImplementedProperty { get; set; }

        public string InterfaceProperty { get; set; }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public string NormalField;

        internal string InternalField;

        protected internal string ProtectedInternalField;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
    }

    private class ClassWithExplicitAndNormalProperty : IInterfaceWithSingleProperty
    {
        string IInterfaceWithSingleProperty.ExplicitlyImplementedProperty { get; set; }

        [UsedImplicitly]
        public int ExplicitlyImplementedProperty { get; set; }
    }

    private class BaseClass
    {
        [UsedImplicitly]
        public string NewProperty { get; set; }
    }

    private interface IInterfaceWithDefaultProperty : IInterfaceWithSingleProperty
    {
        [UsedImplicitly]
        string InterfaceProperty { get; set; }

        [UsedImplicitly]
        string DefaultProperty => "Default";
    }

    private interface IInterfaceWithSingleProperty
    {
        [UsedImplicitly]
        string ExplicitlyImplementedProperty { get; set; }
    }

    private class ClassWithIndexer
    {
        [UsedImplicitly]
        public object Foo { get; set; }

        public string this[int n] => n.ToString(CultureInfo.InvariantCulture);
    }
}

#endif
