using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="Type"/> meets certain expectations.
    /// </summary>
    [DebuggerNonUserCode]
    public class TypeAssertions : ReferenceTypeAssertions<Type, TypeAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public TypeAssertions(Type type)
        {
            Subject = type;
        }

        /// <summary>
        /// Asserts that the current type is equal to the specified <typeparamref name="TExpected"/> type.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> Be<TExpected>(string because = "", params object[] reasonArgs)
        {
            return Be(typeof(TExpected), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current type is equal to the specified <paramref name="expected"/> type.
        /// </summary>
        /// <param name="expected">The expected type</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> Be(Type expected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type to be {0}{reason}, but found <null>.", expected);

            Execute.Assertion
                .ForCondition(Subject == expected)
                .BecauseOf(because, reasonArgs)
                .FailWith(GetFailureMessageIfTypesAreDifferent(Subject, expected));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts than an instance of the subject type is assignable variable of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to which instances of the type should be assignable.</typeparam>
        /// <param name="because">The reason why instances of the type should be assignable to the type.</param>
        /// <param name="reasonArgs">The parameters used when formatting the <paramref name="because"/>.</param>
        /// <returns>An <see cref="AndConstraint{T}"/> which can be used to chain assertions.</returns>
        public new AndConstraint<TypeAssertions> BeAssignableTo<T>(string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(typeof(T).IsAssignableFrom(Subject))
                .BecauseOf(because, reasonArgs)
                .FailWith(
                    "Expected {context:" + Context + "} {0} to be assignable to {1}{reason}, but it is not",
                    Subject,
                    typeof(T));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Creates an error message in case the specified <paramref name="actual"/> type differs from the 
        /// <paramref name="expected"/> type.
        /// </summary>
        /// <returns>
        /// An empty <see cref="string"/> if the two specified types are the same, or an error message that describes that
        /// the two specified types are not the same.
        /// </returns>
        private static string GetFailureMessageIfTypesAreDifferent(Type actual, Type expected)
        {
            if (actual == expected)
            {
                return "";
            }

            string expectedType = (expected != null) ? expected.FullName : "<null>";
            string actualType = (actual != null) ? actual.FullName : "<null>";

            if (expectedType == actualType)
            {
                expectedType = "[" + expected.AssemblyQualifiedName + "]";
                actualType = "[" + actual.AssemblyQualifiedName + "]";
            }

            return string.Format("Expected type to be {0}{{reason}}, but found {1}.", expectedType, actualType);
        }

        /// <summary>
        /// Asserts that the current type is not equal to the specified <typeparamref name="TUnexpected"/> type.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> NotBe<TUnexpected>(string because = "", params object[] reasonArgs)
        {
            return NotBe(typeof(TUnexpected), because, reasonArgs);
        }

        /// <summary>
        /// Asserts that the current type is not equal to the specified <paramref name="unexpected"/> type.
        /// </summary>
        /// <param name="unexpected">The unexpected type</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> NotBe(Type unexpected, string because = "", params object[] reasonArgs)
        {
            Execute.Assertion
                .ForCondition(Subject != unexpected)
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type not to be [" + unexpected.AssemblyQualifiedName + "]{reason}.");

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with the specified <typeparamref name="TAttribute"/>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(string because = "", params object[] reasonArgs)
            where TAttribute : Attribute
        {
            Execute.Assertion
                .ForCondition(Subject.IsDecoratedWith<TAttribute>())
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to be decorated with {1}{reason}, but the attribute was not found.",
                    Subject, typeof (TAttribute));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> is decorated with an attribute of type <typeparamref name="TAttribute"/>
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
        public AndConstraint<TypeAssertions> BeDecoratedWith<TAttribute>(
            Expression<Func<TAttribute, bool>> isMatchingAttributePredicate, string because = "", params object[] reasonArgs)
            where TAttribute : Attribute
        {
            BeDecoratedWith<TAttribute>(because, reasonArgs);

            Execute.Assertion
                .ForCondition(Subject.HasMatchingAttribute(isMatchingAttributePredicate))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to be decorated with {1} that matches {2}{reason}, but no matching attribute was found.",
                    Subject, typeof(TAttribute), isMatchingAttributePredicate.Body);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> implements Interface <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="interfaceType">The interface that should be implemented.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> Implement(Type interfaceType, string because = "", params object[] reasonArgs)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Must be an interface Type.", "interfaceType");
            }

            Execute.Assertion.ForCondition(Subject.GetInterfaces().Contains(interfaceType))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to implement interface {1}{reason}.", Subject, interfaceType);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="Type"/> does not implement Interface <paramref name="interfaceType"/>.
        /// </summary>
        /// <param name="interfaceType">The interface that should be implemented.</param>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        public AndConstraint<TypeAssertions> NotImplement(Type interfaceType, string because = "", params object[] reasonArgs)
        {
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Must be an interface Type.", "interfaceType");
            }

            Execute.Assertion.ForCondition(!Subject.GetInterfaces().Contains(interfaceType))
                .BecauseOf(because, reasonArgs)
                .FailWith("Expected type {0} to not implement interface {1}{reason}.", Subject, interfaceType);

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type does not expose a property named <paramref name="name"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="name">The name of the property.</param>
        public AndConstraint<TypeAssertions> NotHaveProperty(string name, string because = "", params object[] reasonArgs)
        {
            PropertyInfo propertyInfo = Subject
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .SingleOrDefault(p => !p.IsIndexer() && p.Name == name);

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo == null)
                .BecauseOf(because, reasonArgs)
                .FailWith(string.Format("Expected {0} to not exist{{reason}}, but it does.", propertyInfoDescription));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type has a property of type <paramref name="propertyType"/> named <paramref name="name"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="propertyType">The type of the property.</param>
        /// <param name="name">The name of the property.</param>
        public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveProperty(Type propertyType, string name, string because = "", params object[] reasonArgs)
        {
            PropertyInfo propertyInfo = Subject
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .SingleOrDefault(p => !p.IsIndexer() && p.Name == name);

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo != null)
                .BecauseOf(because, reasonArgs)
                .FailWith(String.Format("Expected {0} {1}.{2} to exist{{reason}}, but it does not.",
                    propertyType.Name, Subject.FullName, name));
            
            Execute.Assertion.ForCondition(propertyInfo.PropertyType == propertyType)
                .BecauseOf(because, reasonArgs)
                .FailWith(String.Format("Expected {0} to be of type {1}{{reason}}, but it is not.",
                    propertyInfoDescription, propertyType));

            return new AndWhichConstraint<TypeAssertions, PropertyInfo>(this, propertyInfo);
        }

        /// <summary>
        /// Asserts that the current type does not expose an indexer that takes parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="parameterTypes">The expected indexer's parameter types.</param>
        public AndConstraint<TypeAssertions> NotHaveIndexer(IEnumerable<Type> parameterTypes, string because = "", params object[] reasonArgs)
        {
            PropertyInfo propertyInfo = Subject
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .SingleOrDefault(p => p.IsIndexer() && p.GetIndexParameters().Select(i => i.ParameterType).SequenceEqual(parameterTypes));

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo == null)
                .BecauseOf(because, reasonArgs)
                .FailWith(String.Format("Expected indexer {0}[{1}] to not exist{{reason}}, but it does.",
                    Subject.FullName,
                    parameterTypes.Select(p => p.FullName).Aggregate((p, c) => p + ", " + c)));

            return new AndConstraint<TypeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current type has an indexer of type <paramref name="indexerType"/>.
        /// with parameter types <paramref name="parameterTypes"/>.
        /// </summary>
        /// <param name="because">A formatted phrase as is supported by <see cref="M:System.String.Format(System.String,System.Object[])"/> explaining why the assertion
        ///             is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.</param>
        /// <param name="reasonArgs">Zero or more objects to format using the placeholders in <see cref="!:because"/>.</param>
        /// <param name="indexerType">The type of the indexer.</param>
        /// <param name="parameterTypes">The parameter types for the indexer.</param>
        public AndWhichConstraint<TypeAssertions, PropertyInfo> HaveIndexer(Type indexerType, IEnumerable<Type> parameterTypes, string because = "", params object[] reasonArgs)
        {
            PropertyInfo propertyInfo = Subject
                    .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                    .SingleOrDefault(p => p.IsIndexer() && p.GetIndexParameters().Select(i => i.ParameterType).SequenceEqual(parameterTypes));

            string propertyInfoDescription = "";

            if (propertyInfo != null)
            {
                propertyInfoDescription = PropertyInfoAssertions.GetDescriptionFor(propertyInfo);
            }

            Execute.Assertion.ForCondition(propertyInfo != null)
                .BecauseOf(because, reasonArgs)
                .FailWith(String.Format("Expected {0} {1}[{2}] to exist{{reason}}, but it does not.",
                    indexerType.Name, Subject.FullName, 
                    parameterTypes.Select(p => p.FullName).Aggregate((p,c) => p + ", " + c)));

            Execute.Assertion.ForCondition(propertyInfo.PropertyType == indexerType)
                .BecauseOf(because, reasonArgs)
                .FailWith(String.Format("Expected {0} to be of type {1}{{reason}}, but it is not.",
                    propertyInfoDescription, indexerType));

            return new AndWhichConstraint<TypeAssertions, PropertyInfo>(this, propertyInfo);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "type"; }
        }
    }
}
