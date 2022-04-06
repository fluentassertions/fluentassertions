using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Xunit;
using Xunit.Sdk;

namespace FluentAssertions.Equivalency.Specs
{
    public class DataSetSpecs : DataSpecs
    {
        [Fact]
        public void When_data_sets_are_identical_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2);
        }

        [Fact]
        public void When_data_sets_are_both_null_equivalence_test_should_succeed()
        {
            // Act & Assert
            ((DataSet)null).Should().BeEquivalentTo(null);
        }

        [Fact]
        public void When_data_set_is_null_and_isnt_expected_to_be_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            // Act
            Action action = () => ((DataSet)null).Should().BeEquivalentTo(dataSet);

            // Assert
            action.Should().Throw<XunitException>().WithMessage(
                "Expected *to be non-null, but found null*");
        }

        [Fact]
        public void When_data_set_is_expected_to_be_null_and_isnt_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            // Act
            Action action = () => dataSet.Should().BeEquivalentTo(null);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet value to be null, but found *");
        }

        [Fact]
        public void When_data_set_type_does_not_match_and_not_allowing_msimatched_types_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            var dataSetOfMismatchedType = new TypedDataSetSubclass(typedDataSet);

            // Act
            Action action = () => dataSet.Should().BeEquivalentTo(dataSetOfMismatchedType);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet to be of type *TypedDataSetSubclass, but found System.Data.DataSet*");
        }

        [Fact]
        public void When_data_set_type_does_not_match_but_mismatched_types_are_allowed_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet = CreateDummyDataSet<TypedDataSet>();

            var dataSet = typedDataSet.ToUntypedDataSet();

            var dataSetOfMismatchedType = new TypedDataSetSubclass(typedDataSet);

            // Act & Assert
            dataSet.Should().BeEquivalentTo(dataSetOfMismatchedType, options => options
                .AllowingMismatchedTypes()
                .Excluding(dataSet => dataSet.SchemaSerializationMode));
        }

        [Fact]
        public void When_data_set_name_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.DataSetName += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have DataSetName *different*");
        }

        [Fact]
        public void When_data_set_name_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.DataSetName += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.DataSetName)
                .ExcludingRelated((DataRelation dataRelation) => dataRelation.DataSet));
        }

        [Fact]
        public void When_one_data_set_is_configured_to_be_case_sensitive_and_the_other_is_not_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.CaseSensitive = !typedDataSet2.CaseSensitive;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have CaseSensitive value of True, but found False instead*");
        }

        [Fact]
        public void When_one_data_set_is_configured_to_be_case_sensitive_and_the_other_is_not_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.CaseSensitive = !typedDataSet2.CaseSensitive;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.CaseSensitive));
        }

        [Fact]
        public void When_one_data_set_is_configured_to_enforce_constraints_and_the_other_is_not_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.EnforceConstraints = !typedDataSet2.EnforceConstraints;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have EnforceConstraints value of False, but found True instead*");
        }

        [Fact]
        public void When_one_data_set_is_configured_to_enforce_constraints_and_the_other_is_not_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.EnforceConstraints = !typedDataSet2.EnforceConstraints;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.EnforceConstraints));
        }

        [Fact]
        public void When_one_data_set_has_errors_and_the_other_does_not_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.TypedDataTable1.Rows[0].RowError = "Manually added error";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2, config => config.ExcludingTables("TypedDataTable1"));

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("HasErrors");
        }

        [Fact]
        public void When_one_data_set_has_errors_and_the_other_does_not_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.TypedDataTable1.Rows[0].RowError = "Manually added error";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2,
                config => config.Excluding(dataSet => dataSet.HasErrors).ExcludingTables("TypedDataTable1"));
        }

        [Fact]
        public void When_data_sets_have_mismatched_locale_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet1.Locale = new CultureInfo("en-US");
            typedDataSet2.Locale = new CultureInfo("fr-CA");

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have Locale value of fr-CA, but found en-US instead*");
        }

        [Fact]
        public void When_data_set_locale_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet1.Locale = new CultureInfo("en-US");
            typedDataSet2.Locale = new CultureInfo("fr-CA");

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.Locale));
        }

        [Fact]
        public void When_data_set_namespace_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Namespace += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have Namespace value of *different*");
        }

        [Fact]
        public void When_data_set_namespace_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Namespace += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.Namespace)
                .ExcludingRelated((DataTable dataTable) => dataTable.Namespace)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Namespace));
        }

        [Fact]
        public void When_data_set_prefix_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Prefix += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have Prefix value of *different*");
        }

        [Fact]
        public void When_data_set_prefix_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.Prefix += "different";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.Prefix));
        }

        [Fact]
        public void When_data_set_remoting_format_does_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.RemotingFormat =
                (typedDataSet2.RemotingFormat == SerializationFormat.Binary)
                    ? SerializationFormat.Xml
                    : SerializationFormat.Binary;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1 to have RemotingFormat value of SerializationFormat.Binary*, but found *Xml* instead*");
        }

        [Fact]
        public void When_data_set_remoting_format_does_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            typedDataSet2.RemotingFormat =
                (typedDataSet2.RemotingFormat == SerializationFormat.Binary)
                    ? SerializationFormat.Xml
                    : SerializationFormat.Binary;

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.RemotingFormat)
                .ExcludingRelated((DataTable dataTable) => dataTable.RemotingFormat));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_set_extended_properties_do_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            ApplyChange(typedDataSet2.ExtendedProperties, changeType);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected *dataSet1.ExtendedProperties* to be *, but *");
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        public void When_data_set_extended_properties_do_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed(ChangeType changeType)
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

            ApplyChange(typedDataSet2.ExtendedProperties, changeType);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options.Excluding(dataSet => dataSet.ExtendedProperties));
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "All enum values are accounted for.")]
        public void When_data_set_relations_do_not_match_and_the_corresponding_property_is_not_excluded_equivalence_test_should_fail(ChangeType changeType)
        {
            // Arrange
            TypedDataSetSubclass typedDataSet1;
            TypedDataSetSubclass typedDataSet2;

            if (changeType == ChangeType.Changed)
            {
                typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                typedDataSet2.Relations[0].RelationName += "different";
            }
            else
            {
                var doesNotHaveRelation = CreateDummyDataSet<TypedDataSetSubclass>(includeRelation: false);
                var hasRelation = new TypedDataSetSubclass(doesNotHaveRelation);

                AddRelation(hasRelation);

                if (changeType == ChangeType.Added)
                {
                    typedDataSet1 = doesNotHaveRelation;
                    typedDataSet2 = hasRelation;
                }
                else
                {
                    typedDataSet1 = hasRelation;
                    typedDataSet2 = doesNotHaveRelation;
                }
            }

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected *dataSet1.Relations* to *, but *");
        }

        [Theory]
        [MemberData(nameof(AllChangeTypes))]
        [SuppressMessage("Style", "IDE0010:Add missing cases", Justification = "All enum values are accounted for.")]
        public void When_data_set_relations_do_not_match_but_the_corresponding_property_is_excluded_equivalence_test_should_succeed(ChangeType changeType)
        {
            // Arrange
            TypedDataSetSubclass typedDataSet1;
            TypedDataSetSubclass typedDataSet2;

            if (changeType == ChangeType.Changed)
            {
                typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();
                typedDataSet2 = new TypedDataSetSubclass(typedDataSet1);

                typedDataSet2.Relations[0].RelationName += "different";
            }
            else
            {
                var doesNotHaveRelation = CreateDummyDataSet<TypedDataSetSubclass>(includeRelation: false);
                var hasRelation = new TypedDataSetSubclass(doesNotHaveRelation);

                AddRelation(hasRelation);

                if (changeType == ChangeType.Added)
                {
                    typedDataSet1 = doesNotHaveRelation;
                    typedDataSet2 = hasRelation;
                }
                else
                {
                    typedDataSet1 = hasRelation;
                    typedDataSet2 = doesNotHaveRelation;
                }
            }

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2, options => options
                .Excluding(dataSet => dataSet.Relations)
                .ExcludingRelated((DataTable dataTable) => dataTable.Constraints)
                .ExcludingRelated((DataTable dataTable) => dataTable.ParentRelations)
                .ExcludingRelated((DataTable dataTable) => dataTable.ChildRelations)
                .ExcludingRelated((DataColumn dataColumn) => dataColumn.Unique));
        }

        [Fact]
        public void When_data_set_tables_are_the_same_but_in_a_different_order_equivalence_test_should_succeed()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act & Assert
            dataSet1.Should().BeEquivalentTo(dataSet2);
        }

        [Fact]
        public void When_data_set_table_count_does_not_match_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            typedDataSet2.Tables.Add(new DataTable("ThirdWheel"));

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().Which.Message.Should().Contain("to contain " + dataSet2.Tables.Count);
        }

        [Fact]
        public void When_data_set_table_count_matches_but_tables_are_different_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            typedDataSet2.TypedDataTable2.TableName = "DifferentTableName";

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1.Relations[0].ExtendedProperties* to reference column *ForeignRowID* in table *Different*, but found a reference to *ForeignRowID* in table *TypedDataTable2* instead*");
        }

        [Fact]
        public void When_data_set_tables_contain_different_data_equivalence_test_should_fail()
        {
            // Arrange
            var typedDataSet1 = CreateDummyDataSet<TypedDataSetSubclass>();

            var typedDataSet2 = new TypedDataSetSubclass(typedDataSet1, swapTableOrder: true);

            typedDataSet2.TypedDataTable2[0].Guid = Guid.NewGuid();

            var dataSet1 = typedDataSet1.ToUntypedDataSet();
            var dataSet2 = typedDataSet2.ToUntypedDataSet();

            // Act
            Action action = () => dataSet1.Should().BeEquivalentTo(dataSet2);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet1[TypedDataTable2].Rows[0] to have RowState value of *Modified*, but found *Unchanged* instead*");
        }

        [Fact]
        public void When_object_of_type_unequal_to_DataSet_is_asserted_with_a_DataSet_it_fails()
        {
            // Arrange
            var subject = new
            {
                DataSet = "foobar"
            };

            var expected = new
            {
                DataSet = new DataSet()
            };

            // Act
            Action act = () => subject.Should().BeEquivalentTo(expected);

            // Assert
            act.Should().Throw<XunitException>()
                .WithMessage("Expected*System.Data.DataSet*found System.String*");
        }

        [Fact]
        public void When_data_set_table_count_has_expected_value_equivalence_test_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var correctTableCount = dataSet.Tables.Count;

            // Act & Assert
            dataSet.Should().HaveTableCount(correctTableCount);
        }

        [Fact]
        public void When_data_set_table_count_has_unexpected_value_equivalence_test_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var correctTableCount = dataSet.Tables.Count;

            var incorrectTableCount = correctTableCount + 1;

            // Act
            Action action =
                () => dataSet.Should().HaveTableCount(incorrectTableCount);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet to contain exactly 3 table(s), but found 2.");
        }

        [Fact]
        public void When_data_set_contains_expected_table_and_asserting_that_it_has_that_table_it_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var existingTableName = dataSet.Tables[0].TableName;

            // Act & Assert
            dataSet.Should().HaveTable(existingTableName);
        }

        [Fact]
        public void When_data_set_does_not_contain_expected_table_asserting_that_it_has_that_table_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            string nonExistingTableName = "Unicorn";

            // Act
            Action action =
                () => dataSet.Should().HaveTable(nonExistingTableName);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet to contain a table named *Unicorn*");
        }

        [Fact]
        public void When_data_set_has_all_expected_tables_asserting_that_it_has_them_should_succeed()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var existingTableNames = dataSet.Tables.Cast<DataTable>()
                .Select(table => table.TableName);

            // Act & Assert
            dataSet.Should().HaveTables(existingTableNames);
        }

        [Fact]
        public void When_data_set_has_some_of_the_expected_tables_but_not_all_then_asserting_that_it_has_them_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var tableNames = dataSet.Tables.Cast<DataTable>()
                .Select(table => table.TableName)
                .Concat(new[] { "Unicorn" });

            // Act
            Action action =
                () => dataSet.Should().HaveTables(tableNames);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet to contain a table named *Unicorn*, but it does not.");
        }

        [Fact]
        public void When_data_set_has_none_of_the_expected_tables_then_asserting_that_it_has_them_should_fail()
        {
            // Arrange
            var dataSet = CreateDummyDataSet<TypedDataSetSubclass>();

            var nonExistentTableNames = new[] { "Unicorn", "Dragon" };

            // Act
            Action action =
                () => dataSet.Should().HaveTables(nonExistentTableNames);

            // Assert
            action.Should().Throw<XunitException>().WithMessage("Expected dataSet to contain a table named *Unicorn*, but it does not.");
        }
    }
}
