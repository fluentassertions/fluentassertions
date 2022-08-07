using System.Collections;

namespace FluentAssertions.Common
{
    /// <summary>
    /// Used to provide access to the underlying <typeparamref name="TCollection"/> for an object that wraps an underlying
    /// collection.
    /// </summary>
    /// <typeparam name="TCollection">Collection type.</typeparam>
    public interface ICollectionWrapper<TCollection>
        where TCollection : ICollection
    {
        TCollection UnderlyingCollection { get; }
    }
}
