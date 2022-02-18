using System.ComponentModel;
using System.Reflection;

using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    [MemoryDiagnoser]
    public class CheckIfMemberIsBrowsableBenchmarks
    {
        [Params(true, false)]
        public bool IsBrowsable { get; set; }

        public int BrowsableField;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int NonBrowsableField;

        public FieldInfo SubjectField => typeof(CheckIfMemberIsBrowsableBenchmarks)
            .GetField(IsBrowsable ? nameof(BrowsableField) : nameof(NonBrowsableField));

        [Benchmark]
        public void CheckIfMemberIsBrowsable()
        {
            bool _ =
                SubjectField.GetCustomAttribute<EditorBrowsableAttribute>() is not { State: EditorBrowsableState.Never };
        }
    }
}
