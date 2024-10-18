#if NET6_0_OR_GREATER
#nullable enable

using System;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace FluentAssertions.Json;

internal static class JsonTokenStreamCompare
{
    private const int CharCountBeforeError = 14;
    private const int CharCountAfterError = 150;
    private static readonly JsonComparatorOptions DefaultOptions = new();

    internal static string? IsJsonTokenEquivalent(string actual, string expected, JsonComparatorOptions? options = null)
    {
        var actualBytes = System.Text.Encoding.UTF8.GetBytes(actual);
        var expectedBytes = System.Text.Encoding.UTF8.GetBytes(expected);
        return IsJsonTokenEquivalent(actualBytes, expectedBytes, options);
    }

    internal static string? IsJsonTokenEquivalent(byte[] actualBytes, byte[] expectedBytes, JsonComparatorOptions? options = null)
    {
        options ??= DefaultOptions;
        var actual = new Utf8JsonReader(actualBytes, options.Value);
        var expected = new Utf8JsonReader(expectedBytes, options.Value);
        var comparisonResult = CompareReaders(ref actual, actualBytes, ref expected, expectedBytes);
        return comparisonResult;
    }

    private static string? CompareReaders(ref Utf8JsonReader actual, byte[] actualBytes,
        ref Utf8JsonReader expected, byte[] expectedBytes)
    {
        while (actual.Read() && expected.Read())
        {
            if (actual.TokenType != expected.TokenType)
            {
                return CreateErrorMessage("JsonTokenType mismatch", ref actual, actualBytes, ref expected, expectedBytes);
            }

            switch (actual.TokenType)
            {
                // Ignore tokens without value
                case JsonTokenType.StartObject:
                case JsonTokenType.EndObject:
                case JsonTokenType.None:
                case JsonTokenType.StartArray:
                case JsonTokenType.EndArray:
                case JsonTokenType.True:
                case JsonTokenType.False:
                case JsonTokenType.Null:
                    break;

                // compare the value of tokens with value
                case JsonTokenType.PropertyName:
                    if (!actual.ValueTextEquals(expected.ValueSpan))
                    {
                        return CreateErrorMessage("PropertyName mismatch (validate strict order)", ref actual, actualBytes, ref expected,
                            expectedBytes);
                    }

                    break;
                case JsonTokenType.String:
                    if (!actual.ValueTextEquals(expected.ValueSpan))
                    {
                        return CreateErrorMessage("string mismatch", ref actual, actualBytes, ref expected, expectedBytes);
                    }

                    break;
                case JsonTokenType.Comment: // compare comments if they are not ignored
                    if (actual.GetComment() != expected.GetComment())
                    {
                        return CreateErrorMessage("comment mismatch", ref actual, actualBytes, ref expected, expectedBytes);
                    }

                    break;
                case JsonTokenType.Number:
                    if (actual.GetDecimal() != expected.GetDecimal())
                    {
                        return CreateErrorMessage("number mismatch", ref actual, actualBytes, ref expected, expectedBytes);
                    }

                    break;
            }
        }

        return null;
    }

    private static string? CreateErrorMessage(string message, ref Utf8JsonReader actual, ReadOnlySpan<byte> actualBytes,
        ref Utf8JsonReader expected, ReadOnlySpan<byte> expectedBytes)
    {
        var actualCharacterPositionError = Encoding.UTF8.GetCharCount(actualBytes.Slice(0, (int)actual.TokenStartIndex)); // might be less than pos if there are multi byte characters
        var expectedCharacterPositionError = Encoding.UTF8.GetCharCount(expectedBytes.Slice(0, (int)expected.TokenStartIndex)); // might be less than pos if there are multi byte characters

        return $"""

                {message}
                {GetRelevantErrorPart((int)actual.TokenStartIndex, actualBytes)} (diff at index {actualCharacterPositionError})
                {GetRelevantErrorPart((int)expected.TokenStartIndex, expectedBytes)} (diff at index {expectedCharacterPositionError})
                {"^".PadLeft(CharCountBeforeError + 1)}

                """;
    }

    private static readonly Regex RemoveNewlinesAndIndetionRegex = new Regex(@"\r?\n\s*", RegexOptions.Compiled);

    private static string GetRelevantErrorPart(int pos, ReadOnlySpan<byte> bytes)
    {
        var startBeforePos = Math.Max(0, pos - (CharCountBeforeError * 3)); // take 3 times as many utf8 bytes than characters (might be multi byte characters, and we remove newlines and indentation)
        var lengthAfterPos = Math.Min(CharCountAfterError * 3, bytes.Length - pos); // take 3 times as many utf8 bytes than characters

        var beforeErrorString = Encoding.UTF8.GetString(bytes.Slice(startBeforePos, pos - startBeforePos));
        beforeErrorString = RemoveNewlinesAndIndetionRegex.Replace(beforeErrorString, " ")
            .PadLeft(CharCountBeforeError);
        beforeErrorString = beforeErrorString.Substring(beforeErrorString.Length - CharCountBeforeError);

        var afterErrorString = Encoding.UTF8.GetString(bytes.Slice(pos, lengthAfterPos));
        afterErrorString = RemoveNewlinesAndIndetionRegex.Replace(afterErrorString, " ");
        afterErrorString = afterErrorString.Substring(0, Math.Min(afterErrorString.Length, CharCountAfterError));

        return beforeErrorString + afterErrorString;
    }
}

#endif
