namespace FSharp.Specs

open System
open FluentAssertions
open Xunit
open Xunit.Sdk

module FSharpSpecs =

    [<Fact>]
    let ``Caller identification works in F#`` () =
        // Arrange
        let subject = false

        // Act
        let act = Action(fun () -> subject.Should().BeTrue("") |> ignore)

        // Assert
        await act.Should().ThrowAsync<XunitException>("").WithMessage("Expected subject to be true, but found false.", "")
