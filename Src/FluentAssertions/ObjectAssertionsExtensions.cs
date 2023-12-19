using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Primitives;

namespace FluentAssertionsAsync;

public static class ObjectAssertionsExtensions
{
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
    public static async Task<AndConstraint<ObjectAssertions>> BeDataContractSerializableAsync(this ObjectAssertions assertions,
        string because = "", params object[] becauseArgs)
    {
        return await BeDataContractSerializableAsync<object>(assertions, options => options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="assertions"></param>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionOptions"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static async Task<AndConstraint<ObjectAssertions>> BeDataContractSerializableAsync<T>(this ObjectAssertions assertions,
        Func<EquivalencyOptions<T>, EquivalencyOptions<T>> options, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(options);

        try
        {
            var deserializedObject = CreateCloneUsingDataContractSerializer(assertions.Subject);

            EquivalencyOptions<T> defaultOptions = AssertionOptions.CloneDefaults<T>()
                .RespectingRuntimeTypes().IncludingFields().IncludingProperties();

            await ((T)deserializedObject).Should().BeEquivalentToAsync((T)assertions.Subject, _ => options(defaultOptions));
        }
        catch (Exception exc)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                    + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
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
    public static async Task<AndConstraint<ObjectAssertions>> BeXmlSerializableAsync(this ObjectAssertions assertions, string because = "",
        params object[] becauseArgs)
    {
        try
        {
            object deserializedObject = CreateCloneUsingXmlSerializer(assertions.Subject);

            await deserializedObject.Should().BeEquivalentToAsync(assertions.Subject,
                options => options.RespectingRuntimeTypes().IncludingFields().IncludingProperties());
        }
        catch (Exception exc)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                    + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    private static object CreateCloneUsingXmlSerializer(object subject)
    {
        using var stream = new MemoryStream();
        var serializer = new XmlSerializer(subject.GetType());
        serializer.Serialize(stream, subject);

        stream.Position = 0;
        return serializer.Deserialize(stream);
    }
}
