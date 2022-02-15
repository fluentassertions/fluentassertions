using System;
using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Bogus;

using FluentAssertions.Equivalency;
using FluentAssertions.Equivalency.Steps;
using FluentAssertions.Equivalency.Tracing;
using FluentAssertions.Execution;

namespace Benchmarks
{
    [SimpleJob(RunStrategy.Throughput, warmupCount: 3, targetCount: 20)]
    public class UsersOfGetClosedGenericInterfaces
    {
        private const int ValueCount = 100_000;

        private object[] values;

        private GenericDictionaryEquivalencyStep dictionaryStep;
        private GenericEnumerableEquivalencyStep enumerableStep;

        private IEquivalencyValidationContext context;
        private IEquivalencyAssertionOptions config;

        private class Context : IEquivalencyValidationContext
        {
            public INode CurrentNode { get; }
            public Reason Reason { get; }
            public Tracer Tracer { get; }
            public IEquivalencyAssertionOptions Options { get; }
            public bool IsCyclicReference(object expectation) => throw new NotImplementedException();

            public IEquivalencyValidationContext AsNestedMember(IMember expectationMember) => throw new NotImplementedException();

            public IEquivalencyValidationContext AsCollectionItem<TItem>(string index) => throw new NotImplementedException();

            public IEquivalencyValidationContext AsDictionaryItem<TKey, TExpectation>(TKey key) => throw new NotImplementedException();

            public IEquivalencyValidationContext Clone() => throw new NotImplementedException();
        }

        private class Config : IEquivalencyAssertionOptions
        {
            public IEnumerable<IMemberSelectionRule> SelectionRules => throw new NotImplementedException();

            public IEnumerable<IMemberMatchingRule> MatchingRules => throw new NotImplementedException();

            public bool IsRecursive => throw new NotImplementedException();

            public bool AllowInfiniteRecursion => throw new NotImplementedException();

            public CyclicReferenceHandling CyclicReferenceHandling => throw new NotImplementedException();

            public OrderingRuleCollection OrderingRules => throw new NotImplementedException();

            public ConversionSelector ConversionSelector => throw new NotImplementedException();

            public EnumEquivalencyHandling EnumEquivalencyHandling => throw new NotImplementedException();

            public IEnumerable<IEquivalencyStep> UserEquivalencySteps => throw new NotImplementedException();

            public bool UseRuntimeTyping => false;

            public MemberVisibility IncludedProperties => throw new NotImplementedException();

            public MemberVisibility IncludedFields => throw new NotImplementedException();

            public bool ExcludeNonBrowsable => throw new NotImplementedException();

            public bool CompareRecordsByValue => throw new NotImplementedException();

            public ITraceWriter TraceWriter => throw new NotImplementedException();

            public EqualityStrategy GetEqualityStrategy(Type type) => throw new NotImplementedException();
        }

        [Params(typeof(DBNull), typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(ushort),
            typeof(int), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(DateTime),
            typeof(string), typeof(TimeSpan), typeof(Guid), typeof(Dictionary<int, int>), typeof(IEnumerable<int>))]
        public Type DataType { get; set; }

        [GlobalSetup]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0055:Fix formatting", Justification = "Big long list of one-liners")]
        public void GlobalSetup()
        {
            dictionaryStep = new GenericDictionaryEquivalencyStep();
            enumerableStep = new GenericEnumerableEquivalencyStep();

            values = new object[ValueCount];

            var faker = new Faker();

            faker.Random = new Randomizer(localSeed: 1);

            for (int i = 0; i < values.Length; i++)
            {
                switch (Type.GetTypeCode(DataType))
                {
                    case TypeCode.DBNull:
                        values[i] = DBNull.Value;
                        break;
                    case TypeCode.Boolean:
                        values[i] = faker.Random.Bool();
                        break;
                    case TypeCode.Char:
                        values[i] = faker.Lorem.Letter().Single();
                        break;
                    case TypeCode.SByte:
                        values[i] = faker.Random.SByte();
                        break;
                    case TypeCode.Byte:
                        values[i] = faker.Random.Byte();
                        break;
                    case TypeCode.Int16:
                        values[i] = faker.Random.Short();
                        break;
                    case TypeCode.UInt16:
                        values[i] = faker.Random.UShort();
                        break;
                    case TypeCode.Int32:
                        values[i] = faker.Random.Int();
                        break;
                    case TypeCode.UInt32:
                        values[i] = faker.Random.UInt();
                        break;
                    case TypeCode.Int64:
                        values[i] = faker.Random.Long();
                        break;
                    case TypeCode.UInt64:
                        values[i] = faker.Random.ULong();
                        break;
                    case TypeCode.Single:
                        values[i] = faker.Random.Float();
                        break;
                    case TypeCode.Double:
                        values[i] = faker.Random.Double();
                        break;
                    case TypeCode.Decimal:
                        values[i] = faker.Random.Decimal();
                        break;
                    case TypeCode.DateTime:
                        values[i] = faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(+30));
                        break;
                    case TypeCode.String:
                        values[i] = faker.Lorem.Lines(1);
                        break;

                    default:
                    {
                        if (DataType == typeof(TimeSpan))
                            values[i] = faker.Date.Future() - faker.Date.Future();
                        else if (DataType == typeof(Guid))
                            values[i] = faker.Random.Guid();
                        else if (DataType == typeof(Dictionary<int, int>))
                            values[i] = new Dictionary<int, int>() { { faker.Random.Int(), faker.Random.Int() } };
                        else if (DataType == typeof(IEnumerable<int>))
                            values[i] = new int[] { faker.Random.Int(), faker.Random.Int() };
                        else
                            throw new Exception("Unable to populate data of type " + DataType);

                        break;
                    }
                }
            }

            context = new Context();
            config = new Config();
        }

        [Benchmark]
        public void GenericDictionaryEquivalencyStep_CanHandle()
        {
            for (int i = 0; i < values.Length; i++)
            {
                dictionaryStep.Handle(new Comparands(values[i], values[0], typeof(object)), context, null);
            }
        }

        [Benchmark]
        public void GenericEnumerableEquivalencyStep_CanHandle()
        {
            for (int i = 0; i < values.Length; i++)
            {
                enumerableStep.Handle(new Comparands(values[i], values[0], typeof(object)), context, null);
            }
        }
    }
}
