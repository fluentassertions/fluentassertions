using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Executes a single equivalency assertion on two collections, optionally recursive and with or without strict ordering.
    /// </summary>
    internal class EnumerableEquivalencyValidator
    {
        #region Private Definitions

        private readonly IEquivalencyValidator parent;
        private readonly IEquivalencyValidationContext context;

        #endregion

        public EnumerableEquivalencyValidator(IEquivalencyValidator parent, IEquivalencyValidationContext context)
        {
            this.parent = parent;
            this.context = context;
            Recursive = false;
        }

        public bool Recursive { get; set; }

        public OrderingRuleCollection OrderingRules { get; set; }

        public void Execute<T>(T[] subject, object[] expectation)
        {
            if (AssertIsNotNull(subject) && AssertLengthEquality(subject.Length, expectation.Length))
            {
                if (Recursive)
                {
                    using (context.TraceBlock(path => $"Structurally comparing {subject} and expectation {expectation} at {path}"))
                    {
                        AssertElementGraphEquivalency(subject, expectation);
                    }
                }
                else
                {
                    using (context.TraceBlock(path => $"Comparing subject {subject} and expectation {expectation} at {path} using simple value equality"))
                    {
                        subject.Should().BeEquivalentTo(expectation);
                    }
                }
            }
        }

        private bool AssertIsNotNull(object subject)
        {
            return AssertionScope.Current
                .ForCondition(!ReferenceEquals(subject, null))
                .FailWith("Expected {context:subject} to be a collection, but found <null>.");
        }

        private bool AssertLengthEquality(int subjectLength, int expectationLength)
        {
            return AssertionScope.Current
                .ForCondition(subjectLength == expectationLength)
                .FailWith("Expected {context:subject} to be a collection with {0} item(s){reason}, but found {1}.",
                    expectationLength, subjectLength);
        }

        private void AssertElementGraphEquivalency<T>(T[] subjects, object[] expectations)
        {
            matchedSubjectIndexes = new HashSet<int>();

            foreach (int index in Enumerable.Range(0, expectations.Length))
            {
                object expectation = expectations[index];

                if (!OrderingRules.IsOrderingStrictFor(context))
                {
                    using (context.TraceBlock(path => $"Finding the best match of {expectation} within all items in {subjects} at {path}[{index}]"))
                    {
                        LooselyMatchAgainst(subjects, expectation, index);
                    }
                }
                else
                {
                    using (context.TraceBlock(path => $"Strictly comparing expectation {expectation} at {path} to item with index {index} in {subjects}"))
                    {
                        StrictlyMatchAgainst(subjects, expectation, index);
                    }
                }
            }
        }

        private HashSet<int> matchedSubjectIndexes;

        private void LooselyMatchAgainst<T>(IList<T> subjects, object expectation, int expectationIndex)
        {
            var results = new AssertionResultSet();

            foreach (int index in Enumerable.Range(0, subjects.Count))
            {
                if (!matchedSubjectIndexes.Contains(index))
                {
                    T subject = subjects[index];

                    using (context.TraceBlock(path => $"Comparing subject at {path}[{index}] with the expectation at {path}[{expectationIndex}]"))
                    {
                        string[] failures = TryToMatch(subject, expectation, expectationIndex);

                        results.AddSet(index, failures);
                        if (results.ContainsSuccessfulSet)
                        {
                            context.TraceSingle(_ => $"It's a match");
                            matchedSubjectIndexes.Add(index);
                            break;
                        }
                        else
                        {
                            context.TraceSingle(_ => $"Contained {failures.Length} failures");
                        }
                    }
                }
            }

            foreach (string failure in results.SelectClosestMatchFor(expectationIndex))
            {
                AssertionScope.Current.AddFailure(failure);
            }
        }

        private string[] TryToMatch<T>(T subject, object expectation, int expectationIndex)
        {
            using (var scope = new AssertionScope())
            {
                parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex.ToString(), subject, expectation));

                return scope.Discard();
            }
        }

        private void StrictlyMatchAgainst<T>(T[] subjects, object expectation, int expectationIndex)
        {
            parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex.ToString(), subjects[expectationIndex], expectation));
        }
    }
}