using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a node in the object graph that is being compared as part of a structural equivalency check.
    /// </summary>
    public class Node : INode
    {
        private static readonly Regex MatchFirstIndex = new(@"^\[\d+\]$");

        public GetSubjectId GetSubjectId { get; protected set; } = () => string.Empty;

        public Type Type { get; protected set; }

        public string Path { get; protected set; }

        public string PathAndName => Path.Combine(Name);

        public string Name { get; set; }

        public virtual string Description => $"{GetSubjectId().Combine(PathAndName)}";

        public bool IsRoot
        {
            get
            {
                // If the root is a collection, we need treat the objects in that collection as the root of the graph because all options
                // refer to the type of the collection items.
                return PathAndName.Length == 0 || (RootIsCollection && IsFirstIndex);
            }
        }

        private bool IsFirstIndex => MatchFirstIndex.IsMatch(PathAndName);

        public bool RootIsCollection { get; protected set; }

        public int Depth
        {
            get
            {
                const char memberSeparator = '.';
                return PathAndName.Count(chr => chr == memberSeparator);
            }
        }

        private static bool IsCollection(Type type)
        {
            return !typeof(string).IsAssignableFrom(type) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static INode From<T>(GetSubjectId getSubjectId)
        {
            return new Node
            {
                GetSubjectId = () => getSubjectId() ?? "root",
                Name = string.Empty,
                Path = string.Empty,
                Type = typeof(T),
                RootIsCollection = IsCollection(typeof(T))
            };
        }

        public static INode FromCollectionItem<T>(string index, INode parent)
        {
            return new Node
            {
                Type = typeof(T),
                Name = "[" + index + "]",
                Path = parent.PathAndName,
                GetSubjectId = parent.GetSubjectId,
                RootIsCollection = parent.RootIsCollection
            };
        }

        public static INode FromDictionaryItem<T>(object key, INode parent)
        {
            return new Node
            {
                Type = typeof(T),
                Name = "[" + key + "]",
                Path = parent.PathAndName,
                GetSubjectId = parent.GetSubjectId,
                RootIsCollection = parent.RootIsCollection
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Node)obj);
        }

        private bool Equals(Node other) => Type == other.Type && PathAndName == other.PathAndName;

        public override int GetHashCode()
        {
            unchecked
            {
#pragma warning disable CA1307
                return (Type.GetHashCode() * 397) ^ PathAndName.GetHashCode();
            }
        }

        public override string ToString() => Description;
    }
}
