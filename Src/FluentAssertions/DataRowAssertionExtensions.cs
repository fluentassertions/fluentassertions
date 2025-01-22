using System.Data;
using System.Diagnostics;
using FluentAssertions.Data;
using JetBrains.Annotations;

namespace FluentAssertions;

/// <summary>
/// Contains an extension method for custom assertions in unit tests related to DataRow objects.
/// </summary>
[DebuggerNonUserCode]
public static class DataRowAssertionExtensions
{
    /// <summary>
    /// Returns a <see cref="DataRowAssertions{DataRow}"/> object that can be used to assert the
    /// current <see cref="DataRow"/>.
    /// </summary>
    [Pure]
    public static DataRowAssertions<TDataRow> Should<TDataRow>([System.Diagnostics.CodeAnalysis.NotNullAttribute] this TDataRow actualValue)
        where TDataRow : DataRow
    {
        return new DataRowAssertions<TDataRow>(actualValue);
    }
}
