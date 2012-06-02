using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using FluentAssertions.Primitives;

#if WINRT
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

using FluentAssertions.Common;

namespace FluentAssertions.Specs
{
    [TestClass]
    public class ObjectAssertionSpecs
    {
        #region Be / NotBe

        [TestMethod]
        public void When_two_equal_object_are_expected_to_be_equal_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            someObject.Should().Be(equalObject);
        }

        [TestMethod]
        public void When_two_different_objects_are_expected_to_be_equal_it_should_fail_with_a_clear_explanation()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().Be(nonEqualObject);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected object to be ClassWithCustomEqualMethod(2), but found ClassWithCustomEqualMethod(1).");
        }

        [TestMethod]
        public void When_both_subject_and_expected_are_null_it_should_succeed()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            object someObject = null;
            object expectedObject = null;

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            someObject.Should().Be(expectedObject);
        }

        [TestMethod]
        public void When_the_subject_is_null_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            object someObject = null;
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().Be(nonEqualObject);

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Expected object to be ClassWithCustomEqualMethod(2), but found <null>.");
        }

        [TestMethod]
        public void When_two_different_objects_are_expected_to_be_equal_it_should_fail_and_use_the_reason()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().Be(nonEqualObject, "because it should use the {0}", "reason");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected object to be ClassWithCustomEqualMethod(2) because it should use the reason, but found ClassWithCustomEqualMethod(1).");
        }

        [TestMethod]
        public void When_non_equal_objects_are_expected_to_be_not_equal_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var nonEqualObject = new ClassWithCustomEqualMethod(2);

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            someObject.Should().NotBe(nonEqualObject);
        }

        [TestMethod]
        public void When_two_equal_objects_are_expected_not_to_be_equal_it_should_fail_with_a_clear_explanation()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                someObject.Should().NotBe(equalObject);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect object to be equal to ClassWithCustomEqualMethod(1).");
        }

        #endregion

        #region BeSameAs / NotBeSameAs

        [TestMethod]
        public void When_the_same_objects_are_expected_to_be_the_same_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new ClassWithCustomEqualMethod(1);
            var referenceToSubject = subject;

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Arrange
            //-------------------------------------------------------------------------------------------------------------------
            subject.Should().BeSameAs(referenceToSubject);
        }

        [TestMethod]
        public void When_two_different_objects_are_expected_to_be_the_same_it_should_fail_with_a_clear_explanation()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var subject = new
            {
                Name = "John Doe"
            };
            
            var otherObject = new
            {
                UserName = "JohnDoe"
            };

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeSameAs(otherObject, "they are {0} {1}", "the", "same");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .WithMessage(
                    "Expected reference to object \r\n{ UserName = JohnDoe } because " + 
                    "they are the same, but found object \r\n{ Name = John Doe }.");
        }

        [TestMethod]
        public void When_two_different_objects_are_expected_not_to_be_the_same_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var notSameObject = new ClassWithCustomEqualMethod(1);

            //-------------------------------------------------------------------------------------------------------------------
            // Act / Assert
            //-------------------------------------------------------------------------------------------------------------------
            someObject.Should().NotBeSameAs(notSameObject);
        }

        [TestMethod]
        public void When_two_equal_object_are_expected_not_to_be_the_same_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            ClassWithCustomEqualMethod sameObject = someObject;

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().NotBeSameAs(sameObject, "they are {0} {1}", "the", "same");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>()
                .WithMessage("Did not expect reference to object \r\nClassWithCustomEqualMethod(1) because they are the same.");
        }

        [TestMethod]
        public void When_two_equal_objects_are_expected_not_to_be_equal_it_should_fail_and_use_the_reason()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new ClassWithCustomEqualMethod(1);
            var equalObject = new ClassWithCustomEqualMethod(1);

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () =>
                someObject.Should().NotBe(equalObject, "because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Did not expect object to be equal to ClassWithCustomEqualMethod(1) " +
                    "because we want to test the failure message.");
        }

        #endregion

        #region BeNull / BeNotNull

        [TestMethod]
        public void Should_succeed_when_asserting_null_object_to_be_null()
        {
            object someObject = null;
            someObject.Should().BeNull();
        }

        [TestMethod]
        public void Should_fail_when_asserting_non_null_object_to_be_null()
        {
            var someObject = new object();
            Action act = () => someObject.Should().BeNull();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void When_a_non_null_object_is_expected_to_be_null_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var someObject = new object();
            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().BeNull("because we want to test the failure {0}", "message");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .Where(e => e.Message.StartsWith("Expected <null> because we want to test the failure message, but found System.Object"));
        }

        [TestMethod]
        public void Should_succeed_when_asserting_non_null_object_not_to_be_null()
        {
            var someObject = new object();
            someObject.Should().NotBeNull();
        }

        [TestMethod]
        public void Should_fail_when_asserting_null_object_not_to_be_null()
        {
            object someObject = null;
            Action act = () => someObject.Should().NotBeNull();
            act.ShouldThrow<AssertFailedException>();

        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_null_object_not_to_be_null()
        {
            object someObject = null;
            var assertions = someObject.Should();
            assertions.Invoking(x => x.NotBeNull("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage("Expected non-null value because we want to test the failure message, but found <null>.");
        }

        #endregion

        #region BeOfType

        [TestMethod]
        public void When_object_type_is_exactly_equal_to_the_specified_type_it_should_not_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new Exception();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().BeOfType<Exception>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_object_type_is_different_than_expected_type_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new object();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().BeOfType<int>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be System.Int32, but found System.Object.");
        }

        [TestMethod]
        public void When_object_type_is_different_than_expected_type_it_should_fail_with_descriptive_message()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new object();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().BeOfType<int>("because they are {0} {1}", "of different", "type");

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be System.Int32 because they are of different type, but found System.Object.");
        }

        [TestMethod]
        public void Then_object_type_is_same_as_expected_type_but_in_different_assembly_it_should_fail_with_assembly_qualified_name()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var assertionsFromOtherAssembly = new object().Should();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
#pragma warning disable 436 // disable the warning on conflicting types, as this is the intention for the spec

            Action act = () =>
                assertionsFromOtherAssembly.Should().BeOfType<ObjectAssertions>();

#pragma warning restore 436

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            const string expectedMessage =
                "Expected type to be [FluentAssertions.Primitives.ObjectAssertions, FluentAssertions.*]" +
                    ", but found [FluentAssertions.Primitives.ObjectAssertions, FluentAssertions*].";

            act.ShouldThrow<AssertFailedException>().WithMessage(expectedMessage, ComparisonMode.Wildcard);
        }

        [TestMethod]
        public void When_object_type_is_a_subclass_of_the_expected_type_it_should_fail()
        {
            //-------------------------------------------------------------------------------------------------------------------
            // Arrange
            //-------------------------------------------------------------------------------------------------------------------
            var someObject = new DummyImplementingClass();

            //-------------------------------------------------------------------------------------------------------------------
            // Act
            //-------------------------------------------------------------------------------------------------------------------
            Action act = () => someObject.Should().BeOfType<DummyBaseClass>();

            //-------------------------------------------------------------------------------------------------------------------
            // Assert
            //-------------------------------------------------------------------------------------------------------------------
            act.ShouldThrow<AssertFailedException>().WithMessage(
                "Expected type to be FluentAssertions.Specs.DummyBaseClass, but found FluentAssertions.Specs.DummyImplementingClass.");
        }

        #endregion

        #region BeAssignableTo

        [TestMethod]
        public void Should_succeed_when_asserting_object_assignable_to_for_same_type()
        {
            var someObject = new DummyImplementingClass();
            someObject.Should().BeAssignableTo<DummyImplementingClass>();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_assignable_to_base_type()
        {
            var someObject = new DummyImplementingClass();
            someObject.Should().BeAssignableTo<DummyBaseClass>();
        }

        [TestMethod]
        public void Should_succeed_when_asserting_object_assignable_to_implemented_interface_type()
        {
            var someObject = new DummyImplementingClass();
            someObject.Should().BeAssignableTo<IDisposable>();
        }

        [TestMethod]
        public void Should_fail_with_descriptive_message_when_asserting_object_assignable_to_not_implemented_type()
        {
            var someObject = new DummyImplementingClass();

            someObject.Invoking(
                x => x.Should().BeAssignableTo<DateTime>("because we want to test the failure {0}", "message"))
                .ShouldThrow<AssertFailedException>()
                .WithMessage(string.Format(
                    "Expected to be assignable to {1} because we want to test the failure message, but {0} does not implement {1}",
                    typeof (DummyImplementingClass), typeof (DateTime)));
        }

        #endregion

        #region Miscellaneous

        [TestMethod]
        public void Should_support_chaining_constraints_with_and()
        {
            var someObject = new Exception();
            someObject.Should()
                .BeOfType<Exception>()
                .And
                .NotBeNull();
        }

        #endregion

#if !SILVERLIGHT && !WINRT

        #region BeBinarySerializable

        [TestMethod]
        public void When_an_object_is_binary_serializable_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new SerializableClass
            {
                Name = "John"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeBinarySerializable();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_object_is_not_binary_serializable_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new UnserializableClass
            {
                Name = "John"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeBinarySerializable("we need to store it on {0}", "disk");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .Where(ex => 
                    ex.Message.Contains("to be serializable because we need to store it on disk, but serialization failed with:") &&
                    ex.Message.Contains("marked as serializable"));
        }
        
        [TestMethod]
        public void When_an_object_is_binary_serializable_but_not_deserializable_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new BinarySerializableClassMissingDeserializationConstructor
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeBinarySerializable();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .Where(ex => 
                    ex.Message.Contains("to be serializable, but serialization failed with:") &&
                    ex.Message.Contains("constructor to deserialize"));
        }

        [TestMethod]
        public void When_an_object_is_binary_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new BinarySerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeBinarySerializable();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .Where(ex => 
                    ex.Message.Contains("to be serializable, but serialization failed with:") &&
                    ex.Message.Contains("property Name to be"));
        }

        internal class UnserializableClass
        {
            public string Name { get; set; }
        }

        [Serializable]
        public class SerializableClass
        {
            public string Name { get; set; }
        }

        [Serializable]
        internal class BinarySerializableClassMissingDeserializationConstructor : ISerializable
        {
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {

            }
        }


        [Serializable]
        internal class BinarySerializableClassNotRestoringAllProperties : ISerializable
        {
            public string Name { get; set; }
            public DateTime BirthDay { get; set; }

            public BinarySerializableClassNotRestoringAllProperties()
            {
            }

            public BinarySerializableClassNotRestoringAllProperties(SerializationInfo info, StreamingContext context)
            {
                BirthDay = info.GetDateTime("BirthDay");
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("BirthDay", BirthDay);
            }
        }

        #endregion

#endif

        #region BeXmlSerializable

        [TestMethod]
        public void When_an_object_is_xml_serializable_it_should_succeed()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new XmlSerializableClass
            {
                Name = "John"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeXmlSerializable();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act.ShouldNotThrow();
        }

        [TestMethod]
        public void When_an_object_is_not_xml_serializable_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new NonPublicClass
            {
                Name = "John"
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeXmlSerializable("we need to store it on {0}", "disk");

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .Where(ex =>
                    ex.Message.Contains("to be serializable because we need to store it on disk, but serialization failed with:") &&
                    ex.Message.Contains("Only public types can be processed"));
        }

        [TestMethod]
        public void When_an_object_is_xml_serializable_but_doesnt_restore_all_properties_it_should_fail()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var subject = new XmlSerializableClassNotRestoringAllProperties
            {
                Name = "John",
                BirthDay = 20.September(1973)
            };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            Action act = () => subject.Should().BeXmlSerializable();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            act
                .ShouldThrow<AssertFailedException>()
                .Where(ex =>
                    ex.Message.Contains("to be serializable, but serialization failed with:") &&
                    ex.Message.Contains("property Name to be"));
        }

        internal class NonPublicClass
        {
            public string Name { get; set; }
        }

        public class XmlSerializableClass
        {
            public string Name { get; set; }
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
                BirthDay = DateTime.Parse(reader.ReadElementContentAsString());
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteString(BirthDay.ToString());
            }
        }

        #endregion
    }

    internal class ClassWithCustomEqualMethod
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "T:System.Object" /> class.
        /// </summary>
        public ClassWithCustomEqualMethod(int key)
        {
            Key = key;
        }

        private int Key { get; set; }

        private bool Equals(ClassWithCustomEqualMethod other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Key == Key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (ClassWithCustomEqualMethod))
            {
                return false;
            }
            return Equals((ClassWithCustomEqualMethod) obj);
        }

        public override int GetHashCode()
        {
            return Key;
        }

        public static bool operator ==(ClassWithCustomEqualMethod left, ClassWithCustomEqualMethod right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ClassWithCustomEqualMethod left, ClassWithCustomEqualMethod right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///   Returns a <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   A <see cref = "T:System.String" /> that represents the current <see cref = "T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("ClassWithCustomEqualMethod({0})", Key);
        }
    }

    internal class DummyBaseClass
    {
    }

    internal class DummyImplementingClass : DummyBaseClass, IDisposable
    {
        public void Dispose()
        {
            // Ignore
        }
    }
}