using System;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Ensures that types that are marked as value types are treated as such.
    /// </summary>
    public class ValueTypeEquivalencyStep : IEquivalencyStep
    {
        /// <summary>
        /// Gets a value indicating whether this step can handle the current subject and/or expectation.
        /// </summary>
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            Type type = config.GetExpectationType(context);
            EqualityStrategy strategy = config.GetEqualityStrategy(type);

            bool canHandle = (strategy == EqualityStrategy.Equals) || (strategy == EqualityStrategy.ForceEquals);
            if (canHandle)
            {
                context.TraceSingle(path =>
                {
                    string strategyName = (strategy == EqualityStrategy.Equals)
                        ? "Equals must be used" : "object overrides Equals";

                    return $"Treating {path} as a value type because {strategyName}.";
                });
            }

            return canHandle;
        }

        /// <summary>
        /// Applies a step as part of the task to compare two objects for structural equality.
        /// </summary>
        /// <value>
        /// Should return <c>true</c> if the subject matches the expectation or if no additional assertions
        /// have to be executed. Should return <c>false</c> otherwise.
        /// </value>
        /// <remarks>
        /// May throw when preconditions are not met or if it detects mismatching data.
        /// </remarks>
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator structuralEqualityValidator, IEquivalencyAssertionOptions config)
        {
            context.Subject.Should().Be(context.Expectation, context.Because, context.BecauseArgs);

            return true;
        }
    }
}
