using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency;

public interface IEquivalencyValidator
{
    /// <summary>
    /// Runs a deep recursive equivalency assertion on the provided <paramref name="comparands"/>.
    /// </summary>
    Task RecursivelyAssertEqualityAsync(Comparands comparands, IEquivalencyValidationContext context);
}
