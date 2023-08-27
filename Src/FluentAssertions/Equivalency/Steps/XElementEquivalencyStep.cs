using System.Xml.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class XElementEquivalencyStep : EquivalencyStep<XElement>
{
    protected override EquivalencyResult OnHandle(Comparands comparands, AssertionChain assertionChain,
        IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nestedValidator)
    {
        var subject = (XElement)comparands.Subject;
        var expectation = (XElement)comparands.Expectation;

        subject.Should().BeEquivalentTo(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return EquivalencyResult.EquivalencyProven;
    }
}
