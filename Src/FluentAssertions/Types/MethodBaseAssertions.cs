using System;
using System.Collections.Generic;
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
        protected MethodBaseAssertions() : this(default)
        {
        }

        protected MethodBaseAssertions(TSubject subject) : base(subject)
        {
        }

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
            CSharpAccessModifier subjectAccessModifier = Subject.GetCSharpAccessModifier();

            Execute.Assertion.ForCondition(accessModifier == subjectAccessModifier)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + Subject.Name + " to be {0}{reason}, but it is {1}.",
                    accessModifier, subjectAccessModifier);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected member does not have the specified C# <paramref name="accessModifier"/>.
        /// </summary>
        /// <param name="accessModifier">The unexpected C# access modifier.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotHaveAccessModifier(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(accessModifier != Subject.GetCSharpAccessModifier())
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected method " + Subject.Name + " not to be {0}{reason}, but it is.", accessModifier);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        protected override string Identifier => "methodBase";

        internal static string GetParameterString(MethodBase methodBase)
        {
            IEnumerable<Type> parameterTypes = methodBase.GetParameters().Select(p => p.ParameterType);

            return string.Join(", ", parameterTypes.Select(p => p.FullName));
        }
    }
}
