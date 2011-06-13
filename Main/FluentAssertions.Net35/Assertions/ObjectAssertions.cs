using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    [DebuggerNonUserCode]
    public class ObjectAssertions : ReferenceTypeAssertions<object, ObjectAssertions>
    {
        protected internal ObjectAssertions(object value)
        {
            Subject = value;
        }

        /// <summary>
        ///   Asserts that the value of an object equals another object when using it's <see cref = "object.Equals(object)" /> method.
        /// </summary>
        public AndConstraint<ObjectAssertions> Be(object expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that an object equals another object using its <see cref = "object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> Be(object expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .BecauseOf(reason, reasonArgs)
                .ForCondition(Subject.IsEqualTo(expected))
                .FailWith("Expected " + Verification.SubjectNameOr("object") + " to be {1}{0}, but found {2}.", expected,
                    Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        ///   Asserts that an object does not equal another object using it's <see cref = "object.Equals(object)" /> method.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBe(object expected)
        {
            return NotBe(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that an object does not equal another object using it's <see cref = "object.Equals(object)" /> method.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBe(object expected, string reason, params object[] reasonParameters)
        {
            Execute.Verification
                .ForCondition(!Subject.IsEqualTo(expected))
                .BecauseOf(reason, reasonParameters)
                .FailWith("Did not expect object to be equal to {1}{0}.", expected);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        ///   Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected)
        {
            return BeSameAs(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected, string reason, params object[] reasonParameters)
        {
            Execute.Verification
                .UsingLineBreaks
                .ForCondition(ReferenceEquals(Subject, expected))
                .BecauseOf(reason, reasonParameters)
                .FailWith("Expected reference to object {1}{0}, but found object {2}.", expected, Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        ///   Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected)
        {
            return NotBeSameAs(expected, String.Empty);
        }

        /// <summary>
        ///   Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        ///   start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name = "reasonParameters">
        ///   Zero or more values to use for filling in any <see cref = "string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object expected, string reason,
            params object[] reasonParameters)
        {
            Execute.Verification
                .UsingLineBreaks
                .ForCondition(!ReferenceEquals(Subject, expected))
                .BecauseOf(reason, reasonParameters)
                .FailWith("Did not expect reference to object {1}{0}.", expected);

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> BeNull()
        {
            return BeNull(String.Empty);
        }

        public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonParameters)
        {
            Execute.Verify(ReferenceEquals(Subject, null),
                "Expected <null>{2}, but found {1}.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        public AndConstraint<ObjectAssertions> NotBeNull()
        {
            return NotBeNull(String.Empty);
        }

        public AndConstraint<ObjectAssertions> NotBeNull(string reason, params object[] reasonParameters)
        {
            Execute.Verify(!ReferenceEquals(Subject, null),
                "Expected non-null value{2}, but found <null>.", null, Subject, reason,
                reasonParameters);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        ///   Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
        ///   the values of all properties.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeBinarySerializable()
        {
            return BeBinarySerializable(string.Empty);
        }

        /// <summary>
        ///   Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
        ///   the values of all properties.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> BeBinarySerializable(string reason, params object[] reasonArgs)
        {
            try
            {
                object deserializedObject = CreateCloneUsingBinarySerializer(Subject);

                deserializedObject.ShouldHave().AllProperties().EqualTo(Subject);
            }
            catch (Exception exc)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {1} to be serializable{0}, but serialization failed with:\r\n\r\n{2}", Subject,
                        exc.Message);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        private static object CreateCloneUsingBinarySerializer(object subject)
        {
            var stream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, subject);
            
            stream.Position = 0;
            return binaryFormatter.Deserialize(stream);
        }

        /// <summary>
        ///   Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
        ///   the values of all properties.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeXmlSerializable()
        {
            return BeXmlSerializable(string.Empty);
        }

        /// <summary>
        ///   Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
        ///   the values of all properties.
        /// </summary>
        /// <param name = "reason">
        ///   A formatted phrase as is supported by <see cref = "string.Format(string,object[])" /> explaining why the assertion 
        ///   is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name = "reasonArgs">
        ///   Zero or more objects to format using the placeholders in <see cref = "reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> BeXmlSerializable(string reason, params object[] reasonArgs)
        {
            try
            {
                object deserializedObject = CreateCloneUsingXmlSerializer(Subject);

                deserializedObject.ShouldHave().AllProperties().EqualTo(Subject);
            }
            catch (Exception exc)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {1} to be serializable{0}, but serialization failed with:\r\n\r\n{2}", Subject,
                        exc.Message);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        private static object CreateCloneUsingXmlSerializer(object subject)
        {
            var stream = new MemoryStream();
            var binaryFormatter = new XmlSerializer(subject.GetType());
            binaryFormatter.Serialize(stream, subject);
            
            stream.Position = 0;
            return binaryFormatter.Deserialize(stream);
        }
    }
}