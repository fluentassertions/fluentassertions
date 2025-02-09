using System.Text;

namespace FluentAssertions.Formatting;

/// <summary>
/// Represents the behavior of <see cref="Line"/> when it's still in the building phase and tries
/// to be as efficient as possible by using a <see cref="StringBuilder"/>.
/// </summary>
internal class BuildingLineState : ILineState
{
    private StringBuilder builder = new();

    public ILineState Flush()
    {
        var newState = new FlushedLineState(builder.ToString());
        builder = null;

        return newState;
    }

    public int Length => builder.Length;

    public void Append(string fragment)
    {
        builder.Append(fragment);
    }

    public void InsertAtStart(string fragment)
    {
        builder.Insert(0, fragment);
    }

    public void InsertAt(int startIndex, string fragment)
    {
        builder.Insert(startIndex, fragment);
    }

    public Line Truncate(int characterIndex, int indentation, int whitespaceOffset)
    {
        return null;
    }

    public string Render() => builder.ToString();
}
