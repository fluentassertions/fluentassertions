using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeDataContractSerializable
    {
        [Fact]
        public async Task When_an_object_is_data_contract_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClass
            {
                Name = "John",
                Id = 1
            };

            // Act
            Func<Task> act = () => subject.Should().BeDataContractSerializableAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_an_object_is_not_data_contract_serializable_it_should_fail()
        {
            // Arrange
            var subject = new NonDataContractSerializableClass();

            // Act
            Func<Task> act = () => subject.Should().BeDataContractSerializableAsync("we need to store it on {0}", "disk");

            // Assert
            await act
                .Should().ThrowAsync<XunitException>()
                .WithMessage("*we need to store it on disk*EnumMemberAttribute*");
        }

        [Fact]
        public async Task When_an_object_is_data_contract_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Func<Task> act = () => subject.Should().BeDataContractSerializableAsync();

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*property subject.Name*to be*");
        }

        [Fact]
        public async Task When_a_data_contract_serializable_object_doesnt_restore_an_ignored_property_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Func<Task> act = () => subject.Should()
                .BeDataContractSerializableAsync<DataContractSerializableClassNotRestoringAllProperties>(
                    options => options.Excluding(x => x.Name));

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_injecting_null_options_to_BeDataContractSerializable_it_should_throw()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties();

            // Act
            Func<Task> act = () => subject.Should()
                .BeDataContractSerializableAsync<DataContractSerializableClassNotRestoringAllProperties>(
                    options: null);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>()
                .WithParameterName("options");
        }
    }

    public class NonDataContractSerializableClass
    {
        public Color Color { get; set; }
    }

    public class DataContractSerializableClass
    {
        public string Name { get; set; }

        public int Id;
    }

    [DataContract]
    public class DataContractSerializableClassNotRestoringAllProperties
    {
        public string Name { get; set; }

        [DataMember]
        public DateTime BirthDay { get; set; }
    }

    public enum Color
    {
        Red = 1,
        Yellow = 2
    }
}
