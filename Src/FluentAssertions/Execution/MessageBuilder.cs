using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions.Execution;

/// <summary>
/// Encapsulates expanding the various placeholders supported in a failure message.
/// </summary>
internal class MessageBuilder
{
    private readonly FormattingOptions formattingOptions;

    #region Private Definitions

    private readonly char[] blanks = { '\r', '\n', ' ', '\t' };

    #endregion

    public MessageBuilder(FormattingOptions formattingOptions)
    {
        this.formattingOptions = formattingOptions;
    }

    // SMELL: Too many parameters.
    public string Build(string message, object[] messageArgs, string reason, ContextDataItems contextData, string identifier,
        string fallbackIdentifier)
    {
        message = message.Replace("{reason}", SanitizeReason(reason), StringComparison.Ordinal);

        message = SubstituteIdentifier(message, identifier, fallbackIdentifier);

        message = SubstituteContextualTags(message, contextData);

        message = FormatArgumentPlaceholders(message, messageArgs);

        return message;
    }

    private static string SubstituteIdentifier(string message, string identifier, string fallbackIdentifier)
    {
        const string pattern = @"(?:\s|^)\{context(?:\:(?<default>[a-z|A-Z|\s]+))?\}";

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

    /// <summary>
    /// Replaces all occurrences of {key} or {key:default} with the value of the key in the <paramref name="contextData"/>
    /// dictionary. If the key is not found, the default value is used. If that doesn't exist, the placeholder is left as is.
    /// </summary>
    private static string SubstituteContextualTags(string message, ContextDataItems contextData)
    {
        const string pattern = @"(?<!\{)\{(?<key>[a-z|A-Z]+)(?:\:(?<default>[a-z|A-Z|\s]+))?\}(?!\})";

        return Regex.Replace(message, pattern, match =>
        {
            string key = match.Groups["key"].Value;

            string contextualTag = contextData.AsStringOrDefault(key) ?? match.Groups["default"].Value;

            return contextualTag.Length == 0 ? match.Value : contextualTag;
        });
    }

    /// <summary>
    /// Find all matches of {0}, {0,-2:format}, {0:format} etc. and replace them the objects
    /// passed through <see cref="Formatter"/>, while ignoring any other curly braces.
    /// </summary>
    /// <remarks>
    /// In contrast to <see cref="string.Format(System.IFormatProvider?,string,object?)"/>, this method ignores
    /// any other placeholders using curly braces.
    /// </remarks>
    private string FormatArgumentPlaceholders(string message, object[] args)
    {
        string[] formattedArgs = args.Select(a => Formatter.ToString(a, formattingOptions)).ToArray();

        // Matches the .NET string format {index[,alignment][:format]}, even
        // if the opening and closing curly braces are escaped themselves.
        var matches = Regex.Matches(message,
            @"\{+\d+(,-?\d+)?(:[^\s{}]+)?\}+");

        StringBuilder builder = new();
        int indexInMessage = 0;

        foreach (Match match in matches)
        {
            builder.Append(message[indexInMessage..match.Index]);

            try
            {
                builder.AppendFormat(CultureInfo.InvariantCulture, match.Value, formattedArgs.OfType<object>().ToArray());
            }
            catch (FormatException)
            {
                // If we fail to format the potential placeholder, we just keep it in the final message.
                builder.Append(match.Value);
            }

            indexInMessage = match.Index + match.Length;
        }

        if (indexInMessage < message.Length)
        {
            builder.Append(message[indexInMessage..]);
        }

        return builder.ToString();
    }

    private string SanitizeReason(string reason)
    {
        if (!string.IsNullOrEmpty(reason))
        {
            reason = EnsurePrefix("because", reason);

            return StartsWithBlank(reason) ? reason : " " + reason;
        }

        return string.Empty;
    }

    // SMELL: looks way too complex just to retain the leading whitespace
    private string EnsurePrefix(string prefix, string text)
    {
        string leadingBlanks = ExtractLeadingBlanksFrom(text);
        string textWithoutLeadingBlanks = text.Substring(leadingBlanks.Length);

        return !textWithoutLeadingBlanks.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
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
        return text.Length > 0 && blanks.Contains(text[0]);
    }
}
