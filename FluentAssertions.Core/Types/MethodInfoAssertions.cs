using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="MethodInfo"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class MethodInfoAssertions :
        ReferenceTypeAssertions<MethodInfo, MethodInfoAssertions>
    {
        public MethodInfoAssertions(MethodInfo methodInfo)
        {
            Subject = methodInfo;
        }

        /// <summary>
        /// Asserts that the selected method is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<MethodInfoAssertions, TAttribute> BeDecoratedWith<TAttribute>(
            string because = "", params object[] reasonArgs)
            where TAttribute : Attribute
        {
            return BeDecoratedWith<TAttribute>(attr => true, because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the selected method is decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<MethodInfoAssertions, TAttribute> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate,
            string because = "", params object[] reasonArgs)
            where TAttribute : Attribute
        {
            string failureMessage = "Expected method " + GetDescriptionFor(Subject) +
                " to be decorated with {0}{reason}, but that attribute was not found.";

            TAttribute attribute = Subject.GetCustomAttributes(typeof(TAttribute), false)
                .Cast<TAttribute>()
                .FirstOrDefault(isMatchingAttributePredicate.Compile());

            Execute.Assertion
                .ForCondition(attribute != null)
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndWhichConstraint<MethodInfoAssertions, TAttribute>(this, attribute);
        }

        /// <summary>
        /// Asserts that the selected method is virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoAssertions> BeVirtual(
            string because = "", params object[] reasonArgs)
        {
            string failureMessage = "Expected method " +
                                    GetDescriptionFor(Subject) +
                                    " to be virtual{reason}, but it is not virtual.";

            Execute.Assertion
                .ForCondition(!IsNonVirtual(Subject))
                .BecauseOf(because, reasonArgs)
                .FailWith(failureMessage);

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        internal static string GetDescriptionFor(MethodInfo method)
        {
            string returnTypeName = method.ReturnType.Name;

            return String.Format("{0} {1}.{2}", returnTypeName,
                method.DeclaringType, method.Name);
        }

        internal static bool IsNonVirtual(MethodInfo method)
        {
            return !method.IsVirtual || method.IsFinal;
        }

        protected override string Context
        {
            get { return "method"; }
        }
    }
}