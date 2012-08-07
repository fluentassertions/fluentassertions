namespace FluentAssertions.Structural
{
    internal interface IStructuralEqualityValidator
    {
        void AssertEqualityUsing(StructuralEqualityContext context);
    }
}