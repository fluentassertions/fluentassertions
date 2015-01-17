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
            EquivalencyAssertionOptions<T> options = EquivalencyAssertionOptions<T>.Default();
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
            // Reverse order because Using prepends
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

            foreach (var selectionRule in opts.SelectionRules)
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

            // Reverse order because Using prepends
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

                               SelectedMemberDescription = context.SelectedMemberDescription,
                               SelectedMemberInfo = context.SelectedMemberInfo,
                               SelectedMemberPath =
                                   CollectionMemberSubjectInfo.GetAdjustedPropertyPath(
                                       context.SelectedMemberPath),
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

        private class CollectionMemberMatchingRuleDecorator : IMemberMatchingRule
        {
            private readonly IMemberMatchingRule matchingRule;

            public CollectionMemberMatchingRuleDecorator(IMemberMatchingRule matchingRule)
            {
                this.matchingRule = matchingRule;
            }

            public ISelectedMemberInfo Match(ISelectedMemberInfo subjectMember, object expectation, string memberPath, IEquivalencyAssertionOptions config)
            {
                return matchingRule.Match(subjectMember, expectation, memberPath, config);
            }

            public override string ToString()
            {
                return matchingRule.ToString();
            }
        }

        private class CollectionMemberSelectionRuleDecorator : IMemberSelectionRule
        {
            private readonly IMemberSelectionRule selectionRule;

            public CollectionMemberSelectionRuleDecorator(IMemberSelectionRule selectionRule)
            {
                this.selectionRule = selectionRule;
            }

            public IEnumerable<ISelectedMemberInfo> SelectMembers(IEnumerable<ISelectedMemberInfo> selectedMembers, ISubjectInfo context, IEquivalencyAssertionOptions config)
            {
                return selectionRule.SelectMembers(selectedMembers, new CollectionMemberSubjectInfo(context), config);
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
                SelectedMemberDescription = subjectInfo.SelectedMemberDescription;
                SelectedMemberInfo = subjectInfo.SelectedMemberInfo;
                SelectedMemberPath = GetAdjustedPropertyPath(subjectInfo.SelectedMemberPath);
                RuntimeType = subjectInfo.RuntimeType;
            }

            internal static string GetAdjustedPropertyPath(string propertyPath)
            {
                return propertyPath.Substring(propertyPath.IndexOf(".", StringComparison.Ordinal) + 1);
            }

            public ISelectedMemberInfo SelectedMemberInfo { get; private set; }

            public string SelectedMemberPath { get; private set; }

            public string SelectedMemberDescription { get; private set; }

            public Type CompileTimeType { get; private set; }

            public Type RuntimeType { get; private set; }

            [Obsolete]
            public PropertyInfo PropertyInfo
            {
                get
                {
                    var propertySelectedMemberInfo = SelectedMemberInfo as PropertySelectedMemberInfo;

                    if (propertySelectedMemberInfo != null)
                    {
                        return propertySelectedMemberInfo.PropertyInfo;
                    }

                    return null;
                }
            }

            [Obsolete]
            public string PropertyPath
            {
                get { return SelectedMemberPath; }
            }

            [Obsolete]
            public string PropertyDescription
            {
                get { return SelectedMemberDescription; }
            }
        }
    }
}