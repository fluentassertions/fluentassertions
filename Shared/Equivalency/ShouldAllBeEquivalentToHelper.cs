using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    internal static class ShouldAllBeEquivalentToHelper
    {
        /// <summary>
        /// This converts the configuration provided provided by `ShouldAllBeEquivalentTo`
        /// so that it is usable by `ShouldBeEquivalentTo`
        /// </summary>
        internal static EquivalencyAssertionOptions<TSubject> ForCollectionMemberType<T, TSubject>(this 
            EquivalencyAssertionOptions<TSubject> optionsToConfigure,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> subConfig)
        {
            var options = EquivalencyAssertionOptions<T>.Empty();
            options = subConfig(options);

            ConfigureOrderingRules(optionsToConfigure, options);
            ConfigureMatchingRules(optionsToConfigure, options);
            ConfigureSelectionRules(optionsToConfigure, options);
            ConfigureAssertionRules(optionsToConfigure, options);

            return optionsToConfigure;
        }

        internal static void ConfigureAssertionRules<TActual>(
            EquivalencyAssertionOptions<TActual> actualOptions,
            IEquivalencyAssertionOptions subConfigOptions)
        {
            //Reverse order because Using prepends
            foreach (var assertionRule in subConfigOptions.AssertionRules.Reverse())
            {
                actualOptions.Using(new CollectionMemberAssertionRuleDecorator(assertionRule));
            }
        }

        internal static void ConfigureSelectionRules<TActual>(
            EquivalencyAssertionOptions<TActual> actualOptions,
            IEquivalencyAssertionOptions subConfigOptions)
        {
            var opts = subConfigOptions;
            if (opts.SelectionRules.Any())
            {
                actualOptions.WithoutSelectionRules();
            }

            //Reverse order because Using prepends
            foreach (var selectionRule in opts.SelectionRules.Reverse())
            {
                actualOptions.Using(new CollectionMemberSelectionRuleDecorator(selectionRule));
            }
        }

        internal static void ConfigureOrderingRules<TActual>(
            EquivalencyAssertionOptions<TActual> actualOptions,
            IEquivalencyAssertionOptions subConfigOptions)
        {
            foreach (IOrderingRule orderingRule in subConfigOptions.OrderingRules)
            {
                ((IEquivalencyAssertionOptions)actualOptions).OrderingRules.Add(
                    new CollectionMemberOrderingRuleDecorator(orderingRule));
            }
        }

        internal static void ConfigureMatchingRules<TActual>(
            EquivalencyAssertionOptions<TActual> actualOptions,
            IEquivalencyAssertionOptions subConfigOptions)
        {
            if (subConfigOptions.MatchingRules.Any())
            {
                actualOptions.WithoutMatchingRules();
            }

            //Reverse order because Using prepends
            foreach (var matchingRule in subConfigOptions.MatchingRules.Reverse())
            {
                actualOptions.Using(new CollectionMemberMatchingRuleDecorator(matchingRule));
            }
        }

        private class CollectionMemberAssertionRuleDecorator : IAssertionRule
        {
            private readonly IAssertionRule assertionRule;

            public CollectionMemberAssertionRuleDecorator(IAssertionRule equivalencyStep)
            {
                assertionRule = equivalencyStep;
            }

            public bool AssertEquality(IEquivalencyValidationContext context)
            {
                return assertionRule.AssertEquality(new CollectionMemberEquivalencyValidationContext(context));
            }

            public override string ToString()
            {
                return assertionRule.ToString();
            }

            private class CollectionMemberEquivalencyValidationContext : IEquivalencyValidationContext
            {
                internal CollectionMemberEquivalencyValidationContext(IEquivalencyValidationContext context)
                {
                    PropertyInfo = context.PropertyInfo;
                    PropertyPath = CollectionMemberSubjectInfo.GetAdjustedPropertyPath(context.PropertyPath);
                    PropertyDescription = context.PropertyDescription;
                    CompileTimeType = context.CompileTimeType;
                    RuntimeType = context.RuntimeType;
                    Expectation = context.Expectation;
                    Reason = context.Reason;
                    ReasonArgs = context.ReasonArgs;
                    IsRoot = context.IsRoot;
                    Subject = context.Subject;
                }

                public PropertyInfo PropertyInfo { get; private set; }

                public string PropertyPath { get; private set; }

                public string PropertyDescription { get; private set; }

                public Type CompileTimeType { get; private set; }

                public Type RuntimeType { get; private set; }

                public object Expectation { get; private set; }

                public string Reason { get; private set; }

                public object[] ReasonArgs { get; private set; }

                public bool IsRoot { get; private set; }

                public object Subject { get; private set; }
            }
        }

        private class CollectionMemberOrderingRuleDecorator : IOrderingRule
        {
            private readonly IOrderingRule orderingRule;

            public CollectionMemberOrderingRuleDecorator(IOrderingRule orderingRule)
            {
                this.orderingRule = orderingRule;
            }

            public bool AppliesTo(ISubjectInfo subjectInfo)
            {
                return orderingRule.AppliesTo(new CollectionMemberSubjectInfo(subjectInfo));
            }
        }

        private class CollectionMemberMatchingRuleDecorator : IMatchingRule
        {
            private readonly IMatchingRule matchingRule;

            public CollectionMemberMatchingRuleDecorator(IMatchingRule matchingRule)
            {
                this.matchingRule = matchingRule;
            }

            public PropertyInfo Match(PropertyInfo subjectProperty, object expectation, string propertyPath)
            {
                return matchingRule.Match(subjectProperty, expectation, propertyPath);
            }

            public override string ToString()
            {
                return matchingRule.ToString();
            }
        }

        private class CollectionMemberSelectionRuleDecorator : ISelectionRule
        {
            private readonly ISelectionRule selectionRule;

            public CollectionMemberSelectionRuleDecorator(ISelectionRule selectionRule)
            {
                this.selectionRule = selectionRule;
            }

            public IEnumerable<PropertyInfo> SelectProperties(
                IEnumerable<PropertyInfo> selectedProperties,
                ISubjectInfo context)
            {
                return selectionRule.SelectProperties(selectedProperties, new CollectionMemberSubjectInfo(context));
            }

            public override string ToString()
            {
                return selectionRule.ToString();
            }
        }

        private class CollectionMemberSubjectInfo : ISubjectInfo
        {
            public CollectionMemberSubjectInfo(ISubjectInfo subjectInfo)
            {
                CompileTimeType = subjectInfo.CompileTimeType;
                PropertyDescription = subjectInfo.PropertyDescription;
                PropertyInfo = subjectInfo.PropertyInfo;
                PropertyPath = GetAdjustedPropertyPath(subjectInfo.PropertyPath);
                RuntimeType = subjectInfo.RuntimeType;
            }

            internal static string GetAdjustedPropertyPath(string propertyPath)
            {
                return propertyPath.Substring(propertyPath.IndexOf(".", StringComparison.Ordinal) + 1);
            }

            public PropertyInfo PropertyInfo { get; private set; }

            public string PropertyPath { get; private set; }

            public string PropertyDescription { get; private set; }

            public Type CompileTimeType { get; private set; }

            public Type RuntimeType { get; private set; }
        }
    }
}