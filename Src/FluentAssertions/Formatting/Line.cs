using System;

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

    private ILineState state;

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
        state = new BuildingLineState();
        this.indentation = indentation;
    }

    public Line(string content)
    {
        state = new FlushedLineState(content);
    }

    public Line(string truncatedContent, int indentation, int whitespaceOffset)
    {
        state = new FlushedLineState(truncatedContent);
        this.indentation = indentation;
        this.whitespaceOffset = whitespaceOffset;
    }

    /// <summary>
    /// Is used to close off the internal string builder.
    /// </summary>
    public void Flush()
    {
        state = state.Flush();
    }

    /// <summary>
    /// Gets the length of the content, including any whitespace that was inserted at the beginning of the line.
    /// </summary>
    public int Length => state.Length;

    /// <summary>
    /// Gets the length of the content without the offset of any whitespace that was inserted at the beginning of the line.
    /// </summary>
    public int LengthWithoutOffset => Length - whitespaceOffset;

    public void Append(string fragment)
    {
        state.Append(fragment);
    }

    public void InsertAtStart(string fragment)
    {
        state.InsertAtStart(fragment);
    }

    public void Insert(int characterIndex, string fragment)
    {
        int startIndex = Math.Min(characterIndex + whitespaceOffset, Length);
        state.InsertAt(startIndex, fragment);
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

            state.InsertAt(0, whitespace);

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

        return state.Truncate(characterIndex, indentation, whitespaceOffset);
    }

    public override string ToString()
    {
        // Only trim spaces, but keep line breaks.
        return state.Render().TrimEnd(' ');
    }
}
