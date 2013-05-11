using System.Collections;

namespace FluentAssertions.Equivalency
{
    public class DictionaryEquivalencyStep : IEquivalencyStep
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
        public virtual bool Handle(EquivalencyValidationContext context, IEquivalencyValidator parent)
        {
            var subject = (IDictionary)context.Subject;
            var expectation = context.Expectation as IDictionary;

            if (PreconditionsAreMet(context, expectation, subject))
            {
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
            }

            return true;
        }

        private static bool PreconditionsAreMet(EquivalencyValidationContext context, IDictionary expectation, IDictionary subject)
        {
            bool success = context.Verification
                .ForCondition(expectation != null)
                .FailWith(context.SubjectDescription + " is a dictionary and cannot be compared with a non-dictionary type.");

            if (success)
            {
                success = context.Verification
                    .ForCondition(subject.Keys.Count == expectation.Keys.Count)
                    .FailWith("Expected " + context.SubjectDescription + 
                        " to be a dictionary with {0} item(s), but it only contains {1} item(s).",
                                     expectation.Keys.Count, subject.Keys.Count);
            }
            return success;
        }
    }
}