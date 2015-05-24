using System;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    public abstract class MethodBaseAssertions<TSubject, TAssertions> : MemberInfoAssertions<TSubject, TAssertions>
        where TSubject : MethodInfo
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
        public AndConstraint<MethodBaseAssertions<TSubject, TAssertions>> HaveAccessModifier(
            CSharpAccessModifiers accessModifier, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(accessModifier == Subject.GetCSharpAccessModifier())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected method " + Subject.Name + " to be {0}{reason}, but it is {1}.",
                    accessModifier, Subject.GetCSharpAccessModifier());

            return new AndConstraint<MethodBaseAssertions<TSubject, TAssertions>>(this);
        }

        /// <summary>
        /// Asserts that the selected MethodBase returns void.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<TSubject, TAssertions>> ReturnVoid(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(typeof(void) == Subject.ReturnType)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected the return type of method " + Subject.Name + " to be void {reason}, but it is {0}.",
                    Subject.ReturnType.FullName);

            return new AndConstraint<MethodBaseAssertions<TSubject, TAssertions>>(this);
        }

        /// <summary>
        /// Asserts that the selected MethodBase returns <paramref name="returnType"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <param name="returnType">The expected return type.</param>
        public AndConstraint<MethodBaseAssertions<TSubject, TAssertions>> Return(Type returnType, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion.ForCondition(returnType == Subject.ReturnType)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected the return type of method " + Subject.Name + " to be {0} {reason}, but it is {1}.",
                    returnType, Subject.ReturnType.FullName);

            return new AndConstraint<MethodBaseAssertions<TSubject, TAssertions>>(this);
        }

        protected override string Context
        {
            get { return "methodBase"; }
        }
    }
}