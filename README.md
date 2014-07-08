Fluent Assertions
================

Fluent Assertions is a set of .NET extension methods that allow you to more naturally specify the expected outcome of a TDD or BDD-style test. We currently use it in all our internal and client projects, and it is used in many open-source projects. It runs on .NET 3.5, 4.0 and 4.5 (Desktop and Windows Store), Silverlight 4 and 5 and Windows Phone 7.5. And it supports the unit test frameworks NUnit, XUnit, MBUnit, Gallio and MSpec.

Why?
----
Nothing is more annoying than a unit test that fails without clearly explaining why. More than often, you need to set a breakpoint and start up the debugger to be able to figure out what went wrong. Jeremy D. Miller once gave the advice to "keep out of the debugger hell" and I can only agree with that.

For instance, only test a single condition per test case. If you don't, and the first condition fails, the test engine will not even try to test the other conditions. But if any of the others fail, you'll be on your own to figure out which one. I often run into this problem when developers try to combine multiple related tests that test a member using different parameters into one test case. If you really need to do that, consider using a parameterized test that is being called by several clearly named test cases.

That’s why we designed Fluent Assertions to help you in this area. Not only by using clearly named assertion methods, but also by making sure the failure message provides as much information as possible. Consider this example:

    "1234567890".Should().Be("0987654321");

This will be reported as: 
    
clip_image001[6] 

The fact that both strings are displayed on a separate line is on purpose and happens if any of them is longer than 8 characters. However, if that's not enough, all assertion methods take an optional formatted reason with placeholders, similarly to String.Format, that you can use to enrich the failure message. For instance, the assertion

	new[] { 1, 2, 3 }.Should().Contain(item => item > 3, "at least {0} item should be larger than 3", 1);

will fail with: 

clip_image002[6]

Examples
--------
To verify that a string begins, ends and contains a particular phrase.

	string actual = "ABCDEFGHI";
	actual.Should().StartWith("AB").And.EndWith("HI").And.Contain("EF").And.HaveLength(9);

To verify that a collection contains a specified number of elements and that all elements match a predicate.

	IEnumerable collection = new[] { 1, 2, 3 };
	collection.Should().HaveCount(4, "because we thought we put three items in the collection"))collection.Should().Contain(i => i > 0);

The nice thing about the second failing example is that it will throw an exception with the message 

	"Expected <4> items because we thought we put three items in the collection, but found <3>." 
To verify that a particular business rule is enforced using exceptions.

	var recipe = new RecipeBuilder()
	   .With(new IngredientBuilder().For("Milk").WithQuantity(200, Unit.Milliliters))
	   .Build();
	
	Action action = () => recipe.AddIngredient("Milk", 100, Unit.Spoon);
	
	action
	   .ShouldThrow<RuleViolationException>()
	   .WithMessage("change the unit of an existing ingredient", ComparisonMode.Substring)
	   .And.Violations.Should().Contain(BusinessRule.CannotChangeIngredientQuanity);

What’s new?
-----------

**May 20th, 2013**      
Through a contribution on GitHub, Ufuk Hacıoğulları has added support for MonoTouch.  
 
***March 3rd, 2013***  
We've released a small bugfix on NuGet and CodePlex.
 
**October 7th, 2012**  
Version 2.0 is now out of beta.
 
**August 25th, 2012**  
Fluent Assertions 2.0, a major new version with support for .NET 4.5, Windows Phone and Windows Store Apps is in beta. Read all the details in this blog post.
  
**January 13th, 2012**  
A relatively small release. Read all about it in this blog post.
 
**October 31st, 2011**  
Another release with lots of bug fixes and small improvements and with great contributions by Martin Opdam and Urs Enzler. Download it here or get it through NuGet.
 
About versioning
----------------
The version numbers of Fluent Assertions releases comply to the Semantic Versioning scheme. In other words, release 1.4.0 only adds backwards-compatible functionality and bug fixes compared to 1.3.0. Release 1.4.1 should only include bug fixes. And if we ever introduce breaking changes, the number increased to 2.0.0.

Who are we?
-----------
We are a bunch of developers working for Aviva Solutions who highly value software quality, in particular  
- [Dennis Doomen](https://twitter.com/ddoomen)  
- Martin Opdam 

If you have any comments or suggestions, please let us know via [twitter](https://twitter.com/search?q=fluentassertions&src=typd), through the [issues](https://github.com/dennisdoomen/FluentAssertions/issues) page, or through [StackOverflow](http://stackoverflow.com/questions/tagged/fluent-assertions).
