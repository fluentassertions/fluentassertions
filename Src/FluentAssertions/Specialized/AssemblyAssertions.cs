using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Reflection
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="Assembly"/> is in the expected state.
    /// </summary>
    public class AssemblyAssertions : ReferenceTypeAssertions<Assembly, AssemblyAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyAssertions" /> class.
        /// </summary>
        public AssemblyAssertions(Assembly assembly) : base(assembly)
        {
        }

#if NET45 || NET47 || NETSTANDARD2_0 || NETCOREAPP2_0 // TODO :: Should be able to remove based on: https://github.com/dotnet/corefx/issues/1784#issuecomment-218803619

        /// <summary>
        /// Asserts that an assembly does not reference the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly which should not be referenced.</param>
        public void NotReference(Assembly assembly)
        {
            NotReference(assembly, string.Empty);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public void NotReference(Assembly assembly, string because, params string[] becauseArgs)
        {
            var subjectName = Subject.GetName().Name;
            var assemblyName = assembly.GetName().Name;

            var references = Subject.GetReferencedAssemblies().Select(x => x.Name);

            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .ForCondition(references.All(x => x != assemblyName))
                   .FailWith("Expected assembly {0} not to reference assembly {1}{reason}.", subjectName, assemblyName);
        }

        /// <summary>
        /// Asserts that an assembly references the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly which should be referenced.</param>
        public void Reference(Assembly assembly)
        {
            Reference(assembly, string.Empty);
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
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public void Reference(Assembly assembly, string because, params string[] becauseArgs)
        {
            var subjectName = Subject.GetName().Name;
            var assemblyName = assembly.GetName().Name;

            var references = Subject.GetReferencedAssemblies().Select(x => x.Name);

            Execute.Assertion
                   .BecauseOf(because, becauseArgs)
                   .ForCondition(references.Any(x => x == assemblyName))
                   .FailWith("Expected assembly {0} to reference assembly {1}{reason}, but it does not.", subjectName, assemblyName);
        }
#endif

        /// <summary>
        /// Asserts that the Assembly defines a type called <paramref name="namespace"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="namespace">The namespace of the class.</param>
        /// <param name="name">The name of the class.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndWhichConstraint<AssemblyAssertions, Type> DefineType(string @namespace, string name, string because = "", params object[] becauseArgs)
        {
            var foundType = Subject.GetTypes().SingleOrDefault(t => t.Namespace == @namespace && t.Name == name);

            Execute.Assertion.ForCondition(foundType != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected assembly {0} to define type {1}.{2}, but it does not.", Subject.FullName,
                    @namespace, name);

            return new AndWhichConstraint<AssemblyAssertions, Type>(this, foundType);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "assembly";
    }
}
