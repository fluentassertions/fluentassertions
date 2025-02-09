namespace FluentAssertions.Formatting;

/// <summary>
/// Represents the state management of a line for structured content building or rendering.
/// </summary>
/// <remarks>
/// This interface defines the operations that can be performed on a line,
/// including appending content, inserting content at specific positions,
/// truncating the line, and rendering its content.
/// </remarks>
internal interface ILineState
{
    ILineState Flush();

    int Length { get; }

    void Append(string fragment);

    void InsertAtStart(string fragment);

    void InsertAt(int startIndex, string fragment);

    Line Truncate(int characterIndex, int indentation, int whitespaceOffset);

    string Render();
}
