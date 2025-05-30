using System.Collections;
using System.Collections.Generic;
using FluentAssertions.Execution;

namespace FluentAssertions.Formatting;

/// <summary>
/// A collection of lines that will throw a <see cref="MaxLinesExceededException"/> when the number of lines
/// exceeds the maximum.
/// </summary>
internal class LineCollection(int maxLines) : IEnumerable<Line>
{
    private readonly List<Line> lines = [];

    public int Count => lines.Count;

    public bool HasLinesBeyond(Line line)
    {
        // Null means that we're referring to the top of the list
        return (line is null && lines.Count > 1) || (line is not null && lines.IndexOf(line) < (lines.Count - 1));
    }

    public void Add(Line line)
    {
        lines.Add(line);
        OnCollectionIsModified();
    }

    public void AddLineAfter(Line line, Line newLine)
    {
        int index = lines.IndexOf(line);

        Insert(index + 1, newLine);
    }

    public void InsertAtTop(Line newLine)
    {
        Insert(0, newLine);
    }

    public void InsertAtLineStartOrTop(string fragment)
    {
        // If there's a single line, insert at the beginning of that line
        // If there are more than one line, insert as a new line at the top
        if (lines.Count == 1)
        {
            lines[0].InsertAtStart(fragment);
        }
        else
        {
            Insert(0, new Line(fragment));
        }
    }

    public void SplitLine(Line line, int characterIndex)
    {
        int lineIndex = lines.IndexOf(line);

        Line remainder = line.Truncate(characterIndex);
        if (remainder is not null)
        {
            Insert(lineIndex + 1, remainder);
        }
    }

    private void Insert(int index, Line item)
    {
        lines.Insert(index, item);
        OnCollectionIsModified();

        if (index == 0 && lines.Count > 1)
        {
            lines[1].EnsureWhitespace();
        }
    }

    private void OnCollectionIsModified()
    {
        if (lines.Count > maxLines)
        {
            lines.Add(new Line(0));

            lines.Add(new Line(
                $"(Output has exceeded the maximum of {maxLines} lines. " +
                $"Increase {nameof(FormattingOptions)}.{nameof(FormattingOptions.MaxLines)} on {nameof(AssertionScope)} or {nameof(AssertionConfiguration)} to include more lines.)"));

            throw new MaxLinesExceededException();
        }
    }

    public IEnumerator<Line> GetEnumerator() => lines.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
