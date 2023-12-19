using System.Threading.Tasks;

namespace FluentAssertionsAsync.Equivalency;

/// <summary>
/// Defines a step in the process of comparing two object graphs for structural equivalency.
/// </summary>
public interface IEquivalencyStep
{
    /// <summary>
    /// Executes an operation such as an equivalency assertion on the provided <paramref name="comparands"/>.
    /// </summary>
    /// <value>
    /// Should return <see cref="EquivalencyResult.AssertionCompleted"/> if the subject matches the expectation or if no additional assertions
    /// have to be executed. Should return <see cref="EquivalencyResult.ContinueWithNext"/> otherwise.
    /// </value>
    /// <remarks>
    /// May throw when preconditions are not met or if it detects mismatching data.
    /// </remarks>
    Task<EquivalencyResult> HandleAsync(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator);
}
