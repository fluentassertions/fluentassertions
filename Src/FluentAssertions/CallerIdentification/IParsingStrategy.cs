using System.Text;

namespace FluentAssertions.CallerIdentification
{
    /// <summary>
    /// Represents a stateful parsing strategy that is used to help identify the "caller" to use in an assertion message.
    ///
    /// The strategies will be instantiated at the beginning of a "caller identification" task, and will live until
    /// the statement can be identified (and thus some are stateful).
    /// </summary>
    internal interface IParsingStrategy
    {
        /// <summary>
        /// Given a symbol, the parsing strategy should add/remove from the statement if needed, and then return
        /// - InProgress if the symbol isn't relevant to the strategies (so other strategies can be tried)
        /// - Handled if an action has been taken (and no other strategies should be used for this symbol)
        /// - Done if the statement is complete, and thus further symbols should be read.
        /// </summary>
        ParsingState Parse(char symbol, StringBuilder statement);

        /// <summary>
        /// Returns true if strategy is in the middle of a context (ex: strategy read "/*" and is waiting for "*/"
        /// </summary>
        bool IsWaitingForContextEnd();

        /// <summary>
        /// Used to notify the strategy that we have reached the end of the line (very useful to detect the end of
        /// a single line comment).
        /// </summary>
        void NotifyEndOfLineReached();
    }
}
