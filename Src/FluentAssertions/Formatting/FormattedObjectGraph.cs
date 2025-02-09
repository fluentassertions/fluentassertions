using System;
using System.Linq;

namespace FluentAssertions.Formatting;

/// <summary>
/// This class is used by the <see cref="Formatter"/> class to collect all the output of the (nested calls of an) <see cref="IValueFormatter"/> into
/// a the final representation.
/// </summary>
/// <remarks>
/// The <see cref="FormattedObjectGraph"/> will ensure that the number of lines will be limited
/// to the maximum number of lines provided through its constructor. It will throw
/// a <see cref="MaxLinesExceededException"/> if the number of lines exceeds the maximum.
/// </remarks>
public class FormattedObjectGraph
{
    private readonly LineCollection lines;

    /// <summary>
    /// The current line that is being written to, or <see langword="null"/> if there is no active line.
    /// </summary>
    private Line currentLine;

    /// <summary>
    /// This class is used by the <see cref="Formatter"/> class to collect all the output of the (nested calls of an) <see cref="IValueFormatter"/> into
    /// a the final representation.
    /// </summary>
    /// <remarks>
    /// The <see cref="FormattedObjectGraph"/> will ensure that the number of lines will be limited
    /// to the maximum number of lines provided through its constructor. It will throw
    /// a <see cref="MaxLinesExceededException"/> if the number of lines exceeds the maximum.
    /// </remarks>
    public FormattedObjectGraph(int maxLines)
    {
        lines = new LineCollection(maxLines);
    }

    /// <summary>
    /// Represents the current level of indentation applied to newly added lines or fragments.
    /// </summary>
    /// <remarks>
    /// The indentation level determines the amount of leading whitespace to be added to each line,
    /// calculated based on <see cref="SpacesPerIndentation"/>.  It is incremented or decremented with methods such as
    /// <see cref="WithIndentation"/>, and affects all subsequent lines or fragments added to
    /// the <see cref="FormattedObjectGraph"/>.
    /// </remarks>
    internal int Indentation { get; private set; }

    /// <summary>
    /// The number of spaces that should be used by every indentation level.
    /// </summary>
    public static int SpacesPerIndentation => 4;

    /// <summary>
    /// Returns the number of lines of text currently in the graph.
    /// </summary>
    public int LineCount => lines.Count;

    /// <summary>
    /// Starts a new line with the provided text fragment. Additional text can be added to
    /// that same line through <see cref="AddFragment"/>.
    /// </summary>
    public void AddFragmentOnNewLine(string fragment)
    {
        FlushCurrentLine();
        GetCurrentLine().Append(fragment);
    }

    /// <summary>
    /// If there's only one line, adds a fragment to that line. If there are more lines, adds the fragment as
    /// a new line that does not allow any further fragments.
    /// </summary>
    public void AddLineOrFragment(string fragment)
    {
        if (lines.Count == 1)
        {
            AddFragment(fragment);
        }
        else
        {
            AddLine(fragment);
        }
    }

    /// <summary>
    /// Starts a new line with the provided text that does not allow adding more
    /// fragments of text.
    /// </summary>
    public void AddLine(string content)
    {
        FlushCurrentLine();

        GetCurrentLine().Append(content);
        FlushCurrentLine();
    }

    /// <summary>
    /// Adds a new fragment of text to the current line.
    /// </summary>
    public void AddFragment(string fragment)
    {
        GetCurrentLine().Append(fragment);
    }

    private void FlushCurrentLine()
    {
        // We only need to flush the line if there's something to flush.
        if (currentLine is not null)
        {
            currentLine.Flush();
            currentLine = null;
        }
    }

    private Line GetCurrentLine()
    {
        // We prefer to lazily initialize the current line so we don't waste memory.
        if (currentLine is null)
        {
            currentLine = new Line(Indentation);
            lines.Add(currentLine);
        }

        // A single-line rendering doesn't need any indentation, so we postpone that decision
        // until we know whether there will be more lines.
        if (lines.Count > 1)
        {
            currentLine.EnsureWhitespace();
        }

        return currentLine;
    }

    /// <summary>
    /// Increases the indentation of every line written into this text block until the returned disposable is disposed.
    /// </summary>
    /// <remarks>
    /// The amount of spacing used for each indentation level is determined by <see cref="SpacesPerIndentation"/>.
    /// </remarks>
    public IDisposable WithIndentation()
    {
        Indentation++;

        return new Disposable(() =>
        {
            if (Indentation > 0)
            {
                Indentation--;
            }
        });
    }

    /// <summary>
    /// Get a reference to the current line (or the last line if there is no active line), so that we can
    /// insert fragments and lines at that specific point.
    /// </summary>
    internal Anchor GetAnchor()
    {
        if (lines.Count == 0)
        {
            return new Anchor(this, null);
        }

        return new Anchor(this, currentLine ?? lines.Last());
    }

    internal static string MakeWhitespace(int indent) => new(' ', indent * SpacesPerIndentation);

    internal bool HasLinesBeyond(Line line) => lines.HasLinesBeyond(line);

    internal void AddLineAfter(Line line, string content)
    {
        lines.AddLineAfter(line, new Line(content));
    }

    internal void InsertAtTop(string content)
    {
        lines.InsertAtTop(new Line(content));
    }

    internal void InsertAtLineStartOrTop(string fragment)
    {
        lines.InsertAtLineStartOrTop(fragment);
    }

    internal void SplitLine(Line line, int characterIndex)
    {
        lines.SplitLine(line, characterIndex);
    }

    /// <summary>
    /// Returns the final textual multi-line representation of the object graph.
    /// </summary>
    public override string ToString()
    {
        return string.Join(Environment.NewLine, lines.Select(line => line.ToString()));
    }
}
