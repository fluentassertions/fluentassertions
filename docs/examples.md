---
title: Examples
layout: page
---

To verify that a string begins, ends and contains a particular phrase.

```c#
string actual = "ABCDEFGHI";
actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);
```

To verify that a collection contains a specified number of elements and that all elements match a predicate.

```c#
IEnumerable collection = new[] { 1, 2, 3 };
collection.Should().HaveCount(4, "because we thought we put four items in the collection"))
```

The nice thing about the second failing example is that it will throw an exception with the message

> "Expected <4> items because we thought we put four items in the collection, but found <3>."

To verify that a particular business rule is enforced using exceptions.

```c#
var recipe = new RecipeBuilder()
                    .With(new IngredientBuilder().For("Milk").WithQuantity(200, Unit.Milliliters))
                    .Build();
Action action = () => recipe.AddIngredient("Milk", 100, Unit.Spoon);
action
                    .ShouldThrow<RuleViolationException>()
                    .WithMessage("change the unit of an existing ingredient", ComparisonMode.Substring)
                    .And.Violations.Should().Contain(BusinessRule.CannotChangeIngredientQuanity);
```

One neat feature is the ability to chain a specific assertion on top of an assertion that acts on a collection or graph of objects.

```c#
dictionary.Should().ContainValue(myClass).Which.SomeProperty.Should().BeGreaterThan(0);
someObject.Should().BeOfType<Exception>().Which.Message.Should().Be("Other Message");
xDocument.Should().HaveElement("child").Which.Should().BeOfType<XElement>().And.HaveAttribute("attr", "1");
```

I've run into quite a few of these scenarios in which this chaining would make the unit test a lot easier to read.

## MSTest Migration
{:.no_toc}

The examples below show how you might write equivalent MSTest assertions using Fluent Assertions including the failure message from each case.
We think this is both a useful migration guide and a convincing argument for switching.

If you see something missing, please consider submitting a pull request.

* TOC
{:toc}

{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" caption="Assert"            examples=site.data.mstest-migration.assert %}
{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" caption="CollectionAssert"  examples=site.data.mstest-migration.collectionAssert %}
{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" caption="StringAssert"      examples=site.data.mstest-migration.stringAssert %}
{% include assertion-comparison.html header1="MSTest" header2="Fluent Assertions" caption="Exceptions"        examples=site.data.mstest-migration.exceptions %}