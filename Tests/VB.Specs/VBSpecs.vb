Imports FluentAssertions
Imports Xunit
Imports Xunit.Sdk

Public Class VBSpecs
    <Fact>
    Public Sub Caller_identification_works_with_parentheses()
        ' Arrange
        Const subject = False

        ' Act
        Dim act As Action = Sub() subject.Should().BeTrue()

        ' Assert
        await act.Should().ThrowAsync(Of XunitException).WithMessage("Expected subject to be true, but found false.")
    End Sub

    <Fact>
    Public Sub Caller_identification_works_without_parentheses()
        ' Arrange
        Const subject = False

        ' Act
        Dim act As Action = Sub() subject.Should.BeTrue()

        ' Assert
        await act.Should().ThrowAsync(Of XunitException).WithMessage("Expected subject to be true, but found false.")
    End Sub
End Class
