using System;
using System.Diagnostics;
using System.Reflection;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Types
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="PropertyInfo"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInfoAssertions :
        MemberInfoAssertions<PropertyInfo, PropertyInfoAssertions>
    {
        public PropertyInfoAssertions(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        /// <summary>
        /// Asserts that the selected property is virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeVirtual(
            string because = "", params object[] becauseArgs)
        {
            string failureMessage = "Expected property " +
                                    GetDescriptionFor(Subject) +
                                    " to be virtual{reason}, but it is not.";

            Execute.Assertion
                .ForCondition(Subject.IsVirtual())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property is not virtual.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            string failureMessage = "Expected property " + GetDescriptionFor(Subject) + " not to be virtual{reason}, but it is.";

            Execute.Assertion
                .ForCondition(!Subject.IsVirtual())
                .BecauseOf(because, becauseArgs)
                .FailWith(failureMessage);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property has a setter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeWritable(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CanWrite)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:property} {0} to have a setter{reason}.",
                    Subject);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property has a setter with the specified C# access modifier.
        /// </summary>
        /// <param name="accessModifier">The expected C# access modifier.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeWritable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            Subject.Should().BeWritable(because, becauseArgs);

            Subject.GetSetMethod(true).Should().HaveAccessModifier(accessModifier, because, becauseArgs);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property does not have a setter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> NotBeWritable(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.CanWrite)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:property} {0} not to have a setter{reason}.",
                    Subject);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property has a getter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeReadable(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.ForCondition(Subject.CanRead)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property " + Subject.Name + " to have a getter{reason}, but it does not.");

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property has a getter with the specified C# access modifier.
        /// </summary>
        /// <param name="accessModifier">The expected C# access modifier.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> BeReadable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            Subject.Should().BeReadable(because, becauseArgs);

            Subject.GetGetMethod(true).Should().HaveAccessModifier(accessModifier, because, becauseArgs);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property does not have a getter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> NotBeReadable(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(!Subject.CanRead)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected {context:property} {0} not to have a getter{reason}.",
                    Subject);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property returns a specified type.
        /// </summary>
        /// <param name="propertyType">The expected type of the property.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> Return(Type propertyType,
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.ForCondition(Subject.PropertyType == propertyType)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected Type of property " + Subject.Name + " to be {0}{reason}, but it is {1}.", propertyType, Subject.PropertyType);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property returns <typeparamref name="TReturn"/>.
        /// </summary>
        /// <typeparam name="TReturn">The expected return type.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> Return<TReturn>(string because = "", params object[] becauseArgs)
        {
            return Return(typeof(TReturn), because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the selected property does not return a specified type.
        /// </summary>
        /// <param name="propertyType">The unexpected type of the property.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> NotReturn(Type propertyType, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.PropertyType != propertyType)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected Type of property " + Subject.Name + " not to be {0}{reason}, but it is.", propertyType);

            return new AndConstraint<PropertyInfoAssertions>(this);
        }

        /// <summary>
        /// Asserts that the selected property does not return <typeparamref name="TReturn"/>.
        /// </summary>
        /// <typeparam name="TReturn">The unexpected return type.</typeparam>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<PropertyInfoAssertions> NotReturn<TReturn>(string because = "", params object[] becauseArgs)
        {
            return NotReturn(typeof(TReturn), because, becauseArgs);
        }

        internal static string GetDescriptionFor(PropertyInfo property)
        {
            string propTypeName = property.PropertyType.Name;
            return string.Format("{0} {1}.{2}", propTypeName,
                property.DeclaringType, property.Name);
        }

        internal override string SubjectDescription => GetDescriptionFor(Subject);

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "property";
    }
}
