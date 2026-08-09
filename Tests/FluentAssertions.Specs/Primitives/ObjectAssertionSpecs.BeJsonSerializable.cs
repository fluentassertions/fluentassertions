#if NET6_0_OR_GREATER
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions.Extensions;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeJsonSerializable
    {
        [Fact]
        public void Succeeds_for_a_json_serializable_object()
        {
            // Arrange
            JsonSerializableClass subject = new()
            {
                Name = "John",
                Id = 1
            };

            // Act / Assert
            subject.Should().BeJsonSerializable();
        }

        [Fact]
        public void Fails_for_an_object_that_cannot_be_json_serialized()
        {
            // Arrange
            ClassWithIntPtr subject = new()
            {
                Pointer = new IntPtr(123)
            };

            // Act
            Action act = () => subject.Should().BeJsonSerializable("we need to store it on {0}", "disk");

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable because we need to store it on disk, but serialization failed with:*");
        }

        [Fact]
        public void Fails_when_deserialization_requires_an_unbindable_constructor_parameter()
        {
            // Arrange
            JsonSerializableClassWithUnbindableConstructorParameter subject = new("John")
            {
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeJsonSerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*deserialization constructor*must bind*");
        }

        [Fact]
        public void Succeeds_when_json_ignored_properties_are_present()
        {
            // Arrange
            JsonSerializableClassWithIgnoredProperty subject = new()
            {
                Name = "Deborah",
                CachedSum = 602_214_076_000_000_000_000_000M,
            };

            // Act / Assert
            subject.Should().BeJsonSerializable();
        }

        [Fact]
        public void Succeeds_when_generic_serializer_options_are_required()
        {
            // Arrange
            JsonSerializableWithCustomConverter subject = new()
            {
                Value = new CustomValue("123")
            };

            JsonSerializerOptions serializerOptions = new();
            serializerOptions.Converters.Add(new CustomValueJsonConverter());

            // Act / Assert
            subject.Should().BeJsonSerializable<JsonSerializableWithCustomConverter>(
                options => options,
                serializerOptions);
        }

        [Fact]
        public void Succeeds_for_the_non_generic_options_overload()
        {
            // Arrange
            JsonSerializableClass subject = new()
            {
                Name = "John",
                Id = 1
            };

            // Act / Assert
            subject.Should().BeJsonSerializable(options => options);
        }

        [Fact]
        public void Succeeds_for_the_non_generic_serializer_options_overload()
        {
            // Arrange
            JsonSerializableWithCustomConverter subject = new()
            {
                Value = new CustomValue("123")
            };

            JsonSerializerOptions serializerOptions = new();
            serializerOptions.Converters.Add(new CustomValueJsonConverter());

            // Act / Assert
            subject.Should().BeJsonSerializable(serializerOptions);
        }

        [Fact]
        public void Succeeds_for_the_non_generic_options_and_serializer_options_overload()
        {
            // Arrange
            JsonSerializableWithCustomConverter subject = new()
            {
                Value = new CustomValue("123")
            };

            JsonSerializerOptions serializerOptions = new();
            serializerOptions.Converters.Add(new CustomValueJsonConverter());

            // Act / Assert
            subject.Should().BeJsonSerializable(options => options, serializerOptions);
        }

        [Fact]
        public void Requires_generic_equivalency_options()
        {
            // Arrange
            JsonSerializableClass subject = new();

            // Act
            Action act = () => subject.Should().BeJsonSerializable<JsonSerializableClass>(options: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }

        [Fact]
        public void Requires_non_generic_equivalency_options()
        {
            // Arrange
            JsonSerializableClass subject = new();

            // Act
            Action act = () => subject.Should().BeJsonSerializable(options: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }

    public class JsonSerializableClass
    {
        [UsedImplicitly]
        public string Name { get; set; }

        public int Id { get; set; }
    }

    public class JsonSerializableClassWithUnbindableConstructorParameter(string unboundName)
    {
        [UsedImplicitly]
        public string Name { get; } = unboundName;

        public DateTime BirthDay { get; set; }
    }

    public class JsonSerializableClassWithIgnoredProperty
    {
        public string Name { get; set; }

        [JsonIgnore]
        public decimal CachedSum { get; set; }
    }

    public class ClassWithIntPtr
    {
        public IntPtr Pointer { get; set; }
    }

    public class JsonSerializableWithCustomConverter
    {
        public CustomValue Value { get; set; }
    }

    public sealed class CustomValue(string value)
    {
        public string Value { get; } = value;
    }

    public class CustomValueJsonConverter : JsonConverter<CustomValue>
    {
        public override CustomValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new CustomValue(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, CustomValue value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }
}
#endif
