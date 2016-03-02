using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="MethodBase"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public abstract class MethodBaseAssertions<TSubject, TAssertions> : MemberInfoAssertions<TSubject, TAssertions>
        where TSubject : MethodBase
        where TAssertions : MethodBaseAssertions<TSubject, TAssertions>
    {
        /// <summary>
        /// Asserts that the selected member has the specified C# <paramref name="accessModifier"/>.
        /// </summary>
        /// <param name="accessModifier">The expected C# access modifier.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveAccessModifier(
            CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.ForCondition(accessModifier == Subject.GetCSharpAccessModifier())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + Subject.Name + " to be {0}{reason}, but it is {1}.",
                    accessModifier, Subject.GetCSharpAccessModifier());

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected override string Context
        {
            get { return "methodBase"; }
        }

        internal static string GetParameterString(MethodBase methodBase)
        {
            var parameterTypes = methodBase.GetParameters().Select(p => p.ParameterType);

            return !parameterTypes.Any()
                ? String.Empty
                : parameterTypes.Select(p => p.FullName).Aggregate((p, c) => p + ", " + c);
        } 
    }
}