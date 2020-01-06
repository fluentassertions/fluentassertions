using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains assertions for the <see cref="MethodInfo"/> objects returned by the parent <see cref="MethodInfoSelector"/>.
    /// </summary>
    [DebuggerNonUserCode]
    public class MethodInfoSelectorAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoSelectorAssertions"/> class.
        /// </summary>
        /// <param name="methods">The methods to assert.</param>
        public MethodInfoSelectorAssertions(params MethodInfo[] methods)
        {
            SubjectMethods = methods;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<MethodInfo> SubjectMethods { get; private set; }

        /// <summary>
        /// Asserts that the selected methods are virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoSelectorAssertions> BeVirtual(string because = "", params object[] becauseArgs)
        {
            MethodInfo[] nonVirtualMethods = GetAllNonVirtualMethodsFromSelection();

            string failureMessage =
                "Expected all selected methods to be virtual{reason}, but the following methods are not virtual:" +
                Environment.NewLine +
                GetDescriptionsFor(nonVirtualMethods);

            Execute.Assertion
                .ForCondition(!nonVirtualMethods.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage);

            return new AndConstraint<MethodInfoSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected methods are not virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoSelectorAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            MethodInfo[] virtualMethods = GetAllVirtualMethodsFromSelection();

            string failureMessage =
                "Expected all selected methods not to be virtual{reason}, but the following methods are virtual:" +
                Environment.NewLine +
                GetDescriptionsFor(virtualMethods);

            Execute.Assertion
                .ForCondition(!virtualMethods.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage);

            return new AndConstraint<MethodInfoSelectorAssertions>(this);
        }

        private MethodInfo[] GetAllNonVirtualMethodsFromSelection()
        {
            IEnumerable<MethodInfo> query =
                from method in SubjectMethods
                where method.IsNonVirtual()
                select method;

            return query.ToArray();
        }

        private MethodInfo[] GetAllVirtualMethodsFromSelection()
        {
            IEnumerable<MethodInfo> query =
                from method in SubjectMethods
                where !method.IsNonVirtual()
                select method;

            return query.ToArray();
        }

        /// <summary>
        /// Asserts that the selected methods are decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoSelectorAssertions> BeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            return BeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the selected methods are decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoSelectorAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate, nameof(isMatchingAttributePredicate));

            MethodInfo[] methodsWithoutAttribute = GetMethodsWithout(isMatchingAttributePredicate);

            string failureMessage =
                "Expected all selected methods to be decorated with {0}{reason}, but the following methods are not:" +
                Environment.NewLine +
                GetDescriptionsFor(methodsWithoutAttribute);

            Execute.Assertion
                .ForCondition(!methodsWithoutAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<MethodInfoSelectorAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected methods are not decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            return NotBeDecoratedWith<TAttribute>(_ => true, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the selected methods are not decorated with an attribute of type <typeparamref name="TAttribute"/>
        /// that matches the specified <paramref name="isMatchingAttributePredicate"/>.
        /// </summary>
        /// <param name="isMatchingAttributePredicate">
        /// The predicate that the attribute must match.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<MethodInfoSelectorAssertions> NotBeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] becauseArgs)
            where TAttribute : Attribute
        {
            Guard.ThrowIfArgumentIsNull(isMatchingAttributePredicate, nameof(isMatchingAttributePredicate));

            MethodInfo[] methodsWithAttribute = GetMethodsWith(isMatchingAttributePredicate);

            string failureMessage =
                "Expected all selected methods to not be decorated with {0}{reason}, but the following methods are:" +
                Environment.NewLine +
                GetDescriptionsFor(methodsWithAttribute);

            Execute.Assertion
                .ForCondition(!methodsWithAttribute.Any())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage, typeof(TAttribute));

            return new AndConstraint<MethodInfoSelectorAssertions>(this);
        }

        private MethodInfo[] GetMethodsWithout<TAttribute>(Expression<Func<TAttribute, bool>> isMatchingPredicate)
            where TAttribute : Attribute
        {
            return SubjectMethods.Where(method => !method.IsDecoratedWith(isMatchingPredicate)).ToArray();
        }

        private MethodInfo[] GetMethodsWith<TAttribute>(Expression<Func<TAttribute, bool>> isMatchingPredicate)
            where TAttribute : Attribute
        {
            return SubjectMethods.Where(method => method.IsDecoratedWith(isMatchingPredicate)).ToArray();
        }

        private static string GetDescriptionsFor(IEnumerable<MethodInfo> methods)
        {
            return string.Join(Environment.NewLine,
                methods.Select(MethodInfoAssertions.GetDescriptionFor).ToArray());
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
#pragma warning disable CA1822 // Do not change signature of a public member
        protected string Context => "method";
#pragma warning restore CA1822
    }
}
