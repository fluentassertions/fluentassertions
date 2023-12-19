using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class XElementEquivalencyStep : EquivalencyStep<XElement>
{
    protected override Task<EquivalencyResult> OnHandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        var subject = (XElement)comparands.Subject;
        var expectation = (XElement)comparands.Expectation;

        subject.Should().BeEquivalentTo(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return Task.FromResult(EquivalencyResult.AssertionCompleted);
    }
}
