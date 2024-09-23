using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs.Extensions;

public class ObjectExtensionsSpecs
{
    [Theory]
    [MemberData(nameof(GetNonEquivalentNumericData))]
    public void When_comparing_non_equivalent_boxed_numerics_it_should_fail(object actual, object expected)
    {
        // Arrange
        Func<object, object, bool> comparer = ObjectExtensions.GetComparer<object>();

        // Act
        bool success = comparer(actual, expected);

        // Assert
        success.Should().BeFalse();
    }

    public static TheoryData<object, object> GetNonEquivalentNumericData => new()
    {
        { double.Epsilon, 0M }, // double.Epsilon cannot be represented in Decimal
        { 0M, double.Epsilon },
        { double.Epsilon, 0.3M }, // 0.3M cannot be represented in double
        { 0.3M, double.Epsilon },
        { (byte)2, 256 }, // 256 cannot be represented in byte
        { 256, (byte)2 },
        { -1, (ushort)65535 }, // 65535 is -1 casted to ushort
        { (ushort)65535, -1 },
        { 0.02d, 0 },
        { 0, 0.02d },
        { 0.02f, 0 },
        { 0, 0.02f },
        { long.MaxValue, 9.22337204E+18 },
        { 9.22337204E+18, long.MaxValue },
        { 9223372030000000000L, 9.22337204E+18 },
        { 9.22337204E+18, 9223372030000000000L }
    };

    [Theory]
    [MemberData(nameof(GetNumericAndNumericData))]
    public void When_comparing_a_numeric_to_a_numeric_it_should_succeed(object actual, object expected)
    {
        // Arrange
        Func<object, object, bool> comparer = ObjectExtensions.GetComparer<object>();

        // Act
        bool success = comparer(actual, expected);

        // Assert
        success.Should().BeTrue();
    }

    public static TheoryData<object, object> GetNumericAndNumericData()
    {
        var pairs =
            from x in GetNumericIConvertibles()
            from y in GetNumericIConvertibles()
            select (x, y);

        var data = new TheoryData<object, object>();

        foreach (var (x, y) in pairs)
        {
            data.Add(x, y);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(GetNonNumericAndNumericData))]
    public void When_comparing_a_non_numeric_to_a_numeric_it_should_fail(object actual, object unexpected)
    {
        // Arrange
        Func<object, object, bool> comparer = ObjectExtensions.GetComparer<object>();

        // Act
        bool success = comparer(actual, unexpected);

        // Assert
        success.Should().BeFalse();
    }

    public static TheoryData<object, object> GetNonNumericAndNumericData()
    {
        var pairs =
            from x in GetNonNumericIConvertibles()
            from y in GetNumericIConvertibles()
            select (x, y);

        var data = new TheoryData<object, object>();

        foreach (var (x, y) in pairs)
        {
            data.Add(x, y);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(GetNumericAndNonNumericData))]
    public void When_comparing_a_numeric_to_a_non_numeric_it_should_fail(object actual, object unexpected)
    {
        // Arrange
        Func<object, object, bool> comparer = ObjectExtensions.GetComparer<object>();

        // Act
        bool success = comparer(actual, unexpected);

        // Assert
        success.Should().BeFalse();
    }

    public static TheoryData<object, object> GetNumericAndNonNumericData()
    {
        var pairs =
            from x in GetNumericIConvertibles()
            from y in GetNonNumericIConvertibles()
            select (x, y);

        var data = new TheoryData<object, object>();

        foreach (var (x, y) in pairs)
        {
            data.Add(x, y);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(GetNonNumericAndNonNumericData))]
    public void When_comparing_a_non_numeric_to_a_non_numeric_it_should_fail(object actual, object unexpected)
    {
        // Arrange
        Func<object, object, bool> comparer = ObjectExtensions.GetComparer<object>();

        // Act
        bool success = comparer(actual, unexpected);

        // Assert
        success.Should().BeFalse();
    }

    public static TheoryData<object, object> GetNonNumericAndNonNumericData()
    {
        object[] nonNumerics = GetNonNumericIConvertibles();

        var pairs =
            from x in nonNumerics
            from y in nonNumerics
            where x != y
            select (x, y);

        var data = new TheoryData<object, object>();

        foreach (var (x, y) in pairs)
        {
            data.Add(x, y);
        }

        return data;
    }

    private static object[] GetNumericIConvertibles()
    {
        return
        [
            (byte)1,
            (sbyte)1,
            (short)1,
            (ushort)1,
            1,
            1U,
            1L,
            1UL,
            1F,
            1D,
            1M,
        ];
    }

    private static object[] GetNonNumericIConvertibles()
    {
        return
        [
            true,
            '\u0001',
            new DateTime(1),
            DBNull.Value,
            "1"
        ];
    }
}
