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
        private const int FailedItemsFastFailThreshold = 10;

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

        public void Execute<T>(object[] subject, T[] expectation)
        {
            if (AssertIsNotNull(expectation, subject) && AssertCollectionsHaveSameCount(subject, expectation))
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

        private static bool AssertIsNotNull(object expectation, object[] subject)
        {
            return AssertionScope.Current
                .ForCondition(!(expectation is null))
                .FailWith("Expected {context:subject} to be <null>, but found {0}.", new object[] { subject });
        }

        private static Continuation AssertCollectionsHaveSameCount<T>(ICollection<object> subject, ICollection<T> expectation)
        {
            return AssertionScope.Current
                .WithExpectation("Expected {context:subject} to be a collection with {0} item(s){reason}", expectation.Count)
                .AssertEitherCollectionIsNotEmpty(subject, expectation)
                .Then
                .AssertCollectionHasEnoughItems(subject, expectation)
                .Then
                .AssertCollectionHasNotTooManyItems(subject, expectation)
                .Then
                .ClearExpectation();
        }

        private void AssertElementGraphEquivalency<T>(object[] subjects, T[] expectations)
        {
            unmatchedSubjectIndexes = new List<int>(subjects.Length);
            unmatchedSubjectIndexes.AddRange(Enumerable.Range(0, subjects.Length));

            if (OrderingRules.IsOrderingStrictFor(context))
            {
                AssertElementGraphEquivalencyWithStrictOrdering(subjects, expectations);
            }
            else
            {
                AssertElementGraphEquivalencyWithLooseOrdering(subjects, expectations);
            }
        }

        private void AssertElementGraphEquivalencyWithStrictOrdering<T>(object[] subjects, T[] expectations)
        {
            int failedCount = 0;
            foreach (int index in Enumerable.Range(0, expectations.Length))
            {
                T expectation = expectations[index];

                using (context.TraceBlock(path =>
                    $"Strictly comparing expectation {expectation} at {path} to item with index {index} in {subjects}"))
                {
                    bool succeeded = StrictlyMatchAgainst(subjects, expectation, index);
                    if (!succeeded)
                    {
                        failedCount++;
                        if (failedCount >= FailedItemsFastFailThreshold)
                        {
                            context.TraceSingle(path =>
                                $"Aborting strict order comparison of collections after {FailedItemsFastFailThreshold} items failed at {path}");
                            break;
                        }
                    }
                }
            }
        }

        private void AssertElementGraphEquivalencyWithLooseOrdering<T>(object[] subjects, T[] expectations)
        {
            int failedCount = 0;
            foreach (int index in Enumerable.Range(0, expectations.Length))
            {
                T expectation = expectations[index];

                using (context.TraceBlock(path =>
                    $"Finding the best match of {expectation} within all items in {subjects} at {path}[{index}]"))
                {
                    bool succeeded = LooselyMatchAgainst(subjects, expectation, index);
                    if (!succeeded)
                    {
                        failedCount++;
                        if (failedCount >= FailedItemsFastFailThreshold)
                        {
                            context.TraceSingle(path =>
                                $"Fail failing loose order comparison of collection after {FailedItemsFastFailThreshold} items failed at {path}");
                            break;
                        }
                    }
                }
            }
        }

        private List<int> unmatchedSubjectIndexes;

        private bool LooselyMatchAgainst<T>(IList<object> subjects, T expectation, int expectationIndex)
        {
            var results = new AssertionResultSet();
            int index = 0;
            GetTraceMessage getMessage = path => $"Comparing subject at {path}[{index}] with the expectation at {path}[{expectationIndex}]";
            int indexToBeRemoved = -1;

            for (var metaIndex = 0; metaIndex < unmatchedSubjectIndexes.Count; metaIndex++)
            {
                index = unmatchedSubjectIndexes[metaIndex];
                object subject = subjects[index];

                using (context.TraceBlock(getMessage))
                {
                    string[] failures = TryToMatch(subject, expectation, expectationIndex);

                    results.AddSet(index, failures);
                    if (results.ContainsSuccessfulSet())
                    {
                        context.TraceSingle(_ => "It's a match");
                        indexToBeRemoved = metaIndex;
                        break;
                    }
                    else
                    {
                        context.TraceSingle(_ => $"Contained {failures.Length} failures");
                    }
                }
            }

            if (indexToBeRemoved != -1)
            {
                unmatchedSubjectIndexes.RemoveAt(indexToBeRemoved);
            }

            foreach (string failure in results.SelectClosestMatchFor(expectationIndex))
            {
                AssertionScope.Current.AddPreFormattedFailure(failure);
            }

            return indexToBeRemoved != -1;
        }

        private string[] TryToMatch<T>(object subject, T expectation, int expectationIndex)
        {
            using (var scope = new AssertionScope())
            {
                parent.AssertEqualityUsing(context.CreateForCollectionItem(expectationIndex.ToString(), subject, expectation));

                return scope.Discard();
            }
        }

        private bool StrictlyMatchAgainst<T>(object[] subjects, T expectation, int expectationIndex)
        {
            using (var scope = new AssertionScope())
            {
                object subject = subjects[expectationIndex];
                string indexString = expectationIndex.ToString();
                IEquivalencyValidationContext equivalencyValidationContext =
                    context.CreateForCollectionItem(indexString, subject, expectation);

                parent.AssertEqualityUsing(equivalencyValidationContext);

                bool failed = scope.HasFailures();
                return !failed;
            }
        }
    }
}
