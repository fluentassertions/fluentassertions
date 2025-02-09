using System.Text;

namespace FluentAssertions.Formatting;

/// <summary>
/// Represents the behavior of <see cref="Line"/> when most of the appending and inserting
/// has completed, and it no longer needs an internal <see cref="StringBuilder"/>.
/// </summary>
/// <param name="content"></param>
internal class FlushedLineState(string content) : ILineState
{
    private string content = content;

    public ILineState Flush()
    {
        return this;
    }

    public int Length => content.Length;

    public void Append(string fragment)
    {
        content += fragment;
    }

    public void InsertAtStart(string fragment)
    {
        content = fragment + content;
    }

    public void InsertAt(int startIndex, string fragment)
    {
        content = content.Insert(startIndex, fragment);
    }

    public Line Truncate(int characterIndex, int indentation, int whitespaceOffset)
    {
        string truncatedContent = content.Substring(characterIndex + whitespaceOffset);

        if (truncatedContent.Trim().Length > 0)
        {
            content = content.Substring(0, characterIndex + whitespaceOffset);

            return new Line(new string(' ', whitespaceOffset) + truncatedContent, indentation, whitespaceOffset);
        }

        return null;
    }

    public string Render() => content;
}
