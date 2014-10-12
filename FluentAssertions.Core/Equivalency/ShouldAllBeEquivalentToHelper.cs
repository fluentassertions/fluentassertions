using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FluentAssertions.Equivalency
{
    public static class ShouldAllBeEquivalentToHelper
    {
        /// <summary>
        /// This converts the configuration provided provided by `ShouldAllBeEquivalentTo`
        /// so that it is usable by `ShouldBeEquivalentTo`
        /// </summary>
        public static EquivalencyAssertionOptions<TSubject> ForCollectionMemberType<T, TSubject>(
            EquivalencyAssertionOptions<TSubject> optionsToConfigure,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> subConfig)
        {
            var options = new EquivalencyAssertionOptions<T>();
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
            foreach (var equivalencyStep in subConfigOptions.UserEquivalencySteps.Reverse())
            {
                actualOptions.Using(new CollectionMemberAssertionRuleDecorator(equivalencyStep));
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

        private class CollectionMemberAssertionRuleDecorator : IEquivalencyStep
        {
            private readonly IEquivalencyStep eqivalencyStep;

            public CollectionMemberAssertionRuleDecorator(IEquivalencyStep equivalencyStep)
            {
                eqivalencyStep = equivalencyStep;
            }

            public bool CanHandle(IEquivalencyValidationContext context, IEquivalencyAssertionOptions config)
            {
                return eqivalencyStep.CanHandle(CreateAdjustedCopy(context), config);
            }

            public bool Handle(
                IEquivalencyValidationContext context,
                IEquivalencyValidator parent,
                IEquivalencyAssertionOptions config)
            {
                var equivalencyValidationContext = CreateAdjustedCopy(context);

                return eqivalencyStep.Handle(equivalencyValidationContext, parent, config);
            }

            private static EquivalencyValidationContext CreateAdjustedCopy(IEquivalencyValidationContext context)
            {
                return new EquivalencyValidationContext
                           {
                               CompileTimeType = context.CompileTimeType,
                               Expectation = context.Expectation,
                               PropertyDescription = context.PropertyDescription,
                               PropertyInfo = context.PropertyInfo,
                               PropertyPath =
                                   CollectionMemberSubjectInfo.GetAdjustedPropertyPath(
                                       context.PropertyPath),
                               Reason = context.Reason,
                               ReasonArgs = context.ReasonArgs,
                               Subject = context.Subject
                           };
            }

            public override string ToString()
            {
                return eqivalencyStep.ToString();
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