using System.Reflection;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Provides information on a particular property during an assertion for structural equality of two object graphs.
    /// </summary>
    public interface IEquivalencyValidationContext : ISubjectInfo
    {
        /// <summary>
        /// Gets the value of the <see cref="MatchingExpectationProperty"/>.
        /// </summary>
        object Expectation { get; }

        /// <summary>
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        string Reason { get; }

        /// <summary>
        /// Zero or more objects to format using the placeholders in <see cref="because"/>.
        /// </summary>
        object[] ReasonArgs { get; }

        /// <summary>
        /// Gets a value indicating whether the current context represents the root of the object graph.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Gets the value of the <see cref="ISelectionContext.PropertyInfo"/>
        /// </summary>
        object Subject { get; }
    }
}