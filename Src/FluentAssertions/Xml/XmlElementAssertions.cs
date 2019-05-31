#if !NETSTANDARD1_3 && !NETSTANDARD1_6

using System.Diagnostics;
using System.Xml;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Xml
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XmlElement"/>
    /// is in the expected state./>
    /// </summary>
    [DebuggerNonUserCode]
    public class XmlElementAssertions : XmlNodeAssertions<XmlElement, XmlElementAssertions>
    {
        /// <summary>
        /// Initialized a new instance of the <see cref="XmlElementAssertions"/>
        /// class.
        /// </summary>
        /// <param name="xmlElement"></param>
        public XmlElementAssertions(XmlElement xmlElement)
            : base(xmlElement)
        {
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has the specified
        /// <paramref name="expected"/> inner text.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        public AndConstraint<XmlElementAssertions> HaveInnerText(string expected)
        {
            return HaveInnerText(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has the specified
        /// <paramref name="expected"/> inner text.
        /// </summary>
        /// <param name="expected">The expected value.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XmlElementAssertions> HaveInnerText(string expected, string because, params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.InnerText == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected XML element {0} to have value {1}{reason}, but found {2}.",
                    Subject.Name, expected, Subject.InnerText);

            return new AndConstraint<XmlElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has an attribute
        /// with the specified <paramref name="expectedName"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        public AndConstraint<XmlElementAssertions> HaveAttribute(string expectedName, string expectedValue)
        {
            return HaveAttribute(expectedName, expectedValue, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has an attribute
        /// with the specified <paramref name="expectedName"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XmlElementAssertions> HaveAttribute(string expectedName, string expectedValue, string because, params object[] becauseArgs)
        {
            return HaveAttributeWithNamespace(expectedName, string.Empty, expectedValue, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has an attribute
        /// with the specified <paramref name="expectedName"/>, <param name="expectedNamespace"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        public AndConstraint<XmlElementAssertions> HaveAttributeWithNamespace(string expectedName, string expectedNamespace, string expectedValue)
        {
            return HaveAttributeWithNamespace(expectedName, expectedNamespace, expectedValue, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has an attribute
        /// with the specified <paramref name="expectedName"/>, <param name="expectedNamespace"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XmlElementAssertions> HaveAttributeWithNamespace(
            string expectedName,
            string expectedNamespace,
            string expectedValue,
            string because, params object[] becauseArgs)
        {
            XmlAttribute attribute = Subject.Attributes[expectedName, expectedNamespace];

            string expectedFormattedName =
                (string.IsNullOrEmpty(expectedNamespace) ? "" : "{" + expectedNamespace + "}")
                + expectedName;

            Execute.Assertion
                .ForCondition(attribute != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected XML element to have attribute {0}"
                    + " with value {1}{reason}, but found no such attribute in {2}",
                    expectedFormattedName, expectedValue, Subject);

            Execute.Assertion
                .ForCondition(attribute.Value == expectedValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected XML attribute {0} to have value {1}{reason}, but found {2}.",
                    expectedFormattedName, expectedValue, attribute.Value);

            return new AndConstraint<XmlElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has a direct child element with the specified
        /// <paramref name="expectedName"/> name.
        /// </summary>
        /// <param name="expectedName">The name of the expected child element</param>
        public AndWhichConstraint<XmlElementAssertions, XmlElement> HaveElement(string expectedName)
        {
            return HaveElement(expectedName, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has a direct child element with the specified
        /// <paramref name="expectedName"/> name.
        /// </summary>
        /// <param name="expectedName">The name of the expected child element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<XmlElementAssertions, XmlElement> HaveElement(
            string expectedName,
            string because,
            params object[] becauseArgs)
        {
            return HaveElementWithNamespace(expectedName, string.Empty, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has a direct child element with the specified
        /// <paramref name="expectedName"/> name and <paramref name="expectedNamespace" /> namespace.
        /// </summary>
        /// <param name="expectedName">The name of the expected child element</param>
        /// <param name="expectedNamespace">The namespace of the expected child element</param>
        public AndWhichConstraint<XmlElementAssertions, XmlElement> HaveElementWithNamespace(
            string expectedName, string expectedNamespace)
        {
            return HaveElementWithNamespace(expectedName, expectedNamespace, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlElement"/> has a direct child element with the specified
        /// <paramref name="expectedName"/> name and <paramref name="expectedNamespace" /> namespace.
        /// </summary>
        /// <param name="expectedName">The name of the expected child element</param>
        /// <param name="expectedNamespace">The namespace of the expected child element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<XmlElementAssertions, XmlElement> HaveElementWithNamespace(
            string expectedName,
            string expectedNamespace,
            string because,
            params object[] becauseArgs)
        {
            XmlElement element = Subject[expectedName, expectedNamespace];

            string expectedFormattedName =
                (string.IsNullOrEmpty(expectedNamespace) ? "" : "{" + expectedNamespace + "}")
                + expectedName;

            Execute.Assertion
                .ForCondition(element != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected XML element {0} to have child element \"" +
                    expectedFormattedName.EscapePlaceholders() + "\"{reason}" +
                        ", but no such child element was found.", Subject);

            return new AndWhichConstraint<XmlElementAssertions, XmlElement>(this, element);
        }
    }
}

#endif
