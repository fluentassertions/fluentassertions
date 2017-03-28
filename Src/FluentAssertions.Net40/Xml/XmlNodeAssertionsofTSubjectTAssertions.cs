using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace FluentAssertions.Xml
{
    /// <summary>
    /// Contains a number of methods to assert that an <see cref="XmlNode"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class XmlNodeAssertions<TSubject, TAssertions> : ReferenceTypeAssertions<TSubject, TAssertions>
        where TAssertions: XmlNodeAssertions<TSubject, TAssertions>
        where TSubject : XmlNode
    {
        public XmlNodeAssertions(TSubject xmlNode)
        {
            Subject = xmlNode;
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlNode"/> is equivalent to the <paramref name="expected"/> element.
        /// </summary>
        /// <param name="expected">The expected element</param>
        public AndConstraint<TAssertions> BeEquivalentTo(XmlNode expected)
        {
            return BeEquivalentTo(expected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlNode"/> is equivalent to the <paramref name="expected"/> node.
        /// </summary>
        /// <param name="expected">The expected node</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeEquivalentTo(XmlNode expected, string because, params object[] reasonArgs)
        {
            using (XmlNodeReader subjectReader = new XmlNodeReader(Subject))
            using (XmlNodeReader expectedReader = new XmlNodeReader(expected))
            {
                new XmlReaderValidator(subjectReader, expectedReader, because, reasonArgs).Validate(true);
            }

            return new AndConstraint<TAssertions>((TAssertions)(this));
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlNode"/> is not equivalent to
        /// the <paramref name="unexpected"/> node.
        /// </summary>
        /// <param name="unexpected">The unexpected node</param>
        public AndConstraint<TAssertions> NotBeEquivalentTo(XmlNode unexpected)
        {
            return NotBeEquivalentTo(unexpected, string.Empty);
        }

        /// <summary>
        /// Asserts that the current <see cref="XmlNode"/> is not equivalent to
        /// the <paramref name="unexpected"/> node.
        /// </summary>
        /// <param name="unexpected">The unexpected node</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="reasonArgs">
        /// Zero or more objects to format using the placeholders in <see cref="because" />.
        /// </param>
        /// <returns></returns>
        public AndConstraint<TAssertions> NotBeEquivalentTo(XmlNode unexpected, string because, params object[] reasonArgs)
        {
            using (XmlNodeReader subjectReader = new XmlNodeReader(Subject))
            using (XmlNodeReader unexpectedReader = new XmlNodeReader(unexpected))
            {
                new XmlReaderValidator(subjectReader, unexpectedReader, because, reasonArgs).Validate(false);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Returns the type of the subject the assertion applies on.
        /// </summary>
        protected override string Context
        {
            get { return "Xml Node"; }
        }
    }
}
