using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        var configuration = typeof(ApiApproval).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        var assemblyFile = GetPath("Src", "FluentAssertions", "bin", configuration, frameworkVersion, "FluentAssertions.dll");
        var assembly = Assembly.LoadFile(assemblyFile);
        var publicApi = assembly.GeneratePublicApi(options: null);

        return Verifier
            .Verify(publicApi)
            .ScrubLinesContaining("FrameworkDisplayName")
            .UseDirectory(Path.Combine("ApprovedApi", "FluentAssertions"))
            .UseStringComparer(OnlyIncludeChanges)
            .UseFileName(frameworkVersion)
            .DisableDiff();
    }

    private static string GetSolutionDirectory([CallerFilePath] string path = "") => Path.Combine(Path.GetDirectoryName(path)!, "..", "..");

    private static string GetPath(params string[] paths) => Path.GetFullPath(Path.Combine(paths.Prepend(GetSolutionDirectory()).ToArray()));

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
            var csproj = GetPath("Src", "FluentAssertions", "FluentAssertions.csproj");
            var project = XDocument.Load(csproj);
            var targetFrameworks = project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");

            foreach (string targetFramework in targetFrameworks!.Value.Split(';'))
            {
                Add(targetFramework);
            }
        }
    }
}
