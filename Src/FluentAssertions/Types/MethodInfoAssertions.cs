using System;
using System.Diagnostics;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="MethodInfo"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class MethodInfoAssertions : MethodBaseAssertions<MethodInfo, MethodInfoAssertions>
    {
        public MethodInfoAssertions() : this(default)
        {
        }

        public MethodInfoAssertions(MethodInfo methodInfo) : base(methodInfo)
        {
        }

        /// <summary>
        /// Asserts that the selected method is virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoAssertions> BeVirtual(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.IsNonVirtual())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedMethodXToBeVirtualFormat + Resources.Method_CommaButItIsNotVirtual,
                    SubjectDescription.ToAlreadyFormattedString());

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected method is not virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.IsNonVirtual())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedMethodXNotToBeVirtualFormat + Resources.Common_CommaButItIs,
                    SubjectDescription.ToAlreadyFormattedString());

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected method is async.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<MethodInfoAssertions> BeAsync(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.IsAsync())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedMethodXToBeAsyncFormat + Resources.Common_CommaButItIsNot,
                    SubjectDescription.ToAlreadyFormattedString());

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected method is not async.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="becauseArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<MethodInfoAssertions> NotBeAsync(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.IsAsync())
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedMethodXNotToBeAsyncFormat + Resources.Common_CommaButItIs,
                    SubjectDescription.ToAlreadyFormattedString());

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected method returns void.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> ReturnVoid(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.ForCondition(typeof(void) == Subject.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedReturnTypeOfMethodXToBeVoidFormat + Resources.Common_CommaButItIsYFormat,
                    Subject.Name.ToAlreadyFormattedString(),
                    Subject.ReturnType.FullName);

            return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
        }

        /// <summary>
        /// Asserts that the selected method returns <paramref name="returnType"/>.
        /// </summary>
        /// <param name="returnType">The expected return type.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> Return(Type returnType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.ForCondition(returnType == Subject.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedReturnTypeOfMethodXToBeYFormat + Resources.Common_CommaButItIsZFormat,
                    Subject.Name.ToAlreadyFormattedString(),
                    returnType,
                    Subject.ReturnType.FullName);

            return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
        }

        /// <summary>
        /// Asserts that the selected method returns <typeparamref name="TReturn"/>.
        /// </summary>
        /// <typeparam name="TReturn">The expected return type.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> Return<TReturn>(string because = "", params object[] becauseArgs)
        {
            return Return(typeof(TReturn), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the selected method does not return void.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> NotReturnVoid(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(typeof(void) != Subject.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedReturnTypeOfMethodXNotToBeVoidFormat + Resources.Common_CommaButItIs,
                    Subject.Name.ToAlreadyFormattedString());

            return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
        }

        /// <summary>
        /// Asserts that the selected method does not return <paramref name="returnType"/>.
        /// </summary>
        /// <param name="returnType">The unexpected return type.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> NotReturn(Type returnType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(returnType != Subject.ReturnType)
                .BecauseOf(because, becauseArgs)
                .FailWith(Resources.Method_ExpectedReturnTypeOfMethodXNotToBeYFormat + Resources.Common_CommaButItIs,
                    Subject.Name.ToAlreadyFormattedString(), returnType);

            return new AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>>(this);
        }

        /// <summary>
        /// Asserts that the selected method does not return <typeparamref name="TReturn"/>.
        /// </summary>
        /// <typeparam name="TReturn">The unexpected return type.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodBaseAssertions<MethodInfo, MethodInfoAssertions>> NotReturn<TReturn>(string because = "", params object[] becauseArgs)
        {
            return NotReturn(typeof(TReturn), because, becauseArgs);
        }

        internal static string GetDescriptionFor(MethodInfo method)
        {
            string returnTypeName = method.ReturnType.Name;

            return string.Format(Resources.MethodInfo_DescriptionFormat, returnTypeName,
                method.DeclaringType, method.Name);
        }

        internal override string SubjectDescription => GetDescriptionFor(Subject);

        protected override string Identifier => "method";
    }
}
