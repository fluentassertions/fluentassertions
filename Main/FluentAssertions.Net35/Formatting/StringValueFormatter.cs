using System;

namespace FluentAssertions.Formatting
{
    internal class StringValueFormatter : IValueFormatter
    {
        public bool CanHandle(object value)
        {
            return value is string;
        }

        public string ToString(object value)
        {
            return "\"" + EscapeSpecialCharacters(value.ToString()) + "\"";
        }

        private string EscapeSpecialCharacters(string str)
        {
            return str.Replace("\"", "\\\"").Replace("\n", @"\n").Replace("\r", @"\r");
        }
    }
}