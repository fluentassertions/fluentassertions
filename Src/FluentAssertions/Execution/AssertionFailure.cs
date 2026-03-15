using System;

namespace FluentAssertions.Execution;

/// <summary>
/// Represents an assertion failure whose message rendering is deferred until <see cref="ToString"/> is called.
/// This allows collecting failures without paying the cost of formatting until the message is actually needed.
/// </summary>
[System.Diagnostics.StackTraceHidden]
public class AssertionFailure
{
    private string cachedMessage;
    private Func<string> messageFactory;

    /// <summary>
    /// Creates a new <see cref="AssertionFailure"/> with a deferred message.
    /// </summary>
    internal AssertionFailure(Func<string> messageFactory)
    {
        this.messageFactory = messageFactory;
    }

    /// <summary>
    /// Creates a new <see cref="AssertionFailure"/> with a pre-formatted message.
    /// </summary>
    internal AssertionFailure(string preFormattedMessage)
    {
        cachedMessage = preFormattedMessage;
    }

    /// <summary>
    /// Returns the rendered failure message.
    /// </summary>
    public override string ToString()
    {
        if (cachedMessage is null)
        {
            cachedMessage = messageFactory?.Invoke() ?? string.Empty;
            messageFactory = null;
        }

        return cachedMessage;
    }
}
