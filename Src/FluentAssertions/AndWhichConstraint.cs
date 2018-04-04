using System;
using System.Collections.Generic;
using System.Linq;

using FluentAssertions.Common;
using FluentAssertions.Formatting;

namespace FluentAssertions
{
    /// <summary>
    /// Constraint which can be returned from an assertion which matches a condition and which will allow
    /// further matches to be performed on the matched condition as well as the parent constraint.
    /// </summary>
    /// <typeparam name="TParentConstraint">The type of the original constraint that was matched</typeparam>
    /// <typeparam name="TMatchedElement">The type of the matched object which the parent constraint matched</typeparam>
    public class AndWhichConstraint<TParentConstraint, TMatchedElement> : AndConstraint<TParentConstraint>
    {
        private readonly Lazy<TMatchedElement> matchedConstraint;

        public AndWhichConstraint(TParentConstraint parentConstraint, TMatchedElement matchedConstraint)
            : base(parentConstraint)
        {
            this.matchedConstraint =
                new Lazy<TMatchedElement>(() => matchedConstraint);
        }

        public AndWhichConstraint(TParentConstraint parentConstraint, IEnumerable<TMatchedElement> matchedConstraint)
            : base(parentConstraint)
        {
            this.matchedConstraint =
                new Lazy<TMatchedElement>(
                    () => SingleOrDefault(matchedConstraint));
        }

        private static TMatchedElement SingleOrDefault(
            IEnumerable<TMatchedElement> matchedConstraint)
        {
            TMatchedElement[] matchedElements = matchedConstraint.ToArray();

            if (matchedElements.Length > 1)
            {
                string foundObjects = string.Join(Environment.NewLine,
                    matchedElements.Select(
                        ele => "\t" + Formatter.ToString(ele)));

                string message = string.Format(
                    "More than one object found.  FluentAssertions cannot determine which object is meant.  Found objects:{0}{1}",
                    Environment.NewLine,
                    foundObjects);

                Services.ThrowException(message);
            }

            return matchedElements.Single();
        }

        /// <summary>
        /// Returns the single result of a prior assertion that is used to select a nested or collection item.
        /// </summary>
        public TMatchedElement Which => matchedConstraint.Value;

        /// <summary>
        /// Returns the single result of a prior assertion that is used to select a nested or collection item.
        /// </summary>
        /// <remarks>
        /// Just a convenience property that returns the same value as <see cref="Which"/>.
        /// </remarks>
        public TMatchedElement Subject => Which;
    }
}
