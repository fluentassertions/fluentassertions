namespace FluentAssertions.Xml.Equivalency
{
    internal class ValidationResult
    {
        public ValidationResult(string formatString, params object[] formatParams)
        {
            FormatString = formatString;
            FormatParams = formatParams;
        }

        public string FormatString { get; }

        public object[] FormatParams { get; }
    }
}