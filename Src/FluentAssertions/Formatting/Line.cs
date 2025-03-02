using System;
using System.Text;

namespace FluentAssertions.Formatting;

/// <summary>
/// Represents a single line of output rendered through the <see cref="FormattedObjectGraph"/>.
/// </summary>
internal class Line
{
    /// <summary>
    ///  The level of indentation at the time this line was created.
    /// </summary>
    private int indentation;

    private string content;
    private StringBuilder builder = new();

    /// <summary>
    /// If any whitespace was inserted at the beginning of the line without an <see cref="Anchor"/> knowing about it, this
    /// will be the length of that whitespace so that any calls to <see cref="Insert"/> will be offset by this amount.
    /// </summary>
    private int whitespaceOffset;

    /// <summary>
    /// Creates an empty line with the specified indentation that will be applied when
    /// actual fragments are added.
    /// </summary>
    public Line(int indentation)
    {
        this.indentation = indentation;
    }

    public Line(string content)
    {
        this.content = content.TrimEnd();
    }

    /// <summary>
    /// Is used to close off the internal string builder.
    /// </summary>
    public void Flush()
    {
        if (builder is not null)
        {
            content = builder.ToString();
            builder = null;
        }
    }

    /// <summary>
    /// Gets the length of the content, including any whitespace that was inserted at the beginning of the line.
    /// </summary>
    public int Length => builder?.Length ?? content.Length;

    /// <summary>
    /// Gets the length of the content without the offset of any whitespace that was inserted at the beginning of the line.
    /// </summary>
    public int LengthWithoutOffset => Length - whitespaceOffset;

    public void Append(string fragment)
    {
        builder.Append(fragment);
    }

    public void InsertAtStart(string fragment)
    {
        if (builder is null)
        {
            content = content.Insert(0, fragment);
        }
        else
        {
            builder.Insert(0, fragment);
        }
    }

    public void Insert(int characterIndex, string fragment)
    {
        int startIndex = Math.Min(characterIndex + whitespaceOffset, Length);

        if (builder is null)
        {
            content = content.Insert(startIndex, fragment);
        }
        else
        {
            builder.Insert(startIndex, fragment);
        }
    }

    /// <summary>
    /// Ensures that the line is prefixed with the correct amount of whitespace.
    /// </summary>
    /// <remarks>
    /// Since we don't add the whitespace for the first line until we know that there is a second line, we need to be able
    /// to fixup the whitespace for the second line at a later time.
    /// </remarks>
    public void EnsureWhitespace()
    {
        if (indentation > 0)
        {
            string whitespace = FormattedObjectGraph.MakeWhitespace(indentation);
            whitespaceOffset = whitespace.Length;

            if (content is not null)
            {
                content = whitespace + content;
            }

            if (builder is not null)
            {
                builder.Insert(0, whitespace);
            }

            indentation = 0;
        }
    }

    /// <summary>
    /// Truncates the current line at the specified character index and returns the remainder as a new line.
    /// Returns <see langword="null"/> if the remainder is empty.
    /// </summary>
    public Line Truncate(int characterIndex)
    {
        Flush();

        string truncatedContent = content.Substring(characterIndex + whitespaceOffset);

        if (truncatedContent.Trim().Length > 0)
        {
            content = content.Substring(0, characterIndex + whitespaceOffset);

            return new Line(new string(' ', whitespaceOffset) + truncatedContent)
            {
                indentation = indentation,
                whitespaceOffset = whitespaceOffset
            };
        }

        return null;
    }

    public override string ToString() => content is not null ? content.TrimEnd() : builder.ToString().TrimEnd();
}
