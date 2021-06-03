using System.Xml.Linq;

namespace FluentAssertions.Equivalency.Steps
{
    public class XElementEquivalencyStep : EquivalencyStep<XElement>
    {
        protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            var subject = (XElement)comparands.Subject;
            var expectation = (XElement)comparands.Expectation;

            subject.Should().BeEquivalentTo(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return EquivalencyResult.AssertionCompleted;
        }
    }
}
