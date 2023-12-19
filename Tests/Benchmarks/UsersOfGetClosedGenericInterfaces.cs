using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Bogus;
using FluentAssertionsAsync.Equivalency;
using FluentAssertionsAsync.Equivalency.Steps;
using FluentAssertionsAsync.Equivalency.Tracing;
using FluentAssertionsAsync.Execution;

namespace Benchmarks;

[SimpleJob(RunStrategy.Throughput, warmupCount: 3, iterationCount: 20)]
public class UsersOfGetClosedGenericInterfaces
{
    private const int ValueCount = 100_000;

    private object[] values;

    private GenericDictionaryEquivalencyStep dictionaryStep;
    private GenericEnumerableEquivalencyStep enumerableStep;

    private IEquivalencyValidationContext context;

    private class Context : IEquivalencyValidationContext
    {
        public INode CurrentNode { get; }
        public Reason Reason { get; }
        public Tracer Tracer { get; }
        public IEquivalencyOptions Options { get; internal set; }
        public bool IsCyclicReference(object expectation) => throw new NotImplementedException();

        public IEquivalencyValidationContext AsNestedMember(IMember expectationMember) => throw new NotImplementedException();

        public IEquivalencyValidationContext AsCollectionItem<TItem>(string index) => throw new NotImplementedException();

        public IEquivalencyValidationContext AsDictionaryItem<TKey, TExpectation>(TKey key) =>
            throw new NotImplementedException();

        public IEquivalencyValidationContext Clone() => throw new NotImplementedException();
    }

    private class Config : IEquivalencyOptions
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

        public bool IgnoreNonBrowsableOnSubject => throw new NotImplementedException();

        public bool ExcludeNonBrowsableOnExpectation => throw new NotImplementedException();

        public bool? CompareRecordsByValue => throw new NotImplementedException();

        public ITraceWriter TraceWriter => throw new NotImplementedException();

        public EqualityStrategy GetEqualityStrategy(Type type) => throw new NotImplementedException();
    }

    [Params(typeof(DBNull), typeof(bool), typeof(char), typeof(sbyte), typeof(byte), typeof(short), typeof(ushort),
        typeof(int), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal), typeof(DateTime),
        typeof(string), typeof(TimeSpan), typeof(Guid), typeof(Dictionary<int, int>), typeof(IEnumerable<int>))]
    public Type DataType { get; set; }

    [GlobalSetup]
    [SuppressMessage("Style", "IDE0055:Fix formatting", Justification = "Big long list of one-liners")]
    public void GlobalSetup()
    {
        dictionaryStep = new GenericDictionaryEquivalencyStep();
        enumerableStep = new GenericEnumerableEquivalencyStep();

        var faker = new Faker
        {
            Random = new Randomizer(localSeed: 1)
        };

        values = Enumerable.Range(0, ValueCount).Select(_ => CreateValue(faker)).ToArray();

        context = new Context
        {
            Options = new Config()
        };
    }

    private object CreateValue(Faker faker) => Type.GetTypeCode(DataType) switch
    {
        TypeCode.DBNull => DBNull.Value,
        TypeCode.Boolean => faker.Random.Bool(),
        TypeCode.Char => faker.Lorem.Letter().Single(),
        TypeCode.SByte => faker.Random.SByte(),
        TypeCode.Byte => faker.Random.Byte(),
        TypeCode.Int16 => faker.Random.Short(),
        TypeCode.UInt16 => faker.Random.UShort(),
        TypeCode.Int32 => faker.Random.Int(),
        TypeCode.UInt32 => faker.Random.UInt(),
        TypeCode.Int64 => faker.Random.Long(),
        TypeCode.UInt64 => faker.Random.ULong(),
        TypeCode.Single => faker.Random.Float(),
        TypeCode.Double => faker.Random.Double(),
        TypeCode.Decimal => faker.Random.Decimal(),
        TypeCode.DateTime => faker.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddDays(+30)),
        TypeCode.String => faker.Lorem.Lines(1),
        _ => CustomValue(faker),
    };

    private object CustomValue(Faker faker)
    {
        if (DataType == typeof(TimeSpan))
        {
            return faker.Date.Future() - faker.Date.Future();
        }
        else if (DataType == typeof(Guid))
        {
            return faker.Random.Guid();
        }
        else if (DataType == typeof(Dictionary<int, int>))
        {
            return new Dictionary<int, int> { { faker.Random.Int(), faker.Random.Int() } };
        }
        else if (DataType == typeof(IEnumerable<int>))
        {
            return new[] { faker.Random.Int(), faker.Random.Int() };
        }

        throw new Exception("Unable to populate data of type " + DataType);
    }

    [Benchmark]
    public void GenericDictionaryEquivalencyStep_CanHandle()
    {
        for (int i = 0; i < values.Length; i++)
        {
            dictionaryStep.HandleAsync(new Comparands(values[i], values[0], typeof(object)), context, null);
        }
    }

    [Benchmark]
    public void GenericEnumerableEquivalencyStep_CanHandle()
    {
        for (int i = 0; i < values.Length; i++)
        {
            enumerableStep.HandleAsync(new Comparands(values[i], values[0], typeof(object)), context, null);
        }
    }
}
