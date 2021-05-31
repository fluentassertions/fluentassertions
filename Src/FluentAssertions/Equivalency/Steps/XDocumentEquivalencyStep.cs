using System.Xml.Linq;

namespace FluentAssertions.Equivalency.Steps
{
    public class XDocumentEquivalencyStep : EquivalencyStep<XDocument>
    {
        protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            var subject = (XDocument)comparands.Subject;
            var expectation = (XDocument)comparands.Expectation;

            subject.Should().BeEquivalentTo(expectation, context.Reason.FormattedMessage, context.Reason.Arguments);

            return EquivalencyResult.AssertionCompleted;
        }
    }
}
