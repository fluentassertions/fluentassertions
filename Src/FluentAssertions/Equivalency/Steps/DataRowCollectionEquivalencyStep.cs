using System;
using System.Data;
using System.Linq;
using FluentAssertions.Data;
using FluentAssertions.Execution;

namespace FluentAssertions.Equivalency.Steps
{
    public class DataRowCollectionEquivalencyStep : EquivalencyStep<DataRowCollection>
    {
        protected override EquivalencyResult OnHandle(Comparands comparands, IEquivalencyValidationContext context, IEquivalencyValidator nestedValidator)
        {
            if (comparands.Subject is not DataRowCollection)
            {
                AssertionScope.Current
                    .FailWith("Expected {context:value} to be of type DataRowCollection, but found {0}",
                        comparands.Subject.GetType());
            }
            else
            {
                RowMatchMode rowMatchMode = RowMatchMode.Index;

                if (context.Options is DataEquivalencyAssertionOptions<DataSet> dataSetConfig)
                {
                    rowMatchMode = dataSetConfig.RowMatchMode;
                }
                else if (context.Options is DataEquivalencyAssertionOptions<DataTable> dataTableConfig)
                {
                    rowMatchMode = dataTableConfig.RowMatchMode;
                }

                var subject = (DataRowCollection)comparands.Subject;
                var expectation = (DataRowCollection)comparands.Expectation;

                bool success = AssertionScope.Current
                    .ForCondition(subject.Count == expectation.Count)
                    .FailWith("Expected {context:DataRowCollection} to contain {0} row(s){reason}, but found {1}",
                        expectation.Count, subject.Count);

                if (success)
                {
                    switch (rowMatchMode)
                    {
                        case RowMatchMode.Index:
                            MatchRowsByIndexAndCompare(context, nestedValidator, subject, expectation);
                            break;

                        case RowMatchMode.PrimaryKey:
                            MatchRowsByPrimaryKeyAndCompare(nestedValidator, context, subject, expectation);
                            break;

                        default:
                            AssertionScope.Current.FailWith(
                                "Unknown RowMatchMode {0} when trying to compare {context:DataRowCollection}", rowMatchMode);
                            break;
                    }
                }
            }

            return EquivalencyResult.AssertionCompleted;
        }

        private static void MatchRowsByIndexAndCompare(IEquivalencyValidationContext context, IEquivalencyValidator parent, DataRowCollection subject, DataRowCollection expectation)
        {
            for (int index = 0; index < expectation.Count; index++)
            {
                IEquivalencyValidationContext nestedContext = context.AsCollectionItem<DataRow>(index);
                parent.RecursivelyAssertEquality(new Comparands(subject[index], expectation[index], typeof(DataRow)), nestedContext);
            }
        }

        private static void MatchRowsByPrimaryKeyAndCompare(IEquivalencyValidator parent, IEquivalencyValidationContext context, DataRowCollection subject, DataRowCollection expectation)
        {
            Type[] subjectPrimaryKeyTypes = null;
            Type[] expectationPrimaryKeyTypes = null;

            if (subject.Count > 0)
            {
                subjectPrimaryKeyTypes = GatherPrimaryKeyColumnTypes(subject[0].Table, "subject");
            }

            if (expectation.Count > 0)
            {
                expectationPrimaryKeyTypes = GatherPrimaryKeyColumnTypes(expectation[0].Table, "expectation");
            }

            bool matchingTypes = ComparePrimaryKeyTypes(subjectPrimaryKeyTypes, expectationPrimaryKeyTypes);

            if (matchingTypes)
            {
                GatherRowsByPrimaryKeyAndCompareData(parent, context, subject, expectation);
            }
        }

        private static Type[] GatherPrimaryKeyColumnTypes(DataTable table, string comparisonTerm)
        {
            Type[] primaryKeyTypes = null;

            if ((table.PrimaryKey is null) || (table.PrimaryKey.Length == 0))
            {
                AssertionScope.Current
                    .FailWith("Table {0} containing {1} {context:DataRowCollection} does not have a primary key. RowMatchMode.PrimaryKey cannot be applied.", table.TableName, comparisonTerm);
            }
            else
            {
                primaryKeyTypes = new Type[table.PrimaryKey.Length];

                for (int i = 0; i < table.PrimaryKey.Length; i++)
                {
                    primaryKeyTypes[i] = table.PrimaryKey[i].DataType;
                }
            }

            return primaryKeyTypes;
        }

        private static bool ComparePrimaryKeyTypes(Type[] subjectPrimaryKeyTypes, Type[] expectationPrimaryKeyTypes)
        {
            bool matchingTypes = false;

            if ((subjectPrimaryKeyTypes is not null) && (expectationPrimaryKeyTypes is not null))
            {
                matchingTypes = subjectPrimaryKeyTypes.Length == expectationPrimaryKeyTypes.Length;

                for (int i = 0; matchingTypes && (i < subjectPrimaryKeyTypes.Length); i++)
                {
                    if (subjectPrimaryKeyTypes[i] != expectationPrimaryKeyTypes[i])
                    {
                        matchingTypes = false;
                    }
                }

                if (!matchingTypes)
                {
                    AssertionScope.Current
                        .FailWith("Subject and expectation primary keys of table containing {context:DataRowCollection} do not have the same schema and cannot be compared. RowMatchMode.PrimaryKey cannot be applied.");
                }
            }

            return matchingTypes;
        }

        private static void GatherRowsByPrimaryKeyAndCompareData(IEquivalencyValidator parent, IEquivalencyValidationContext context, DataRowCollection subject, DataRowCollection expectation)
        {
            var expectationRowByKey = expectation.Cast<DataRow>()
                .ToDictionary(row => ExtractPrimaryKey(row));

            foreach (DataRow subjectRow in subject.Cast<DataRow>())
            {
                CompoundKey key = ExtractPrimaryKey(subjectRow);

                if (!expectationRowByKey.TryGetValue(key, out DataRow expectationRow))
                {
                    AssertionScope.Current
                        .FailWith("Found unexpected row in {context:DataRowCollection} with key {0}", key);
                }
                else
                {
                    expectationRowByKey.Remove(key);

                    IEquivalencyValidationContext nestedContext = context.AsCollectionItem<DataRow>(key.ToString());
                    parent.RecursivelyAssertEquality(new Comparands(subjectRow, expectationRow, typeof(DataRow)), nestedContext);
                }
            }

            if (expectationRowByKey.Count > 0)
            {
                if (expectationRowByKey.Count > 1)
                {
                    AssertionScope.Current
                        .FailWith("{0} rows were expected in {context:DataRowCollection} and not found", expectationRowByKey.Count);
                }
                else
                {
                    AssertionScope.Current
                        .FailWith("Expected to find a row with key {0} in {context:DataRowCollection}{reason}, but no such row was found", expectationRowByKey.Keys.Single());
                }
            }
        }

        private class CompoundKey : IEquatable<CompoundKey>
        {
            private readonly object[] values;

            public CompoundKey(params object[] values)
            {
                this.values = values;
            }

            public bool Equals(CompoundKey other)
            {
                if (other is null)
                {
                    return false;
                }

                if (values.Length != other.values.Length)
                {
                    return false;
                }

                for (int i = 0; i < values.Length; i++)
                {
                    if (!values[i].Equals(other.values[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            public override bool Equals(object obj) => Equals(obj as CompoundKey);

            public override int GetHashCode()
            {
                int hash = 0;

                for (int i = 0; i < values.Length; i++)
                {
                    hash = (hash * 389) ^ values[i].GetHashCode();
                }

                return hash;
            }

            public override string ToString()
            {
                return "{ " + string.Join(", ", values) + " }";
            }
        }

        private static CompoundKey ExtractPrimaryKey(DataRow row)
        {
            DataColumn[] primaryKey = row.Table.PrimaryKey;

            var values = new object[primaryKey.Length];

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = row[primaryKey[i]];
            }

            return new CompoundKey(values);
        }
    }
}
