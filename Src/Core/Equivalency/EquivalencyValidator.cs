using System;
using System.Collections.Generic;
using System.Linq;
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
                scope.AddNonReportable("objects", new ObjectTracker(config.CyclicReferenceHandling));

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
            if (ContinueRecursion(context.SelectedMemberPath))
            {
                AssertionScope scope = AssertionScope.Current;
                scope.AddNonReportable("context", (context.SelectedMemberDescription.Length == 0) ? "subject" : context.SelectedMemberDescription);
                scope.AddNonReportable("subject", context.Subject);
                scope.AddNonReportable("expectation", context.Expectation);

                var objectTracker = scope.Get<ObjectTracker>("objects");

                if (!objectTracker.IsCyclicReference(new ObjectReference(context.Subject, context.SelectedMemberPath)))
                {
                    bool wasHandled = false;

                    foreach (var step in AssertionOptions.EquivalencySteps)
                    {
                        if (step.CanHandle(context, config))
                        {
                            if (step.Handle(context, this, config))
                            {
                                wasHandled = true;
                                break;
                            }
                        }
                    }

                    if (!wasHandled)
                    {
                        Execute.Assertion.FailWith(
                            "No IEquivalencyStep was found to handle the context.  " +
                            "This is likely a bug in Fluent Assertions.");
                    }
                }
            }
        }

        private bool ContinueRecursion(string memberAccessPath)
        {
            if (config.AllowInfiniteRecursion || !HasReachedMaximumRecursionDepth(memberAccessPath))
            {
                return true;
            }
            
            AssertionScope.Current.FailWith(
                "The maximum recursion depth was reached.  " +
                "The maximum recursion depth limitation prevents stack overflow from " +
                "occuring when certain types of cycles exist in the object graph " +
                "or the object graph's depth is very high or infinite.  " +
                "This limitation may be disabled using the config parameter." +
                Environment.NewLine + Environment.NewLine +
                "The member access chain when max depth was hit was: " +
                memberAccessPath);

            return false;
        }

        private static bool HasReachedMaximumRecursionDepth(string propertyPath)
        {
            int depth = propertyPath.Cast<char>().Count(chr => chr == '.');

            return (depth >= MaxDepth);
        }
    }
}
