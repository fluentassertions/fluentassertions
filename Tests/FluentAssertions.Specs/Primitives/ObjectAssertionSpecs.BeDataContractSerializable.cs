using System;
using System.Runtime.Serialization;
using FluentAssertions.Extensions;
using JetBrains.Annotations;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeDataContractSerializable
    {
        [Fact]
        public void When_an_object_is_data_contract_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new DataContractSerializableClass
            {
                Name = "John",
                Id = 1
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable();

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void When_an_object_is_not_data_contract_serializable_it_should_fail()
        {
            // Arrange
            var subject = new NonDataContractSerializableClass();

            // Act
            Action act = () => subject.Should().BeDataContractSerializable("we need to store it on {0}", "disk");

            // Assert
            act
                .Should().Throw<XunitException>()
                .WithMessage("*we need to store it on disk*EnumMemberAttribute*");
        }

        [Fact]
        public void When_an_object_is_data_contract_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*property subject.Name*to be*");
        }

        [Fact]
        public void When_an_object_is_not_binary_serializable_and_has_properties_marked_IgnoreDataMember_it_should_fail()
        {
            // Arrange
            var subject = new DataContractSerializableClassWithIgnoredDataMember()
            {
                Name = "Deborah",
                CachedSum = 602_214_076_000_000_000_000_000M,
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable();

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected property subject.CachedSum to be*");
        }

        [Fact]
        public void When_an_object_is_binary_serializable_and_has_properties_marked_NonSerialized_it_should_fail()
        {
            // Arrange
            var subject = new BinarySerializableClassWithNonSerializedMember()
            {
                Name = "Deborah",
                CachedSum = 602_214_076_000_000_000_000_000M,
            };

            // Act
            Action act = () => subject.Should().BeDataContractSerializable(options => options);

            // Assert
            act.Should().Throw<XunitException>().WithMessage("*Expected field subject.CachedSum to be*");
        }

        [Fact]
        public void When_injecting_null_options_to_BeDataContractSerializable_it_should_throw()
        {
            // Arrange
            var subject = new DataContractSerializableClassNotRestoringAllProperties();

            // Act
            Action act = () => subject.Should()
                .BeDataContractSerializable<DataContractSerializableClassNotRestoringAllProperties>(
                    options: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("options");
        }
    }

    public class NonDataContractSerializableClass
    {
        public Color Color { get; set; }
    }

    public class DataContractSerializableClass
    {
        [UsedImplicitly]
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

    [DataContract]
    public class DataContractSerializableClassWithIgnoredDataMember
    {
        [DataMember]
        public string Name { get; set; }

        [IgnoreDataMember]
        public decimal CachedSum { get; set; }
    }

    [Serializable]
    public class BinarySerializableClassWithNonSerializedMember
    {
        // These members need to be fields for .ExcludeNonSerialized
        public string Name;

        [NonSerialized]
        public decimal CachedSum;
    }

    public enum Color
    {
        Red = 1,
        Yellow = 2
    }
}
