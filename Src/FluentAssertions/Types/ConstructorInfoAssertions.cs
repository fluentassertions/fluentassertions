using System.Diagnostics;
using System.Reflection;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="ConstructorInfo"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class ConstructorInfoAssertions : MethodBaseAssertions<ConstructorInfo, ConstructorInfoAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInfoAssertions"/> class.
        /// </summary>
        /// <param name="constructorInfo">The constructorInfo from which to select properties.</param>
        public ConstructorInfoAssertions(ConstructorInfo constructorInfo) : base(constructorInfo)
        {
        }

        internal static string GetDescriptionFor(ConstructorInfo constructorInfo)
        {
            return string.Format("{0}({1})",
                constructorInfo.DeclaringType, GetParameterString(constructorInfo));
        }

        internal override string SubjectDescription => GetDescriptionFor(Subject);

        protected override string Identifier => "constructor";
    }
}
