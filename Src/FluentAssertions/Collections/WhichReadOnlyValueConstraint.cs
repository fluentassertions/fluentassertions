namespace FluentAssertions.Collections
{
    public class WhichReadOnlyValueConstraint<TKey, TValue> : AndConstraint<GenericReadOnlyDictionaryAssertions<TKey, TValue>>
    {
        public WhichReadOnlyValueConstraint(GenericReadOnlyDictionaryAssertions<TKey, TValue> parentConstraint, TValue value)
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
