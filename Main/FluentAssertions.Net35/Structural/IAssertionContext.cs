using System.Reflection;

namespace FluentAssertions.Structural
{
    /// <summary>
    /// Provides the required information for executing an equality assertion between a subject and an expectation.
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    public interface IAssertionContext<out TSubject>
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> of the property that returned the current object, or <c>null</c> if the current
        /// object represents the root object.
        /// </summary>
        PropertyInfo SubjectProperty { get; }

        /// <summary>
        /// Gets the value of the <see cref="SubjectProperty" />
        /// </summary>
        TSubject Subject { get; }

        /// <summary>
        /// Gets the value of the expectation object that was matched with the subject using a <see cref="IMatchingRule"/>.
        /// </summary>
        TSubject Expectation { get; }

        /// <summary>
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])"/> explaining why the assertion 
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </summary>
        string Reason { get; set; }

        /// <summary>
        /// Zero or more objects to format using the placeholders in <see cref="Reason"/>.
        /// </summary>
        object[] ReasonArgs { get; set; }
    }
}