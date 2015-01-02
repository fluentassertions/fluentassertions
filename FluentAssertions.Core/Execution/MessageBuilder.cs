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
            string result = SubstituteReasonTag(message, reason);
            result = SubstituteContextualTags(result, contextData);
            result = FormatArgumentPlaceholders(result, messageArgs, SanitizeReason(reason));
            return result;
        }

        // SMELL: just substitutes the reason tag with an ordinary formatting tag.
        private string SubstituteReasonTag(string failureMessage, string reason)
        {
            return !String.IsNullOrEmpty(reason)
                ? ReplaceReasonTagWithFormatSpecification(failureMessage)
                : failureMessage.Replace(ReasonTag, String.Empty);
        }

        // SMELL: should use regex to do this
        private static string ReplaceReasonTagWithFormatSpecification(string failureMessage)
        {
            if (failureMessage.Contains(ReasonTag))
            {
                string message = IncrementAllFormatSpecifiers(failureMessage);
                return message.Replace(ReasonTag, "{0}");
            }

            return failureMessage;
        }

        private string SubstituteContextualTags(string message, ContextDataItems contextData)
        {
            var regex = new Regex(@"\{(?<key>[a-z|A-Z]+)(?:\:(?<default>[a-z|A-Z|\s]+))?\}");
            return regex.Replace(message, match =>
            {
                string key = match.Groups["key"].Value;
                return contextData.AsStringOrDefault(key) ?? match.Groups["default"].Value;
            });
        }

        private static string IncrementAllFormatSpecifiers(string message)
        {
            for (int index = 9; index >= 0; index--)
            {
                int newIndex = index + 1;
                string oldTag = "{" + index + "}";
                string newTag = "{" + newIndex + "}";
                message = message.Replace(oldTag, newTag);
            }

            return message;
        }

        private string FormatArgumentPlaceholders(string failureMessage, object[] failureArgs, string reason)
        {
            var values = new List<string>();
            if (!String.IsNullOrEmpty(reason))
            {
                values.Add(reason);
            }

            values.AddRange(failureArgs.Select(a => Formatter.ToString(a, useLineBreaks)));

            string formattedMessage = values.Any() ? String.Format(failureMessage, values.ToArray()) : failureMessage;
            return formattedMessage.Replace("{{{{", "{{").Replace("}}}}", "}}").Capitalize();
        }

        private string SanitizeReason(string reason)
        {
            if (!string.IsNullOrEmpty(reason))
            {
                reason = EnsurePrefix("because", reason);

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