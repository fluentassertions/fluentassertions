# Copilot Instructions for Fluent Assertions

## Project Overview

Fluent Assertions is a .NET library providing a rich set of extension methods that allow you to more naturally specify the expected outcome of assertions. It supports multiple .NET test frameworks (xUnit, NUnit, MSTest, etc.).

## Build & Test

- Build with `dotnet build` or `./build.sh` / `.\build.ps1` (Nuke-based build system)
- Run tests with `dotnet test`
- Run spell check before pushing: `./build.sh --target spellcheck` or `.\build.ps1 --target spellcheck`
- After intentional public API changes, run `./AcceptApiChanges.sh` or `.\AcceptApiChanges.ps1` to update approval baselines

## Code Style

- Follow the [C# Coding Guidelines](https://csharpcodingguidelines.com/)
- 4 spaces indentation, max line length of 130 characters
- Opening braces on new lines (`csharp_new_line_before_open_brace = all`)
- Use language keywords instead of BCL types (`int` not `Int32`, `string` not `String`)
- Use pattern matching over casts (`is` pattern matching, `as` with null check)
- Avoid `this.` qualifier unless necessary
- Only use `var` when the type is not immediately obvious

## Assertion Classes

Assertion classes follow this pattern:

```csharp
// Concrete class (for simplicity of use)
[DebuggerNonUserCode]
public class FooAssertions : FooAssertions<FooAssertions>
{
    public FooAssertions(Foo value, AssertionChain assertionChain)
        : base(value, assertionChain) { }
}

// Generic base class (for extensibility)
#pragma warning disable CS0659, S1206 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
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

    public Foo Subject { get; }

    /// <summary>
    /// Asserts that ...
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

- All assertion classes must have the `[DebuggerNonUserCode]` attribute
- Assertion methods always return `AndConstraint<TAssertions>` or `AndWhichConstraint<TAssertions, T>` to enable chaining
- Use `{context:typename}` in failure messages to reference the subject
- `because` parameter must be decorated with `[StringSyntax("CompositeFormat")]`
- `Should()` extension methods live in `AssertionExtensions.cs`

## Test Conventions

Tests use xUnit with Arrange-Act-Assert pattern:

```csharp
namespace FluentAssertions.Specs.Primitives;

public partial class FooAssertionSpecs
{
    public class BeSomething  // Nested class per assertion method
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
                .WithMessage("Expected ...");
        }
    }
}
```

- Use fact-based test method names (e.g. `Succeeds_for_*`, `The_X_must_be_Y`, `An_X_is_required`) – avoid "Should", "When", and "Asserting"
- Each assertion method gets its own nested class within the spec class
- Spec files are split using partial classes: one file per assertion method (e.g., `FooAssertionSpecs.BeSomething.cs`)
- Test projects target `net47`, `net6.0`, and `net8.0`

## Contributing

- Target PRs at the `main` branch
- Any public API change requires prior approval via a GitHub issue labeled `api-approved`
- Update `docs/_pages/releases.md` when adding features or fixing bugs
- Update documentation in `docs/_pages/` if assertions are added or changed
