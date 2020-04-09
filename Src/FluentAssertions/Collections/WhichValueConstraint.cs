using System.Collections.Generic;

namespace FluentAssertions.Collections
{
    public class WhichValueConstraint<TCollection, TKey, TValue, TAssertions> : AndConstraint<TAssertions>
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        where TAssertions : GenericDictionaryAssertions<TCollection, TKey, TValue, TAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhichValueConstraint{TCollection, TKey, TValue, TAssertions}"/> class.
        /// </summary>
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
