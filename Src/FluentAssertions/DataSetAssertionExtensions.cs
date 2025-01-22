using System.Data;
using System.Diagnostics;
using FluentAssertions.Data;
using JetBrains.Annotations;

namespace FluentAssertions;

/// <summary>
/// Contains an extension method for custom assertions in unit tests related to DataSet objects.
/// </summary>
[DebuggerNonUserCode]
public static class DataSetAssertionExtensions
{
    /// <summary>
    /// Returns a <see cref="DataSetAssertions{DataSet}"/> object that can be used to assert the
    /// current <see cref="DataSet"/>.
    /// </summary>
    [Pure]
    public static DataSetAssertions<TDataSet> Should<TDataSet>([System.Diagnostics.CodeAnalysis.NotNullAttribute] this TDataSet actualValue)
        where TDataSet : DataSet
    {
        return new DataSetAssertions<TDataSet>(actualValue);
    }
}
