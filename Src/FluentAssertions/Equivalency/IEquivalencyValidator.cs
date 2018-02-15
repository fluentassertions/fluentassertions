namespace FluentAssertions.Equivalency
{
    public interface IEquivalencyValidator
    {
        void AssertEqualityUsing(IEquivalencyValidationContext context);
    }
}
