using System.Diagnostics;
using System.Xml.Linq;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XElement"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XElementAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        protected internal XElementAssertions(XElement xElement)
        {
            Subject = xElement;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public XElement Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> equals the <paramref name="expected"/> element,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected element</param>
        public AndConstraint<XElementAssertions> Be(XElement expected)
        {
            return Be(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> equals the <paramref name="expected"/> element,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected element</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XElementAssertions> Be(XElement expected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Equals(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML element to be {0}{reason}, but found {1}", expected, Subject);

            return new AndConstraint<XElementAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> does not equal the <paramref name="unexpected"/> element,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected element</param>
        public AndConstraint<XElementAssertions> NotBe(XElement unexpected)
        {
            return NotBe(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XElement"/> does not equal the <paramref name="unexpected"/> element,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected element</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XElementAssertions> NotBe(XElement unexpected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.Equals(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML element not to be {0}{reason}.", unexpected);

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
        /// <param name="expectedName">The name of the expected attribute</param>
        /// <param name="expectedValue">The value of the expected attribute</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XElementAssertions> HaveAttribute(string expectedName, string expectedValue, string reason,
            params object [] reasonArgs)
        {
            XAttribute attribute = Subject.Attribute(expectedName);

            Execute.Verification
                .ForCondition(attribute != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith(
                    "Expected XML element to have attribute {0} with value {1}{reason}, but found no such attribute in {2}",
                    expectedName,
                    expectedValue, Subject);

            Execute.Verification
                .ForCondition(attribute.Value == expectedValue)
                .BecauseOf(reason, reasonArgs)
                .FailWith(
                    "Expected XML attribute {0} to have value {1}{reason}, but found {2}.", expectedName, expectedValue, attribute.Value);

            return new AndConstraint<XElementAssertions>(this);
        }
    }
}