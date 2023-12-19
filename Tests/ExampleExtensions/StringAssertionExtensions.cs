using FluentAssertionsAsync;
using FluentAssertionsAsync.Primitives;

namespace ExampleExtensions;

public static class StringAssertionExtensions
{
    public static void BePalindromic(this StringAssertions assertions)
    {
        char[] charArray = assertions.Subject.ToCharArray();
        Array.Reverse(charArray);
        string reversedSubject = new string(charArray);

        assertions.Subject.Should().Be(reversedSubject);
    }
}
