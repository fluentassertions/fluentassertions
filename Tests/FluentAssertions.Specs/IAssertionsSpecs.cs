using System;
using System.Globalization;
using Xunit;

namespace FluentAssertions.Specs;

internal class IAssertionsSpecs
{
    [Fact]
    public void Test()
    {
        var assertions = new FormatableAssertions<int>(42);

        assertions.BeFormatted("0.00", "42.00");
    }
}

internal sealed class FormatableAssertions<TSubject> : IAssertions<IFormattable>
    where TSubject : IFormattable
{
    public FormatableAssertions(TSubject subject)
    {
        Subject = subject;
    }

    public TSubject Subject { get; }

    IFormattable IAssertions<IFormattable>.Subject => Subject;
}

public static class FormatableExtensions
{
    public static AndConstraint<TAssertions> BeFormatted<TAssertions>(this TAssertions assertions, string format, string result)
        where TAssertions : IAssertions<IFormattable>
    {
        assertions.Subject.ToString(format, CultureInfo.InvariantCulture).Should().Be(result);

        return new AndConstraint<TAssertions>(assertions);
    }
}
