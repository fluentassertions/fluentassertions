using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FluentAssertions.Execution;

namespace FluentAssertions.Xml.Equivalency
{
    internal class XmlReaderValidator
    {
        private readonly AssertionScope assertion;
        private readonly XmlReaderWrapper subjectReader;
        private readonly XmlReaderWrapper otherReader;

        private Node currentNode = Node.CreateRoot();

        public XmlReaderValidator(XmlReader subjectReader, XmlReader otherReader, string because, object[] reasonArgs)
        {
            assertion = Execute.Assertion.BecauseOf(because, reasonArgs);

            this.subjectReader = new XmlReaderWrapper(subjectReader);
            this.otherReader = new XmlReaderWrapper(otherReader);
        }

        public void Validate(bool expectedEquivalence)
        {
            ValidationResult validationResult = Validate();

            if (expectedEquivalence && validationResult != null)
            {
                assertion.FailWith(validationResult.FormatString, validationResult.FormatParams);
            }

            if (!expectedEquivalence && validationResult is null)
            {
                assertion.FailWith("Did not expect Xml to be equivalent{reason}, but it is.");
            }
        }

        private ValidationResult Validate()
        {
            while (!subjectReader.EOF && !otherReader.EOF)
            {
                if (subjectReader.NodeType != otherReader.NodeType)
                {
                    return new ValidationResult("Expected node of type {0} at {1}{reason}, but found {2}.",
                        otherReader.NodeType, GetCurrentXPath(), subjectReader.NodeType);
                }

                ValidationResult validationResult = null;

#pragma warning disable IDE0010 // System.Xml.XmlNodeType has many members we do not care about
                switch (subjectReader.NodeType)
#pragma warning restore IDE0010
                {
                    case XmlNodeType.Element:
                        validationResult = ValidateStartElement();
                        if (validationResult != null)
                        {
                            return validationResult;
                        }

                        // starting new element, add local name to location stack
                        // to build XPath info
                        currentNode = currentNode.Push(subjectReader.LocalName);

                        validationResult = ValidateAttributes();

                        if (subjectReader.IsEmptyElement)
                        {
                            // The element is already complete. (We will NOT get an EndElement node.)
                            // Update node information.
                            currentNode = currentNode.Pop();
                        }

                        // check whether empty element and self-closing element needs to be synchronized
                        if (subjectReader.IsEmptyElement && !otherReader.IsEmptyElement)
                        {
                            otherReader.MoveToEndElement();
                        }
                        else if (otherReader.IsEmptyElement && !subjectReader.IsEmptyElement)
                        {
                            subjectReader.MoveToEndElement();
                        }

                        break;
                    case XmlNodeType.EndElement:
                        // No need to verify end element, if it doesn't match
                        // the start element it isn't valid XML, so the parser
                        // would handle that.
                        currentNode = currentNode.Pop();
                        break;
                    case XmlNodeType.Text:
                        validationResult = ValidateText();
                        break;
                    default:
                        throw new NotSupportedException($"{subjectReader.NodeType} found at {GetCurrentXPath()} is not supported for equivalency comparison.");
                }

                if (validationResult != null)
                {
                    return validationResult;
                }

                subjectReader.Read();
                otherReader.Read();
            }

            if (!otherReader.EOF)
            {
                return new ValidationResult("Expected {0}{reason}, but found end of document.",
                    otherReader.LocalName);
            }

            if (!subjectReader.EOF)
            {
                return new ValidationResult("Expected end of document{reason}, but found {0}.",
                    subjectReader.LocalName);
            }

            return null;
        }

        private ValidationResult ValidateAttributes()
        {
            IList<AttributeData> expectedAttributes = otherReader.GetAttributes();
            IList<AttributeData> subjectAttributes = subjectReader.GetAttributes();

            foreach (AttributeData subjectAttribute in subjectAttributes)
            {
                AttributeData expectedAttribute = expectedAttributes.SingleOrDefault(
                    ea => ea.NamespaceUri == subjectAttribute.NamespaceUri
                    && ea.LocalName == subjectAttribute.LocalName);

                if (expectedAttribute is null)
                {
                    return new ValidationResult("Did not expect to find attribute {0} at {1}{reason}.",
                        subjectAttribute.QualifiedName, GetCurrentXPath());
                }

                if (subjectAttribute.Value != expectedAttribute.Value)
                {
                    return new ValidationResult("Expected attribute {0} at {1} to have value {2}{reason}, but found {3}.",
                        subjectAttribute.LocalName, GetCurrentXPath(), expectedAttribute.Value, subjectAttribute.Value);
                }
            }

            if (subjectAttributes.Count != expectedAttributes.Count)
            {
                AttributeData missingAttribute = expectedAttributes.First(ea =>
                    !subjectAttributes.Any(sa =>
                        ea.NamespaceUri == sa.NamespaceUri
                        && sa.LocalName == ea.LocalName));

                return new ValidationResult("Expected attribute {0} at {1}{reason}, but found none.",
                    missingAttribute.LocalName, GetCurrentXPath());
            }

            return null;
        }

        private ValidationResult ValidateStartElement()
        {
            if (subjectReader.LocalName != otherReader.LocalName)
            {
                return new ValidationResult("Expected local name of element at {0} to be {1}{reason}, but found {2}.",
                    GetCurrentXPath(), otherReader.LocalName, subjectReader.LocalName);
            }

            if (subjectReader.NamespaceURI != otherReader.NamespaceURI)
            {
                return new ValidationResult("Expected namespace of element {0} at {1} to be {2}{reason}, but found {3}.",
                    subjectReader.LocalName, GetCurrentXPath(), otherReader.NamespaceURI, subjectReader.NamespaceURI);
            }

            return null;
        }

        private ValidationResult ValidateText()
        {
            string subject = subjectReader.Value;
            string expected = otherReader.Value;

            if (subject != expected)
            {
                return new ValidationResult("Expected content to be {0} at {1}{reason}, but found {2}.",
                    expected, GetCurrentXPath(), subject);
            }

            return null;
        }

        private string GetCurrentXPath()
        {
            var resultBuilder = new StringBuilder();

            foreach (var location in currentNode.GetPath().Reverse())
            {
                if (location.Count > 1)
                {
                    resultBuilder.AppendFormat("/{0}[{1}]", location.Name, location.Count);
                }
                else
                {
                    resultBuilder.AppendFormat("/{0}", location.Name);
                }
            }

            if (resultBuilder.Length == 0)
            {
                return "/";
            }

            return resultBuilder.ToString();
        }
    }
}
