using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentAssertions.Execution;

namespace FluentAssertions.Xml.Equivalency
{
    internal class XmlReaderValidator
    {
        private readonly AssertionScope assertion;
        private readonly XmlIterator subjectIterator;
        private readonly XmlIterator expectationIterator;
        private Node currentNode = Node.CreateRoot();

        public XmlReaderValidator(XmlReader subjectReader, XmlReader expectationReader, string because, object[] reasonArgs)
        {
            assertion = Execute.Assertion.BecauseOf(because, reasonArgs);

            subjectIterator = new XmlIterator(subjectReader);
            expectationIterator = new XmlIterator(expectationReader);
        }

        public void Validate(bool shouldBeEquivalent)
        {
            Failure failure = Validate();

            if (shouldBeEquivalent && failure != null)
            {
                assertion.FailWith(failure.FormatString, failure.FormatParams);
            }

            if (!shouldBeEquivalent && failure is null)
            {
                assertion.FailWith("Did not expect Xml to be equivalent{reason}, but it is.");
            }
        }

        private Failure Validate()
        {
            while (!subjectIterator.IsEndOfDocument && !expectationIterator.IsEndOfDocument)
            {
                if (subjectIterator.NodeType != expectationIterator.NodeType)
                {
                    return new Failure("Expected node of type {0} at {1}{reason}, but found {2}.",
                        expectationIterator.NodeType, currentNode.GetXPath(), subjectIterator.NodeType);
                }

                Failure failure = null;

                switch (subjectIterator.NodeType)
                {
                    case XmlNodeType.Element:
                        failure = ValidateStartElement();
                        if (failure != null)
                        {
                            return failure;
                        }

                        // starting new element, add local name to location stack
                        // to build XPath info
                        currentNode = currentNode.Push(subjectIterator.LocalName);

                        failure = ValidateAttributes();

                        if (subjectIterator.IsEmptyElement)
                        {
                            // The element is already complete. (We will NOT get an EndElement node.)
                            // Update node information.
                            currentNode = currentNode.Parent;
                        }

                        // check whether empty element and self-closing element needs to be synchronized
                        if (subjectIterator.IsEmptyElement && !expectationIterator.IsEmptyElement)
                        {
                            expectationIterator.MoveToEndElement();
                        }
                        else if (expectationIterator.IsEmptyElement && !subjectIterator.IsEmptyElement)
                        {
                            subjectIterator.MoveToEndElement();
                        }

                        break;

                    case XmlNodeType.EndElement:
                        // No need to verify end element, if it doesn't match
                        // the start element it isn't valid XML, so the parser
                        // would handle that.
                        currentNode.Pop();
                        currentNode = currentNode.Parent;
                        break;

                    case XmlNodeType.Text:
                        failure = ValidateText();
                        break;

                    default:
                        throw new NotSupportedException($"{subjectIterator.NodeType} found at {currentNode.GetXPath()} is not supported for equivalency comparison.");
                }

                if (failure != null)
                {
                    return failure;
                }

                subjectIterator.Read();
                expectationIterator.Read();
            }

            if (!expectationIterator.IsEndOfDocument)
            {
                return new Failure("Expected {0}{reason}, but found end of document.",
                    expectationIterator.LocalName);
            }

            if (!subjectIterator.IsEndOfDocument)
            {
                return new Failure("Expected end of document{reason}, but found {0}.",
                    subjectIterator.LocalName);
            }

            return null;
        }

        private Failure ValidateAttributes()
        {
            IList<AttributeData> expectedAttributes = expectationIterator.GetAttributes();
            IList<AttributeData> subjectAttributes = subjectIterator.GetAttributes();

            foreach (AttributeData subjectAttribute in subjectAttributes)
            {
                AttributeData expectedAttribute = expectedAttributes.SingleOrDefault(
                    ea => ea.NamespaceUri == subjectAttribute.NamespaceUri
                    && ea.LocalName == subjectAttribute.LocalName);

                if (expectedAttribute is null)
                {
                    return new Failure("Did not expect to find attribute {0} at {1}{reason}.",
                        subjectAttribute.QualifiedName, currentNode.GetXPath());
                }

                if (subjectAttribute.Value != expectedAttribute.Value)
                {
                    return new Failure("Expected attribute {0} at {1} to have value {2}{reason}, but found {3}.",
                        subjectAttribute.LocalName, currentNode.GetXPath(), expectedAttribute.Value, subjectAttribute.Value);
                }
            }

            if (subjectAttributes.Count != expectedAttributes.Count)
            {
                AttributeData missingAttribute = expectedAttributes.First(ea =>
                    !subjectAttributes.Any(sa =>
                        ea.NamespaceUri == sa.NamespaceUri
                        && sa.LocalName == ea.LocalName));

                return new Failure("Expected attribute {0} at {1}{reason}, but found none.",
                    missingAttribute.LocalName, currentNode.GetXPath());
            }

            return null;
        }

        private Failure ValidateStartElement()
        {
            if (subjectIterator.LocalName != expectationIterator.LocalName)
            {
                return new Failure("Expected local name of element at {0} to be {1}{reason}, but found {2}.",
                    currentNode.GetXPath(), expectationIterator.LocalName, subjectIterator.LocalName);
            }

            if (subjectIterator.NamespaceUri != expectationIterator.NamespaceUri)
            {
                return new Failure("Expected namespace of element {0} at {1} to be {2}{reason}, but found {3}.",
                    subjectIterator.LocalName, currentNode.GetXPath(), expectationIterator.NamespaceUri, subjectIterator.NamespaceUri);
            }

            return null;
        }

        private Failure ValidateText()
        {
            string subject = subjectIterator.Value;
            string expected = expectationIterator.Value;

            if (subject != expected)
            {
                return new Failure("Expected content to be {0} at {1}{reason}, but found {2}.",
                    expected, currentNode.GetXPath(), subject);
            }

            return null;
        }

    }
}
