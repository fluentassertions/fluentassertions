using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions.Execution;

namespace FluentAssertions.Formatting
{
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
        public int LineCount => lines.Count + ((lineBuilder.Length > 0) ? 1 : 0);

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

        private string Whitespace => new(' ', indentation * SpacesPerIndentation);
    }
}
