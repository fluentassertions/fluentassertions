## AssertionScope

Take the following example:
```csharp
var scores = new int[0];

scores.Should().NotBeEmpty();
```
This will fail with the following message:

> Expected collection not to be empty.


This is nice but if this assert is in a larger context of say a browser automation test it does not provide much context of what we where expecting not to be empty. We can add a reason:
```csharp
var scores = new int[0];

scores.Should().NotBeEmpty("scores should be assigned");
```
Which improves the output 

> Expected collection not to be empty because scores should be assigned.

but really I would like to change the first part of the text “ Expected collection..” to better describe the context.
To do this we can give our assertion it’s own named AssertionScope.

```csharp
var scores = new int[0];

using(new FluentAssertions.Execution.AssertionScope("scores"))
{
	scores.Should().NotBeEmpty();
}
```

Giving the context a name changes the output to
> Expected scores not to be empty.


Creating a new AssertionScope also has another effect in that it delays the reporting of failures till the AssertionScope is disposed. So all the assertions in the scope will be evaluated and output at once.
```csharp
var scores = new[] { 2, 5, 3, 3 };

using (new FluentAssertions.Execution.AssertionScope("scores"))
{
	scores.Should().HaveCount(3, "only 3 players")
		.And.OnlyHaveUniqueItems("we need a winner")
		.And.BeInAscendingOrder();
}
```

> ```
Expected scores to contain 3 item(s) because only 3 players, but found 4.
Expected scores to only have unique items because we need a winner, but item 3 is not unique.
Expected scores to contain items in ascending order, but found {2, 5, 3, 3} where item at index 1 is in wrong order.
Expected scores to contain items in ascending order, but found {2, 5, 3, 3} where item at index 3 is in wrong order.
```