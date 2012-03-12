using System.Diagnostics;
using System.Xml.Linq;

namespace FluentAssertions.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XDocument"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XDocumentAssertions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentAssertions" /> class.
        /// </summary>
        protected internal XDocumentAssertions(XDocument document)
        {
            Subject = document;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public XDocument Subject { get; private set; }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> equals the <paramref name="expected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected document</param>
        public AndConstraint<XDocumentAssertions> Be(XDocument expected)
        {
            return Be(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> equals the <paramref name="expected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="expected">The expected document</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> Be(XDocument expected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Equals(expected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document to be {0}{reason}, but found {1}", expected, Subject);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> does not equal the <paramref name="unexpected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected document</param>
        public AndConstraint<XDocumentAssertions> NotBe(XDocument unexpected)
        {
            return NotBe(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> does not equal the <paramref name="unexpected"/> document,
        /// using its <see cref="object.Equals(object)" /> implementation.
        /// </summary>
        /// <param name="unexpected">The unexpected document</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> NotBe(XDocument unexpected, string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!Subject.Equals(unexpected))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML document to be {0}{reason}.", unexpected);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument"/> is <c>null</c>.
        /// </summary>
        public AndConstraint<XDocumentAssertions> BeNull()
        {
            return BeNull(string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument"/> is <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<XDocumentAssertions> BeNull(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document to be <null>{reason}, but found {0}.", Subject);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument"/> is not <c>null</c>.
        /// </summary>
        public AndConstraint<XDocumentAssertions> NotBeNull()
        {
            return NotBeNull(string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument"/> is not <c>null</c>.
        /// </summary>
        /// <param name="reason">
        /// A formatted phrase explaining why the assertion should be satisfied. If the phrase does not 
        /// start with the word <i>because</i>, it is prepended to the message.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more values to use for filling in any <see cref="string.Format(string,object[])" /> compatible placeholders.
        /// </param>
        public AndConstraint<XDocumentAssertions> NotBeNull(string reason, params object [] reasonArgs)
        {
            Execute.Verification
                .ForCondition(!ReferenceEquals(Subject, null))
                .BecauseOf(reason, reasonArgs)
                .FailWith("Did not expect XML document to be <null>{reason}.");

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name of the expected root element of the current document.</param>
        public AndConstraint<XDocumentAssertions> HaveRoot(string expected)
        {
            return HaveRoot(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XDocument"/> has a root element with the specified
        /// <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">The name of the expected root element of the current document.</param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> HaveRoot(string expected, string reason, params object[] reasonArgs)
        {
            XElement root = Subject.Root;

            Execute.Verification
                .ForCondition((root != null) && root.Name == expected)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document to have root element <" + expected + ">{reason}" +
                    ", but found {0}.", Subject);

            return new AndConstraint<XDocumentAssertions>(this);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
        /// child element with the specified <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">
        /// The name of the expected child element of the current document's Root <see cref="XDocument.Root"/> element.
        /// </param>
        public AndConstraint<XDocumentAssertions> HaveElement(string expected)
        {
            return HaveElement(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the <see cref="XDocument.Root"/> element of the current <see cref="XDocument"/> has a direct
        /// child element with the specified <paramref name="expected"/> name.
        /// </summary>
        /// <param name="expected">
        /// The name of the expected child element of the current document's Root <see cref="XDocument.Root"/> element.
        /// </param>
        /// <param name="reason">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="reason" />.
        /// </param>
        public AndConstraint<XDocumentAssertions> HaveElement(string expected, string reason, params object[] reasonArgs)
        {
            Execute.Verification
                .ForCondition(Subject.Root != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document {0} to have root element with child <" + expected + ">{reason}" +
                    ", but XML document has no Root element.", Subject);

            Execute.Verification
                .ForCondition(Subject.Root.Element(expected) != null)
                .BecauseOf(reason, reasonArgs)
                .FailWith("Expected XML document {0} to have root element with child <" + expected + ">{reason}" +
                    ", but no such child element was found.", Subject);

            return new AndConstraint<XDocumentAssertions>(this);
        }
    }
}