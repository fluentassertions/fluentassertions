using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FluentAssertions.Execution;

namespace FluentAssertions.Xml
{
    internal class XmlReaderValidator
    {
        private readonly AssertionScope assertion;
        private readonly XmlReaderWrapper subjectReader;
        private readonly XmlReaderWrapper otherReader;

        private readonly Stack<string> locationStack = new Stack<string>();
        private readonly Dictionary<string, int> countPerElementPath = new Dictionary<string, int>();

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
                        locationStack.Push(subjectReader.LocalName);

                        // building key according XPath to count repeating element names
                        UpdateElementPathCount();

                        validationResult = ValidateAttributes();

                        if (subjectReader.IsEmptyElement)
                        {
                            // The element is already complete. (We will NOT get an EndElement node.)
                            // Update node information.
                            locationStack.Pop();
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
                        locationStack.Pop();
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

        private void UpdateElementPathCount()
        {
            var locationKey = "/" + string.Join("/", locationStack.Reverse());
            countPerElementPath.TryGetValue(locationKey, out var locationCount);
            countPerElementPath[locationKey] = ++locationCount;
        }

        private string GetCurrentXPath()
        {
            var keyBuilder = new StringBuilder();
            var resultBuilder = new StringBuilder();
            foreach (var location in locationStack.Reverse())
            {
                keyBuilder.Append($"/{location}");
                resultBuilder.Append($"/{location}");
                var locationCount = countPerElementPath[keyBuilder.ToString()];
                if (locationCount > 1)
                {
                    resultBuilder.Append($"[{locationCount}]");
                }
            }

            if (resultBuilder.Length == 0)
            {
                return "/";
            }

            return resultBuilder.ToString();
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

        private class XmlReaderWrapper
        {
            private readonly XmlReader reader;

            private bool skipOnce;

            public XmlReaderWrapper(XmlReader reader)
            {
                this.reader = reader;

                this.reader.MoveToContent();
            }

            public XmlNodeType NodeType => this.reader.NodeType;

            public string LocalName => this.reader.LocalName;

            public string NamespaceURI => this.reader.NamespaceURI;

            public string Value => this.reader.Value;

            public bool IsEmptyElement => this.reader.IsEmptyElement;

            public bool EOF => this.reader.EOF;

            public bool Read()
            {
                if (this.skipOnce)
                {
                    this.skipOnce = false;
                    return true;
                }

                if (!this.reader.Read())
                {
                    return false;
                }

                this.reader.MoveToContent();
                return true;
            }

            public void MoveToEndElement()
            {
                this.reader.Read();
                if (this.reader.NodeType != XmlNodeType.EndElement)
                {
                    // advancing failed
                    // skip reading on next attempt to "simulate" rewind reader
                    this.skipOnce = true;
                }

                this.reader.MoveToContent();
            }

            public IList<AttributeData> GetAttributes()
            {
                var attributes = new List<AttributeData>();

                if (this.reader.MoveToFirstAttribute())
                {
                    do
                    {
                        if (reader.NamespaceURI != "http://www.w3.org/2000/xmlns/")
                        {
                            attributes.Add(new AttributeData(this.reader.NamespaceURI, this.reader.LocalName, this.reader.Value, this.reader.Prefix));
                        }
                    }
                    while (this.reader.MoveToNextAttribute());

                    this.reader.MoveToElement();
                }

                return attributes;
            }
        }
    }
}
