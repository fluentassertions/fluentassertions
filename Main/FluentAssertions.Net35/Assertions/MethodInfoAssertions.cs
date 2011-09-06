using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains assertions for the <see cref="MethodInfo"/> objects returned by the parent <see cref="MethodSelector"/>.
    /// </summary>
    [DebuggerNonUserCode]
    public class MethodInfoAssertions
    {
        /// <summary>
        /// Gets the <see cref="Type"/> that contains the specified methods.
        /// </summary>
        public Type SubjectType { get; private set; }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public IEnumerable<MethodInfo> SubjectMethods { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInfoAssertions"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> that contains the specified methods</param>
        /// <param name="methods">The methods.</param>
        public MethodInfoAssertions(Type type, IEnumerable<MethodInfo> methods)
        {
            SubjectType = type;
            SubjectMethods = methods;
        }

        /// <summary>
        /// Asserts that the selected methods are virtual.
        /// </summary>
        public AndConstraint<MethodInfoAssertions> BeVirtual()
        {
            return BeVirtual(string.Empty);
        }

        /// <summary>
        /// Asserts that the selected methods are virtual.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<MethodInfoAssertions> BeVirtual(string reason, params object[] reasonArgs)
        {
            IEnumerable<MethodInfo> nonVirtualMethods = GetAllNonVirtualMethodsFromSelection();

            Execute.Verification
                .ForCondition(!nonVirtualMethods.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected all selected methods from type {0} to be virtual{reason}, but the following methods are" +
                    " not virtual:\r\n" + GetDescriptionsFor(nonVirtualMethods), SubjectType);

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        private MethodInfo[] GetAllNonVirtualMethodsFromSelection()
        {
            return SubjectMethods.Where(method => !method.IsVirtual).ToArray();
        }

        /// <summary>
        /// Asserts that the selected methods are decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        public AndConstraint<MethodInfoAssertions> BeDecoratedWith<TAttribute>()
        {
            return BeDecoratedWith<TAttribute>(string.Empty);
        }

        /// <summary>
        /// Asserts that the selected methods are decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<MethodInfoAssertions> BeDecoratedWith<TAttribute>(string reason, params object[] reasonArgs)
        {
            IEnumerable<MethodInfo> methodsWithoutAttribute = GetMethodsWithout<TAttribute>();

            Execute.Verification
                .ForCondition(!methodsWithoutAttribute.Any())
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected all selected methods from type {0} to be decorated with {1}{reason}, but the" +
                    " following methods are not:\r\n" + GetDescriptionsFor(methodsWithoutAttribute), SubjectType, typeof(TAttribute));

            return new AndConstraint<MethodInfoAssertions>(this);
        }

        private MethodInfo[] GetMethodsWithout<TAttribute>()
        {
            return SubjectMethods.Where(method => !IsDecoratedWith<TAttribute>(method)).ToArray();
        }

        private static bool IsDecoratedWith<TAttribute>(MethodInfo method)
        {
            return method.GetCustomAttributes(false).OfType<TAttribute>().Any();
        }

        private static string GetDescriptionsFor(IEnumerable<MethodInfo> methods)
        {
            return string.Join(Environment.NewLine, methods.Select(m => m.ToString()).ToArray());
        }
    }
}