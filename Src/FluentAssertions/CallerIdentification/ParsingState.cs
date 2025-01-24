namespace FluentAssertions.CallerIdentification;

internal enum ParsingState
{
    /// <summary>
    /// Is returned by a parser when the next one can take a look at the current symbol
    /// </summary>
    InProgress,

    /// <summary>
    /// Is returned by a parser when it decides a symbol has been processed enough and no
    /// other parsers need to look at the current symbol anymore.
    /// </summary>
    GoToNextSymbol,

    /// <summary>
    /// Is returned by a parser if it has found a candidate identifier.
    /// </summary>
    CandidateFound,

    /// <summary>
    /// Is returned by a parser to indicate that the parsing has been fully completed.
    /// </summary>
    Completed
}
