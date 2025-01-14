namespace FluentAssertions.Equivalency;

public interface IValidateChildNodeEquivalency
{
    /// <summary>
    /// Runs a deep recursive equivalency assertion on the provided <paramref name="comparands"/>.
    /// </summary>
    void AssertEquivalencyOf(Comparands comparands, IEquivalencyValidationContext context);
}
