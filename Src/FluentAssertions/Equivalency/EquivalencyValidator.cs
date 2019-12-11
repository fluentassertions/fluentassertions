using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions.Common;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency
{
    /// <summary>
    /// Is responsible for validating the equality of one or more properties of a subject with another object.
    /// </summary>
    public class EquivalencyValidator : IEquivalencyValidator
    {
        #region Private Definitions

        private const int MaxDepth = 10;

        private readonly IEquivalencyAssertionOptions config;

        private readonly Dictionary<Type, bool> isComplexTypeMap = new Dictionary<Type, bool>();

        #endregion

        public EquivalencyValidator(IEquivalencyAssertionOptions config)
        {
            this.config = config;
        }

        public void AssertEquality(EquivalencyValidationContext context)
        {
            using (var scope = new AssertionScope())
            {
                scope.AddReportable("configuration", config.ToString());

                scope.BecauseOf(context.Because, context.BecauseArgs);

                AssertEqualityUsing(context);

                if (context.Tracer != null)
                {
                    scope.AddReportable("trace", context.Tracer.ToString());
                }
            }
        }

        public void AssertEqualityUsing(IEquivalencyValidationContext context)
        {
            if (ShouldCompareMembersThisDeep(context.SelectedMemberPath))
            {
                UpdateScopeWithReportableContext(context);

                if (!IsCyclicReference(context))
                {
                    RunStepsUntilEquivalencyIsProven(context);
                }
            }
        }

        private bool ShouldCompareMembersThisDeep(string selectedMemberPath)
        {
            const char memberSeparator = '.';
            var depth = selectedMemberPath.Count(chr => chr == memberSeparator);
            bool shouldRecurse = config.AllowInfiniteRecursion || depth < MaxDepth;

            if (!shouldRecurse)
            {
                AssertionScope.Current.FailWith("The maximum recursion depth was reached.  ");
            }

            return shouldRecurse;
        }

        private static void UpdateScopeWithReportableContext(IEquivalencyValidationContext context)
        {
            if (context.SelectedMemberDescription.Length > 0)
            {
                AssertionScope.Current.Context = context.SelectedMemberDescription;
            }

            AssertionScope.Current.TrackComparands(context.Subject, context.Expectation);
        }

        private bool IsCyclicReference(IEquivalencyValidationContext context)
        {
            CyclicReferenceDetector objectTracker = AssertionScope.Current.Get<CyclicReferenceDetector>("cyclic_reference_detector");
            if (objectTracker is null)
            {
                objectTracker = new CyclicReferenceDetector(config.CyclicReferenceHandling);
                AssertionScope.Current.AddNonReportable("cyclic_reference_detector", objectTracker);
            }

            bool isComplexType = IsComplexType(context.Expectation);

            var reference = new ObjectReference(context.Expectation, context.SelectedMemberPath, isComplexType);
            return objectTracker.IsCyclicReference(reference, context.Because, context.BecauseArgs);
        }

        private bool IsComplexType(object expectation)
        {
            if (expectation is null)
            {
                return false;
            }

            Type type = expectation.GetType();

            if (!isComplexTypeMap.TryGetValue(type, out bool isComplexType))
            {
                isComplexType = !type.OverridesEquals();
                isComplexTypeMap[type] = isComplexType;
            }

            return isComplexType;
        }

        private void RunStepsUntilEquivalencyIsProven(IEquivalencyValidationContext context)
        {
            foreach (IEquivalencyStep step in AssertionOptions.EquivalencySteps)
            {
                if (step.CanHandle(context, config) && step.Handle(context, this, config))
                {
                    return;
                }
            }

            throw new NotImplementedException($"No {nameof(IEquivalencyStep)} was found to handle the context. ");
        }
    }
}
