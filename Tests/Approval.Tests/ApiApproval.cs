using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using PublicApiGenerator;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace Approval.Tests;

[UsesVerify]
public class ApiApproval
{
    [Theory]
    [ClassData(typeof(TargetFrameworksTheoryData))]
    public Task ApproveApi(string frameworkVersion)
    {
        string codeBase = Assembly.GetExecutingAssembly().Location;
        var uri = new UriBuilder(new Uri(codeBase));
        string assemblyPath = Uri.UnescapeDataString(uri.Path);
        var containingDirectory = Path.GetDirectoryName(assemblyPath);
        var configurationName = new DirectoryInfo(containingDirectory).Parent.Name;

        var assemblyFile = Path.GetFullPath(
            Path.Combine(
                GetSourceDirectory(),
                Path.Combine("..", "..", "Src", "FluentAssertions", "bin", configurationName, frameworkVersion,
                    "FluentAssertions.dll")));

        var assembly = Assembly.LoadFile(Path.GetFullPath(assemblyFile));
        var publicApi = assembly.GeneratePublicApi(options: null);

        return Verifier
            .Verify(publicApi)
            .ScrubLinesContaining("FrameworkDisplayName")
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

    private class TargetFrameworksTheoryData : TheoryData<string>
    {
        public TargetFrameworksTheoryData()
        {
            var csproj = Path.Combine(GetSourceDirectory(),
                Path.Combine("..", "..", "Src", "FluentAssertions", "FluentAssertions.csproj"));

            var project = XDocument.Load(csproj);
            var targetFrameworks = project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");

            foreach (string targetFramework in targetFrameworks!.Value.Split(';'))
            {
                Add(targetFramework);
            }
        }
    }
}
