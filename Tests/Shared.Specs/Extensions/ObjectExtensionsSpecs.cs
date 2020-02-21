using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using Xunit;

namespace FluentAssertions.Specs
{
    public class ObjectExtensionsSpecs
    {
        [Theory]
        [MemberData(nameof(GetNonEquivalentNumericData))]
        public void When_comparing_non_equivalent_boxed_numerics_it_should_fail(object actual, object expected)
        {
            actual.IsSameOrEqualTo(expected).Should().BeFalse();
        }

        public static IEnumerable<object[]> GetNonEquivalentNumericData()
        {
            yield return new object[] { double.Epsilon, 0M }; // double.Epsilon cannot be represented in Decimal
            yield return new object[] { 0M, double.Epsilon };
            yield return new object[] { double.Epsilon, 0.3M }; // 0.3M cannot be represented in double
            yield return new object[] { 0.3M, double.Epsilon };
            yield return new object[] { (byte)2, 256 }; // 256 cannot be represented in byte
            yield return new object[] { 256, (byte)2 };
            yield return new object[] { -1, (ushort)65535 }; // 65535 is -1 casted to ushort
            yield return new object[] { (ushort)65535, -1 };
            yield return new object[] { 0.02d, 0 };
            yield return new object[] { 0, 0.02d };
            yield return new object[] { 0.02f, 0 };
            yield return new object[] { 0, 0.02f };
            yield return new object[] { long.MaxValue, 9.22337204E+18 };
            yield return new object[] { 9.22337204E+18, long.MaxValue };
            yield return new object[] { 9223372030000000000L, 9.22337204E+18 };
            yield return new object[] { 9.22337204E+18, 9223372030000000000L };
        }

        [Theory]
        [MemberData(nameof(GetNumericAndNumericData))]
        public void When_comparing_a_numeric_to_a_numeric_it_should_succeed(object actual, object expected)
        {
            actual.IsSameOrEqualTo(expected).Should().BeTrue();
        }

        public static IEnumerable<object[]> GetNumericAndNumericData()
        {
            return from x in GetNumericIConvertibles()
                   from y in GetNumericIConvertibles()
                   select new[] { x, y };
        }

        [Theory]
        [MemberData(nameof(GetNonNumericAndNumericData))]
        public void When_comparing_a_non_numeric_to_a_numeric_it_should_fail(object actual, object unexpected)
        {
            // Act
            bool isSameOrEquals = actual.IsSameOrEqualTo(unexpected);

            // Assert
            isSameOrEquals.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetNonNumericAndNumericData()
        {
            return from x in GetNonNumericIConvertibles()
                   from y in GetNumericIConvertibles()
                   select new[] { x, y };
        }

        [Theory]
        [MemberData(nameof(GetNumericAndNonNumericData))]
        public void When_comparing_a_numeric_to_a_non_numeric_it_should_fail(object actual, object unexpected)
        {
            // Act
            bool isSameOrEquals = actual.IsSameOrEqualTo(unexpected);

            // Assert
            isSameOrEquals.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetNumericAndNonNumericData()
        {
            return from x in GetNumericIConvertibles()
                   from y in GetNonNumericIConvertibles()
                   select new[] { x, y };
        }

        [Theory]
        [MemberData(nameof(GetNonNumericAndNonNumericData))]
        public void When_comparing_a_non_numeric_to_a_non_numeric_it_should_fail(object actual, object unexpected)
        {
            // Act
            bool isSameOrEquals = actual.IsSameOrEqualTo(unexpected);

            // Assert
            isSameOrEquals.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetNonNumericAndNonNumericData()
        {
            object[] nonNumerics = GetNonNumericIConvertibles();
            return from x in nonNumerics
                   from y in nonNumerics
                   where x != y
                   select new[] { x, y };
        }

        private static object[] GetNumericIConvertibles()
        {
            return new object[]
            {
                (byte)1,
                (sbyte)1,
                (short)1,
                (ushort)1,
                (int)1,
                (uint)1,
                (long)1,
                (ulong)1,
                (float)1,
                (double)1,
                (decimal)1,
            };
        }

        private static object[] GetNonNumericIConvertibles()
        {
            return new object[]
            {
                true,
                '\u0001',
                new DateTime(1),
                DBNull.Value,
                "1"
            };
        }
    }
}
