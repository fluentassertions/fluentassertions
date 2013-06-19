using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(EquivalencyValidationContext context)
        {
            return IsCollection(context.Subject);
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
            if (ExpectationIsCollection(context.Expectation))
            {
                var validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || context.Config.IsRecursive
                };

                validator.Validate((IEnumerable)context.Subject, (IEnumerable)context.Expectation);
            }

            return true;
        }

        private static bool ExpectationIsCollection(object expectation)
        {
            return VerificationScope.Current
                .ForCondition(IsCollection(expectation))
                .FailWith("{context:Subject} is a collection and cannot be compared with a non-collection type.");
        }

        private static bool IsCollection(object value)
        {
            return (!(value is string) && (value is IEnumerable));
        }
    }

    internal class EnumerableEquivalencyValidator
    {
        private readonly IEquivalencyValidator parent;
        private readonly EquivalencyValidationContext context;

        public EnumerableEquivalencyValidator(IEquivalencyValidator parent, EquivalencyValidationContext context)
        {
            this.parent = parent;
            this.context = context;
            Recursive = false;
        }

        public bool Recursive { get; set; }

        public void Validate(IEnumerable subject, IEnumerable expectation)
        {
            if (HaveSameLength(subject, expectation))
            {
                AssertElementsAreEquivalent(subject, expectation);
            }
        }

        private bool HaveSameLength(IEnumerable subject, IEnumerable expectation)
        {
            int subjectLength = subject.Cast<object>().Count();
            int expectationLength = expectation.Cast<object>().Count();

            return VerificationScope.Current
                .ForCondition(subjectLength == expectationLength)
                .FailWith("Expected {context:subject} to be a collection with {0} item(s){reason}, but found {1}.",
                    expectationLength, subjectLength);
        }

        private void AssertElementsAreEquivalent(IEnumerable subject, IEnumerable expectation)
        {
            if (Recursive)
            {
                AssertElementGraphEquivalency(subject.Cast<object>().ToArray(), expectation.Cast<object>().ToArray());
            }
            else
            {
                subject.Should().Equal(expectation);
            }
        }

        private void AssertElementGraphEquivalency(object[] subjects, object[] expectations)
        {
            for (int index = 0; index < expectations.Length; index++)
            {
                var results = new Dictionary<int, IEnumerable<string>>();
                for (int subjectIndex = 0; subjectIndex < subjects.Length; subjectIndex++)
                {
                    using (var scope = new VerificationScope())
                    {
                        parent.AssertEqualityUsing(context.CreateForCollectionItem(index, subjects[subjectIndex],
                            expectations[index]));

                        string[] failures = scope.Discard();

                        results[subjectIndex] = failures;

                        if (!failures.Any())
                        {
                            break;
                        }
                    }
                }

                if (results.All(v => v.Value.Any()))
                {
                    int fewestFailures = results.Values.Min(r => r.Count());
                    var bestResults = results.Where(r => r.Value.Count() == fewestFailures).ToArray();

                    var bestMatch = (bestResults.Any(r => r.Key == index))
                        ? bestResults.Single(r => r.Key == index)
                        : bestResults.First();

                    foreach (string failure in bestMatch.Value)
                    {
                        VerificationScope.Current.FailWith(failure);
                    }
                }
            }
        }
    }
}