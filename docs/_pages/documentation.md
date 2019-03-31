---
title: Documentation
permalink: /documentation/
toc: true
layout: single
sidebar:
  nav: "sidebar"

---

## Coding by Example ##
As you may have noticed, the purpose of this open-source project is to not only be the best assertion framework in the .NET realm, but to also demonstrate high-quality code.
We heavily practice Test Driven Development and one of the promises TDD makes is that unit tests can be treated as your API's documentation.
So although you are free to go through the many examples here, please consider to analyze the many [unit tests](https://github.com/fluentassertions/fluentassertions/tree/master/Tests/Shared.Specs).

## Supported Test Frameworks ##
Fluent Assertions supports a lot of different unit testing frameworks. Just add a reference to the corresponding test framework assembly to the unit test project. Fluent Assertions will automatically find the corresponding assembly and use it for throwing the framework-specific exceptions.

If, for some unknown reason, Fluent Assertions fails to find the assembly, and you're running under .NET 4.5 or a .NET Standard 2.0 project, try specifying the framework explicitly using a configuration setting in the project’s app.config. If it cannot find any of the supported frameworks, it will fall back to using a custom `AssertFailedException` exception class.

```xml
<configuration>
  <appSettings>
    <!-- Supported values: nunit, xunit, mstest, mspec, mbunit and gallio -->
    <add key="FluentAssertions.TestFramework" value="nunit"/>
  </appSettings>
</configuration>
```
Just add NuGet package "FluentAssertions" to your test project.

## Subject Identification ##
Fluent Assertions can use the C# code of the unit test to extract the name of the subject and use that in the assertion failure. Consider for instance this statement:

```csharp
string username = "dennis";
username.Should().Be("jonas");
```

This will throw a test framework-specific exception with the following message:

`Expected username to be "jonas", but "dennis" differs near 'd' (index 0)`.`

The way this works is that Fluent Assertions will try to traverse the current stack trace to find the line and column numbers as well as the full path to the source file. Since it needs the debug symbols for that, this will require you to compile the unit tests in debug mode, even on your build servers. Also, since only .NET Standard 2.0 and the full .NET Framework support getting direct access to the current stack trace, subject identification only works for the platforms targeting those frameworks.

Now, if you've built your own extensions that use Fluent Assertions directly, you can tell it to skip that extension code while traversing the stack trace. Consider for example the customer assertion:

```csharp
    public class CustomerAssertions
    {
        private readonly Customer customer;

        public CustomerAssertions(Customer customer)
        {
            this.customer = customer;
        }

        [CustomAssertion]
        public void BeActive(string because = "", params object[] becauseArgs)
        {
            customer.Active.Should().BeTrue(because, becauseArgs);
        }
    }
```

And it's usage:

```
myClient.Should().BeActive("because we don't work with old clients");
```

Without the `[CustomAssertion]` attribute, Fluent Assertions would find the line that calls `Should().BeTrue()` and treat the `customer` variable as the subject-under-test (SUT). But by applying this attribute, it will ignore this invocation and instead find the SUT by looking for a call to `Should().BeActive()` and use the `myClient` variable instead.

## Basic assertions ##
The following assertions are available to all types of objects.

```csharp
object theObject = null;
theObject.Should().BeNull("because the value is null");
theObject.Should().NotBeNull();

theObject = "whatever";
theObject.Should().BeOfType<string>("because a {0} is set", typeof(string));
theObject.Should().BeOfType(typeof(string), "because a {0} is set", typeof(string));
```

Sometimes you might like to first assert that an object is of a certain type using `BeOfType` and then continue with additional assertions on the result of casting that object to the specified type.
You can do that by chaining those assertions onto the `Which` property like this.

```csharp
someObject.Should().BeOfType<Exception>()
  .Which.Message.Should().Be("Other Message");
```

To assert that two objects are equal (through their implementation of `Object.Equals`), use 

```csharp
string otherObject = "whatever";
theObject.Should().Be(otherObject, "because they have the same values");
theObject.Should().NotBe(otherObject);
```
 
If you want to make sure two objects are not just functionally equal but refer to the exact same object in memory, use the following two methods.

```csharp
theObject = otherObject;
theObject.Should().BeSameAs(otherObject);
theObject.Should().NotBeSameAs(otherObject);
```

Other examples of some general purpose assertions include

```csharp
var ex = new ArgumentException();
ex.Should().BeAssignableTo<Exception>("because it is an exception");
ex.Should().NotBeAssignableTo<DateTime>("because it is an exception");

var dummy = new Object();
dummy.Should().Match(d => (d.ToString() == "System.Object"));
dummy.Should().Match<string>(d => (d == "System.Object"));
dummy.Should().Match((string d) => (d == "System.Object"));
```

Some users requested the ability to easily downcast an object to one of its derived classes in a fluent way.

```csharp
customer.Animals.First().As<Human>().Height.Should().Be(178);
```

We’ve also added the possibility to assert that an object can be serialized and deserialized using the XML, binary or data contract formatters.

```csharp
theObject.Should().BeXmlSerializable();
theObject.Should().BeBinarySerializable();
theObject.Should().BeDataContractSerializable();
```

Internally, `BeBinarySerializable` uses the [Object graph comparison](#object-graph-comparison) API, so if you are in need of excluding certain properties from the comparison (for instance, because its backing field is `[NonSerializable]`, you can do this:

```csharp
theObject.Should().BeBinarySerializable<MyClass>(
    options => options.Excluding(s => s.SomeNonSerializableProperty));
```

Fluent Assertions has special support for `[Flags]` based enumerations, which allow you to do something like this:

```csharp
regexOptions.Should().HaveFlag(RegexOptions.Global);
regexOptions.Should().NotHaveFlag(RegexOptions.CaseInsensitive);
```

## Nullable types ##

```csharp
short? theShort = null;
theShort.Should().NotHaveValue();
theShort.Should().BeNull();

int? theInt = 3;
theInt.Should().HaveValue();
theInt.Should().NotBeNull();

DateTime? theDate = null;
theDate.Should().NotHaveValue();
theDate.Should().BeNull();
```
## Booleans ##

```csharp
bool theBoolean = false;
theBoolean.Should().BeFalse("it's set to false");

theBoolean = true;
theBoolean.Should().BeTrue();
theBoolean.Should().Be(otherBoolean);
```

Obviously the above assertions also work for nullable booleans, but if you really want to be make sure a boolean is either `true` or `false` and not `null`, you can use these methods.

```csharp
theBoolean.Should().NotBeFalse();
theBoolean.Should().NotBeTrue();
```

## Strings ##

For asserting whether a string is null, empty or contains whitespace only, you have a wide range of methods to your disposal.

```csharp
string theString = "";
theString.Should().NotBeNull();
theString.Should().BeNull();
theString.Should().BeEmpty();
theString.Should().NotBeEmpty("because the string is not empty");
theString.Should().HaveLength(0);
theString.Should().BeNullOrWhiteSpace(); // either null, empty or whitespace only
theString.Should().NotBeNullOrWhiteSpace();
```

Obviously you’ll find all the methods you would expect for string assertions.

```csharp
theString = "This is a String";
theString.Should().Be("This is a String");
theString.Should().NotBe("This is another String");
theString.Should().BeEquivalentTo("THIS IS A STRING");

theString.Should().BeOneOf(
    "That is a String",
    "This is a String",
);

theString.Should().Contain("is a");
theString.Should().ContainAll("should", "contain", "all", "of", "these");
theString.Should().ContainAny("any", "of", "these", "will", "do");
theString.Should().NotContain("is a");
theString.Should().NotContainAll("can", "contain", "some", "but", "not", "all");
theString.Should().NotContainAny("can't", "contain", "any", "of", "these");
theString.Should().ContainEquivalentOf("WE DONT CARE ABOUT THE CASING");
theString.Should().NotContainEquivalentOf("HeRe ThE CaSiNg Is IgNoReD As WeLl");

theString.Should().StartWith("This");
theString.Should().NotStartWith("This");
theString.Should().StartWithEquivalent("this");
theString.Should().NotStartWithEquivalentOf("this");

theString.Should().EndWith("a String");
theString.Should().NotEndWith("a String");
theString.Should().EndWithEquivalent("a string");
theString.Should().NotEndWithEquivalentOf("a string");
```

We even support wildcards.
For instance, if you would like to assert that some email address is correct, use this:

```csharp
emailAddress.Should().Match("*@*.com");
homeAddress.Should().NotMatch("*@*.com");
```

If the casing of the input string is irrelevant, use this:

```csharp
emailAddress.Should().MatchEquivalentOf("*@*.COM");
emailAddress.Should().NotMatchEquivalentOf("*@*.COM");
```

And if wildcards aren't enough for you, you can always use some regular expression magic:

```csharp
someString.Should().MatchRegex("h.*\\sworld.$");
subject.Should().NotMatchRegex(".*earth.*");
```

## Numeric types and everything else that implements IComparable<T\> ##

```csharp
int theInt = 5;
theInt.Should().BeGreaterOrEqualTo(5);
theInt.Should().BeGreaterOrEqualTo(3);
theInt.Should().BeGreaterThan(4);
theInt.Should().BeLessOrEqualTo(5);
theInt.Should().BeLessThan(6);
theInt.Should().BePositive();
theInt.Should().Be(5);
theInt.Should().NotBe(10);
theInt.Should().BeInRange(1, 10);
theInt.Should().NotBeInRange(6, 10);

theInt = 0;
//theInt.Should().BePositive(); => Expected positive value, but found 0
//theInt.Should().BeNegative(); => Expected negative value, but found 0

theInt = -8;
theInt.Should().BeNegative();
int? nullableInt = 3;
nullableInt.Should().Be(3);

double theDouble = 5.1;
theDouble.Should().BeGreaterThan(5);
byte theByte = 2;
theByte.Should().Be(2);
```

Notice that `Should().Be()` and `Should().NotBe()` are not available for floats and doubles. Floating point variables are inheritably inaccurate and should never be compared for equality. Instead, either use the `Should().BeInRange()` method or the following method specifically designed for floating point or `decimal` variables.

```csharp
float value = 3.1415927F;
value.Should().BeApproximately(3.14F, 0.01F);
```

This will verify that the value of the float is between 3.139 and 3.141.

Conversely, to assert that the value differs by an amount, you can do this.

```csharp
float value = 3.5F;
value.Should().NotBeApproximately(2.5F, 0.5F);
```

This will verify that the value of the float is not between 2.0 and 3.0.

To assert that a value matches one of the provided values, you can do this.

```csharp
value.Should().BeOneOf(new[] { 3, 6});
```

## Dates and times ##

For asserting a `DateTime` or a `DateTimeOffset` against various constraints, FA offers a bunch of methods that, provided that you use the extension methods for representing dates and times, really help to keep your assertions readable.

```csharp
var theDatetime = 1.March(2010).At(22, 15).AsLocal();

theDatetime.Should().Be(1.March(2010).At(22, 15));
theDatetime.Should().BeAfter(1.February(2010));
theDatetime.Should().BeBefore(2.March(2010));
theDatetime.Should().BeOnOrAfter(1.March(2010));
theDatetime.Should().BeOnOrBefore(1.March(2010));
theDatetime.Should().BeSameDateAs(1.March(2010).At(22, 16));
theDatetime.Should().BeIn(DateTimeKind.Local);

theDatetime.Should().NotBe(1.March(2010).At(22, 16));
theDatetime.Should().NotBeAfter(2.March(2010));
theDatetime.Should().NotBeBefore(1.February(2010));
theDatetime.Should().NotBeOnOrAfter(2.March(2010));
theDatetime.Should().NotBeOnOrBefore(1.February(2010));
theDatetime.Should().NotBeSameDateAs(2.March(2010));

theDatetime.Should().BeOneOf(
    1.March(2010).At(21, 15),
    1.March(2010).At(22, 15),
    1.March(2010).At(23, 15)
);
```

Notice how we use extension methods like `March`, `At` to represent dates in a more human readable form. There's a lot more like these, including `2000.Microseconds()`, `3.Nanoseconds` as well as methods like `AsLocal` and `AsUtc` to convert between representations. You can even do relative calculations like `2.Hours().Before(DateTime.Now)`.

If you only care about specific parts of a date or time, use the following assertion methods instead.

```csharp
theDatetime.Should().HaveDay(1);
theDatetime.Should().HaveMonth(3);
theDatetime.Should().HaveYear(2010);
theDatetime.Should().HaveHour(22);
theDatetime.Should().HaveMinute(15);
theDatetime.Should().HaveSecond(0);

theDatetime.Should().NotHaveDay(2);
theDatetime.Should().NotHaveMonth(4);
theDatetime.Should().NotHaveYear(2011);
theDatetime.Should().NotHaveHour(23);
theDatetime.Should().NotHaveMinute(16);
theDatetime.Should().NotHaveSecond(1);

var theDatetimeOffset = 1.March(2010).AsUtc().ToDateTimeOffset(2.Hours());

theDatetimeOffset.Should().HaveOffset(2);
theDatetimeOffset.Should().NotHaveOffset(3);
```

We've added a whole set of methods for asserting that the difference between two DateTime objects match a certain time frame.
All five methods support a Before and After extension method.

```csharp
theDatetime.Should().BeLessThan(10.Minutes()).Before(otherDatetime); // Equivalent to <
theDatetime.Should().BeWithin(2.Hours()).After(otherDatetime);       // Equivalent to <=
theDatetime.Should().BeMoreThan(1.Days()).Before(deadline);          // Equivalent to >
theDatetime.Should().BeAtLeast(2.Days()).Before(deliveryDate);       // Equivalent to >=
theDatetime.Should().BeExactly(24.Hours()).Before(appointment);      // Equivalent to ==
```

To assert that a date/time is (not) within a specified number of milliseconds from another date/time value you can use this method.

```csharp
theDatetime.Should().BeCloseTo(1.March(2010).At(22, 15), 2000); // 2000 milliseconds
theDatetime.Should().BeCloseTo(1.March(2010).At(22, 15));       // default is 20 milliseconds
theDatetime.Should().BeCloseTo(1.March(2010).At(22, 15), 2.Seconds());

theDatetime.Should().NotBeCloseTo(2.March(2010), 1.Hours());
```

This can be particularly useful if your database truncates date/time values.

## TimeSpans ##

FA also support a few dedicated methods that apply to (nullable) `TimeSpan` instances directly:

```csharp
var timeSpan = new TimeSpan(12, 59, 59);
timeSpan.Should().BePositive();
timeSpan.Should().BeNegative();
timeSpan.Should().Be(12.Hours());
timeSpan.Should().NotBe(1.Days());
timeSpan.Should().BeLessThan(someOtherTimeSpan);
timeSpan.Should().BeLessOrEqualTo(someOtherTimeSpan);
timeSpan.Should().BeGreaterThan(someOtherTimeSpan);
timeSpan.Should().BeGreaterOrEqualTo(someOtherTimeSpan);
```

Similarly to the [date and time assertions](#dates-and-times), `BeCloseTo` and `NotBeCloseTo` are also available for time spans:

```csharp
timeSpan.Should().BeCloseTo(new TimeSpan(13, 0, 0), 10.Ticks());
timeSpan.Should().NotBeCloseTo(new TimeSpan(14, 0, 0), 10.Ticks());
```

## Collections ##

A collection object in .NET is so versatile that the number of assertions on them require the same level of versatility.
Most, if not all, are so self-explanatory that we'll just list them here.

```csharp
IEnumerable collection = new[] { 1, 2, 5, 8 };

collection.Should().NotBeEmpty()
    .And.HaveCount(4)
    .And.ContainInOrder(new[] { 2, 5 })
    .And.ContainItemsAssignableTo<int>();

collection.Should().Equal(new List<int> { 1, 2, 5, 8 });
collection.Should().Equal(1, 2, 5, 8);
collection.Should().NotEqual(8, 2, 3, 5);
collection.Should().BeEquivalentTo(8, 2, 1, 5);
collection.Should().NotBeEquivalentTo(new[] {8, 2, 3, 5});

collection.Should().HaveCount(c => c > 3)
  .And.OnlyHaveUniqueItems();

collection.Should().HaveCountGreaterThan(3);
collection.Should().HaveCountGreaterOrEqualTo(4);
collection.Should().HaveCountLessOrEqualTo(4);
collection.Should().HaveCountLessThan(5);
collection.Should().NotHaveCount(3);
collection.Should().HaveSameCount(new[] { 6, 2, 0, 5 });
collection.Should().NotHaveSameCount(new[] { 6, 2, 0 });

collection.Should().StartWith(1);
collection.Should().StartWith(new[] { 1, 2 });
collection.Should().EndWith(8);
collection.Should().EndWith(new[] { 5, 8 });

collection.Should().BeSubsetOf(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, });

collection.Should().ContainSingle();
collection.Should().ContainSingle(x => x > 3);
collection.Should().Contain(8)
  .And.HaveElementAt(2, 5)
  .And.NotBeSubsetOf(new[] {11, 56});

collection.Should().Contain(x => x > 3);
collection.Should().Contain(collection, "", 5, 6); // It should contain the original items, plus 5 and 6.

collection.Should().OnlyContain(x => x < 10);
collection.Should().ContainItemsAssignableTo<int>();

collection.Should().ContainInOrder(new[] { 1, 5, 8 });

collection.Should().NotContain(82);
collection.Should().NotContain(new[] { 82, 83 });
collection.Should().NotContainNulls();
collection.Should().NotContain(x => x > 10);

object boxedValue = 2;
collection.Should().ContainEquivalentOf(boxedValue); // Compared by object equivalence

const int successor = 5;
const int predecessor = 5;
collection.Should().HaveElementPreceding(successor, element);
collection.Should().HaveElementSucceeding(predecessor, element);

collection.Should().BeEmpty();
collection.Should().BeNullOrEmpty();
collection.Should().NotBeNullOrEmpty();

IEnumerable otherCollection = new[] { 1, 2, 5, 8, 1 };
IEnumerable anotherCollection = new[] { 10, 20, 50, 80, 10 };
collection.Should().IntersectWith(otherCollection);
collection.Should().NotIntersectWith(anotherCollection);

collection.Should().BeInAscendingOrder();
collection.Should().BeInDescendingOrder();
collection.Should().NotBeAscendingInOrder();
collection.Should().NotBeDescendingInOrder();
```

The `collection.Should().ContainEquivalentOf(boxedValue)` asserts that a collection contains at least one object that is equivalent to the expected object. The comparison is governed by the same rules and options as the [Object graph comparison](#object-graph-comparison).

Those last two methods can be used to assert a collection contains items in ascending or descending order.
For simple types that might be fine, but for more complex types, it requires you to implement `IComparable`, something that doesn't make a whole lot of sense in all cases.
That's why we offer overloads that take an expression.

```csharp
collection.Should().BeInAscendingOrder(x => x.SomeProperty);
collection.Should().BeInDescendingOrder(x => x.SomeProperty);
```

When asserting on a projection of a collection the failure message will be less descriptive as it only knows about the projected value and not object containing that property.
```csharp
collection.Select(x => x.SomeProperty).Should().OnlyHaveUniqueItems();
```

Therefore we offer two overloads that takes an expression to select the property.
```csharp
collection.Should().OnlyHaveUniqueItems(x => x.SomeProperty);
collection.Should().NotContainNulls(x => x.SomeProperty);
```

Special overloads of `Equal()`, `StartWith` and `EndWith` take a lambda that is used for checking the two collections without relying on the type’s Equals() method.
Consider for instance two collections that contain some kind of domain entity persisted to a database and then reloaded.
Since the actual object instance is different, if you want to make sure a particular property was properly persisted, you usually do something like this:

```csharp
persistedCustomers.Select(c => c.Name).Should().Equal(customers.Select(c => c.Name);
persistedCustomers.Select(c => c.Name).Should().StartWith(customers.Select(c => c.Name);
persistedCustomers.Select(c => c.Name).Should().EndWith(customers.Select(c => c.Name);
```

With these new overloads, you can rewrite them into:

```csharp
persistedCustomers.Should().Equal(customers, (c1, c2) => c1.Name == c2.Name);
persistedCustomers.Should().StartWith(customers, (c1, c2) => c1.Name == c2.Name);
persistedCustomers.Should().EndWith(customers, (c1, c2) => c1.Name == c2.Name);
```

You can also perform assertions on all elements of a collection:
```csharp
IEnumerable<BaseType> collection = new BaseType[] { new DerivedType() };

collection.Should().AllBeAssignableTo<DerivedType>();
collection.Should().AllBeOfType<DerivedType>();
collection.Should().AllBeEquivalentTo(referenceObject);
```

## Dictionaries ##

You can apply Fluent Assertions to your generic dictionaries as well.
Of course you can assert any dictionary to be null or not null, and empty or not empty.
Like this:

```csharp
Dictionary<int, string> dictionary;
dictionary.Should().BeNull();

dictionary = new Dictionary<int, string>();
dictionary.Should().NotBeNull();
dictionary.Should().BeEmpty();
dictionary.Add(1, "first element");
dictionary.Should().NotBeEmpty();
```

You can also assert the equality of the entire dictionary, where the equality of the keys and values will be validated using their Equals implementation.
Like this:

```csharp
var dictionary1 = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" }
};

var dictionary2 = new Dictionary<int, string>
{
    { 1, "One" },
    { 2, "Two" }
};

var dictionary3 = new Dictionary<int, string>
{
    { 3, "Three" },
};

dictionary1.Should().Equal(dictionary2);
dictionary1.Should().NotEqual(dictionary3);
```

Or you can assert that the dictionary contains a certain key or value:

```csharp
dictionary.Should().ContainKey(1);
dictionary.Should().ContainKeys(1, 2);
dictionary.Should().NotContainKey(9);
dictionary.Should().NotContainKeys(9, 10);
dictionary.Should().ContainValue("One");
dictionary.Should().ContainValues("One", "Two");
dictionary.Should().NotContainValue("Nine");
dictionary.Should().NotContainValues("Nine", "Ten");
```

You can also assert that the dictionary has a certain number of items:

```csharp
dictionary.Should().HaveCount(2);
dictionary.Should().NotHaveCount(3);
```

And finally you can assert that the dictionary contains a specific key/value pair or not:

```csharp
KeyValuePair<int, string> item1 = new KeyValuePair<int, string>(1, "One");
KeyValuePair<int, string> item2 = new KeyValuePair<int, string>(2, "Two");

dictionary.Should().Contain(item1);
dictionary.Should().Contain(item1, item2);
dictionary.Should().Contain(2, "Two");
dictionary.Should().NotContain(item1);
dictionary.Should().NotContain(item1, item2);
dictionary.Should().NotContain(9, "Nine");
```

Chaining additional assertion is supported as well.

```csharp
dictionary.Should().ContainValue(myClass)
  .Which.SomeProperty.Should().BeGreaterThan(0);
```

## Guids ##

The assertions you can do on Guids are simple.
You can assert their equality to another Guid, or you can assert that a Guid is empty.

```csharp
Guid theGuid = Guid.NewGuid();
Guid sameGuid = theGuid;
Guid otherGuid = Guid.NewGuid();

theGuid.Should().Be(sameGuid);
theGuid.Should().NotBe(otherGuid);
theGuid.Should().NotBeEmpty();

Guid.Empty.Should().BeEmpty();
```

## Enums ##
With the standard `Should().Be()` method, Enums are compared using .NET's `Enum.Equals()` implementation.
This means that the Enums must be of the same type, and have the same underlying value.

## Exceptions ##

The following example verifies that the `Foo()` method throws an `InvalidOperationException` which `Message` property has a specific value.

```csharp
subject.Invoking(y => y.Foo("Hello"))
    .Should().Throw<InvalidOperationException>()
    .WithMessage("Hello is not allowed at this moment");
```

But if you, like me, prefer the arrange-act-assert syntax, you can also use an action in your act part.

```csharp
Action act = () => subject.Foo2("Hello");

act.Should().Throw<InvalidOperationException>()
    .WithInnerException<ArgumentException>()
    .WithMessage("whatever");
```

Notice that the example also verifies that the exception has a particular inner exception with a specific message. In fact, you can even check the individual properties of the exception instance using the And property.

```csharp
Action act = () => subject.Foo(null);

act.Should().Throw<ArgumentNullException>()
 .And.ParamName.Should().Be("message");
```

An alternative syntax for doing the same is by chaining one or more calls to the `Where()` method:

```csharp
Action act = () => subject.Foo(null);
act.Should().Throw<ArgumentNullException>().Where(e => e.Message.StartsWith("did"));
```

However, we discovered that testing the exception message for a substring is so common, that we changed the default behavior of `WithMessage` to support wildcard expressions and match in a case-insensitive way.

```csharp
Action act = () => subject.Foo(null);
act
  .Should().Throw<ArgumentNullException>()
  .WithMessage("?did*");
```

On the other hand, you may want to verify that no exceptions were thrown.

```csharp
Action act = () => subject.Foo("Hello");
act.Should().NotThrow();
```

I know that a unit test will fail anyhow if an exception was thrown, but this syntax returns a clearer description of the exception that was thrown and fits better to the AAA syntax.

If you want to verify that a specific exception is not thrown, and want to ignore others, you can do that using an overload:

```csharp
Action act = () => subject.Foo("Hello");
act.Should().NotThrow<InvalidOperationException>();
```

Sometimes you may want to retry an assertion until it either succeeds or a given time elapses. For instance, you could be testing a network service which should become available after a certain time, say, 10 seconds:
```csharp
Action act = () => service.IsReady().Should().BeTrue();
act.Should().NotThrowAfter(10.Seconds(), 100.Milliseconds());
```
The second argument of `NotThrowAfter` specifies the time that should pass before `act` is executed again after an execution which threw an exception.


If the method you are testing returns an `IEnumerable` or `IEnumerable<T>` and it uses the `yield` keyword to construct that collection, just calling the method will not cause the effect you expected because the real work is not done until you actually iterate over that collection. You can use the `Enumerating()` extension method to force enumerating the collection like this.

```csharp
Func<IEnumerable<char>> func = () => obj.SomeMethodThatUsesYield("blah");
func.Enumerating().Should().Throw<ArgumentException>();
```

You do have to use the `Func<T>` type instead of `Action<T>` then.

The exception throwing API follows the same rules as the `try`...`catch`...construction does.
In other words, if you're expecting a certain exception to be (not) thrown, and a more specific exception is thrown instead, it would still satisfy the assertion.
So throwing an `ApplicationException` when an `Exception` was expected will not fail the assertion.
However, if you really want to be explicit about the exact type of exception, you can use `ThrowExactly` and `WithInnerExceptionExactly`.

.NET 4.0 and later includes the `AggregateException` which is typically thrown by code that runs through the Parallel Task Library or using the new `async` keyword.
All of the above also works for exceptions that are aggregated, whether or not you are asserting on the actual `AggregateException` or any of its (nested) aggregated exceptions.

Talking about the `async` keyword, you can also verify that an asynchronously executed method throws or doesn't throw an exception:

```csharp
Func<Task> act = async () => { await asyncObject.ThrowAsync<ArgumentException>(); };
await act.Should().ThrowAsync<InvalidOperationException>();
await act.Should().NotThrowAsync();
act.Should().Throw<InvalidOperationException>();
act.Should().NotThrow();
```

Alternatively, you can use the `Awaiting` method like this:

```csharp
Func<Task> act = () => asyncObject.Awaiting(async x => await x.ThrowAsync<ArgumentException>());
act.Should().Throw<ArgumentException>();
```

Both give you the same results, so it's just a matter of personal preference.

As for synchronous methods, you can also check that an asynchronously executed method executes successfully after a given wait time using `NotThrowAfter`:

```csharp
Stopwatch watch = Stopwatch.StartNew();
Func<Task> act = async () =>
{
    if (watch.ElapsedMilliseconds <= 1000)
    {
        throw new ArgumentException("The wait time has not yet elapsed.");
    }

    await Task.CompletedTask;
};

act.Should().Throw<ArgumentException>();
await act.Should().NotThrowAfterAsync(2.Seconds(), 100.Milliseconds());
act.Should().NotThrowAfter(2.Seconds(), 100.Milliseconds());
```

If you prefer single-statement assertions, consider using the `FluentActions` static class, which has `Invoking`, `Awaiting`, and `Enumerating` methods:

```csharp
FluentActions.Invoking(() => MyClass.Create(null)).Should().Throw<ArgumentNullException>();
```

To make it even more concise:

```csharp
using static FluentAssertions.FluentActions;

...

Invoking(() => MyClass.Create(null)).Should().Throw<ArgumentNullException>();
```

## Object graph comparison ##

Consider the class `Order` and its wire-transfer equivalent `OrderDto` (a so-called [DTO](http://en.wikipedia.org/wiki/Data_transfer_object)).
Suppose also that an order has one or more `Product`s and an associated `Customer`.
Coincidentally, the `OrderDto` will have one or more `ProductDto`s and a corresponding `CustomerDto`.
You may want to make sure that all exposed members of all the objects in the `OrderDto` object graph match the equally named members of the `Order` object graph.

You may assert the structural equality of two object graphs with `Should().BeEquivalentTo()`:
```csharp
orderDto.Should().BeEquivalentTo(order);
```

### Recursion ###

The comparison is recursive by default.
To avoid infinite recursion, Fluent Assertions will recurse up to 10 levels deep by default, but if you want to force it to go as deep as possible, use the `AllowingInfiniteRecursion` option.
On the other hand, if you want to disable recursion, just use this option:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.ExcludingNestedObjects());
```

### Value Types ###

To determine whether Fluent Assertions should recurs into an object's properties or fields, it needs to understand what types have value semantics and what types should be treated as reference types. The default behavior is to treat every type that overrides `Object.Equals` as on object that was designed to have value semantics. Unfortunately, anonymous types and tuples also override this method, but because we tend to use them quite often in equivalency comparison, we always compare them by their properties.

You can easily override this by using the `ComparingByValue<T>` or `ComparingByMembers<T>` options for individual assertions:

```csharp
subject.Should().BeEquivalentTo(expected,
   options => options.ComparingByValue<IPAddress>());
```

Or  do the same using the global options:

```csharp
AssertionOptions.AssertEquivalencyUsing(options => options
    .ComparingByValue<DirectoryInfo`());
```

### Auto-Conversion ###
In the past, Fluent Assertions would attempt to convert the value of a property of the subject-under-test to the type of the corresponding property on the expectation. But a lot of people complained about this behavior where a string property representing a date and time would magically match a `DateTime` property. As of 5.0, this conversion will no longer happen. However, you can still adjust the assertion by using the `WithAutoConversion` or `WithAutoConversionFor` options:

```csharp
subject.Should().BeEquivalentTo(expectation, options => options
    .WithAutoConversionFor(x => x.SelectedMemberPath.Contains("Birthdate")));
```

### Compile-time types vs. run-time types ###

By default, Fluent Assertions respects an object's or member's declared (compile-time) type when selecting members to process during a recursive comparison.
That is to say if the subject is a `OrderDto` but the variable it is assigned to has type `Dto` only the members defined by the latter class would be considered when comparing the object to the `order` variable.
This behavior can be configured and you can choose to use run-time types if you prefer:

```csharp
Dto orderDto = new OrderDto();

// Use runtime type information of orderDto
orderDto.Should().BeEquivalentTo(order, options => 
    options.RespectingRuntimeTypes());

// Use declared type information of orderDto
orderDto.Should().BeEquivalentTo(order, options => 
    options.RespectingDeclaredTypes());
```

One exception to this rule is when the declared type is `object`.
Since `object` doesn't expose any properties, it makes no sense to respect the declared type.
So if the subject or member's type is `object`, it will use the run-time type for that node in the graph. This will also work better with (multidimensional) arrays.

### Matching Members ###

All public members of the `Order` object must be available on the `OrderDto` having the same name. If any members are missing, an exception will be thrown.
However, you may customize this behavior.
For instance, if you want to include only the members both object graphs have:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.ExcludingMissingMembers());
```

### Selecting Members ###

If you want to exclude certain (potentially deeply nested) individual members using the `Excluding()` method:

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => o.Customer.Name));
```

The `Excluding()` method on the options object also takes a lambda expression that offers a bit more flexibility for deciding what member to exclude:

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .Excluding(ctx => ctx.SelectedMemberPath == "Level.Level.Text"));
```

Maybe far-fetched, but you may even decide to exclude a member on a particular nested object by its index.

```csharp
orderDto.Should().BeEquivalentTo(order, options => 
    options.Excluding(o => o.Products[1].Status));
```

Of course, `Excluding()` and `ExcludingMissingMembers()` can be combined.

You can also take a different approach and explicitly tell Fluent Assertions which members to include. You can directly specify a property expression or use a predicate that acts on the provided `ISubjectInfo`.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Including(o => o.OrderNumber)
    .Including(pi => pi.SelectedMemberPath.EndsWith("Date"));
```

### Including properties and/or fields ###

You may also configure member inclusion more broadly.
Barring other configuration, Fluent Assertions will include all `public` properties and fields.
This behavior can be changed:

```csharp
// Include Fields
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingFields();

// Include Properties
orderDto.Should().BeEquivalentTo(order, options => options
    .IncludingProperties();

// Exclude Fields
orderDto.Should().BeEquivalentTo(order, options => options
    .ExcludingFields();

// Exclude Properties
orderDto.Should().BeEquivalentTo(order, options => options
    .ExcludingProperties();
```

This configuration affects the initial inclusion of members and happens before any `Exclude`s or other `IMemberSelectionRule`s.
This configuration also affects matching.
For example, that if properties are excluded, properties will not be inspected when looking for a match on the expected object.

### Equivalency Comparison Behavior ###
In addition to influencing the members that are including in the comparison, you can also override the actual assertion operation that is executed on a particular member.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
    .When(info => info.SelectedMemberPath.EndsWith("Date")));
```

If you want to do this for all members of a certain type, you can shorten the above call like this.

```csharp
orderDto.Should().BeEquivalentTo(order, options => options 
    .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 1000))
    .WhenTypeIs<DateTime>();
```

### Enums ###

By default, ``Should().BeEquivalentTo()`` compares `Enum` members by the enum's underlying numeric value.
An option to compare an `Enum` only by name is also available, using the following configuration :

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.ComparingEnumsByName());
```

### Collections and Dictionaries ###
Considering our running example, you could use the following against a collection of `OrderDto`s: 

```csharp
orderDtos.Should().BeEquivalentTo(orders, options => options.Excluding(o => o.Customer.Name));
```

You can also assert that all instances of `OrderDto` are structurally equal to a single object:

```csharp
orderDtos.Should().AllBeEquivalentTo(singleOrder);
```

### Ordering ###

Fluent Assertions will, by default, ignore the order of the items in the collections, regardless of whether the collection is at the root of the object graph or tucked away in a nested property or field.
If the order is important, you can override the default behavior with the following option:

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.WithStrictOrdering());
```

You can even tell FA to use strict ordering only for a particular collection or dictionary member, similar to how you exclude certain members:

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.WithStrictOrderingFor(s => s.Products));
```

And you can tell FA to generally use strict ordering but ignore it for a particular collection or dictionary member:

```csharp
orderDto.Should().BeEquivalentTo(expectation, options => options.WithStrictOrdering().WithoutStrictOrderingFor(s => s.Products));
```

In case you chose to use strict ordering by default you can still configure non-strict ordering in specific tests:
```csharp
AssertionOptions.AssertEquivalencyUsing(options => options.WithStrictOrdering());

orderDto.Should().BeEquivalentTo(expectation, options => options.WithoutStrictOrdering());
```

**Notice:** For performance reasons, collections of bytes are compared in exact order. This is even true when applying `WithoutStrictOrdering()`.

### Diagnostics
`Should().BeEquivalentTo` is a very powerful feature, and one of the unique selling points of Fluent Assertions. But sometimes it can be a bit overwhelming, especially if some assertion fails under unexpected conditions. To help you understand how Fluent Assertions compared two (collections of) object graphs, the failure message will always include the relevant configuration settings:

```txt
Xunit.Sdk.XunitException
Expected item[0] to be 0x06, but found 0x01.
Expected item[1] to be 0x05, but found 0x02.
Expected item[2] to be 0x04, but found 0x03.
Expected item[3] to be 0x03, but found 0x04.
Expected item[4] to be 0x02, but found 0x05.
Expected item[5] to be 0x01, but found 0x06.

With configuration:
- Use declared types and members
- Compare enums by value
- Include all non-private properties
- Include all non-private fields
- Match member by name (or throw)
- Be strict about the order of items in byte arrays
```

However, sometimes that's not enough. For those scenarios where you need to understand a but more, you can add the `WithTracing` option. When added to the assertion call, it would extend the above output with something like this:

```txt
With trace:
  Structurally comparing System.Object[] and expectation System.Byte[] at root
  {
    Strictly comparing expectation 6 at root to item with index 0 in System.Object[]
    {
      Treating item[0] as a value type
    }
    Strictly comparing expectation 5 at root to item with index 1 in System.Object[]
    {
      Treating item[1] as a value type
    }
    Strictly comparing expectation 4 at root to item with index 2 in System.Object[]
    {
      Treating item[2] as a value type
    }
    Strictly comparing expectation 3 at root to item with index 3 in System.Object[]
    {
      Treating item[3] as a value type
    }
    Strictly comparing expectation 2 at root to item with index 4 in System.Object[]
    {
      Treating item[4] as a value type
    }
    Strictly comparing expectation 1 at root to item with index 5 in System.Object[]
    {
      Treating item[5] as a value type
    }
  }
  ```

  By default, the trace is displayed only when an assertion fails as part of the message.
  If the trace is needed for the successful assertion you can catch it like this:

  ```csharp
object object1 = [...];
object object2 = [...];
var traceWriter = new StringBuilderTraceWriter();            
object1.Should().BeEquivalentTo(object2, opts => opts.WithTracing(traceWriter));             
string trace = traceWriter.ToString();
  ```

Alternatively, you could write your own implementation of `ITraceWriter` for special purposes e.g. writing to a file.

### Global Configuration ###
Even though the structural equivalency API is pretty flexible, you might want to change some of these options on a global scale.
This is where the static class `AssertionOptions` comes into play.
For instance, to always compare enumerations by name, use the following statement:

```csharp
AssertionOptions.AssertEquivalencyUsing(options => 
   options.ComparingEnumsByValue);
``` 

All the options available to an individual call to `Should().BeEquivalentTo` are supported, with the exception of some of the overloads that are specific to the type of the subject (for obvious reasons).

## Event Monitoring ##

Fluent Assertions has a set of extensions that allow you to verify that an object raised a particular event.
Before you can invoke the assertion extensions, you must first tell Fluent Assertions that you want to monitor the object:

```csharp
var subject = new EditCustomerViewModel();
using (var monitoredSubject = subject.Monitor())
{
    subject.Foo();
    monitoredSubject.Should().Raise("NameChangedEvent");
}
```

Notice that Fluent Assertions will keep monitoring the `subject` for as long as the `using` block lasts.

Assuming that we’re dealing with a MVVM implementation, you might want to verify that it raised its `PropertyChanged` event for a particular property:

```csharp
monitoredSubject
  .Should().Raise("PropertyChanged")
  .WithSender(subject)
  .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "SomeProperty");
```

Notice that `WithSender()` verifies that all occurrences had its sender argument set to the specified object.
`WithArgs()` just verifies that at least one occurrence had a matching `EventArgs` object.
In other words, event monitoring only works for events that comply with the standard two-argument sender/args .NET pattern.

Since verifying for `PropertyChanged` events is so common, I’ve included a specialized shortcut to the example above:

```csharp
subject.Should().Raise().PropertyChangeFor(x => x.SomeProperty);
```

You can also do the opposite; asserting that a particular event was not raised.

```csharp
subject.Should().NotRaisePropertyChangeFor(x => x.SomeProperty);
```

Or...

```csharp
subject.Should().NotRaise("SomeOtherEvent");
```

There's also a generic version of `Monitor()`.
It is used to limit which events you want to listen to.
You do that by providing a type which defines the events.

```csharp
var subject = new ClassWithManyEvents();
using (var monitor = subject.Monitor<IInterfaceWithFewEvents>();
{
    
}
```

This generic version of `Monitor()` is also very useful if you wish to monitor events of a dynamically generated class using `System.Reflection.Emit`.
Since events are dynamically generated and are not present in parent class non-generic version of `Monitor()` will not find the events.
This way you can tell the event monitor which interface was implemented in the generated class.

```csharp
POCOClass subject = EmitViewModelFromPOCOClass();

using (var monitor = subject.Monitor<ISomeInterface>())
{
    // POCO class doesn't have INotifyPropertyChanged implemented
    monitor.Should().Raise("SomeEvent");
}
```

The object returned by `Monitor` exposes a method named `GetEventRecorder` as well as the properties `MonitoredEvents` and `OccurredEvents` that you can use to directly interact with the monitor, e.g. to create your own extensions. For example:

```csharp
    var eventSource = new ClassThatRaisesEventsItself();
    using (var monitor = eventSource.Monitor<IEventRaisingInterface>())
    {
        EventMetadata[] metadata = monitor.MonitoredEvents;

        metadata.Should().BeEquivalentTo(new[]
        {
            new
            {
                EventName = nameof(IEventRaisingInterface.InterfaceEvent),
                HandlerType = typeof(EventHandler)
            }
        });
    }
```

## Type, Method, and Property assertions ##

We have added a number of assertions on types and on methods and properties of types.
These are rather technical assertions and, although we like our unit tests to read as functional specifications for the application, we still see a use for assertions on the members of a class.
For example when you use policy injection on your classes and require its methods to be virtual.
Forgetting to make a method virtual will avoid the policy injection mechanism from creating a proxy for it, but you will only notice the consequences at runtime.
Therefore it can be useful to create a unit test that asserts such requirements on your classes.
Some examples.

```csharp
typeof(MyPresentationModel).Should().BeDecoratedWith<SomeAttribute>();

typeof(MyPresentationModel)
  .Should().BeDecoratedWithOrInherit<SomeInheritedOrDirectlyDecoratedAttribute>();

typeof(MyPresentationModel).Should().NotBeDecoratedWith<SomeAttribute>();
typeof(MyPresentationModel)
  .Should().NotBeDecoratedWithOrInherit<SomeInheritedOrDirectlyDecoratedAttribute>();

typeof(MyBaseClass).Should().BeAbstract();
typeof(InjectedClass).Should().NotBeStatic();

MethodInfo method = GetMethod();
method.Should().BeVirtual();

PropertyInfo property = GetSomeProperty();
property.Should().BeVirtual()
  .And.BeDecoratedWith<SomeAttribute>();
```

You can also perform assertions on multiple methods or properties in a certain type by using the `Methods()` or `Properties()` extension methods and some optional filtering methods.
Like this:

```csharp
typeof(MyPresentationModel).Methods()
  .ThatArePublicOrInternal 
  .ThatReturnVoid
  .Should()
  .BeVirtual("because this is required to intercept exceptions")
    .And.BeWritable()
    .And.BeAsync();

typeof(MyController).Methods()
  .ThatDoNotReturn<ActionResult>()
  .ThatAreNotDecoratedWith<HttpPostAttribute>()
  .Should().NotBeVirtual()
    .And.NotBeAsync()
    .And.NotReturnVoid()
    .And.NotReturn<ActionResult>();

typeof(MyController).Methods()
  .ThatReturn<ActionResult>()
  .ThatAreDecoratedWith<HttpPostAttribute>()
  .Should()
  .BeDecoratedWith<ValidateAntiForgeryTokenAttribute>(
    "because all Actions with HttpPost require ValidateAntiForgeryToken");
```

If you also want to assert that an attribute has a specific property value, use this syntax.

```csharp
typeWithAttribute.Should()
  .BeDecoratedWith<DummyClassAttribute>(a => ((a.Name == "Unexpected") && a.IsEnabled));
```

You can assert methods or properties from all types in an assembly that apply to certain filters, like this:

```csharp
var types = typeof(ClassWithSomeAttribute).Assembly.Types()
  .ThatAreDecoratedWith<SomeAttribute>()
  .ThatImplement<ISomeInterface>()
  .ThatAreInNamespace("Internal.Main.Test");

var properties = types.Properties().ThatArePublicOrInternal;
properties.Should().BeVirtual();
```

Alternatively you can use this more fluent syntax instead.

```csharp
AllTypes.From(assembly)
  .ThatAreDecoratedWith<SomeAttribute>()
  .ThatImplement<ISomeInterface>()
  .ThatDeriveFrom<IDisposable>()
  .ThatAreUnderNamespace("Internal.Main.Test");

AllTypes.From(assembly)
  .ThatAreNotDecoratedWith<SomeAttribute>()
  .ThatDoNotImplement<ISomeInterface>()
  .ThatDoNotDeriveFrom<IDisposable>()
  .ThatAreNotUnderNamespace("Internal.Main")
  .ThatAreNotInNamespace("Internal.Main.Test");
```

There are so many possibilities and specialized methods that none of these examples do them good. Check out the `TypeAssertionSpecs.cs` from the source for more examples. 

## Assembly References ##
If you're running .NET 4.5 or .NET Standard 2.0, you have access to methods to assert an assembly does or does not reference another assembly.
These are typically used to enforce layers within an application, such as for example, asserting the web layer does not reference the data layer.
To assert the references, use the the following syntax:

```csharp 
assembly.Should().Reference(otherAssembly);
assembly.Should().NotReference(otherAssembly);
```

## XML classes ##

Fluent Assertions has support for assertions on several of the LINQ-to-XML classes:

```csharp
xDocument.Should().HaveRoot("configuration");
xDocument.Should().HaveElement("settings");

xElement.Should().HaveValue("36");
xElement.Should().HaveAttribute("age", "36");
xElement.Should().HaveElement("address");
xElement.Should().HaveElementWithNamespace("address", "http://www.example.com/2012/test");

xElement.Should().HaveInnerText("some textanother textmore text");
```

Those two last assertions also support `XName` parameters:

```csharp
xElement.Should().HaveAttribute(XName.Get("age", "http://www.example.com/2012/test"), "36");
xElement.Should().HaveElement(XName.Get("address", "http://www.example.com/2012/test"));

xAttribute.Should().HaveValue("Amsterdam");
```

You can also perform a deep comparison between two elements like this.

```csharp
xDocument.Should().BeEquivalentTo(XDocument.Parse("<configuration><item>value</item></configuration>"));
xElement.Should().BeEquivalentTo(XElement.Parse("<item>value</item>"));
```

Chaining additional assertions on top of a particular (root) element is possible through this syntax.

```csharp
xDocument.Should().HaveElement("child")
  .Which.Should().BeOfType<XElement>()
    .And.HaveAttribute("attr", "1");
```

## Execution Time ##

New in version 1.4 is a method to assert that the execution time of particular method or action does not exceed a predefined value.
To verify the execution time of a method, use the following syntax:

```csharp
public class SomePotentiallyVerySlowClass
{
    public void ExpensiveMethod()
    {
        for (short i = 0; i < short.MaxValue; i++)
        {
            string tmp = " ";
            if (!string.IsNullOrEmpty(tmp))
            {
                tmp += " ";
            }
        }
    }
}
var subject = new SomePotentiallyVerySlowClass();
subject.ExecutionTimeOf(s => s.ExpensiveMethod()).Should().BeLessOrEqualTo(500.Milliseconds());
```

Alternatively, to verify the execution time of an arbitrary action, use this syntax:

```csharp
Action someAction = () => Thread.Sleep(100);
someAction.ExecutionTime().Should().BeLessOrEqualTo(200.Milliseconds());
```

The supported assertions on `ExecutionTime()` are a subset of those found for `TimeSpan`s, namely:
```
someAction.ExecutionTime().Should().BeLessOrEqualTo(200.Milliseconds());
someAction.ExecutionTime().Should().BeLessThan(200.Milliseconds());
someAction.ExecutionTime().Should().BeGreaterThan(100.Milliseconds());
someAction.ExecutionTime().Should().BeGreaterOrEqualTo(100.Milliseconds());
someAction.ExecutionTime().Should().BeCloseTo(150.Milliseconds(), 50.Milliseconds());
```


## Assertion Scope ##

You can batch multiple assertions into an `AssertionScope` so that FluentAssertions throws one exception at the end of the scope with all failures.

E.g.

```csharp
using (new AssertionScope())
{
    5.Should().Be(10);
    "Actual".Should().Be("Expected");
}
```

The above will batch the two failures, and throw an exception at the point of disposing the `AssertionScope` displaying both errors.

E.g. Exception thrown at point of dispose contains:

    Expected value to be 10, but found 5.
    Expected string to be "Expected" with a length of 8, but "Actual" has a length of 6, differs near "Act" (index 0).
    
        at........
        
For more information take a look at the [AssertionScopeSpecs.cs](https://github.com/fluentassertions/fluentassertions/blob/master/Tests/Shared.Specs/AssertionScopeSpecs.cs) in Unit Tests.

