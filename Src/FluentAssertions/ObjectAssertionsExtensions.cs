using System;
using System.Diagnostics.CodeAnalysis;

using System.IO;
using System.Runtime.Serialization;
#if NET6_0_OR_GREATER
using System.Text.Json;
using System.Text.Json.Serialization;
#endif
using System.Xml.Serialization;
using FluentAssertions.Common;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;

namespace FluentAssertions;

public static class ObjectAssertionsExtensions
{
    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeDataContractSerializable(this ObjectAssertions assertions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeDataContractSerializable<object>(assertions, options => options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{Object}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{Object}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeDataContractSerializable(this ObjectAssertions assertions,
        Func<EquivalencyOptions<object>, EquivalencyOptions<object>> options,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        return BeDataContractSerializable<object>(assertions, options, because, becauseArgs);
    }

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the data contract serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{TExpectation}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{TExpectation}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeDataContractSerializable<T>(this ObjectAssertions assertions,
        Func<EquivalencyOptions<T>, EquivalencyOptions<T>> options,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(options);

        try
        {
            var deserializedObject = CreateCloneUsingDataContractSerializer(assertions.Subject);

            EquivalencyOptions<T> defaultOptions = AssertionConfiguration.Current.Equivalency.CloneDefaults<T>()
                .PreferringRuntimeMemberTypes().IncludingFields().IncludingProperties();

            ((T)deserializedObject).Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
        }
        catch (Exception exc)
        {
            assertions.CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                    + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable(this ObjectAssertions assertions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeJsonSerializable<object>(assertions, options => options, serializerOptions: null, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable<T>(this ObjectAssertions assertions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeJsonSerializable(assertions, options => options, serializerOptions: null, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{Object}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{Object}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable(this ObjectAssertions assertions,
        Func<EquivalencyOptions<object>, EquivalencyOptions<object>> options,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeJsonSerializable<object>(assertions, options, serializerOptions: null, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="serializerOptions">
    /// The <see cref="JsonSerializerOptions"/> used for both serialization and deserialization.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable(this ObjectAssertions assertions,
        JsonSerializerOptions serializerOptions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeJsonSerializable<object>(assertions, options => options, serializerOptions, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{Object}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{Object}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="serializerOptions">
    /// The <see cref="JsonSerializerOptions"/> used for both serialization and deserialization.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable(this ObjectAssertions assertions,
        Func<EquivalencyOptions<object>, EquivalencyOptions<object>> options,
        JsonSerializerOptions serializerOptions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeJsonSerializable<object>(assertions, options, serializerOptions, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{T}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{T}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable<T>(this ObjectAssertions assertions,
        Func<EquivalencyOptions<T>, EquivalencyOptions<T>> options,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeJsonSerializable(assertions, options, serializerOptions: null, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using <see cref="JsonSerializer"/> and that it still retains
    /// the values of all serializable members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{T}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{T}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="serializerOptions">
    /// The <see cref="JsonSerializerOptions"/> used for both serialization and deserialization.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="options"/> is <see langword="null"/>.</exception>
    public static AndConstraint<ObjectAssertions> BeJsonSerializable<T>(this ObjectAssertions assertions,
        Func<EquivalencyOptions<T>, EquivalencyOptions<T>> options,
        JsonSerializerOptions serializerOptions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(options);

        try
        {
            object deserializedObject = CreateCloneUsingJsonSerializer(assertions.Subject, serializerOptions);

            EquivalencyOptions<T> defaultOptions = AssertionConfiguration.Current.Equivalency.CloneDefaults<T>()
                .PreferringRuntimeMemberTypes().IncludingProperties()
                .Excluding(member => MemberIsIgnoredByJsonSerialization(member));

            deserializedObject.Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
        }
        catch (Exception exc)
        {
            assertions.CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                    + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    private static bool MemberIsIgnoredByJsonSerialization(IMemberInfo member)
    {
        Type declaringType = member.DeclaringType;
        if (declaringType is null)
        {
            return false;
        }

        bool ignoredProperty = declaringType.FindProperty(member.Name, MemberVisibility.Public | MemberVisibility.Internal)
            ?.IsDecoratedWith<JsonIgnoreAttribute>() is true;

        bool ignoredField = declaringType.FindField(member.Name, MemberVisibility.Public | MemberVisibility.Internal)
            ?.IsDecoratedWith<JsonIgnoreAttribute>() is true;

        return ignoredProperty || ignoredField;
    }

    private static object CreateCloneUsingJsonSerializer(object subject, JsonSerializerOptions serializerOptions)
    {
        string json = JsonSerializer.Serialize(subject, subject.GetType(), serializerOptions);
        return JsonSerializer.Deserialize(json, subject.GetType(), serializerOptions);
    }
#endif

    private static object CreateCloneUsingDataContractSerializer(object subject)
    {
        using MemoryStream stream = new();
        DataContractSerializer serializer = new(subject.GetType());
        serializer.WriteObject(stream, subject);
        stream.Position = 0;
        return serializer.ReadObject(stream);
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
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeXmlSerializable(this ObjectAssertions assertions,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeXmlSerializable<object>(assertions, options => options, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{Object}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{Object}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeXmlSerializable(this ObjectAssertions assertions,
        Func<EquivalencyOptions<object>, EquivalencyOptions<object>> options,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
        => BeXmlSerializable<object>(assertions, options, because, becauseArgs);

    /// <summary>
    /// Asserts that an object can be serialized and deserialized using the XML serializer and that it stills retains
    /// the values of all members.
    /// </summary>
    /// <param name="options">
    /// A reference to the <see cref="EquivalencyOptions{T}"/> configuration object that can be used
    /// to influence the way the object graphs are compared. You can also provide an alternative instance of the
    /// <see cref="EquivalencyOptions{T}"/> class. The global defaults are determined by the
    /// <see cref="AssertionConfiguration"/> class.
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public static AndConstraint<ObjectAssertions> BeXmlSerializable<T>(this ObjectAssertions assertions,
        Func<EquivalencyOptions<T>, EquivalencyOptions<T>> options,
        [StringSyntax("CompositeFormat")] string because = "", params object[] becauseArgs)
    {
        try
        {
            var deserializedObject = CreateCloneUsingXmlSerializer(assertions.Subject);

            EquivalencyOptions<T> defaultOptions = AssertionConfiguration.Current.Equivalency.CloneDefaults<T>()
                .PreferringRuntimeMemberTypes().IncludingFields().IncludingProperties();

            deserializedObject.Should().BeEquivalentTo((T)assertions.Subject, _ => options(defaultOptions));
        }
        catch (Exception exc)
        {
            assertions.CurrentAssertionChain
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {0} to be serializable{reason}, but serialization failed with:"
                    + Environment.NewLine + Environment.NewLine + "{1}.",
                    assertions.Subject,
                    exc.Message);
        }

        return new AndConstraint<ObjectAssertions>(assertions);
    }

    [SuppressMessage("Security", "CA5369:Use XmlReader for \'XmlSerializer.Deserialize()\'")]
    private static object CreateCloneUsingXmlSerializer(object subject)
    {
        using var stream = new MemoryStream();
        var serializer = new XmlSerializer(subject.GetType());
        serializer.Serialize(stream, subject);

        stream.Position = 0;
        return serializer.Deserialize(stream);
    }
}
