# Fluent Assertions – Claude Guidelines

## Project Overview

Fluent Assertions is a .NET library providing a rich set of fluent extension methods that allow developers to more naturally specify the expected outcome of test assertions. It supports multiple .NET test frameworks (xUnit, NUnit, MSTest, MSpec, TUnit, etc.) and targets `net47`, `net6.0`, and `net8.0`.

The source lives in `Src/FluentAssertions/` and the tests in `Tests/FluentAssertions.Specs/`.

## Build & Test Commands

```bash
# Build
dotnet build

# Run all tests
dotnet test

# Run tests for a specific project
dotnet test Tests/FluentAssertions.Specs/FluentAssertions.Specs.csproj

# Spell check documentation (run before pushing)
./build.sh --target spellcheck          # Linux/macOS
.\build.ps1 --target spellcheck         # Windows

# Accept intentional public API changes (after running approval tests)
./AcceptApiChanges.sh                   # Linux/macOS
.\AcceptApiChanges.ps1                  # Windows
```

The project uses a [Nuke](https://nuke.build/)-based build system. The build scripts `build.sh` / `build.ps1` / `build.cmd` are thin wrappers around Nuke and support all standard Nuke targets.

## Contributing Workflow

- Always target the `develop` branch for pull requests
- Prefer rebase over merge when updating a local branch
- **Any change to the public API requires prior approval**: open a GitHub issue, get it labeled `api-approved`, then open the PR
- After intentional public API changes, run `AcceptApiChanges.sh` / `AcceptApiChanges.ps1` to update the API approval baselines in `Tests/Approval.Tests/`
- Update `docs/_pages/releases.md` when adding features or fixing bugs
- Update `docs/_pages/` documentation when assertions are added or changed
- Run the spell checker before pushing: `./build.sh --target spellcheck`

## Code Style

Follow the [C# Coding Guidelines](https://csharpcodingguidelines.com/). Key rules enforced by `.editorconfig`:

- 4 spaces indentation; max line length of 130 characters
- CRLF line endings
- Opening braces on their own line (`csharp_new_line_before_open_brace = all`)
- Use language keywords instead of BCL types (`int` not `Int32`, `string` not `String`)
- Prefer pattern matching over casts (`is` patterns, not `as` + null check)
- Avoid `this.` qualifier unless required for disambiguation
- Use `var` only when the type is immediately apparent from the right-hand side
- Access modifiers are required on all non-interface members
- `readonly` is required on fields that are never reassigned
- Constant fields use PascalCase

## Assertion Classes

All assertion classes follow this dual-class pattern (concrete + generic base) to support both simple use and extensibility via inheritance:

```csharp
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions.Execution;

namespace FluentAssertions.Primitives;

/// <summary>
/// Contains a number of methods to assert that a <see cref="Foo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class FooAssertions : FooAssertions<FooAssertions>
{
    public FooAssertions(Foo value, AssertionChain assertionChain)
        : base(value, assertionChain)
    {
    }
}

#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
/// <summary>
/// Contains a number of methods to assert that a <see cref="Foo"/> is in the expected state.
/// </summary>
[DebuggerNonUserCode]
public class FooAssertions<TAssertions>
    where TAssertions : FooAssertions<TAssertions>
{
    private readonly AssertionChain assertionChain;

    public FooAssertions(Foo value, AssertionChain assertionChain)
    {
        this.assertionChain = assertionChain;
        Subject = value;
    }

    /// <summary>
    /// Gets the object whose value is being asserted.
    /// </summary>
    public Foo Subject { get; }

    /// <summary>
    /// Asserts that the foo satisfies some condition.
    /// </summary>
    /// <param name="because">
    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    /// </param>
    /// <param name="becauseArgs">
    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    /// </param>
    public AndConstraint<TAssertions> BeSomething(
        [StringSyntax("CompositeFormat")] string because = "",
        params object[] becauseArgs)
    {
        assertionChain
            .ForCondition(/* condition */)
            .BecauseOf(because, becauseArgs)
            .FailWith("Expected {context:foo} to be something{reason}.");

        return new AndConstraint<TAssertions>((TAssertions)this);
    }
}
```

Key points:
- All assertion classes must have `[DebuggerNonUserCode]`
- Generic parameter is `TAssertions` constrained to the class itself
- `Subject` property exposes the value under test
- Assertion methods return `AndConstraint<TAssertions>` to support chaining (`.And`)
- `because` / `becauseArgs` parameters are required on every assertion method
- `because` is decorated with `[StringSyntax("CompositeFormat")]`
- Failure messages use `{context:typename}` for the subject reference and `{reason}` for the `because` clause
- The `Should()` extension method for new types is added to `Src/FluentAssertions/AssertionExtensions.cs`
- XML doc comments on `Should()` extension methods follow the pattern: `Returns an <see cref="ReturnType"/> object that can be used to assert the current <see cref="ParameterType"/>.`

## Test Conventions

Tests use xUnit and the Arrange-Act-Assert (AAA) pattern. Each spec class is a partial class split into one file per assertion method.

**File naming**: `FooAssertionSpecs.BeSomething.cs` (partial class file per assertion method)

```csharp
using System;
using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Specs.Primitives;

public partial class FooAssertionSpecs
{
    public class BeSomething  // One nested class per assertion method
    {
        [Fact]
        public void Succeeds_for_foo_with_the_expected_value()
        {
            // Arrange
            var subject = new Foo(expectedValue);

            // Act / Assert
            subject.Should().BeSomething();
        }

        [Fact]
        public void The_foo_must_satisfy_some_condition()
        {
            // Arrange
            var subject = new Foo(unexpectedValue);

            // Act
            Action act = () => subject.Should().BeSomething();

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected foo to be something*");
        }

        [Fact]
        public void Fails_for_null_foo()
        {
            // Arrange
            Foo subject = null;

            // Act
            Action act = () => subject.Should().BeSomething();

            // Assert
            act.Should().Throw<XunitException>();
        }
    }
}
```

Naming rules:
- Use fact-based test method names (e.g. `Succeeds_for_*`, `The_X_must_be_Y`, `An_X_is_required`) – avoid "Should", "When", and "Asserting"
- Separate the "Act" and "Assert" steps only when testing failure paths; success-path tests can combine them as `// Act / Assert`
