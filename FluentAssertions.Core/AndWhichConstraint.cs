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
    /// <typeparam name="TMatchedElement">The type of the matched object which the parent constarint matched</typeparam>
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

            if (matchedElements.Count() > 1)
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
        /// Returns the instance which the original parent constraint matched, so that further matches can be performed
        /// </summary>
        public TMatchedElement Which
        {
            get { return matchedConstraint.Value; }
        }
    }
}

