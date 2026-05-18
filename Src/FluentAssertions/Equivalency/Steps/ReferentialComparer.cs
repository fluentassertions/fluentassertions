using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentAssertions.Equivalency.Steps;

/// <summary>
/// Provides a mechanism for comparing tuples that consist of a subject, an expectation,
/// and an expectation index. The comparison is based on object references and the expectation index.
/// </summary>
[System.Diagnostics.StackTraceHidden]
internal sealed class ReferentialComparer : IEqualityComparer<(object Subject, object Expectation, int ExpectationIndex)>
{
    public bool Equals((object Subject, object Expectation, int ExpectationIndex) x,
        (object Subject, object Expectation, int ExpectationIndex) y)
    {
        return ReferenceEquals(x.Subject, y.Subject)
               && ReferenceEquals(x.Expectation, y.Expectation)
               && x.ExpectationIndex == y.ExpectationIndex;
    }

    public int GetHashCode((object Subject, object Expectation, int ExpectationIndex) obj)
    {
        int hashCode = RuntimeHelpers.GetHashCode(obj.Subject);
        hashCode = (hashCode * 397) + RuntimeHelpers.GetHashCode(obj.Expectation);
        hashCode = (hashCode * 397) + obj.ExpectationIndex;
        return hashCode;
    }
}
