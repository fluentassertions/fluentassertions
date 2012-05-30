namespace FluentAssertions.Structural
{
    internal interface IStructuralEqualityValidator
    {
        void AssertEquality(StructuralEqualityContext context);
    }
}