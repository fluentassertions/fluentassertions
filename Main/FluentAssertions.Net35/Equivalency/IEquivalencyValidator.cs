namespace FluentAssertions.Equivalency
{
    public interface IEquivalencyValidator
    {
        void AssertEqualityUsing(EquivalencyValidationContext context);
    }
}