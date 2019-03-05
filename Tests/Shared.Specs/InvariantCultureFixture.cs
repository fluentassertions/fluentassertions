using System;
using System.Globalization;
using System.Threading;

namespace FluentAssertions.Specs
{
    public class InvariantCultureFixture : IDisposable
    {
        private CultureInfo originalCulture;
        private CultureInfo originalUICulture;

        public InvariantCultureFixture()
        {
#if NET45 || NET47 || NETCOREAPP2_0
            this.originalCulture = Thread.CurrentThread.CurrentCulture;
            this.originalUICulture = Thread.CurrentThread.CurrentUICulture;

            var culture = new CultureInfo("en-us", false);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            CultureInfo.CurrentCulture.ClearCachedData();
            CultureInfo.CurrentUICulture.ClearCachedData();
#else
            originalCulture = CultureInfo.CurrentCulture;
            originalUICulture = CultureInfo.CurrentUICulture;

            var culture = new CultureInfo("en-us");
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
#endif
        }

        public void Dispose()
        {
#if NET45 || NET47 || NETCOREAPP2_0
            Thread.CurrentThread.CurrentCulture = this.originalCulture;
            Thread.CurrentThread.CurrentUICulture = this.originalUICulture;

            CultureInfo.CurrentCulture.ClearCachedData();
            CultureInfo.CurrentUICulture.ClearCachedData();
#else
            CultureInfo.CurrentCulture = this.originalCulture;
            CultureInfo.CurrentUICulture = this.originalUICulture;
#endif
        }
    }
}
