using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution;

/// <summary>
/// Encapsulates expanding the various placeholders supported in a failure message.
/// </summary>
internal class FailureMessageFormatter(FormattingOptions formattingOptions)
{
    private static readonly char[] Blanks = ['\r', '\n', ' ', '\t'];
    private string reason;
    private ContextDataDictionary contextData;
    private string identifier;
    private string fallbackIdentifier;

    public FailureMessageFormatter WithReason(string reason)
    {
        this.reason = SanitizeReason(reason ?? string.Empty);
        return this;
    }

    private static string SanitizeReason(string reason)
    {
        if (!string.IsNullOrEmpty(reason))
        {
            reason = EnsurePrefix("because", reason);
            reason = reason.EscapePlaceholders();

            return StartsWithBlank(reason) ? reason : " " + reason;
        }

        return string.Empty;
    }

    // SMELL: looks way too complex just to retain the leading whitespace
    private static string EnsurePrefix(string prefix, string text)
    {
        string leadingBlanks = ExtractLeadingBlanksFrom(text);
        string textWithoutLeadingBlanks = text.Substring(leadingBlanks.Length);

        return !textWithoutLeadingBlanks.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? leadingBlanks + prefix + " " + textWithoutLeadingBlanks
            : text;
    }

    private static string ExtractLeadingBlanksFrom(string text)
    {
        string trimmedText = text.TrimStart(Blanks);
        int leadingBlanksCount = text.Length - trimmedText.Length;

        return text.Substring(0, leadingBlanksCount);
    }

    private static bool StartsWithBlank(string text)
    {
        return text.Length > 0 && Blanks.Contains(text[0]);
    }

    public FailureMessageFormatter WithContext(ContextDataDictionary contextData)
    {
        this.contextData = contextData;
        return this;
    }

    public FailureMessageFormatter WithIdentifier(string identifier)
    {
        this.identifier = identifier;
        return this;
    }

    public FailureMessageFormatter WithFallbackIdentifier(string fallbackIdentifier)
    {
        this.fallbackIdentifier = fallbackIdentifier;
        return this;
    }

    public string Format(string message, object[] messageArgs)
    {
        message = message.Replace("{reason}", reason, StringComparison.Ordinal);

        message = SubstituteIdentifier(message, identifier?.EscapePlaceholders(), fallbackIdentifier);

        message = SubstituteContextualTags(message, contextData);

        message = FormatArgumentPlaceholders(message, messageArgs);

        return message;
    }

    private static string SubstituteIdentifier(string message, string identifier, string fallbackIdentifier)
    {
        const string pattern = @"(?:\s|^)\{context(?:\:(?<default>[a-zA-Z\s]+))?\}";

        message = Regex.Replace(message, pattern, match =>
        {
            const string result = " ";

            if (!string.IsNullOrEmpty(identifier))
            {
                return result + identifier;
            }

            string defaultIdentifier = match.Groups["default"].Value;

            if (!string.IsNullOrEmpty(defaultIdentifier))
            {
                return result + defaultIdentifier;
            }

            if (!string.IsNullOrEmpty(fallbackIdentifier))
            {
                return result + fallbackIdentifier;
            }

            return " object";
        });

        return message.TrimStart();
    }

    private static string SubstituteContextualTags(string message, ContextDataDictionary contextData)
    {
        const string pattern = @"(?<!\{)\{(?<key>[a-zA-Z]+)(?:\:(?<default>[a-zA-Z\s]+))?\}(?!\})";

        return Regex.Replace(message, pattern, match =>
        {
            string key = match.Groups["key"].Value;
            string contextualTags = contextData.AsStringOrDefault(key);
            string contextualTagsSubstituted = contextualTags?.EscapePlaceholders();

            return contextualTagsSubstituted ?? match.Groups["default"].Value;
        });
    }

    private string FormatArgumentPlaceholders(string failureMessage, object[] failureArgs)
    {
        object[] values = failureArgs.Select(object (a) => Formatter.ToString(a, formattingOptions)).ToArray();

        try
        {
            return string.Format(CultureInfo.InvariantCulture, failureMessage, values);
        }
        catch (FormatException formatException)
        {
            return
                $"**WARNING** failure message '{failureMessage}' could not be formatted with string.Format{Environment.NewLine}{formatException.StackTrace}";
        }
    }
}
