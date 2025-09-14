using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions.Common;

namespace FluentAssertions.Equivalency;

internal class Node : INode
{
    private static readonly Regex MatchFirstIndex = new(@"^\[[0-9]+\]$");

    private GetSubjectId subjectIdProvider;

    private string cachedSubjectId;
    private Pathway subject;

    public GetSubjectId GetSubjectId
    {
        get => () => cachedSubjectId ??= subjectIdProvider();
        protected init => subjectIdProvider = value;
    }

    public Type Type { get; protected set; }

    public Type ParentType { get; protected set; }

    public Pathway Subject
    {
        get => subject;
        set
        {
            subject = value;

            if (Expectation is null)
            {
                Expectation = value;
            }
        }
    }

    public Pathway Expectation { get; protected set; }

    public bool IsRoot
    {
        get
        {
            // If the root is a collection, we need treat the objects in that collection as the root of the graph because all options
            // refer to the type of the collection items.
            return Subject.PathAndName.Length == 0 || (RootIsCollection && IsFirstIndex);
        }
    }

    private bool IsFirstIndex => MatchFirstIndex.IsMatch(Subject.PathAndName);

    public bool RootIsCollection { get; protected set; }

    public void AdjustForRemappedSubject(IMember subjectMember)
    {
        Subject = subjectMember.Subject;
    }

    public int Depth
    {
        get
        {
            const char memberSeparator = '.';
            return Subject.PathAndName.Count(chr => chr == memberSeparator);
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
            subjectIdProvider = () => getSubjectId() ?? "root",
            Subject = new Pathway(string.Empty, string.Empty, _ => getSubjectId()),
            Type = typeof(T),
            ParentType = null,
            RootIsCollection = IsCollection(typeof(T))
        };
    }

    public static INode FromCollectionItem<T>(string index, INode parent)
    {
        Pathway.GetDescription getDescription = pathAndName => parent.GetSubjectId().Combine(pathAndName);

        string itemName = "[" + index + "]";

        return new Node
        {
            Type = typeof(T),
            ParentType = parent.Type,
            Subject = new Pathway(parent.Subject, itemName, getDescription),
            Expectation = new Pathway(parent.Expectation, itemName, getDescription),
            GetSubjectId = parent.GetSubjectId,
            RootIsCollection = parent.RootIsCollection
        };
    }

    public static INode FromDictionaryItem<T>(object key, INode parent)
    {
        Pathway.GetDescription getDescription = pathAndName => parent.GetSubjectId().Combine(pathAndName);

        string itemName = "[" + key + "]";

        return new Node
        {
            Type = typeof(T),
            ParentType = parent.Type,
            Subject = new Pathway(parent.Subject, itemName, getDescription),
            Expectation = new Pathway(parent.Expectation, itemName, getDescription),
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

    private bool Equals(Node other) => (Type, Subject.Name, Subject.Path) == (other.Type, other.Subject.Name, other.Subject.Path);

    public override int GetHashCode()
    {
        unchecked
        {
#pragma warning disable CA1307
            int hashCode = Type.GetHashCode();
            hashCode = (hashCode * 397) + Subject.Path.GetHashCode();
            hashCode = (hashCode * 397) + Subject.Name.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => Subject.Description;
}
