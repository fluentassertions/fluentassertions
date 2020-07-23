using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ApprovalTests;
using ApprovalTests.Core;
using ApprovalTests.Reporters;
using ApprovalTests.Writers;
using PublicApiGenerator;
using Xunit;

namespace Approval.Tests
{
    public class ApiApproval
    {
        [Theory]
        [InlineData("FluentAssertions", "net47")]
        [InlineData("FluentAssertions", "netstandard2.0")]
        [InlineData("FluentAssertions", "netstandard2.1")]
        [InlineData("FluentAssertions", "netcoreapp2.1")]
        [InlineData("FluentAssertions", "netcoreapp3.0")]
        [UseReporter(typeof(DiffReporter))]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ApproveApi(string projectName, string frameworkVersion)
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(new Uri(codeBase));
            string assemblyPath = Uri.UnescapeDataString(uri.Path);
            var containingDirectory = Path.GetDirectoryName(assemblyPath);
            var configurationName = new DirectoryInfo(containingDirectory).Parent.Name;
            var assemblyFile = Path.GetFullPath(
                Path.Combine(
                    GetSourceDirectory(),
                    $"../../Artifacts/{configurationName}/{frameworkVersion}/{projectName}.dll"));

            var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyFile));
            var publicApi = ApiGenerator.GeneratePublicApi(assembly, options: null);

            Approvals.Verify(
                WriterFactory.CreateTextWriter(publicApi),
                new ApprovalNamer(projectName, frameworkVersion),
                Approvals.GetReporter());
        }

        private class ApprovalNamer : IApprovalNamer
        {
            public ApprovalNamer(string projectName, string frameworkVersion)
            {
                Name = frameworkVersion;
                SourcePath = Path.Combine(GetSourceDirectory(), "ApprovedApi", projectName);
            }

            public string SourcePath { get; }

            public string Name { get; }
        }

        private static string GetSourceDirectory([CallerFilePath] string path = "") => Path.GetDirectoryName(path);
    }
}
