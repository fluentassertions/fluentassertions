Fluent Assertions
================

Fluent Assertions is a set of .NET extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style test. We currently use it in all our internal and client projects, and it is used in many open-source projects. It runs on the following frameworks:

* .NET 3.5, 4.0 and 4.5
* Windows Store for Windows 8
* Silverlight 4 and 5
* Windows Phone 7.5 and 8. 
 
It supports the following unit test frameworks:

* MSTest (Visual Studio 2010, Visual Studio 2012 Update 2 and Visual Studio 2013)
* [NUnit](http://www.nunit.org/)
* [XUnit](http://xunit.codeplex.com/)
* [MBUnit](http://code.google.com/p/mb-unit/)
* [Gallio](http://code.google.com/p/mb-unit/)
* [NSpec](http://nspec.org/)
* [MSpec](https://github.com/machine/machine.specifications)

The releases are available as Zipped downloads from the [Releases](https://github.com/dennisdoomen/fluentassertions/releases) section or by getting the corresponding [NuGet package](https://www.nuget.org/packages/FluentAssertions).

Why?
----
Nothing is more annoying then a unit test that fails without clearly explaining why. More than often, you need to set a breakpoint and start up the debugger to be able to figure out what went wrong. Jeremy D. Miller once gave the advice to ["keep out of the debugger hell"](http://codebetter.com/jeremymiller/2005/08/18/testing-granularity-feedback-cycles-and-holistic-development/) and I can only agree with that.

For instance, only test a single condition per test case. If you don't, and the first condition fails, the test engine will not even try to test the other conditions. But if any of the others fail, you'll be on your own to figure out which one. I often run into this problem when developers try to combine multiple related tests that test a member using different parameters into one test case. If you really need to do that, consider using a parameterized test that is being called by several clearly named test cases.

That’s why we designed Fluent Assertions to help you in this area. Not only by using clearly named assertion methods, but also by making sure the failure message provides as much information as possible. Consider this example:

```csharp
"1234567890".Should().Be("0987654321");
```

This will be reported as: 
    
	Expected string to be
	"0987654321", but
	"1234567890" differs near "123" (index 0).

The fact that both strings are displayed on a separate line is on purpose and happens if any of them is longer than 8 characters. However, if that's not enough, all assertion methods take an optional formatted reason with placeholders, similarly to String.Format, that you can use to enrich the failure message. For instance, the assertion

```csharp
new[] { 1, 2, 3 }.Should().Contain(item => item > 3, "at least {0} item should be larger than 3", 1);
```

will fail with: 

	Collection {1, 2, 3} should have an item matching (item > 3) because at least 1 item should be larger than 3.

Examples
--------
To verify that a string begins, ends and contains a particular phrase.

```csharp
string actual = "ABCDEFGHI";
actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
```

To verify that a collection contains a specified number of elements and that all elements match a predicate.

```csharp
IEnumerable collection = new[] { 1, 2, 3 };
collection.Should().HaveCount(4, "because we thought we put three items in the collection"))collection.Should().Contain(i => i > 0);
```

The nice thing about the second failing example is that it will throw an exception with the message 

	"Expected <4> items because we thought we put three items in the collection, but found <3>." 
To verify that a particular business rule is enforced using exceptions.

```csharp
var recipe = new RecipeBuilder()
   .With(new IngredientBuilder().For("Milk").WithQuantity(200, Unit.Milliliters))
   .Build();

Action action = () => recipe.AddIngredient("Milk", 100, Unit.Spoon);

action
   .ShouldThrow<RuleViolationException>()
   .WithMessage("change the unit of an existing ingredient", ComparisonMode.Substring)
   .And.Violations.Should().Contain(BusinessRule.CannotChangeIngredientQuanity);
```

What’s new?
-----------

**December 30th, 2013**
Another nice update in the form of v2.2. Read all about it in the [Releases](https://github.com/dennisdoomen/fluentassertions/releases/tag/v2.2) section.

**August 28th, 2013**
Release 2.1 is a fact and introduces a few big improvements on the equivalency test. Read all about it [in this blog post](http://www.dennisdoomen.net/2013/08/it-took-almost-year-but-fluent.html). 

**May 20th, 2013**
Through a contribution on GitHub, Ufuk Hacıoğulları has added support for MonoTouch.  
  
About versioning
----------------
The version numbers of Fluent Assertions releases comply to the Semantic Versioning scheme. In other words, release 1.4.0 only adds backwards-compatible functionality and bug fixes compared to 1.3.0. Release 1.4.1 should only include bug fixes. And if we ever introduce breaking changes, the number increased to 2.0.0.

What do you need to compile the solution?
-----------------------------
* Visual Studio 2013
* Windows 8.1
* The Windows Phone 8 SDK
* You need to import the test certificate by running `InstallPfx.bat` through an elevated command prompt. 

Who are we?
-----------
We are a bunch of developers working for Aviva Solutions who highly value software quality, in particular  
- [Dennis Doomen](https://twitter.com/ddoomen)  
- [Martin Opdam](https://twitter.com/mpopdam) 

Notable contributors from the last couple of months include
- [Adam Voss](https://github.com/vossad01)

The [Xamarin](https://github.com/onovotny/fluentassertions) version has been built by
- [Oren Novotny](https://twitter.com/onovotny)

If you have any comments or suggestions, please let us know via [twitter](https://twitter.com/search?q=fluentassertions&src=typd), through the [issues](https://github.com/dennisdoomen/FluentAssertions/issues) page, or through [StackOverflow](http://stackoverflow.com/questions/tagged/fluent-assertions).
