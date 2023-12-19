using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class XDocumentEquivalencyStep : EquivalencyStep<XDocument>
{
    protected override Task<EquivalencyResult> OnHandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        var subject = (XDocument)comparands.Subject;
        var expectation = (XDocument)comparands.Expectation;

        subject.Should().BeEquivalentTo(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return Task.FromResult(EquivalencyResult.AssertionCompleted);
    }
}
