using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using FluentAssertions.Collections;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Equivalency.Steps;
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
        /// <param name="assertions"></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// <param name="assertions"></param>
        /// <param name="options">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TExpectation}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TExpectation}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// <param name="assertions"></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
        /// <param name="assertions"></param>
        /// <param name="options">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TExpectation}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TExpectation}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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

        /// <summary>
        /// Asserts the source item and the expected item are equivalent objects.
        /// Objects are considered equivalent if their types are the same and the values of their properties are the same using deep equality.
        /// </summary>
        /// <param name="objectAssertions">The provided item, from a fluent assertion.</param>
        /// <param name="expectation">The expected item to compare to</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static void BeSameTypeEquivalentTo<T>(
            this ObjectAssertions objectAssertions,
            T expectation,
            string because = "",
            params object[] becauseArgs)
        {
            BeSameTypeEquivalentTo(objectAssertions, expectation, config => config, because, becauseArgs);
        }

        /// <summary>
        /// Asserts the source item and the expected item are equivalent objects.
        /// Objects are considered equivalent if their types are the same and the values of their properties are the same using deep equality.
        /// </summary>
        /// <param name="objectAssertions">The provided item, from a fluent assertion.</param>
        /// <param name="expectation">The expected item to compare to</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static void BeSameTypeEquivalentTo<T>(
            this ObjectAssertions objectAssertions,
            T expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config,
            string because = "",
            params object[] becauseArgs)
        {
            objectAssertions
               .BeEquivalentTo(
                    expectation,
                    options =>
                       config(options)
                           .WithStrictOrdering()
                           .RespectingRuntimeTypes()
                           .Using(new TypeEquivalencyStep()),
                    because,
                    because,
                    becauseArgs);
        }

        /// <summary>
        /// Asserts the items in the provided collection and the expected collection have equivalent objects at the same indexes.
        /// Objects are considered equivalent if their types are the same and the values of their properties are the same using deep equality.
        /// </summary>
        /// <param name="collection">The provided collection</param>
        /// <param name="expectations">The expected collection</param>
        public static AndConstraint<GenericCollectionAssertions<T>> BeInOrderSameTypeEquivalentTo<T>(
            this GenericCollectionAssertions<T> collection,
            params T[] expectations)
        {
            return collection
               .BeInOrderSameTypeEquivalentTo(
                    Enumerable.AsEnumerable(expectations),
                    options => options,
                    string.Empty);
        }

        /// <summary>
        /// Asserts the items in the provided collection and the expected collection have equivalent objects at the same indexes.
        /// Objects are considered equivalent if their types are the same and the values of their properties are the same using deep equality.
        /// </summary>
        /// <param name="collection">The provided collection</param>
        /// <param name="expectation">The expected collection</param>
        /// <param name="config">
        /// A reference to the <see cref="EquivalencyAssertionOptions{TSubject}"/> configuration object that can be used
        /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
        /// <see cref="EquivalencyAssertionOptions{TSubject}"/> class. The global defaults are determined by the
        /// <see cref="AssertionOptions"/> class.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
        /// </param>
        public static AndConstraint<GenericCollectionAssertions<T>> BeInOrderSameTypeEquivalentTo<T>(
            this GenericCollectionAssertions<T> collection,
            IEnumerable<T> expectation,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config,
            string because = "",
            params object[] becauseArgs)
        {
            return collection
               .BeEquivalentTo(
                    expectation,
                    options =>
                       config(options)
                           .WithStrictOrdering()
                           .RespectingRuntimeTypes()
                           .Using(new TypeEquivalencyStep()),
                    because,
                    because,
                    becauseArgs);
        }

        private static object CreateCloneUsingBinarySerializer(object subject)
        {
            using var stream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter
            {
                Binder = new SimpleBinder(subject.GetType())
            };

            binaryFormatter.Serialize(stream, subject);
            stream.Position = 0;
            return binaryFormatter.Deserialize(stream);
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
            using var stream = new MemoryStream();
            var serializer = new DataContractSerializer(subject.GetType());
            serializer.WriteObject(stream, subject);
            stream.Position = 0;
            return serializer.ReadObject(stream);
        }

        /// <summary>
        /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
        /// the values of all members.
        /// </summary>
        /// <param name="assertions"></param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
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
            using var stream = new MemoryStream();
            var binaryFormatter = new XmlSerializer(subject.GetType());
            binaryFormatter.Serialize(stream, subject);

            stream.Position = 0;
            return binaryFormatter.Deserialize(stream);
        }
    }
}
