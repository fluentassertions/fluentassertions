using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using PublicApiGenerator;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Approval.Tests
{
    [UsesVerify]
    public class ApiApproval
    {
        [Theory]
        [InlineData("net47")]
        [InlineData("netstandard2.0")]
        [InlineData("netstandard2.1")]
        [InlineData("netcoreapp2.1")]
        [InlineData("netcoreapp3.0")]
        public Task ApproveApi(string frameworkVersion)
        {
            VerifierSettings.DisableClipboard();
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(new Uri(codeBase));
            string assemblyPath = Uri.UnescapeDataString(uri.Path);
            var containingDirectory = Path.GetDirectoryName(assemblyPath);
            var configurationName = new DirectoryInfo(containingDirectory).Parent.Name;
            var assemblyFile = Path.GetFullPath(
                Path.Combine(
                    GetSourceDirectory(),
                    Path.Combine("..", "..", "Artifacts", configurationName, frameworkVersion, "FluentAssertions.dll")));

            var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyFile));
            var publicApi = assembly.GeneratePublicApi(options: null);

            return Verifier
                .Verify(publicApi)
                .UseDirectory(Path.Combine("ApprovedApi", "FluentAssertions"))
                .UseStringComparer(OnlyIncludeChanges)
                .UseFileName(frameworkVersion)
                .DisableDiff();
        }

        private static string GetSourceDirectory([CallerFilePath] string path = "") => Path.GetDirectoryName(path);

        // Copied from https://github.com/VerifyTests/Verify.DiffPlex/blob/master/src/Verify.DiffPlex/VerifyDiffPlex.cs
        public static Task<CompareResult> OnlyIncludeChanges(string received, string verified, IReadOnlyDictionary<string, object> _)
        {
            var diff = InlineDiffBuilder.Diff(verified, received);

            var builder = new StringBuilder();
            foreach (var line in diff.Lines)
            {
                switch (line.Type)
                {
                    case ChangeType.Inserted:
                        builder.Append("+ ");
                        break;
                    case ChangeType.Deleted:
                        builder.Append("- ");
                        break;
                    default:
                        // omit unchanged files
                        continue;
                }
                builder.AppendLine(line.Text);
            }

            var compareResult = CompareResult.NotEqual(builder.ToString());
            return Task.FromResult(compareResult);
        }
    }
}
