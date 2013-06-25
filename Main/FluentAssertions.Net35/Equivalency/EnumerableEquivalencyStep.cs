using System.Collections;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class EnumerableEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the verificationScope subject and/or expectation.
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
                EnumerableEquivalencyValidator validator = new EnumerableEquivalencyValidator(parent, context)
                {
                    Recursive = context.IsRoot || context.Config.IsRecursive
                };

                validator.Validate(
                    ((IEnumerable) context.Subject).Cast<object>().ToArray(),
                    ((IEnumerable) context.Expectation).Cast<object>().ToArray());
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

        public void Validate(object[] subject, object[] expectation)
        {
            if (AssertLengthEquality(subject.Length, expectation.Length))
            {
                if (Recursive)
                {
                    AssertElementGraphEquivalency(subject, expectation);
                }
                else
                {
                    subject.Should().Equal(expectation);
                }
            }
        }

        private bool AssertLengthEquality(int subjectLength, int expectationLength)
        {
            return VerificationScope.Current
                .ForCondition(subjectLength == expectationLength)
                .FailWith("Expected {context:subject} to be a collection with {0} item(s){reason}, but found {1}.",
                    expectationLength, subjectLength);
        }

        private void AssertElementGraphEquivalency(object[] subjects, object[] expectations)
        {
            for (int index = 0; index < expectations.Length; index++)
            {
                object expectation = expectations[index];

                LooselyMatchAgainst(subjects, expectation, index);
            }
        }

        private void LooselyMatchAgainst(object[] subjects, object expectation, int expectationIndex)
        {
            var results = new AssertionResultSet();

            for (int index = 0; index < subjects.Length; index++)
            {
                object subject = subjects[index];

                results.AddSet(index, TryToMatch(subject, expectation, expectationIndex));
                if (results.ContainsSuccessfulSet)
                {
                    break;
                }
            }

            foreach (string failure in results.SelectClosestMatchFor(expectationIndex))
            {
                VerificationScope.Current.FailWith(failure);
            }
        }

        private string[] TryToMatch(object subject, object expectation, int index)
        {
            using (var scope = new VerificationScope())
            {
                parent.AssertEqualityUsing(context.CreateForCollectionItem(index, subject, expectation));

                return scope.Discard();
            }
        }
    }
}