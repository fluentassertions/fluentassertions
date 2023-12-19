using System;

namespace FluentAssertionsAsync.Formatting;

public class MaxLinesExceededException : Exception
{
    public MaxLinesExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public MaxLinesExceededException(string message)
        : base(message)
    {
    }

    public MaxLinesExceededException()
    {
    }
}
