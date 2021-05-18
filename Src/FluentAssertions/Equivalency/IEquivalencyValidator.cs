namespace FluentAssertions.Equivalency
{
    public interface IEquivalencyValidator
    {
        /// <summary>
        /// Runs a deep recursive equivalency assertion on the provided <paramref name="comparands"/>.
        /// </summary>
        void RecursivelyAssertEquality(Comparands comparands, IEquivalencyValidationContext context);
    }
}
