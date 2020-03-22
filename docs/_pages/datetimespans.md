---
title: Dates & Times
permalink: /datetimespans/
layout: single
classes: wide
sidebar:
  nav: "sidebar"
---

## Dates and times ##

For asserting a `DateTime` or a `DateTimeOffset` against various constraints, Fluent Assertions offers a bunch of methods that, provided that you use the extension methods for representing dates and times, really help to keep your assertions readable.

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

var theDatetimeOffset = 1.March(2010).AsUtc().WithOffset(2.Hours());

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

To assert that a date/time is (not) within a specified time span from another date/time value you can use this method.

```csharp
theDatetime.Should().BeCloseTo(1.March(2010).At(22, 15), 2.Seconds());

theDatetime.Should().NotBeCloseTo(2.March(2010), 1.Hours());
```

This can be particularly useful if your database truncates date/time values.

## TimeSpans ##

Fluent Assertions also support a few dedicated methods that apply to (nullable) `TimeSpan` instances directly:

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
