using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Provides information on a particular property during an assertion for structural equality of two object graphs.
    /// </summary>
    public interface IStructuralEqualityContext : ISelectionContext
    {
        /// <summary>
        /// Gets the property of the <see cref="Expectation"/> that was matched against the <see cref="SubjectProperty"/>, 
        /// or <c>null</c> if <see cref="IsRoot"/> is <c>true</c>.
        /// </summary>
        PropertyInfo MatchingExpectationProperty { get; }

        /// <summary>
        /// Gets the value of the <see cref="MatchingExpectationProperty"/>.
        /// </summary>
        object Expectation { get; }

        /// <summary>
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        string Reason { get;  }

        /// <summary>
        /// Zero or more objects to format using the placeholders in <see cref="Reason"/>.
        /// </summary>
        object[] ReasonArgs { get;  }

        /// <summary>
        /// Gets a verification object associated with the current <see cref="Reason"/> and <see cref="ReasonArgs"/>.
        /// </summary>
        Verification Verification { get; }

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