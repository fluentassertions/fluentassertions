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
            return config.GetUserEquivalencySteps(config.ConversionSelector).Any(s => s.CanHandle(context, config));
        }

        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent,
            IEquivalencyAssertionOptions config)
        {
            foreach (IEquivalencyStep step in config.GetUserEquivalencySteps(config.ConversionSelector))
            {
                if (step.CanHandle(context, config) && step.Handle(context, parent, config))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
