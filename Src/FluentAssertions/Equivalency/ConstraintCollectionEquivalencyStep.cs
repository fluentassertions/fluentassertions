using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    public class ConstraintCollectionEquivalencyStep : IEquivalencyStep
    {
        public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
        {
            return typeof(ConstraintCollection).IsAssignableFrom(config.GetExpectationType(context.RuntimeType, context.CompileTimeType));
        }

        [SuppressMessage("Style", "IDE0038:Use pattern matching", Justification = "Would decrease code clarity")]
        public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator parent, IEquivalencyAssertionOptions config)
        {
            if (!(context.Subject is ConstraintCollection))
            {
                AssertionScope.Current
                    .FailWith("Expected a value of type ConstraintCollection at {context:Constraints}, but found {0}", context.Subject.GetType());
            }
            else
            {
                var subject = (ConstraintCollection)context.Subject;
                var expectation = (ConstraintCollection)context.Expectation;

                var subjectConstraints = subject.Cast<Constraint>().ToDictionary(constraint => constraint.ConstraintName);
                var expectationConstraints = expectation.Cast<Constraint>().ToDictionary(constraint => constraint.ConstraintName);

                var constraintNames = subjectConstraints.Keys.Union(expectationConstraints.Keys);

                foreach (var constraintName in constraintNames)
                {
                    AssertionScope.Current
                        .ForCondition(subjectConstraints.TryGetValue(constraintName, out var subjectConstraint))
                        .FailWith("Expected constraint named {0} in {context:Constraints collection}{reason}, but did not find one", constraintName);

                    AssertionScope.Current
                        .ForCondition(expectationConstraints.TryGetValue(constraintName, out var expectationConstraint))
                        .FailWith("Found unexpected constraint named {0} in {context:Constraints collection}", constraintName);

                    if ((subjectConstraint != null) && (expectationConstraint != null))
                    {
                        var nestedContext = context.AsCollectionItem(
                            constraintName,
                            subjectConstraint,
                            expectationConstraint);

                        if (nestedContext != null)
                        {
                            parent.AssertEqualityUsing(nestedContext);
                        }
                    }
                }
            }

            return true;
        }
    }
}
