using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FluentAssertions.Execution;
using FluentAssertions.Localization;

namespace FluentAssertions.Xml
{
    internal class XmlReaderValidator
    {
        private readonly IAssertionScope assertion;
        private readonly XmlReader subjectReader;
        private readonly XmlReader otherReader;

        private string GetCurrentLocation() => "/" + string.Join("/", locationStack.Reverse());

        private readonly Stack<string> locationStack = new Stack<string>();

        public XmlReaderValidator(XmlReader subjectReader, XmlReader otherReader, string because, object[] reasonArgs)
        {
            assertion = Execute.Assertion.BecauseOf(because, reasonArgs);

            this.subjectReader = subjectReader;
            this.otherReader = otherReader;
        }

        private class ValidationResult
        {
            public ValidationResult(string formatString, params object[] formatParams)
            {
                FormatString = formatString;
                FormatParams = formatParams;
            }

            public string FormatString { get; }

            public object[] FormatParams { get; }
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
                assertion.FailWith(Resources.Xml_DidNotExpectXmlToBeEquivalentButItIs);
            }
        }

        private ValidationResult Validate()
        {
            subjectReader.MoveToContent();
            otherReader.MoveToContent();
            while (!subjectReader.EOF && !otherReader.EOF)
            {
                if (subjectReader.NodeType != otherReader.NodeType)
                {
                    return new ValidationResult(Resources.Xml_ExpectedNodeOfTypeX0AtX1Format + Resources.Common_CommaButFoundX2Format,
                        otherReader.NodeType, GetCurrentLocation(), subjectReader.NodeType);
                }

                ValidationResult validationResult = null;

                switch (subjectReader.NodeType)
                {
                    case XmlNodeType.Element:
                        validationResult = ValidateStartElement();
                        if (validationResult != null)
                        {
                            return validationResult;
                        }

                        locationStack.Push(subjectReader.LocalName);
                        validationResult = ValidateAttributes();
                        if (subjectReader.IsEmptyElement)
                        {
                            locationStack.Pop();
                        }

                        break;
                    case XmlNodeType.EndElement:
                        // No need to verify end element, if it doesn't match
                        // the start element it isn't valid XML, so the parser
                        // would handle that.
                        locationStack.Pop();
                        break;
                    case XmlNodeType.Text:
                        validationResult = ValidateText();
                        break;
                    default:
                        throw new NotSupportedException(
                            string.Format(Resources.Xml_X0FoundAtX1IsNotSupportedForEquivalencyComparisonFormat,
                                subjectReader.NodeType, GetCurrentLocation()));
                }

                if (validationResult != null)
                {
                    return validationResult;
                }

                subjectReader.Read();
                otherReader.Read();

                subjectReader.MoveToContent();
                otherReader.MoveToContent();
            }

            if (!otherReader.EOF)
            {
                return new ValidationResult(Resources.Xml_ExpectedX0ButFoundEndOfDocumentFormat,
                    otherReader.LocalName);
            }

            if (!subjectReader.EOF)
            {
                return new ValidationResult(Resources.Xml_ExpectedEndOfDocument + Resources.Common_CommaButFoundX0Format,
                    subjectReader.LocalName);
            }

            return null;
        }

        private class AttributeData
        {
            public AttributeData(string namespaceUri, string localName, string value, string prefix)
            {
                NamespaceUri = namespaceUri;
                LocalName = localName;
                Value = value;
                Prefix = prefix;
            }

            public string NamespaceUri { get; }

            public string LocalName { get; }

            public string Value { get; }

            public string Prefix { get; }

            public string QualifiedName
            {
                get
                {
                    if (string.IsNullOrEmpty(Prefix))
                    {
                        return LocalName;
                    }

                    return Prefix + ":" + LocalName;
                }
            }
        }

        private ValidationResult ValidateAttributes()
        {
            IList<AttributeData> expectedAttributes = GetAttributes(otherReader);
            IList<AttributeData> subjectAttributes = GetAttributes(subjectReader);

            foreach (AttributeData subjectAttribute in subjectAttributes)
            {
                AttributeData expectedAttribute = expectedAttributes.SingleOrDefault(
                    ea => ea.NamespaceUri == subjectAttribute.NamespaceUri
                    && ea.LocalName == subjectAttribute.LocalName);

                if (expectedAttribute is null)
                {
                    return new ValidationResult(Resources.Xml_DidNotExpectToFindAttributeX0AtX1Format,
                        subjectAttribute.QualifiedName, GetCurrentLocation());
                }

                if (subjectAttribute.Value != expectedAttribute.Value)
                {
                    return new ValidationResult(Resources.Xml_ExpectedAttributeX0AtX1ToHaveValueX2Format + Resources.Common_CommaButFoundX3Format,
                        subjectAttribute.LocalName, GetCurrentLocation(), expectedAttribute.Value, subjectAttribute.Value);
                }
            }

            if (subjectAttributes.Count != expectedAttributes.Count)
            {
                AttributeData missingAttribute = expectedAttributes.First(ea =>
                    !subjectAttributes.Any(sa =>
                        ea.NamespaceUri == sa.NamespaceUri
                        && sa.LocalName == ea.LocalName));

                return new ValidationResult(Resources.Xml_ExpectedAttributeX0AtX1ButFoundNoneFormat,
                    missingAttribute.LocalName, GetCurrentLocation());
            }

            return null;
        }

        private IList<AttributeData> GetAttributes(XmlReader reader)
        {
            IList<AttributeData> attributes = new List<AttributeData>();

            if (reader.MoveToFirstAttribute())
            {
                do
                {
                    if (reader.NamespaceURI != "http://www.w3.org/2000/xmlns/")
                    {
                        attributes.Add(new AttributeData(reader.NamespaceURI, reader.LocalName, reader.Value, reader.Prefix));
                    }
                }
                while (reader.MoveToNextAttribute());
            }

            return attributes;
        }

        private ValidationResult ValidateStartElement()
        {
            if (subjectReader.LocalName != otherReader.LocalName)
            {
                return new ValidationResult(Resources.Xml_ExpectedLocalNameOfElementAtX0ToBeX1Format + Resources.Common_CommaButFoundX2Format,
                    GetCurrentLocation(), otherReader.LocalName, subjectReader.LocalName);
            }

            if (subjectReader.NamespaceURI != otherReader.NamespaceURI)
            {
                return new ValidationResult(Resources.Xml_ExpectedNamespaceOfElementX0AtX1ToBeX2Format  + Resources.Common_CommaButFoundX3Format,
                    subjectReader.LocalName, GetCurrentLocation(), otherReader.NamespaceURI, subjectReader.NamespaceURI);
            }

            return null;
        }

        private ValidationResult ValidateText()
        {
            string subject = subjectReader.Value;
            string expected = otherReader.Value;

            if (subject != expected)
            {
                return new ValidationResult(Resources.Xml_ExpectedContentToBeX0AtX1Format + Resources.Common_CommaButFoundX2Format,
                    expected, GetCurrentLocation(), subject);
            }

            return null;
        }
    }
}
