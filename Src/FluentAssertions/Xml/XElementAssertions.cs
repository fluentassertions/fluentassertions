using System.Diagnostics;
using System.Xml;
using System.Xml.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using FluentAssertions.Xml.Equivalency;

namespace FluentAssertions.Xml
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XElement"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XElementAssertions : ReferenceTypeAssertions<XElement, XElementAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XElementAssertions" /> class.
        /// </summary>
        public XElementAssertions(XElement xElement) : base(xElement)
        {
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> equals the
        /// <paramref name="expected"/> element, by using
        /// <see cref="XNode.DeepEquals(XNode, XNode)"/>
        /// </summary>
        /// <param name="expected">The expected element</param>
        public AndConstraint<XElementAssertions> Be(XElement expected)
        {
            return Be(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> equals the
        /// <paramref name="expected"/> element, by using
        /// <see cref="XNode.DeepEquals(XNode, XNode)"/>
        /// </summary>
        /// <param name="expected">The expected element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XElementAssertions> Be(XElement expected, string because, params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(XNode.DeepEquals(Subject, expected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected XML element to be {0}{reason}, but found {1}.", expected, Subject);

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> does not equal the
        /// <paramref name="unexpected"/> element, using
        /// <see cref="XNode.DeepEquals(XNode, XNode)" />.
        /// </summary>
        /// <param name="unexpected">The unexpected element</param>
        public AndConstraint<XElementAssertions> NotBe(XElement unexpected)
        {
            return NotBe(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> does not equal the
        /// <paramref name="unexpected"/> element, using
        /// <see cref="XNode.DeepEquals(XNode, XNode)" />.
        /// </summary>
        /// <param name="unexpected">The unexpected element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XElementAssertions> NotBe(XElement unexpected, string because, params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition((Subject is null && !(unexpected is null)) || !XNode.DeepEquals(Subject, unexpected))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected XML element not to be {0}{reason}.", unexpected);

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> is equivalent to the
        /// <paramref name="expected"/> element, using a semantic equivalency
        /// comparison.
        /// </summary>
        /// <param name="expected">The expected element</param>
        public AndConstraint<XElementAssertions> BeEquivalentTo(XElement expected)
        {
            return BeEquivalentTo(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> is equivalent to the
        /// <paramref name="expected"/> element, using a semantic equivalency
        /// comparison.
        /// </summary>
        /// <param name="expected">The expected element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XElementAssertions> BeEquivalentTo(XElement expected, string because, params object[] becauseArgs)
        {
            using (XmlReader subjectReader = Subject.CreateReader())
            using (XmlReader expectedReader = expected.CreateReader())
            {
                var xmlReaderValidator = new XmlReaderValidator(subjectReader, expectedReader, because, becauseArgs);
                xmlReaderValidator.Validate(true);
            }

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> is not equivalent to
        /// the <paramref name="unexpected"/> element, using a semantic
        /// equivalency comparison.
        /// </summary>
        /// <param name="unexpected">The unexpected element</param>
        public AndConstraint<XElementAssertions> NotBeEquivalentTo(XElement unexpected)
        {
            return NotBeEquivalentTo(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> is not equivalent to
        /// the <paramref name="unexpected"/> element, using a semantic
        /// equivalency comparison.
        /// </summary>
        /// <param name="unexpected">The unexpected element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XElementAssertions> NotBeEquivalentTo(XElement unexpected, string because, params object[] becauseArgs)
        {
            using (XmlReader subjectReader = Subject.CreateReader())
            using (XmlReader otherReader = unexpected.CreateReader())
            {
                var xmlReaderValidator = new XmlReaderValidator(subjectReader, otherReader, because, becauseArgs);
                xmlReaderValidator.Validate(false);
            }

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        public AndConstraint<XElementAssertions> HaveValue(string expected)
        {
            return HaveValue(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XElementAssertions> HaveValue(string expected, string because, params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Value == expected)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected XML element '{0}' to have value {1}{reason}, but found {2}.",
                    Subject.Name, expected, Subject.Value);

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        public AndConstraint<XElementAssertions> HaveAttribute(string expectedName, string expectedValue)
        {
            return HaveAttribute(expectedName, expectedValue, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name <see cref="XName"/> of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        public AndConstraint<XElementAssertions> HaveAttribute(XName expectedName, string expectedValue)
        {
            return HaveAttribute(expectedName, expectedValue, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>
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
        public AndConstraint<XElementAssertions> HaveAttribute(string expectedName, string expectedValue, string because,
            params object[] becauseArgs)
        {
            return HaveAttribute(XNamespace.None + expectedName, expectedValue, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has an attribute with the specified <paramref name="expectedName"/>
        /// and <paramref name="expectedValue"/>.
        /// </summary>
        /// <param name="expectedName">The name <see cref="XName"/> of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<XElementAssertions> HaveAttribute(XName expectedName, string expectedValue, string because,
            params object[] becauseArgs)
        {
            XAttribute attribute = Subject.Attribute(expectedName);
            string expectedText = expectedName.ToString().EscapePlaceholders();

            Execute.Assertion
                .ForCondition(attribute != null)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected XML element to have attribute \"" + expectedText + "\" with value {0}{reason}, but found no such attribute in {1}",
                    expectedValue, Subject);

            Execute.Assertion
                .ForCondition(attribute.Value == expectedValue)
                .BecauseOf(because, becauseArgs)
                .FailWith(
                    "Expected XML attribute \"" + expectedText + "\" to have value {0}{reason}, but found {1}.", expectedValue, attribute.Value);

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has a direct child element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name of the expected child element</param>
        public AndWhichConstraint<XElementAssertions, XElement> HaveElement(string expected)
        {
            return HaveElement(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has a direct child element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name <see cref="XName"/> of the expected child element</param>
        public AndWhichConstraint<XElementAssertions, XElement> HaveElement(XName expected)
        {
            return HaveElement(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has a direct child element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name <see cref="XName"/> of the expected child element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<XElementAssertions, XElement> HaveElement(string expected, string because, params object[] becauseArgs)
        {
            return HaveElement(XNamespace.None + expected, because, becauseArgs);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> has a direct child element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name <see cref="XName"/> of the expected child element</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndWhichConstraint<XElementAssertions, XElement> HaveElement(XName expected, string because, params object[] becauseArgs)
        {
            XElement xElement = Subject.Element(expected);
            Execute.Assertion
                .ForCondition(xElement != null)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected XML element {0} to have child element \"" + expected.ToString().EscapePlaceholders() + "\"{reason}" +
                        ", but no such child element was found.", Subject);

            return new AndWhichConstraint<XElementAssertions, XElement>(this, xElement);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Identifier => "XML element";
    }
}
