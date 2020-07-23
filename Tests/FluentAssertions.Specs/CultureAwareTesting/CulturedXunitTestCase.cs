using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FluentAssertions.Specs.CultureAwareTesting
{
    public class CulturedXunitTestCase : XunitTestCase
    {
        private string culture;

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
        public CulturedXunitTestCase() { }

        public CulturedXunitTestCase(IMessageSink diagnosticMessageSink,
                                     TestMethodDisplay defaultMethodDisplay,
                                     TestMethodDisplayOptions defaultMethodDisplayOptions,
                                     ITestMethod testMethod,
                                     string culture,
                                     object[] testMethodArguments = null)
            : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
        {
            Initialize(culture);
        }

        private void Initialize(string culture)
        {
            this.culture = culture;

            Traits.Add("Culture", culture);

            DisplayName += $"[{culture}]";
        }

        protected override string GetUniqueID()
            => $"{base.GetUniqueID()}[{culture}]";

        public override void Deserialize(IXunitSerializationInfo data)
        {
            base.Deserialize(data);

            Initialize(data.GetValue<string>("Culture"));
        }

        public override void Serialize(IXunitSerializationInfo data)
        {
            base.Serialize(data);

            data.AddValue("Culture", culture);
        }

        public override async Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                        IMessageBus messageBus,
                                                        object[] constructorArguments,
                                                        ExceptionAggregator aggregator,
                                                        CancellationTokenSource cancellationTokenSource)
        {
            CultureInfo originalCulture = CurrentCulture;
            CultureInfo originalUICulture = CurrentUICulture;

            try
            {
                var cultureInfo = new CultureInfo(culture);
                CurrentCulture = cultureInfo;
                CurrentUICulture = cultureInfo;

                return await base.RunAsync(diagnosticMessageSink, messageBus, constructorArguments, aggregator, cancellationTokenSource);
            }
            finally
            {
                CurrentCulture = originalCulture;
                CurrentUICulture = originalUICulture;
            }
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
