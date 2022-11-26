namespace FluentAssertions.Extensions;

public static class OccurrenceConstraintExtensions
{
    public static OccurrenceConstraint Times(this int times)
    {
        return Exactly.Times(times);
    }

    public static OccurrenceConstraint TimesOrLess(this int times)
    {
        return AtMost.Times(times);
    }

    public static OccurrenceConstraint TimesOrMore(this int times)
    {
        return AtLeast.Times(times);
    }
}
