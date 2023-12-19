using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertionsAsync.Execution;

namespace FluentAssertionsAsync.Formatting;

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
    private readonly int maxLines;
    private readonly List<string> lines = new();
    private readonly StringBuilder lineBuilder = new();
    private int indentation;
    private string lineBuilderWhitespace = string.Empty;

    public FormattedObjectGraph(int maxLines)
    {
        this.maxLines = maxLines;
    }

    /// <summary>
    /// The number of spaces that should be used by every indentation level.
    /// </summary>
    public static int SpacesPerIndentation => 4;

    /// <summary>
    /// Returns the number of lines of text currently in the graph.
    /// </summary>
    public int LineCount => lines.Count + (lineBuilder.Length > 0 ? 1 : 0);

    /// <summary>
    /// Starts a new line with the provided text fragment. Additional text can be added to
    /// that same line through <see cref="AddFragment"/>.
    /// </summary>
    public void AddFragmentOnNewLine(string fragment)
    {
        FlushCurrentLine();

        AddFragment(fragment);
    }

    /// <summary>
    /// Starts a new line with the provided line of text that does not allow
    /// adding more fragments of text.
    /// </summary>
    public void AddLine(string line)
    {
        FlushCurrentLine();

        AppendSafely(Whitespace + line);
    }

    /// <summary>
    /// Adds a new fragment of text to the current line.
    /// </summary>
    public void AddFragment(string fragment)
    {
        if (lineBuilderWhitespace.Length > 0)
        {
            lineBuilder.Append(lineBuilderWhitespace);
            lineBuilderWhitespace = string.Empty;
        }

        lineBuilder.Append(fragment);
    }

    /// <summary>
    /// Adds a new line if there are no lines and no fragment that would cause a new line.
    /// </summary>
    internal void EnsureInitialNewLine()
    {
        if (LineCount == 0)
        {
            InsertInitialNewLine();
        }
    }

    /// <summary>
    /// Inserts an empty line as the first line unless it is already.
    /// </summary>
    private void InsertInitialNewLine()
    {
        if (lines.Count == 0 || !string.IsNullOrEmpty(lines[0]))
        {
            lines.Insert(0, string.Empty);
            lineBuilderWhitespace = Whitespace;
        }
    }

    private void FlushCurrentLine()
    {
        if (lineBuilder.Length > 0)
        {
            AppendSafely(lineBuilderWhitespace + lineBuilder);

            lineBuilder.Clear();
            lineBuilderWhitespace = Whitespace;
        }
    }

    private void AppendSafely(string line)
    {
        if (lines.Count == maxLines)
        {
            lines.Add(string.Empty);

            lines.Add(
                $"(Output has exceeded the maximum of {maxLines} lines. " +
                $"Increase {nameof(FormattingOptions)}.{nameof(FormattingOptions.MaxLines)} on {nameof(AssertionScope)} or {nameof(AssertionOptions)} to include more lines.)");

            throw new MaxLinesExceededException();
        }

        lines.Add(line);
    }

    /// <summary>
    /// Increases the indentation of every line written into this text block until the returned disposable is disposed.
    /// </summary>
    /// <remarks>
    /// The amount of spacing used for each indentation level is determined by <see cref="SpacesPerIndentation"/>.
    /// </remarks>
    public IDisposable WithIndentation()
    {
        indentation++;

        return new Disposable(() =>
        {
            if (indentation > 0)
            {
                indentation--;
            }
        });
    }

    /// <summary>
    /// Returns the final textual multi-line representation of the object graph.
    /// </summary>
    public override string ToString()
    {
        return string.Join(Environment.NewLine, lines.Concat(new[] { lineBuilder.ToString() }));
    }

    internal PossibleMultilineFragment KeepOnSingleLineAsLongAsPossible()
    {
        return new PossibleMultilineFragment(this);
    }

    private string Whitespace => MakeWhitespace(indentation);

    private static string MakeWhitespace(int indent) => new(' ', indent * SpacesPerIndentation);

    /// <summary>
    /// Write fragments that may be on a single line or span multiple lines,
    /// and this is not known until later parts of the fragment are written.
    /// </summary>
    internal record PossibleMultilineFragment
    {
        private readonly FormattedObjectGraph parentGraph;
        private readonly int startingLineBuilderIndex;
        private readonly int startingLineCount;

        public PossibleMultilineFragment(FormattedObjectGraph parentGraph)
        {
            this.parentGraph = parentGraph;
            startingLineBuilderIndex = parentGraph.lineBuilder.Length;
            startingLineCount = parentGraph.lines.Count;
        }

        /// <summary>
        /// Write the fragment at the position the graph was in when this instance was created.
        ///
        /// <para>
        /// If more lines have been added since this instance was created then write the
        /// fragment on a new line, otherwise write it on the same line.
        /// </para>
        /// </summary>
        internal void AddStartingLineOrFragment(string fragment)
        {
            if (FormatOnSingleLine)
            {
                parentGraph.lineBuilder.Insert(startingLineBuilderIndex, fragment);
            }
            else
            {
                parentGraph.InsertInitialNewLine();
                parentGraph.lines.Insert(startingLineCount + 1, parentGraph.Whitespace + fragment);
                InsertAtStartOfLine(startingLineCount + 2, MakeWhitespace(1));
            }
        }

        private bool FormatOnSingleLine => parentGraph.lines.Count == startingLineCount;

        private void InsertAtStartOfLine(int lineIndex, string insertion)
        {
            if (!parentGraph.lines[lineIndex].StartsWith(insertion, StringComparison.Ordinal))
            {
                parentGraph.lines[lineIndex] = parentGraph.lines[lineIndex].Insert(0, insertion);
            }
        }

        public void InsertLineOrFragment(string fragment)
        {
            if (FormatOnSingleLine)
            {
                parentGraph.lineBuilder.Insert(startingLineBuilderIndex, fragment);
            }
            else
            {
                parentGraph.lines[startingLineCount] = parentGraph.lines[startingLineCount]
                    .Insert(startingLineBuilderIndex, InsertNewLineIntoFragment(fragment));
            }
        }

        private string InsertNewLineIntoFragment(string fragment)
        {
            if (StartingLineHasBeenAddedTo())
            {
                return fragment + Environment.NewLine + MakeWhitespace(parentGraph.indentation + 1);
            }

            return fragment;
        }

        private bool StartingLineHasBeenAddedTo() => parentGraph.lines[startingLineCount].Length > startingLineBuilderIndex;

        /// <summary>
        /// If more lines have been added since this instance was created then write the
        /// fragment on a new line, otherwise write it on the same line.
        /// </summary>
        internal void AddLineOrFragment(string fragment)
        {
            if (FormatOnSingleLine)
            {
                parentGraph.AddFragment(fragment);
            }
            else
            {
                parentGraph.AddFragmentOnNewLine(fragment);
            }
        }

        /// <summary>
        /// Write the fragment.  If more lines have been added since this instance was
        /// created then also flush the line and indent the next line.
        /// </summary>
        internal void AddEndingLineOrFragment(string fragment)
        {
            if (FormatOnSingleLine)
            {
                parentGraph.AddFragment(fragment);
            }
            else
            {
                parentGraph.AddFragment(fragment);
                parentGraph.FlushCurrentLine();
                parentGraph.lineBuilderWhitespace += MakeWhitespace(1);
            }
        }

        internal void AddFragment(string fragment) => parentGraph.AddFragment(fragment);
    }
}
