using System.Xml.Linq;

namespace FluentAssertions.Equivalency.Steps;

public class XAttributeEquivalencyStep : EquivalencyStep<XAttribute>
{
    protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context,
        IValidateChildNodeEquivalency nested)
    {
        var subject = (XAttribute)comparands.Subject;
        var expectation = (XAttribute)comparands.Expectation;

        subject.Should().Be(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

        return EquivalencyResult.EquivalencyProven;
    }
}
