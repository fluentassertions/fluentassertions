using System;
using System.Diagnostics;

namespace FluentAssertions
{
    [DebuggerNonUserCode]
    public static class NullableNumericExtensions
    {
        public static AndConstraint<NumericAssertions<T?>> HaveValue<T>(this NumericAssertions<T?> parent) where T : struct
        {
            return HaveValue(parent, String.Empty);
        }

        public static AndConstraint<NumericAssertions<T?>> HaveValue<T>(this NumericAssertions<T?> parent, string reason, params object[] reasonParameters) where T: struct
        {
            Verification.Verify(!ReferenceEquals(parent.Subject, null), "Expected a value{2}.", null, null, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T?>>(parent);
        }

        public static AndConstraint<NumericAssertions<T?>> NotHaveValue<T>(this NumericAssertions<T?> parent) where T:struct
        {
            return NotHaveValue(parent, String.Empty);
        }

        public static AndConstraint<NumericAssertions<T?>> NotHaveValue<T>(this NumericAssertions<T?> parent, string reason, params object[] reasonParameters) where T : struct
        {
            Verification.Verify(ReferenceEquals(parent.Subject, null), "Did not expect a value{2}, but found {1}.", null, parent.Subject, reason, reasonParameters);

            return new AndConstraint<NumericAssertions<T?>>(parent);
        }

    }
}