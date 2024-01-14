using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions;

public class AndWhich<TParent, TSubject> : AndConstraint<TParent>
{
    private readonly Assertion assertion;
    private readonly string pathPostfix;
    private readonly Lazy<TSubject> getSubject;

    public AndWhich(TParent parent, TSubject subject, Assertion assertion, string pathPostfix)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => subject);

        this.assertion = assertion;
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
            assertion.AddCallerPostfix(pathPostfix);

            Assertion.ReuseOnce(assertion);

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
