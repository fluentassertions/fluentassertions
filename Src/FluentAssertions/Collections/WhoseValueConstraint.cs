using System.Collections.Generic;

namespace FluentAssertions.Collections
{
    public class WhoseValueConstraint<TCollection, TKey, TValue, TAssertions> : AndConstraint<TAssertions>
        where TCollection : IEnumerable<KeyValuePair<TKey, TValue>>
        where TAssertions : GenericDictionaryAssertions<TCollection, TKey, TValue, TAssertions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhoseValueConstraint{TCollection, TKey, TValue, TAssertions}"/> class.
        /// </summary>
        public WhoseValueConstraint(TAssertions parentConstraint, TValue value)
            : base(parentConstraint)
        {
            WhoseValue = value;
        }

        /// <summary>
        /// Gets the value of the object referred to by the key.
        /// </summary>
        public TValue WhoseValue { get; }
    }
}
