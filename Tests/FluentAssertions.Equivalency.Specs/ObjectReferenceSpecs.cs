using FluentAssertionsAsync.Equivalency.Execution;
using Xunit;

namespace FluentAssertionsAsync.Equivalency.Specs;

public class ObjectReferenceSpecs
{
    [Theory]
    [InlineData("", "", false)]
    [InlineData("[a]", "", true)]
    [InlineData("[a]", "[b]", false)]
    [InlineData("[a]", "[a][b]", true)]
    [InlineData("[a][a]", "[b][a]", false)]
    [InlineData("[a][b][c][d][e][a]", "[a]", true)]
    [InlineData("[a][b][c][d][e][a]", "[a][b][c]", true)]
    public void Equals_should_be_symmetrical(string xPath, string yPath, bool expectedResult)
    {
        /*
          From the .NET documentation:

             The following statements must be true for all implementations of the Equals(Object) method.
             [..]

             x.Equals(y) returns the same value as y.Equals(x).

          Earlier implementations of ObjectReference.Equals were not symmetrical.
        */

        // Arrange
        var obj = new object();

        var x = new ObjectReference(obj, xPath);
        var y = new ObjectReference(obj, yPath);

        // Act
        var forwardResult = x.Equals(y);
        var backwardResult = y.Equals(x);

        // Assert
        forwardResult.Should().Be(backwardResult, "because the contract for Equals specifies symmetry");
        forwardResult.Should().Be(expectedResult, "because of the semantics of ObjectReference.Equals (sanity check)");
    }
}
