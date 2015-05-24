using System;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    public abstract class MethodBaseAssertions<TSubject, TAssertions> : MemberInfoAssertions<TSubject, TAssertions>
        where TSubject : MethodBase
        where TAssertions : MethodBaseAssertions<TSubject, TAssertions>
    {
        /// <summary>
        /// Asserts that the selected member is has the specified C# access modifier.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveAccessModifier(
            CSharpAccessModifiers accessModifier, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(accessModifier == Subject.GetCSharpAccessModifier())
                .BecauseOf(because, reasonArgs)
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