namespace FluentAssertionsAsync.Equivalency;

public enum EqualityStrategy
{
    /// <summary>
    /// The object overrides <see cref="object.Equals(object)"/>, so use that.
    /// </summary>
    Equals,

    /// <summary>
    /// The object does not seem to override <see cref="object.Equals(object)"/>, so compare by members
    /// </summary>
    Members,

    /// <summary>
    /// Compare using <see cref="object.Equals(object)"/>, whether or not the object overrides it.
    /// </summary>
    ForceEquals,

    /// <summary>
    /// Compare the members, regardless of an <see cref="object.Equals(object)"/> override exists or not.
    /// </summary>
    ForceMembers,
}
