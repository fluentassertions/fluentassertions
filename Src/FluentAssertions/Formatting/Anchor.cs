namespace FluentAssertions.Formatting;

/// <summary>
/// Represents a point in the formatted object graph where a new fragment or line can be inserted.
/// </summary>
internal class Anchor
{
    private readonly FormattedObjectGraph parent;
    private readonly int indentation;
    private readonly int characterIndex;
    private readonly Line line;
    private readonly bool lineWasEmptyAtCreation;

    public Anchor(FormattedObjectGraph parent, Line line)
    {
        indentation = parent.Indentation;
        this.parent = parent;
        this.line = line;
        lineWasEmptyAtCreation = line is null || line.Length == 0;

        // Track the point in the graph where this instance was created.
        characterIndex = line?.LengthWithoutOffset ?? 0;
    }

    public bool UseLineBreaks { get; set; }

    public void InsertFragment(string fragment)
    {
        // Insert the fragment to the line and character position the anchor points at.
        if (line is null)
        {
            parent.InsertAtLineStartOrTop(fragment);
        }
        else
        {
            line.Insert(characterIndex, fragment);
        }

        // If the current line already contained text beyond the anchor point, move that part to the next line.
        if (line is not null && !RenderOnSingleLine)
        {
            parent.SplitLine(line, characterIndex + fragment.Length);
        }
    }

    public void InsertLineOrFragment(string fragment)
    {
        if (RenderOnSingleLine)
        {
            if (line is null)
            {
                parent.InsertAtLineStartOrTop(fragment);
            }
            else
            {
                line.Insert(characterIndex, fragment);
            }
        }
        else
        {
            string fragmentWithWhitespace = FormattedObjectGraph.MakeWhitespace(indentation) + fragment;

            // If the line was empty when the anchor was created, we can insert the fragment right here.
            // But if it wasn't empty, we need to continue the fragment on the next line.
            if (lineWasEmptyAtCreation)
            {
                parent.InsertAtTop(fragmentWithWhitespace);
            }
            else
            {
                parent.AddLineAfter(line, fragmentWithWhitespace);
            }
        }
    }

    internal void AddLineOrFragment(string fragment)
    {
        if (line is null)
        {
            parent.AddLineOrFragment(fragment);
        }
        else if (RenderOnSingleLine)
        {
            line.Append(fragment);
        }
        else
        {
            parent.AddLine(fragment);
        }
    }

    private bool RenderOnSingleLine => !UseLineBreaks && !parent.HasLinesBeyond(line);
}
