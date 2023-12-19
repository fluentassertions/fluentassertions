using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluentAssertionsAsync.Equivalency.Steps;

public class XAttributeEquivalencyStep : EquivalencyStep<XAttribute>
{
    protected override Task<EquivalencyResult> OnHandleAsync(Comparands comparands, IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        var subject = (XAttribute)comparands.Subject;
        var expectation = (XAttribute)comparands.Expectation;

        subject.Should().Be(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return Task.FromResult(EquivalencyResult.AssertionCompleted);
    }
}
