using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FluentAssertions.Specs.CultureAwareTesting
{
    public class CulturedXunitTheoryTestCaseRunner : XunitTheoryTestCaseRunner
    {
        private readonly string culture;
        private CultureInfo originalCulture;
        private CultureInfo originalUICulture;

        public CulturedXunitTheoryTestCaseRunner(CulturedXunitTheoryTestCase culturedXunitTheoryTestCase,
                                                 string displayName,
                                                 string skipReason,
                                                 object[] constructorArguments,
                                                 IMessageSink diagnosticMessageSink,
                                                 IMessageBus messageBus,
                                                 ExceptionAggregator aggregator,
                                                 CancellationTokenSource cancellationTokenSource)
            : base(culturedXunitTheoryTestCase, displayName, skipReason, constructorArguments, diagnosticMessageSink, messageBus, aggregator, cancellationTokenSource)
        {
            culture = culturedXunitTheoryTestCase.Culture;
        }

        protected override Task AfterTestCaseStartingAsync()
        {
            try
            {
                originalCulture = CurrentCulture;
                originalUICulture = CurrentUICulture;

                var cultureInfo = new CultureInfo(culture);
                CurrentCulture = cultureInfo;
                CurrentUICulture = cultureInfo;
            }
            catch (Exception ex)
            {
                Aggregator.Add(ex);
                return Task.FromResult(0);
            }

            return base.AfterTestCaseStartingAsync();
        }

        protected override Task BeforeTestCaseFinishedAsync()
        {
            if (originalUICulture != null)
                CurrentUICulture = originalUICulture;
            if (originalCulture != null)
                CurrentCulture = originalCulture;

            return base.BeforeTestCaseFinishedAsync();
        }

        private static CultureInfo CurrentCulture
        {
            get => CultureInfo.CurrentCulture;
            set => CultureInfo.CurrentCulture = value;
        }

        private static CultureInfo CurrentUICulture
        {
            get => CultureInfo.CurrentUICulture;
            set => CultureInfo.CurrentUICulture = value;
        }
    }
}
