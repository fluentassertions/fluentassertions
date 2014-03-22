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
        private readonly TMatchedElement matchedConstraint;


        public AndWhichConstraint(TParentConstraint parentConstraint, TMatchedElement matchedConstraint)
            : base(parentConstraint)
        {
            this.matchedConstraint = matchedConstraint;
        }

        /// <summary>
        /// Returns the instance which the original parent constraint matched, so that further matches can be performed
        /// </summary>
        public TMatchedElement Which
        {
            get { return matchedConstraint; }
        }
    }
}

