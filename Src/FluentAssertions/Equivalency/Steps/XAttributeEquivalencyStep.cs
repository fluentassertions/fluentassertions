using System.Xml.Linq;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps;

public class XAttributeEquivalencyStep : EquivalencyStep<XAttribute>
{
    protected override EquivalencyResult OnHandle(Comparands comparands, AssertionChain assertionChain,
        IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nestedValidator)
    {
        var subject = (XAttribute)comparands.Subject;
        var expectation = (XAttribute)comparands.Expectation;

        subject.Should().Be(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return EquivalencyResult.EquivalencyProven;
    }
}
