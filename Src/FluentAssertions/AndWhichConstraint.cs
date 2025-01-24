using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Execution;
using FluentAssertions.Formatting;

namespace FluentAssertions;

/// <summary>
/// Provides a <see cref="Which"/> property that can be used in chained assertions where the prior assertions returns a
/// single object that the assertion continues on.
/// </summary>
public class AndWhichConstraint<TParent, TSubject> : AndConstraint<TParent>
{
    private readonly AssertionChain assertionChain;
    private readonly string pathPostfix;
    private readonly Lazy<TSubject> getSubject;

    /// <summary>
    /// Creates an object that allows continuing an assertion executed through <paramref name="parent"/> and
    /// which resulted in a single <paramref name="subject"/>.
    /// </summary>
    public AndWhichConstraint(TParent parent, TSubject subject)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => subject);
    }

    /// <summary>
    /// Creates an object that allows continuing an assertion executed through <paramref name="parent"/> and
    /// which resulted in a single <paramref name="subject"/> on an existing <paramref name="assertionChain"/>, but where
    /// the previous caller identifier is post-fixed with <paramref name="pathPostfix"/>.
    /// </summary>
    public AndWhichConstraint(TParent parent, TSubject subject, AssertionChain assertionChain, string pathPostfix = "")
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => subject);

        this.assertionChain = assertionChain;
        this.pathPostfix = pathPostfix;
    }

    /// <summary>
    /// Creates an object that allows continuing an assertion executed through <paramref name="parent"/> and
    /// which resulted in a potential collection of objects through <paramref name="subjects"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="subjects"/> contains more than one object, a clear exception is thrown.
    /// </remarks>
    // REFACTOR: In a next major version, we need to remove this overload and make the AssertionChain required
    public AndWhichConstraint(TParent parent, IEnumerable<TSubject> subjects)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => Single(subjects));
    }

    /// <summary>
    /// Creates an object that allows continuing an assertion executed through <paramref name="parent"/> and
    /// which resulted in a potential collection of objects through <paramref name="subjects"/> on an
    /// existing <paramref name="assertionChain"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="subjects"/> contains more than one object, a clear exception is thrown.
    /// </remarks>
    public AndWhichConstraint(TParent parent, IEnumerable<TSubject> subjects, AssertionChain assertionChain)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => Single(subjects));

        this.assertionChain = assertionChain;
    }

    /// <summary>
    /// Creates an object that allows continuing an assertion executed through <paramref name="parent"/> and
    /// which resulted in a potential collection of objects through <paramref name="subjects"/> on an
    /// existing <paramref name="assertionChain"/>, but where
    /// the previous caller identifier is post-fixed with <paramref name="pathPostfix"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="subjects"/> contains more than one object, a clear exception is thrown.
    /// </remarks>
    public AndWhichConstraint(TParent parent, IEnumerable<TSubject> subjects, AssertionChain assertionChain, string pathPostfix)
        : base(parent)
    {
        getSubject = new Lazy<TSubject>(() => Single(subjects));

        this.assertionChain = assertionChain;
        this.pathPostfix = pathPostfix;
    }

    private static TSubject Single(IEnumerable<TSubject> subjects)
    {
        TSubject[] matchedElements = subjects.ToArray();

        if (matchedElements.Length > 1)
        {
            string foundObjects = string.Join(Environment.NewLine,
                matchedElements.Select(ele => "\t" + Formatter.ToString(ele)));

            string message = "More than one object found.  FluentAssertions cannot determine which object is meant."
                + $"  Found objects:{Environment.NewLine}{foundObjects}";

            AssertionEngine.TestFramework.Throw(message);
        }

        return matchedElements.Single();
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
            if (pathPostfix is not null and not "")
            {
                assertionChain.WithCallerPostfix(pathPostfix).ReuseOnce();
            }
            else
            {
                // Make sure the caller identification restarts with the code following the Which property.
                assertionChain?.AdvanceToNextIdentifier();
                assertionChain?.ReuseOnce();
            }

            return getSubject.Value;
        }
    }
}
