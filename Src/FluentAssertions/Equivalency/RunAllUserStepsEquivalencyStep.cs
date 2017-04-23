using System.Linq;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Represents a composite equivalency step that passes the execution to all user-supplied steps that can handle the 
    /// current context.
    /// </summary>
    public class RunAllUserStepsEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return config.UserEquivalencySteps.Any(s => s.CanHandle(context, config));
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            return config.UserEquivalencySteps
                .Where(s => s.CanHandle(context, config))
                .Any(step => step.Handle(context, parent, config));
        }
    }
}