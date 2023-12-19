using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertionsAsync.Common;

namespace FluentAssertionsAsync.Equivalency;

public class Node : INode
{
    private static readonly Regex MatchFirstIndex = new(@"^\[[0-9]+\]$");

    private string path;
    private string name;
    private string pathAndName;

    public GetSubjectId GetSubjectId { get; protected set; } = () => string.Empty;

    public Type Type { get; protected set; }

    public Type ParentType { get; protected set; }

    public string Path
    {
        get => path;
        protected set
        {
            path = value;
            pathAndName = null;
        }
    }

    public string PathAndName => pathAndName ??= Path.Combine(Name);

    public string Name
    {
        get => name;
        set
        {
            name = value;
            pathAndName = null;
        }
    }

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
            ParentType = null,
            RootIsCollection = IsCollection(typeof(T))
        };
    }

    public static INode FromCollectionItem<T>(string index, INode parent)
    {
        return new Node
        {
            Type = typeof(T),
            ParentType = parent.Type,
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
            ParentType = parent.Type,
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

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Node)obj);
    }

    private bool Equals(Node other) => (Type, Name, Path) == (other.Type, other.Name, other.Path);

    public override int GetHashCode()
    {
        unchecked
        {
#pragma warning disable CA1307
            int hashCode = Type.GetHashCode();
            hashCode = (hashCode * 397) + Path.GetHashCode();
            hashCode = (hashCode * 397) + Name.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => Description;
}
