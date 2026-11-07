---
name: fluentassertions
description: >
  Helps write expressive, maintainable .NET test assertions with Fluent Assertions (`.Should()`).
  Use this skill whenever you write, review or refactor C# unit or integration tests that use FluentAssertions:
  asserting on objects, strings, numbers, collections, dictionaries, exceptions (sync and async), dates and times,
  events, types and members, XML, JSON (`JsonNode`), comparing object graphs or DTOs with `BeEquivalentTo`,
  batching failures with `AssertionScope`, configuring global options, or building custom assertions.
  Also use it to replace `Should().BeTrue()` style or raw `Assert.*` (MSTest, xUnit, NUnit) assertions.
---

# Fluent Assertions

Extension methods that make test assertions read like specifications and produce precise failure messages.
Everything starts with `using FluentAssertions;` and a call to `.Should()` on the subject.
Add `using FluentAssertions.Extensions;` for the date, time and occurrence helpers (`1.March(2024)`, `5.Seconds()`, `2.TimesOrMore()`),
and `using FluentAssertions.Execution;` for `AssertionScope` and `AssertionChain`.
Version 8 targets .NET Standard 2.0/2.1, .NET Framework 4.7+ and .NET 6+, and works with xUnit, NUnit, MSTest, MSpec and TUnit
(detected automatically; override with `GlobalConfiguration.TestFramework = TestFramework.NUnit` if needed).

## Golden rules

1. **Put `Should()` on the real subject, as early as possible.** FA can only report what it knows.
   `actual.Count().Should().Be(3)` reports "Expected 2 to be 3"; `actual.Should().HaveCount(3)` lists the items it found.
2. **If an assertion ends in `Should().BeTrue()` / `BeFalse()`, there is almost always a better one.** See the rewrite table below.
3. **Use `BeEquivalentTo` to compare object graphs** (DTOs, records, anonymous objects, collections of them)
   instead of asserting property by property.
4. **Add a reason with `because`** when the intent is not obvious. Every assertion takes `string because = "", params object[] becauseArgs`.
   Write it so it reads after "because": `x.Should().BeTrue("the order was paid")`. Do not start it with "because".
5. **Chain with `.And`, drill down with `.Which`.** `ContainSingle()`, `ContainKey()`, `BeOfType<T>()`, `Throw<T>()`,
   `HaveElement()` and many others return the matched value through `.Which` (or `.Subject` / `.And` for exceptions).
6. **Use `AssertionScope` to report all failures at once** instead of stopping at the first one.
7. **Never compare `float`/`double` with `Be`**: use `BeApproximately(expected, precision)` or `BeInRange`.
8. **Use FA's date/time helpers** (`1.March(2024).At(14, 30)`, `5.Seconds()`) for readable values.

## Rewrite table: from weak to expressive

| Instead of | Write |
| --- | --- |
| `list.Any().Should().BeTrue()` | `list.Should().NotBeEmpty()` |
| `list.Any().Should().BeFalse()` / `list.Should().HaveCount(0)` | `list.Should().BeEmpty()` |
| `list.Any(x => x.IsActive).Should().BeTrue()` / `list.Where(p).Should().NotBeEmpty()` | `list.Should().Contain(x => x.IsActive)` |
| `list.Any(x => x.IsActive).Should().BeFalse()` | `list.Should().NotContain(x => x.IsActive)` |
| `list.All(x => x.IsActive).Should().BeTrue()` | `list.Should().OnlyContain(x => x.IsActive)` (fails on empty) or `AllSatisfy(...)` |
| `list.Contains(item).Should().BeTrue()` | `list.Should().Contain(item)` |
| `list.Count().Should().Be(n)` | `list.Should().HaveCount(n)` |
| `list.Count().Should().BeGreaterThan(n)` | `list.Should().HaveCountGreaterThan(n)` (also `...OrEqualTo`, `HaveCountLessThan[OrEqualTo]`, `NotHaveCount`) |
| `list.Should().HaveCount(1)` + `list.First()...` | `list.Should().ContainSingle().Which...` |
| `list.Where(p).Should().HaveCount(1)` | `list.Should().ContainSingle(p)` |
| `list.Should().HaveCount(other.Count())` | `list.Should().HaveSameCount(other)` |
| `list.Should().NotBeNull().And.NotBeEmpty()` | `list.Should().NotBeNullOrEmpty()` |
| `list[k].Should().Be(x)` / `list.ElementAt(k)...` | `list.Should().HaveElementAt(k, x)` |
| `list.OrderBy(x => x.P).Should().Equal(list)` | `list.Should().BeInAscendingOrder(x => x.P)` (or `BeInDescendingOrder`) |
| `list.Select(x => x.P).Should().Equal(other.Select(x => x.P))` | `list.Should().Equal(other, (a, b) => a.P == b.P)` |
| `list.Select(x => x.P).Should().OnlyHaveUniqueItems()` | `list.Should().OnlyHaveUniqueItems(x => x.P)` |
| `list.Intersect(other).Should().BeEmpty()` | `list.Should().NotIntersectWith(other)` |
| `dict.ContainsKey(k).Should().BeTrue()` | `dict.Should().ContainKey(k)` |
| `dict.Should().ContainKey(k).And.ContainValue(v)` | `dict.Should().Contain(k, v)` |
| `str.StartsWith("x").Should().BeTrue()` | `str.Should().StartWith("x")` |
| `string.IsNullOrEmpty(str).Should().BeFalse()` | `str.Should().NotBeNullOrEmpty()` (also `[Not]BeNullOrWhiteSpace`) |
| `str.Length.Should().Be(n)` | `str.Should().HaveLength(n)` |
| `n.Should().BeGreaterThan(0)` | `n.Should().BePositive()` (or `BeNegative()`) |
| `(lo <= n && n <= hi).Should().BeTrue()` | `n.Should().BeInRange(lo, hi)` |
| `nullable.HasValue.Should().BeTrue()` | `nullable.Should().HaveValue()` |
| `date.Date.Should().Be(other.Date)` | `date.Should().BeSameDateAs(other)` |
| `date.Month.Should().Be(3)` | `date.Should().HaveMonth(3)` (also `HaveYear/Day/Hour/Minute/Second`) |
| `obj.GetType().Should().Be(typeof(T))` | `obj.Should().BeOfType<T>()` |
| `(obj is T).Should().BeTrue()` | `obj.Should().BeAssignableTo<T>()` |
| `ex.Message.Should().Contain("x")` after `Throw` | `.Should().Throw<T>().WithMessage("*x*")` |
| `try { ...; Assert.Fail(); } catch (T) { }` | `act.Should().Throw<T>()` |

## Objects

```csharp
obj.Should().BeNull();                 obj.Should().NotBeNull();
obj.Should().Be(expected);             // uses Equals; overload takes an IEqualityComparer<T>
obj.Should().BeSameAs(other);          // reference equality
obj.Should().BeOneOf(a, b, c);
obj.Should().BeOfType<Customer>().Which.Name.Should().Be("Jane");
obj.Should().BeAssignableTo<IEntity>();
obj.Should().Match<Customer>(c => c.IsActive);
obj.Should().Satisfy<Customer>(c =>     // nested assertions instead of a predicate
{
    c.Name.Should().Be("Jane");
    c.Address.City.Should().Be("Amsterdam");
});
obj.As<Customer>().Name.Should().Be("Jane");    // fluent downcast
obj.Should().BeJsonSerializable();     // also BeXmlSerializable / BeDataContractSerializable
```

## Object graphs: `BeEquivalentTo`

Compares public properties and fields recursively by name, regardless of the runtime types.
Every member of the **expectation** must exist on the subject. Collection order is ignored by default.
Use an anonymous object as the expectation to assert only the members you care about.

```csharp
orderDto.Should().BeEquivalentTo(order);
orderDto.Should().BeEquivalentTo(new { Number = 42, Customer = new { Name = "Jane" } });
orderDtos.Should().BeEquivalentTo(orders);           // collections, any order
orderDtos.Should().AllBeEquivalentTo(new { Status = "Open" });
orderDto.Should().NotBeEquivalentTo(other);
```

Common options (combine freely):

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Excluding(o => o.Id)                                  // one member (can be nested: o.Customer.Name)
    .Excluding(o => new { o.CreatedAt, o.Version })        // several at once
    .ExcludingMembersNamed("Id", "ETag")                    // by name, anywhere in the graph
    .Excluding<DateTime>()                                  // all members of a type (also open generics)
    .Including(o => o.Number)                               // or select explicitly what to include
    .ExcludingMissingMembers()                              // only compare members both sides have
    .For(o => o.Lines).Exclude(l => l.Id)                   // exclude a member in every collection item
    .WithStrictOrdering()                                   // or WithStrictOrderingFor(o => o.Lines)
    .WithMapping<OrderDto>(e => e.Name, s => s.OrderName)   // expectation member first, subject member second
    .ComparingByValue<Money>()                              // use Equals instead of recursing
    .ComparingByMembers<MyRecord>()                         // recurse even though Equals is overridden
    .ComparingEnumsByName()                                 // default compares enums by value
    .ComparingNullCollectionsAsEmpty()
    .ComparingNullStringsAsEmpty()
    .IgnoringCase().IgnoringNewlineStyle()                  // string options; also IgnoringLeading/TrailingWhitespace
    .WithStrictTyping()                                     // types must match exactly
    .PreferringRuntimeMemberTypes()                         // default uses declared types
    .WithAutoConversion()                                   // e.g. "42" matches 42
    .IncludingFields().IncludingInternalProperties()
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Seconds()))
        .WhenTypeIs<DateTime>()
    .WithTracing());                                        // add a trace to the failure message (also WithFullDump)
```

Inline assertions inside the expectation are often more readable than `Using/When`:

```csharp
actual.Should().BeEquivalentTo(new
{
    Name = "John",
    Age = Value.ThatMatches<int>(age => age < 40),
    Email = Value.ThatSatisfies<string>(e => e.Should().EndWith("@example.com"))
});
```

Notes:
- Types that override `Equals` are compared by value, **except** records, anonymous types and tuples, which are compared by members.
- Recursion stops at 10 levels by default (`AllowingInfiniteRecursion()` to lift it). Cyclic references are detected.
- `byte[]` is always compared in strict order.
- On .NET 6+ a `JsonNode` can be compared to an anonymous object (`IgnoringJsonPropertyCasing()` is available).
- The failure message lists the options in effect, which helps diagnose surprising results.

## Collections

```csharp
items.Should().NotBeEmpty().And.HaveCount(4).And.OnlyHaveUniqueItems();
items.Should().Equal(1, 2, 5, 8);                    // same items, same order, Equals
items.Should().BeEquivalentTo(new[] { 8, 2, 1, 5 }); // same items, any order, structural
items.Should().Contain(8).And.NotContain(82);
items.Should().Contain(x => x > 3);
items.Should().ContainSingle(x => x.Id == 42).Which.Name.Should().Be("Jane");
items.Should().ContainEquivalentOf(new { Id = 42 });
items.Should().OnlyContain(x => x > 0);
items.Should().ContainInOrder(2, 5);                 // also ContainInConsecutiveOrder
items.Should().StartWith(1).And.EndWith(8);
items.Should().BeSubsetOf(superset);                 // also BeSupersetOf, BeProper..., IntersectWith
items.Should().BeInAscendingOrder(x => x.Name);      // ordinal for strings; pass a comparer to change that
items.Should().BeOrderedBy(x => x.Name, CollectionSortOrder.Descending);   // FluentAssertions.Collections
items.Should().AllBeOfType<Car>();                   // also AllBeAssignableTo<T>
items.Should().NotContainNulls(x => x.Name);
items.Should().HaveElementAt(2, 5);
names.Should().ContainMatch("* failed");             // wildcards * and ?, not regex

// Assert per item
items.Should().AllSatisfy(x => x.Id.Should().BePositive());
items.Should().SatisfyRespectively(                  // one inspector per item, in order
    first => first.Name.Should().Be("John"),
    second => second.Name.Should().Be("Jane"));
items.Should().Satisfy(                              // one predicate per item, any order
    x => x.Name == "Jane",
    x => x.Name == "John");
```

The same assertions work on `Span<T>`, `ReadOnlySpan<T>`, `Memory<T>` and `ReadOnlyMemory<T>`.

## Dictionaries

```csharp
dict.Should().ContainKey(1).WhoseValue.Should().Be("One");
dict.Should().ContainKeys(1, 2).And.NotContainKey(9);
dict.Should().ContainValue("One").Which.Should().StartWith("O");
dict.Should().Contain(1, "One");
dict.Should().Equal(otherDict);
dict.Should().BeEquivalentTo(new Dictionary<int, string> { [1] = "One" });
```

## Strings

```csharp
s.Should().Be("exact");                           // failure message shows where the strings differ
s.Should().BeEquivalentTo("EXACT");               // case-insensitive
s.Should().BeEquivalentTo("a\nb", o => o.IgnoringNewlineStyle().IgnoringTrailingWhitespace());
s.Should().Contain("is a", Exactly.Once());       // also AtLeast/AtMost/MoreThan/LessThan, 2.TimesOrMore()
s.Should().ContainAll("a", "b").And.NotContainAny("x", "y");
s.Should().StartWith("This").And.EndWith("String");   // ...EquivalentOf variants ignore case
s.Should().Match("*@*.com");                      // wildcards; MatchEquivalentOf ignores case
s.Should().MatchRegex(@"^\d{4}$");
s.Should().ContainLine("a full line").And.HaveLineCount(3);
s.Should().NotBeNullOrWhiteSpace();
s.Should().BeUpperCased();
```

## Numbers, booleans, enums, GUIDs, nullables

```csharp
n.Should().Be(5);    n.Should().BePositive();    n.Should().BeInRange(1, 10);
n.Should().BeGreaterThan(4).And.BeLessThanOrEqualTo(5);
d.Should().BeApproximately(3.14, 0.01);  d.Should().NotBeNaN();
flag.Should().BeTrue("the order was paid");
nullableFlag.Should().NotBeFalse();            // true or null
status.Should().Be(Status.Active);  options.Should().HaveFlag(Options.A);
status.Should().BeDefined();  status.Should().HaveValue(1);  status.Should().HaveSameNameAs(OtherEnum.Active);
id.Should().NotBeEmpty();  id.Should().Be("11111111-aaaa-bbbb-cccc-999999999999");
maybe.Should().HaveValue().And.Be(3);              // or NotHaveValue() / BeNull()
```

## Dates, times and durations

```csharp
var when = 1.March(2024).At(22, 15).AsUtc();
when.Should().Be(1.March(2024).At(22, 15));
when.Should().BeAfter(1.February(2024)).And.BeBefore(2.March(2024));
when.Should().BeCloseTo(DateTime.UtcNow, 2.Seconds());     // tolerate clock / database precision
when.Should().BeSameDateAs(1.March(2024));
when.Should().HaveYear(2024).And.HaveMonth(3);
when.Should().BeIn(DateTimeKind.Utc);
when.Should().BeWithin(2.Hours()).Before(deadline);        // also BeLessThan/BeMoreThan/BeAtLeast/BeExactly ... Before/After
offset.Should().HaveOffset(2.Hours());
duration.Should().BeLessThan(5.Minutes());
duration.Should().BeCloseTo(1.Hours(), 1.Seconds());
```

Helpers: `1.March(2024)`, `.At(14, 30)`, `.AsUtc()`, `.AsLocal()`, `.WithOffset(2.Hours())`,
`10.Milliseconds()`, `5.Seconds()`, `2.Minutes()`, `1.Hours()`, `3.Days()`, `2.Hours().Before(dt)`.
The same assertions exist for `DateTimeOffset`, `DateOnly` and `TimeOnly`.

## Exceptions

Wrap the act in a delegate and assert on the delegate.

```csharp
Action act = () => sut.Withdraw(-1);
act.Should().Throw<ArgumentOutOfRangeException>()
    .WithMessage("*negative*")                    // wildcard, case-insensitive
    .WithParameterName("amount");

act.Should().Throw<DomainException>()
    .Where(e => e.Code == 42)
    .WithInnerException<ArgumentException>();

act.Should().Throw<DomainException>().Which.Code.Should().Be(42);
act.Should().ThrowExactly<ArgumentException>();   // Throw<T> also accepts derived exceptions
act.Should().NotThrow();                          // clearer than letting the test crash
act.Should().NotThrowAfter(10.Seconds(), 100.Milliseconds());   // retry until it stops throwing

// async: always await the assertion
Func<Task> act = () => sut.WithdrawAsync(-1);
await act.Should().ThrowAsync<ArgumentOutOfRangeException>().WithMessage("*negative*");
await act.Should().NotThrowAsync();
await act.Should().CompleteWithinAsync(1.Seconds());

Func<Task<int>> getAnswer = () => sut.GetAnswerAsync();
await getAnswer.Should().CompleteWithinAsync(1.Seconds()).WithResult(42);

// single-statement style
using static FluentAssertions.FluentActions;
Invoking(() => Order.Create(null)).Should().Throw<ArgumentNullException>();
await Awaiting(() => sut.SaveAsync()).Should().NotThrowAsync();

// lazy iterators (yield return) only run when enumerated
sut.Enumerating(s => s.GetItems(-1)).Should().Throw<ArgumentException>();
```

`AggregateException`s are unwrapped automatically by `Throw`/`ThrowAsync`, but not by `ThrowExactly`.

## Events

```csharp
using var monitor = viewModel.Monitor();
viewModel.Name = "Jane";
monitor.Should().RaisePropertyChangeFor(vm => vm.Name);
monitor.Should().NotRaisePropertyChangeFor(vm => vm.Age);
monitor.Should().Raise(nameof(viewModel.Saved))
    .WithSender(viewModel)
    .WithArgs<SavedEventArgs>(e => e.Id == 42);
```

Not available on .NET Standard 2.0.

## Types, members and assemblies (architecture tests)

```csharp
typeof(OrderController).Should().BeDecoratedWith<AuthorizeAttribute>();
typeof(OrderController).Methods()
    .ThatReturn<IActionResult>()
    .ThatAreDecoratedWith<HttpPostAttribute>()
    .Should().BeDecoratedWith<ValidateAntiForgeryTokenAttribute>();
AllTypes.From(typeof(Order).Assembly)
    .ThatAreUnderNamespace("MyApp.Domain")
    .Should().NotBeDecoratedWith<SerializableAttribute>();
typeof(Order).Assembly.Should().NotReference(typeof(DbContext).Assembly);
```

## Other subjects

```csharp
// XML (System.Xml.Linq)
doc.Should().HaveRoot("config").And.HaveElement("settings").Which.Should().HaveAttribute("env", "prod");
element.Should().BeEquivalentTo(XElement.Parse("<item>value</item>"));

// JSON (System.Text.Json, .NET 6+)
node.Should().HaveProperty("name").Which.ToString().Should().Be("John");
node.Should().BeEquivalentTo(new { name = "John", tags = new[] { "a", "b" } });

// URIs
uri.Should().HaveScheme("https").And.HaveHost("api.example.com").And.HaveQueryParameter("page", "2");

// Streams
stream.Should().BeReadable().And.HaveLength(1024).And.HavePosition(0);

// Execution time (prefer generous limits; timing tests are flaky)
sut.ExecutionTimeOf(s => s.Calculate()).Should().BeLessThan(500.Milliseconds());
```

HTTP responses are not covered by Fluent Assertions itself. Use a companion package such as `FluentAssertions.Web`.

## Assertion scopes

```csharp
using (new AssertionScope())
{
    order.Number.Should().Be(42);
    order.Status.Should().Be(Status.Open);
}   // throws once here, listing every failure

using var scope = new AssertionScope("order");   // prefixes subject names: "Expected order/..."
string[] failures = scope.Discard();             // collect failures without throwing
```

## Global configuration

Configure once per test assembly, never inside individual tests (the state is static and tests run in parallel).

```csharp
// AssertionEngineInitializer lives in FluentAssertions.Extensibility
[assembly: AssertionEngineInitializer(typeof(FluentAssertionsSetup), nameof(FluentAssertionsSetup.Initialize))]

public static class FluentAssertionsSetup
{
    public static void Initialize()
    {
        AssertionConfiguration.Current.Equivalency.Modify(options => options
            .ComparingByValue<Money>()
            .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1.Milliseconds()))
            .WhenTypeIs<DateTime>());

        FluentAssertions.License.Accepted = true;   // hides the v8 license notice; only when you are entitled to it
    }
}
```

`[ModuleInitializer]` (.NET 5+) or MSTest's `[AssemblyInitialize]` also work.

## Custom assertions

Add a `Should()` extension that returns your own assertions class.
Mark helper methods with `[CustomAssertion]` so FA reports the caller's variable name, not the variable inside the helper.

```csharp
public static class DirectoryInfoExtensions
{
    public static DirectoryInfoAssertions Should(this DirectoryInfo instance) =>
        new(instance, AssertionChain.GetOrCreate());
}

public class DirectoryInfoAssertions(DirectoryInfo subject, AssertionChain chain)
    : ReferenceTypeAssertions<DirectoryInfo, DirectoryInfoAssertions>(subject, chain)
{
    protected override string Identifier => "directory";

    [CustomAssertion]
    public AndConstraint<DirectoryInfoAssertions> ContainFile(string filename,
        string because = "", params object[] becauseArgs)
    {
        chain
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.GetFiles().Any(f => f.Name == filename))
            .FailWith("Expected {context:directory} to contain {0}{reason}, but it did not.", filename);

        return new AndConstraint<DirectoryInfoAssertions>(this);
    }
}
```

- `{context:fallback}` becomes the caller's expression, `{reason}` the formatted `because`, `{0}`... formatted arguments.
- Return `AndWhichConstraint<TAssertions, TValue>` to support `.Which`.
- Use `.Given(() => ...)` for lazy projections and `.WithExpectation("Expected ...", chain => ...)` to share a message prefix.
- Value types derive from `ValueTypeAssertions<T, TSelf>` or `NullableValueTypeAssertions<T, TSelf>`.
- Add `[assembly: CustomAssertionsAssembly]` to treat a whole helper assembly as custom assertion code.
- Implement `IValueFormatter` and register it with `Formatter.AddFormatter(...)` to control how a type appears in failure messages.

## Pitfalls

- Subject names in messages ("Expected order.Number ...") come from the source file, so tests need debug symbols and
  must not use `PathMap`. Without them FA falls back to generic names like "value".
- `Be` on reference types uses `Equals`. For DTOs without value semantics use `BeEquivalentTo`.
- `BeEquivalentTo` on strings is a case-insensitive string comparison, not object-graph equivalency.
- `Throw<T>` matches derived types; use `ThrowExactly<T>` when the exact type matters.
- Always `await` async assertions (`ThrowAsync`, `NotThrowAsync`, `CompleteWithinAsync`); otherwise they never run.
- `OnlyContain` fails on an empty collection, but `AllSatisfy` passes on one. Add `NotBeEmpty()` if needed.
- `Equal` on collections checks order; `BeEquivalentTo` ignores it unless you add `WithStrictOrdering()`.
- Version 8 renamed `AssertionOptions` to `AssertionConfiguration.Current` and replaced `Execute.Assertion` with `AssertionChain`.

Full documentation: https://fluentassertions.com
