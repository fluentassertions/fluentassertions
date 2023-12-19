using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertionsAsync.Common;
using FluentAssertionsAsync.Execution;
using FluentAssertionsAsync.Primitives;

namespace FluentAssertionsAsync.Types;

/// <summary>
/// Contains a number of methods to assert that an <see cref="Assembly"/> is in the expected state.
/// </summary>
public class AssemblyAssertions : ReferenceTypeAssertions<Assembly, AssemblyAssertions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyAssertions" /> class.
    /// </summary>
    public AssemblyAssertions(Assembly assembly)
        : base(assembly)
    {
    }

    /// <summary>
    /// Asserts that an assembly does not reference the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly which should not be referenced.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is <see langword="null"/>.</exception>
    public AndConstraint<AssemblyAssertions> NotReference(Assembly assembly, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assembly);

        var assemblyName = assembly.GetName().Name;

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected assembly not to reference assembly {0}{reason}, but {context:assembly} is <null>.",
                assemblyName);

        if (success)
        {
            var subjectName = Subject!.GetName().Name;

            IEnumerable<string> references = Subject.GetReferencedAssemblies().Select(x => x.Name);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(!references.Contains(assemblyName))
                .FailWith("Expected assembly {0} not to reference assembly {1}{reason}.", subjectName, assemblyName);
        }

        return new AndConstraint<AssemblyAssertions>(this);
    }

    /// <summary>
    /// Asserts that an assembly references the specified assembly.
    /// </summary>
    /// <param name="assembly">The assembly which should be referenced.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="assembly"/> is <see langword="null"/>.</exception>
    public AndConstraint<AssemblyAssertions> Reference(Assembly assembly, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNull(assembly);

        var assemblyName = assembly.GetName().Name;

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected assembly to reference assembly {0}{reason}, but {context:assembly} is <null>.", assemblyName);

        if (success)
        {
            var subjectName = Subject!.GetName().Name;

            IEnumerable<string> references = Subject.GetReferencedAssemblies().Select(x => x.Name);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(references.Contains(assemblyName))
                .FailWith("Expected assembly {0} to reference assembly {1}{reason}, but it does not.", subjectName, assemblyName);
        }

        return new AndConstraint<AssemblyAssertions>(this);
    }

    /// <summary>
    /// Asserts that the assembly defines a type called <paramref name="namespace"/> and <paramref name="name"/>.
    /// </summary>
    /// <param name="namespace">The namespace of the class.</param>
    /// <param name="name">The name of the class.</param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
    public AndWhichConstraint<AssemblyAssertions, Type> DefineType(string @namespace, string name, string because = "",
        params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(name);

        bool success = Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject is not null)
            .FailWith("Expected assembly to define type {0}.{1}{reason}, but {context:assembly} is <null>.",
                @namespace, name);

        Type foundType = null;

        if (success)
        {
            foundType = Subject!.GetTypes().SingleOrDefault(t => t.Namespace == @namespace && t.Name == name);

            Execute.Assertion
                .ForCondition(foundType is not null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected assembly {0} to define type {1}.{2}{reason}, but it does not.",
                    Subject.FullName, @namespace, name);
        }

        return new AndWhichConstraint<AssemblyAssertions, Type>(this, foundType);
    }

    /// <summary>Asserts that the assembly is unsigned.</summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    public AndConstraint<AssemblyAssertions> BeUnsigned(string because = "", params object[] becauseArgs)
    {
        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .FailWith("Can't check for assembly signing if {context:assembly} reference is <null>.");

        if (success)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(Subject!.GetName().GetPublicKey() is not { Length: > 0 })
                .FailWith(
                    "Did not expect the assembly {0} to be signed{reason}, but it is.", Subject.FullName);
        }

        return new AndConstraint<AssemblyAssertions>(this);
    }

    /// <summary>Asserts that the assembly is signed with the specified public key.</summary>
    /// <param name="publicKey">
    /// The base-16 string representation of the public key, like "e0851575614491c6d25018fadb75".
    /// </param>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because"/>.
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="publicKey"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><paramref name="publicKey"/> is empty.</exception>
    public AndConstraint<AssemblyAssertions> BeSignedWithPublicKey(string publicKey, string because = "", params object[] becauseArgs)
    {
        Guard.ThrowIfArgumentIsNullOrEmpty(publicKey);

        bool success = Execute.Assertion
            .ForCondition(Subject is not null)
            .FailWith("Can't check for assembly signing if {context:assembly} reference is <null>.");

        if (success)
        {
            var bytes = Subject!.GetName().GetPublicKey() ?? Array.Empty<byte>();
            string assemblyKey = ToHexString(bytes);

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .WithExpectation("Expected assembly {0} to have public key {1} ", Subject.FullName, publicKey)
                .ForCondition(bytes.Length != 0)
                .FailWith("{reason}, but it is unsigned.")
                .Then
                .ForCondition(string.Equals(assemblyKey, publicKey, StringComparison.OrdinalIgnoreCase))
                .FailWith("{reason}, but it has {0} instead.", assemblyKey)
                .Then
                .ClearExpectation();
        }

        return new AndConstraint<AssemblyAssertions>(this);
    }

    private static string ToHexString(byte[] bytes) =>
#if NET6_0_OR_GREATER
        Convert.ToHexString(bytes);
#else
        BitConverter.ToString(bytes).Replace("-", string.Empty, StringComparison.Ordinal);
#endif

    /// <summary>
    /// Returns the type of the subject the assertion applies on.
    /// </summary>
    protected override string Identifier => "assembly";
}
