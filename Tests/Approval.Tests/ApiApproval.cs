using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using PublicApiGenerator;
using Xunit;

namespace Approval.Tests;

public class ApiApproval
{
    [Theory]
    [ClassData(typeof(TargetFrameworksTheoryData))]
    public void ApproveApi(string framework)
    {
        var configuration = typeof(ApiApproval).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()!.Configuration;
        var assemblyFile = CombinedPaths("Src", "FluentAssertions", "bin", configuration, framework, "FluentAssertions.dll");
        var assembly = Assembly.LoadFile(assemblyFile);
        var publicApi = assembly.GeneratePublicApi(options: null);
        var expected = GetExpected(publicApi, framework);

        var diff = InlineDiffBuilder.Diff(expected, publicApi);

        Assert.False(diff.HasDifferences, StringifyDiff(diff));
    }

    private class TargetFrameworksTheoryData : TheoryData<string>
    {
        public TargetFrameworksTheoryData()
        {
            var csproj = CombinedPaths("Src", "FluentAssertions", "FluentAssertions.csproj");
            var project = XDocument.Load(csproj);
            var targetFrameworks = project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
            AddRange(targetFrameworks!.Value.Split(';'));
        }
    }

    private static string GetSolutionDirectory([CallerFilePath] string path = "") =>
        Path.Combine(Path.GetDirectoryName(path)!, "..", "..");

    private static string CombinedPaths(params string[] paths) =>
        Path.GetFullPath(Path.Combine(paths.Prepend(GetSolutionDirectory()).ToArray()));

    private string GetExpected(string publicApi, string targetFramework, [CallerFilePath] string path = "")
    {
        var projectPath = Path.Combine(Path.GetDirectoryName(path)!, "ApprovedApi", "FluentAssertions");
        var receivedPath = Path.Combine(projectPath, $"{targetFramework}.received.txt");
        var verifiedPath = Path.Combine(projectPath, $"{targetFramework}.verified.txt");

        File.WriteAllText(receivedPath, publicApi);
        return File.Exists(verifiedPath) ? File.ReadAllText(verifiedPath) : string.Empty;
    }

    private static string StringifyDiff(DiffPaneModel diff)
    {
        var builder = new StringBuilder();
        if (diff.HasDifferences)
        {
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
        }

        return builder.ToString();
    }
}
