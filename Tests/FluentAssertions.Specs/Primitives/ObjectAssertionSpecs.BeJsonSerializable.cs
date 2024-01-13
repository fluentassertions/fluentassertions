#if NET6_0_OR_GREATER
using System;
using System.Text.Json.Serialization;
using Xunit;

namespace FluentAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeJsonSerializable
    {
        [Fact]
        public void Can_serialize_simple_classes()
        {
            // Arrange
            var subject = new SimpleClassWithPrimitiveTypes()
            {
                Integer = 1,
                Long = long.MaxValue - 2,
                Decimal = 0.2M,
                Double = 0.3,
                Float = 42.0F,
                Guid = Guid.NewGuid(),
                String = "foo",
                DateTime = DateTime.UtcNow,
                Bool = true,
                ByteArray = [0x20, 0x21, 0x22, 0x23],
                Char = 'c'
            };

            // Act / Assert
            subject.Should().BeJsonSerializable();
        }

        [Fact]
        public void Can_serialize_nested_classes()
        {
            // Arrange
            var subject = new ClassWithNestedClasses
            {
                Id = 1,
                MestedClass = new NestedClass
                {
                    Value = "foo"
                }
            };

            // Act / Assert
            subject.Should().BeJsonSerializable();
        }

        [Fact]
        public void Cannot_serialize_classes_without_default_constructor()
        {
            // Arrange
            const string reasonText = "this is the reason";
            var subject = new ClassWithoutDefaultConstructor(10);

            // Act
            Action act = () => subject.Should().BeJsonSerializable(reasonText);

            // Assert
            act.Should().Throw<Xunit.Sdk.XunitException>()
                .WithMessage($"*to be JSON serializable*{reasonText}*but serializing*failed with*");
        }

        [Fact]
        public void Cannot_serialize_classes_with_ignored_property()
        {
            // Arrange
            const string reasonText = "it has an ignored property";
            var subject = new SimpleClassWithIgnoredProperty()
            {
                IgnoredProperty = DateTime.Now
            };

            // Act
            Action act = () => subject.Should().BeJsonSerializable(reasonText);

            // Assert
            act.Should().Throw<Xunit.Sdk.XunitException>()
                .WithMessage($"*to be JSON serializable*{reasonText}*but serializing*failed with*");
        }

        [Fact]
        public void Use_explicit_type_for_serialization()
        {
            // Arrange
            var subject = new SimpleClassWithIgnoredProperty()
            {
                IgnoredProperty = DateTime.Now
            };

            // Act / Assert
            subject.Should().BeJsonSerializable<SimpleClassWithPrimitiveTypes>();
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Local
        private class SimpleClassWithIgnoredProperty : SimpleClassWithPrimitiveTypes
        {
            [JsonIgnore]
            public DateTime IgnoredProperty { get; set; }
        }

        private class SimpleClassWithPrimitiveTypes
        {
            public int Integer { get; set; }

            public long Long { get; set; }

            public Guid Guid { get; set; }

            public string String { get; set; }

            public DateTime DateTime { get; set; }

            public decimal Decimal { get; set; }

            public double Double { get; set; }

            public float Float { get; set; }

            public bool Bool { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
            public byte[] ByteArray { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

            public char Char { get; set; }
        }

        private class ClassWithoutDefaultConstructor
        {
            public int Id { get; }

            public ClassWithoutDefaultConstructor(int value)
            {
                Id = value;
            }
        }

        private class ClassWithNestedClasses
        {
            public int Id { get; set; }

            public NestedClass MestedClass { get; set; }
        }

        private class NestedClass
        {
            public string Value { get; set; }
        }
    }
}
#endif
