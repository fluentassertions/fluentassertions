namespace FluentAssertions.Collections
{
    public class WhichValueConstraint<TKey, TValue, TAssertions> : AndConstraint<TAssertions>
        where TAssertions : GenericDictionaryAssertions<TKey, TValue, TAssertions>
    {
        public WhichValueConstraint(TAssertions parentConstraint, TValue value)
            : base(parentConstraint)
        {
            WhichValue = value;
        }

        /// <summary>
        /// Gets the value of the object referred to by the key.
        /// </summary>
        public TValue WhichValue { get; }
    }
}
