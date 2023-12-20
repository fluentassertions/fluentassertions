using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FluentAssertionsAsync.Extensions;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertionsAsync.Specs.Primitives;

public partial class ObjectAssertionSpecs
{
    public class BeXmlSerializable
    {
        [Fact]
        public async Task When_an_object_is_xml_serializable_it_should_succeed()
        {
            // Arrange
            var subject = new XmlSerializableClass
            {
                Name = "John",
                Id = 1
            };

            // Act
            Func<Task> act = () => subject.Should().BeXmlSerializableAsync();

            // Assert
            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task When_an_object_is_not_xml_serializable_it_should_fail()
        {
            // Arrange
            var subject = new NonPublicClass
            {
                Name = "John"
            };

            // Act
            Func<Task> act = () => subject.Should().BeXmlSerializableAsync("we need to store it on {0}", "disk");

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage(
                    "*to be serializable because we need to store it on disk, but serialization failed with:*NonPublicClass*");
        }

        [Fact]
        public async Task When_an_object_is_xml_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            // Arrange
            var subject = new XmlSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            // Act
            Func<Task> act = () => subject.Should().BeXmlSerializableAsync();

            // Assert
            await act.Should().ThrowAsync<XunitException>()
                .WithMessage("*to be serializable, but serialization failed with:*Name*to be*");
        }
    }

    public class XmlSerializableClass
    {
        public string Name { get; set; }

        public int Id;
    }

    public class XmlSerializableClassNotRestoringAllProperties : IXmlSerializable
    {
        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            BirthDay = DateTime.Parse(reader.ReadElementContentAsString(), CultureInfo.InvariantCulture);
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(BirthDay.ToString(CultureInfo.InvariantCulture));
        }
    }

    internal class NonPublicClass
    {
        public string Name { get; set; }
    }
}
