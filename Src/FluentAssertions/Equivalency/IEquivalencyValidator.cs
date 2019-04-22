namespace FluentAssertions.Equivalency
{
    public interface IEquivalencyValidator
    {
        void RecursivelyAssertEquality(IEquivalencyValidationContext context);
    }
}
