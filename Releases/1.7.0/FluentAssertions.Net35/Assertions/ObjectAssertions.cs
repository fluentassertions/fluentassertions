using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

using FluentAssertions.Common;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="object"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ObjectAssertions : ReferenceTypeAssertions<object, ObjectAssertions>
    {
        protected internal ObjectAssertions(object value)
        {
            Subject = value;
        }

        /// <summary>
        /// Asserts that the value of an object equals another object when using it's <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="expected">The expected value</param>
        public AndConstraint<ObjectAssertions> Be(object expected)
        {
            return Be(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object equals another object using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> Be(object expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .BecauseOf(reason, reasonArgs)
                .ForCondition(Subject.IsSameOrEqualTo(expected))
                .FailWith("Expected " + Verification.SubjectNameOr("object") + " to be {0}{reason}, but found {1}.", expected,
                    Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using it's <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        public AndConstraint<ObjectAssertions> NotBe(object unexpected)
        {
            return NotBe(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object does not equal another object using it's <see cref="object.Equals(object)" /> method.
        /// </summary>
        /// <param name="unexpected">The unexpected value</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBe(object unexpected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.IsSameOrEqualTo(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect object to be equal to {0}{reason}.", unexpected);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name="expected">The expected object</param>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected)
        {
            return BeSameAs(expected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object reference refers to the exact same object as another object reference.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> BeSameAs(object expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .UsingLineBreaks
                .ForCondition(ReferenceEquals(Subject, expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected reference to object {0}{reason}, but found object {1}.", expected, Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name="unexpected">The unexpected object</param>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object unexpected)
        {
            return NotBeSameAs(unexpected, String.Empty);
        }

        /// <summary>
        /// Asserts that an object reference refers to a different object than another object reference refers to.
        /// </summary>
        /// <param name="unexpected">The unexpected object</param>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBeSameAs(object unexpected, string reason,
            params object[] reasonArgs)
        {
            Execute.Verification
                .UsingLineBreaks
                .ForCondition(!ReferenceEquals(Subject, unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect reference to object {0}{reason}.", unexpected);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that the object is <c>null</c>.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeNull()
        {
            return BeNull(String.Empty);
        }

        /// <summary>
        /// Asserts that the object is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> BeNull(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected <null>{reason}, but found {0}.", Subject);

            return new AndConstraint<ObjectAssertions>(this);
        }

        /// <summary>
        /// Asserts that the object is not <c>null</c>.
        /// </summary>
        public AndConstraint<ObjectAssertions> NotBeNull()
        {
            return NotBeNull(String.Empty);
        }

        /// <summary>
        /// Asserts that the object is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<ObjectAssertions> NotBeNull(string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected non-null value{reason}, but found <null>.");

            return new AndConstraint<ObjectAssertions>(this);
        }

#if !SILVERLIGHT

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
        /// the values of all properties.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeBinarySerializable()
        {
            return BeBinarySerializable(string.Empty);
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
        /// the values of all properties.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> BeBinarySerializable(string reason, params object[] reasonArgs)
        {
            try
            {
                object deserializedObject = CreateCloneUsingBinarySerializer(Subject);

                deserializedObject.ShouldHave().AllRuntimeProperties().EqualTo(Subject);
            }
            catch (Exception exc)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:\r\n\r\n{1}", Subject,
                        exc.Message);
            }

            return new AndConstraint<ObjectAssertions>(this);
        }

        private static object CreateCloneUsingBinarySerializer(object subject)
        {
            var stream = new MemoryStream();
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, subject);
            
            stream.Position = 0;
            return binaryFormatter.Deserialize(stream);
        }
#endif

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
        /// the values of all properties.
        /// </summary>
        public AndConstraint<ObjectAssertions> BeXmlSerializable()
        {
            return BeXmlSerializable(string.Empty);
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
        /// the values of all properties.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<ObjectAssertions> BeXmlSerializable(string reason, params object[] reasonArgs)
        {
            try
            {
                object deserializedObject = CreateCloneUsingXmlSerializer(Subject);

                deserializedObject.ShouldHave().AllRuntimeProperties().EqualTo(Subject);
            }
            catch (Exception exc)
            {
                Execute.Verification
                    .BecauseOf(reason, reasonArgs)
                    .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:\r\n\r\n{1}", Subject,
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