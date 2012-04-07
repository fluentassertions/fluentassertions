using System.Collections;
using System.Linq;

namespace FluentAssertions.Assertions.Structural
{
    internal class EnumerableEqualityStep : IStructuralEqualityStep
    {
        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Returns <c>true</c> if this step finalizes the comparison task, returns <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met, or if it detects mismatching data.
        /// </remarks>
        public bool Execute(StructuralEqualityContext context, IStructuralEqualityValidator parent)
        {
            if (IsCollection(context.Subject))
            {
                FluentAssertions.Execute.Verification
                    .ForCondition(IsCollection(context.Expectation))
                    .BecauseOf(context.Reason, context.ReasonArgs)
                    .FailWith(((context.PropertyPath.Length == 0) ? "Subject" : context.PropertyPath) + " is a collection and cannot be compared with a non-collection type.",
                        context.Subject, context.Subject.GetType().FullName);

                var subject = ((IEnumerable)context.Subject).Cast<object>().ToArray();
                var expectation = ((IEnumerable)context.Expectation).Cast<object>().ToArray();

                FluentAssertions.Execute.Verification
                    .ForCondition(subject.Length == expectation.Length)
                    .BecauseOf(context.Reason, context.ReasonArgs)
                    .FailWith("Expected " + ((context.PropertyPath.Length == 0) ? "subject" : context.PropertyPath) + " to be a collection with {0} item(s){reason}, but found {1}.",
                        expectation.Length, subject.Length);

                if (context.IsRoot || context.Recursive)
                {
                    if (subject.SequenceEqual(expectation))
                    {
                        return true;
                    }
                    
                    for (int i = 0; i < subject.Length; i++)
                    {
                        string childPropertyName = "[" + i + "]"; 
                        if (context.PropertyPath.Length == 0)
                        {
                            childPropertyName = "item" + childPropertyName;
                        }

                        parent.AssertEquality(context.CreateNested(subject[i], expectation[i], childPropertyName));
                    }
                }
                else
                {
                    subject.Should().Equal(expectation, context.Reason, context.ReasonArgs);
                }

                return true;
            }

            return false;
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }
    }
}