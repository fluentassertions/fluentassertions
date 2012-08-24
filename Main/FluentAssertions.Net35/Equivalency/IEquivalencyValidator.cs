namespace FluentAssertions.Equivalency
{
    internal interface IEquivalencyValidator
    {
        void AssertEqualityUsing(EquivalencyValidationContext context);
    }
}