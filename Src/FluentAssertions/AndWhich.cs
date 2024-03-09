using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions;

public class AndWhich<TParent, TSubject> : AndConstraint<TParent>
{
    private readonly AssertionChain assertionChain;
    private readonly string pathPostfix;
    private readonly Lazy<TSubject> getSubject;

    public AndWhich(TParent parent, TSubject subject, AssertionChain assertionChain, string pathPostfix)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => subject);

        this.assertionChain = assertionChain;
        this.pathPostfix = pathPostfix;
    }

    public AndWhich(TParent parent, IEnumerable<TSubject> subjects)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => SingleOrDefault(subjects));
    }

    /// <summary>
    /// Returns the single result of a prior assertion that is used to select a nested or collection item.
    /// </summary>
    /// <remarks>
    /// Just a convenience property that returns the same value as <see cref="Which"/>.
    /// </remarks>
    public TSubject Subject => Which;

    /// <summary>
    /// Returns the single result of a prior assertion that is used to select a nested or collection item.
    /// </summary>
    public TSubject Which
    {
        get
        {
            assertionChain.AddCallerPostfix(pathPostfix);

            AssertionChain.ReuseOnce(assertionChain);

            return getSubject.Value;
        }
    }

    private static TSubject SingleOrDefault(IEnumerable<TSubject> subjects)
    {
        TSubject[] matchedElements = subjects.ToArray();
        if (matchedElements.Length > 1)
        {
            string foundObjects = string.Join(Environment.NewLine,
                matchedElements.Select(ele => "\t" + Formatter.ToString(ele)));

            string message = "More than one object found.  FluentAssertions cannot determine which object is meant."
                + $"  Found objects:{Environment.NewLine}{foundObjects}";

            Services.ThrowException(message);
        }

        return matchedElements.Single();
    }
}
