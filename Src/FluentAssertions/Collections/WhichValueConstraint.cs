namespace FluentAssertions.Collections
{
    public class WhichValueConstraint<TKey, TValue> : AndConstraint<GenericDictionaryAssertions<TKey, TValue>>
    {
        public WhichValueConstraint(GenericDictionaryAssertions<TKey, TValue> parentConstraint, TValue value)
            : base(parentConstraint)
        {
            WhichValue = value;
        }

        /// <summary>
        /// Gets the value of the object referred to by the key.
        /// </summary>
        public TValue WhichValue { get; private set; }
    }
}
