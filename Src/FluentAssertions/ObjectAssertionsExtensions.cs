#if !NETSTANDARD1_3 && !NETSTANDARD1_6

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions
{
    public static class ObjectAssertionsExtensions
    {
        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
        /// the values of all members.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static AndConstraint<ObjectAssertions> BeBinarySerializable(this ObjectAssertions assertions, string because = "",
            params object[] becauseArgs)
        {
            return BeBinarySerializable<object>(assertions, options => options, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the binary serializer and that it stills retains
        /// the values of all members.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static AndConstraint<ObjectAssertions> BeBinarySerializable<T>(this ObjectAssertions assertions,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options, string because = "",
            params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(options, nameof(options));

            try
            {
                object deserializedObject = CreateCloneUsingBinarySerializer(assertions.Subject);

                EquivalencyAssertionOptions<T> defaultOptions = AssertionOptions.CloneDefaults<T>()
                    .RespectingRuntimeTypes().IncludingFields().IncludingProperties();

                ((T)deserializedObject).Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
            }
            catch (Exception exc)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:{1}{1}{2}.",
                        assertions.Subject,
                        Environment.NewLine,
                        exc.Message);
            }

            return new AndConstraint<ObjectAssertions>(assertions);
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
        /// the values of all members.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static AndConstraint<ObjectAssertions> BeDataContractSerializable(this ObjectAssertions assertions,
            string because = "", params object[] becauseArgs)
        {
            return BeDataContractSerializable<object>(assertions, options => options, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
        /// the values of all members.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static AndConstraint<ObjectAssertions> BeDataContractSerializable<T>(this ObjectAssertions assertions,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> options, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsNull(options, nameof(options));

            try
            {
                var deserializedObject = CreateCloneUsingDataContractSerializer(assertions.Subject);

                EquivalencyAssertionOptions<T> defaultOptions = AssertionOptions.CloneDefaults<T>()
                    .RespectingRuntimeTypes().IncludingFields().IncludingProperties();

                ((T)deserializedObject).Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
            }
            catch (Exception exc)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:{1}{1}{2}.",
                        assertions.Subject,
                        Environment.NewLine,
                        exc.Message);
            }

            return new AndConstraint<ObjectAssertions>(assertions);
        }

        private static object CreateCloneUsingBinarySerializer(object subject)
        {
            using (var stream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter
                {
                    Binder = new SimpleBinder(subject.GetType())
                };

                binaryFormatter.Serialize(stream, subject);
                stream.Position = 0;
                return binaryFormatter.Deserialize(stream);
            }
        }

        private class SimpleBinder : SerializationBinder
        {
            private readonly Type type;

            public SimpleBinder(Type type)
            {
                this.type = type;
            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                if ((type.FullName == typeName) && (type.Assembly.FullName == assemblyName))
                {
                    return type;
                }
                else
                {
                    return null;
                }
            }
        }

        private static object CreateCloneUsingDataContractSerializer(object subject)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(subject.GetType());
                serializer.WriteObject(stream, subject);
                stream.Position = 0;
                return serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
        /// the values of all members.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public static AndConstraint<ObjectAssertions> BeXmlSerializable(this ObjectAssertions assertions, string because = "",
            params object[] becauseArgs)
        {
            try
            {
                object deserializedObject = CreateCloneUsingXmlSerializer(assertions.Subject);

                deserializedObject.Should().BeEquivalentTo(assertions.Subject,
                    options => options.RespectingRuntimeTypes().IncludingFields().IncludingProperties());
            }
            catch (Exception exc)
            {
                Execute.Assertion
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:{1}{1}{2}.",
                        assertions.Subject,
                        Environment.NewLine,
                        exc.Message);
            }

            return new AndConstraint<ObjectAssertions>(assertions);
        }

        private static object CreateCloneUsingXmlSerializer(object subject)
        {
            using (var stream = new MemoryStream())
            {
                var binaryFormatter = new XmlSerializer(subject.GetType());
                binaryFormatter.Serialize(stream, subject);

                stream.Position = 0;
                return binaryFormatter.Deserialize(stream);
            }
        }
    }
}

#endif
