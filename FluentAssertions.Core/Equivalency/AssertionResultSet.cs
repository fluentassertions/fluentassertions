using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a collection of assertion results obtained through a <see cref="AssertionScope"/>.
    /// </summary>
    internal class AssertionResultSet
    {
        private readonly Dictionary<object, string[]> set = new Dictionary<object, string[]>();

        /// <summary>
        /// Adds the failures (if any) resulting from executing an assertion within a
        ///  <see cref="AssertionScope"/> identified by a key. 
        /// </summary>
        public void AddSet(object key, string[] failures)
        {
            set[key] = failures;
        }

        /// <summary>
        /// Returns  the closest match compared to the set identified by the provided <paramref name="key"/> or
        /// an empty array if one of the results represents a successful assertion. 
        /// </summary>
        /// <remarks>
        ///  The closest match is the set that contains the least amount of failures, or no failures at all, and preferably 
        /// the set that is identified by the <paramref name="key"/>.        
        /// </remarks>
        public string[] SelectClosestMatchFor(object key = null)
        {
            if (!ContainsSuccessfulSet)
            {
                KeyValuePair<object, string[]> bestMatch = BestResultSets.Any(r => r.Key.Equals(key))
                    ? BestResultSets.Single(r => r.Key.Equals(key))
                    : BestResultSets.First();

                return bestMatch.Value;
            }

            return new string[0];
        }

        private KeyValuePair<object, string[]>[] BestResultSets
        {
            get
            {
                int fewestFailures = set.Values.Min(r => r.Count());
                return set.Where(r => r.Value.Count() == fewestFailures).ToArray();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this collection contains a set without any failures at all.
        /// </summary>
        public bool ContainsSuccessfulSet
        {
            get { return set.Values.Any(v => v.Length == 0); }
        }
    }
}