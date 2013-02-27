using System.Collections;

namespace FluentAssertions.Equivalency
{
    internal class DictionaryEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context)
        {
            return (context.Subject is IDictionary);
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent)
        {
            var subject = (IDictionary)context.Subject;
            var expectation = (IDictionary)context.Expectation;

            foreach (object key in subject.Keys)
            {
                if (context.Config.IsRecursive)
                {
                    parent.AssertEqualityUsing(context.CreateForDictionaryItem(key, subject[key], expectation[key]));
                }
                else
                {
                    subject[key].Should().Be(expectation[key], context.Reason, context.ReasonArgs);
                }
            }

            return true;
        }
    }
}