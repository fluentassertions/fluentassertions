using System.Diagnostics;
using System.Xml.Linq;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XAttribute"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XAttributeAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XAttributeAssertions" /> class.
        /// </summary>
        protected internal XAttributeAssertions(XAttribute attribute)
        {
            Subject = attribute;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public XAttribute Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="XAttribute"/> equals the <paramref name="expected"/> attribute.
        /// </summary>
        /// <param name="expected">The expected attribute</param>
        public AndConstraint<XAttributeAssertions> Be(XAttribute expected)
        {
            return Be(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XAttribute"/> equals the <paramref name="expected"/> attribute.
        /// </summary>
        /// <param name="expected">The expected attribute</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XAttributeAssertions> Be(XAttribute expected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Name.Equals(expected.Name) && Subject.Value.Equals(expected.Value))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML attribute to be {0}{reason}, but found {1}", expected, Subject);

            return new AndConstraint<XAttributeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XAttribute"/> does not equal the <paramref name="unexpected"/> attribute,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected attribute</param>
        public AndConstraint<XAttributeAssertions> NotBe(XAttribute unexpected)
        {
            return NotBe(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XAttribute"/> does not equal the <paramref name="unexpected"/> attribute,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected attribute</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XAttributeAssertions> NotBe(XAttribute unexpected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.Name.Equals(unexpected.Name) || !Subject.Value.Equals(unexpected.Value))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML attribute to be {0}{reason}.", unexpected);

            return new AndConstraint<XAttributeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="XAttribute"/> is <c>null</c>.
        /// </summary>
        public AndConstraint<XAttributeAssertions> BeNull()
        {
            return BeNull(string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XAttribute"/> is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<XAttributeAssertions> BeNull(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML attribute to be <null>{reason}, but found {0}.", Subject);

            return new AndConstraint<XAttributeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="XAttribute"/> is not <c>null</c>.
        /// </summary>
        public AndConstraint<XAttributeAssertions> NotBeNull()
        {
            return NotBeNull(string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XAttribute"/> is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<XAttributeAssertions> NotBeNull(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML attribute to be <null>{reason}.");

            return new AndConstraint<XAttributeAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XAttribute"/> has the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        public AndConstraint<XAttributeAssertions> HaveValue(string expected)
        {
            return HaveValue(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XAttribute"/> has the specified <paramref name="expected"/> value.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XAttributeAssertions> HaveValue(string expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Value == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML attribute '{0}' to have value {1}{reason}, but found {2}.",
                    Subject.Name, expected, Subject.Value);

            return new AndConstraint<XAttributeAssertions>(this);
        }
    }
}