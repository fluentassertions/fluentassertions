using System;

namespace FluentAssertions.Formatting;

/// <summary>
/// Implementation of the contained PossibleMultilineFragment.
/// </summary>
public partial class FormattedObjectGraph
{
    /// <summary>
    /// Write fragments that may be on a single line or span multiple lines,
    /// and this is not known until later parts of the fragment are written.
    /// </summary>
    internal record PossibleMultilineFragment
    {
        private readonly FormattedObjectGraph formattedGraph;
        private readonly int startingLineBuilderIndex;
        private readonly int startingLineCount;

        public PossibleMultilineFragment(FormattedObjectGraph formattedGraph)
        {
            this.formattedGraph = formattedGraph;
            startingLineBuilderIndex = formattedGraph.lineBuilder.Length;
            startingLineCount = formattedGraph.lines.Count;
        }

        /// <summary>
        /// Write the fragment at the position the graph was in when this instance was created.
        ///
        /// <para>
        /// If more lines have been added since this instance was created then write the
        /// fragment on a new line, otherwise write it on the same line.
        /// </para>
        /// </summary>
        internal void AddFragmentAtStart(string fragment)
        {
            if (FormatOnSingleLine)
            {
                formattedGraph.lineBuilder.Insert(startingLineBuilderIndex, fragment);
            }
            else
            {
                formattedGraph.lines.Insert(startingLineCount, Environment.NewLine + fragment);
                InsertAtStartOfLine(startingLineCount + 1, MakeWhitespace(1));
            }
        }

        private bool FormatOnSingleLine => formattedGraph.lines.Count == startingLineCount;

        private void InsertAtStartOfLine(int lineIndex, string insertion)
        {
            formattedGraph.lines[lineIndex] = formattedGraph.lines[lineIndex].Insert(0, insertion);
        }

        /// <summary>
        /// If more lines have been added since this instance was created then write the
        /// fragment on a new line, otherwise write it on the same line.
        /// </summary>
        internal void AddLineOrFragment(string fragment)
        {
            if (FormatOnSingleLine)
            {
                formattedGraph.AddFragment(fragment);
            }
            else
            {
                formattedGraph.AddFragmentOnNewLine(fragment);
            }
        }

        /// <summary>
        /// Write the fragment.  If more lines have been added since this instance was
        /// created then also flush the line and indent the next line.
        /// </summary>
        internal void AddFragmentAtEndOfLine(string fragment)
        {
            if (FormatOnSingleLine)
            {
                formattedGraph.AddFragment(fragment);
            }
            else
            {
                formattedGraph.AddFragment(fragment);
                formattedGraph.FlushCurrentLine();
                formattedGraph.lineBuilderWhitespace += MakeWhitespace(1);
            }
        }

        internal void AddFragment(string fragment) => formattedGraph.AddFragment(fragment);
    }
}
