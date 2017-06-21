#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

#endregion

namespace FluentAssertions.Execution
{
    /// <summary>
    /// Encapsulates expanding the various placeholders supported in a failure message. 
    /// </summary>
    internal class MessageBuilder
    {
        #region Private Definitions

        private readonly bool useLineBreaks;

        private readonly char[] blanks = {'\r', '\n', ' ', '\t'};

        /// <summary>
        /// Represents the phrase that can be used in <see cref="AssertionScope.FailWith"/> as a placeholder for the reason of an assertion.
        /// </summary>
        private const string ReasonTag = "{reason}";

        #endregion

        public MessageBuilder(bool useLineBreaks)
        {
            this.useLineBreaks = useLineBreaks;
        }

        public string Build(string message, object[] messageArgs, string reason, ContextDataItems contextData)
        {
            string result = SubstituteReasonTag(message, SanitizeReason(reason));
            result = SubstituteContextualTags(result, contextData);
            result = FormatArgumentPlaceholders(result, messageArgs);
            return result;
        }

        private string SubstituteReasonTag(string failureMessage, string reason)
        {
            return Regex.Replace(failureMessage, ReasonTag, reason);
        }

        private string SubstituteContextualTags(string message, ContextDataItems contextData)
        {
            var regex = new Regex(@"\{(?<key>[a-z|A-Z]+)(?:\:(?<default>[a-z|A-Z|\s]+))?\}");
            return regex.Replace(message, match =>
            {
                string key = match.Groups["key"].Value;
                return 
                    contextData.AsStringOrDefault(key)?.Replace("{", "{{").Replace("}", "}}") ??
                    match.Groups["default"].Value;
            });
        }

        private string FormatArgumentPlaceholders(string failureMessage, object[] failureArgs)
        {
            string[] values = failureArgs.Select(a => Formatter.ToString(a, useLineBreaks)).ToArray();

            string formattedMessage = string.Format(failureMessage, values);
            return formattedMessage.Replace("{{{{", "{{").Replace("}}}}", "}}");
        }

        private string SanitizeReason(string reason)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                reason = EnsurePrefix("because", reason);
                reason = reason.Replace("{", "{{").Replace("}", "}}");

                return StartsWithBlank(reason) ? reason : " " + reason;
            }

            return "";
        }

        // SMELL: looks way too complex just to retain the leading whitepsace
        private string EnsurePrefix(string prefix, string text)
        {
            string leadingBlanks = ExtractLeadingBlanksFrom(text);
            string textWithoutLeadingBlanks = text.Substring(leadingBlanks.Length);

            return !textWithoutLeadingBlanks.StartsWith(prefix, StringComparison.CurrentCultureIgnoreCase)
                ? leadingBlanks + prefix + " " + textWithoutLeadingBlanks
                : text;
        }

        private string ExtractLeadingBlanksFrom(string text)
        {
            string trimmedText = text.TrimStart(blanks);
            int leadingBlanksCount = text.Length - trimmedText.Length;

            return text.Substring(0, leadingBlanksCount);
        }

        private bool StartsWithBlank(string text)
        {
            return (text.Length > 0) && blanks.Any(blank => text[0] == blank);
        }
    }
}