using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a node in the object graph as it is expected in a structural equivalency check.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// The name of the variable on which a structural equivalency assertion is executed or
        /// the default if reflection failed.
        /// </summary>
        GetSubjectId GetSubjectId { get; }

        /// <summary>
        /// Gets the name of this node.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of this node.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the path from the root object UNTIL the current node, separated by dots or index/key brackets.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets the full path from the root object up to and including the name of the node.
        /// </summary>
        string PathAndName { get; }

        /// <summary>
        /// Gets the path including the description of the subject.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the current node is the root.
        /// </summary>
        bool IsRoot { get; }

        /// <summary>
        /// Gets a value indicating if the root of this graph is a collection.
        /// </summary>
        bool RootIsCollection { get; }
    }

    /// <summary>
    /// Allows deferred fetching of the subject ID.
    /// </summary>
    public delegate string GetSubjectId();
}
