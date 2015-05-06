using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Reflection
{
    #if !PORTABLE && !SILVERLIGHT && !WINRT && !DNXCORE
    public class AssemblyAssertions
    {
        public AssemblyAssertions(Assembly assembly)
        {
            Subject = assembly;
        }

        public Assembly Subject { get; private set; }

        /// <summary>
        /// Asserts that an assembly does not reference the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly which should not be referenced.</param>
        public void NotReference(Assembly assembly)
        {
            NotReference(assembly, String.Empty);
        }

        /// <summary>
        /// Asserts that an assembly does not reference the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly which should not be referenced.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public void NotReference(Assembly assembly, string because, params string[] reasonArgs)
        {
            var subjectName = Subject.GetName().Name;
            var assemblyName = assembly.GetName().Name;

            var references = Subject.GetReferencedAssemblies().Select(x => x.Name);

            Execute.Assertion
                   .BecauseOf(because, reasonArgs)
                   .ForCondition(references.All(x => x != assemblyName))
                   .FailWith("Assembly {0} should not reference assembly {1}{reason}", subjectName, assemblyName);
        }

        /// <summary>
        /// Asserts that an assembly references the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly which should be referenced.</param>
        public void Reference(Assembly assembly)
        {
            Reference(assembly, String.Empty);
        }

        /// <summary>
        /// Asserts that an assembly references the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly which should be referenced.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public void Reference(Assembly assembly, string because, params string[] reasonArgs)
        {
            var subjectName = Subject.GetName().Name;
            var assemblyName = assembly.GetName().Name;

            var references = Subject.GetReferencedAssemblies().Select(x => x.Name);

            Execute.Assertion
                   .BecauseOf(because, reasonArgs)
                   .ForCondition(references.Any(x => x == assemblyName))
                   .FailWith("Assembly {0} should reference assembly {1}{reason}, but it does not", subjectName, assemblyName);
        }
    }
#endif
}
