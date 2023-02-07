using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions.Equivalency;

namespace FluentAssertions.Data;

internal class DataEquivalencyAssertionOptions<T> : EquivalencyAssertionOptions<T>, IDataEquivalencyAssertionOptions<T>
{
    private readonly HashSet<string> excludeTableNames = new();
    private readonly HashSet<string> excludeColumnNames = new();
    private readonly Dictionary<string, HashSet<string>> excludeColumnNamesByTableName = new();

    public bool AllowMismatchedTypes { get; private set; }

    public bool IgnoreUnmatchedColumns { get; private set; }

    public bool ExcludeOriginalData { get; private set; }

    public RowMatchMode RowMatchMode { get; private set; }

    public ISet<string> ExcludeTableNames => excludeTableNames;

    public ISet<string> ExcludeColumnNames => excludeColumnNames;

    public DataEquivalencyAssertionOptions(EquivalencyAssertionOptions defaults)
        : base(defaults)
    {
    }

    public IDataEquivalencyAssertionOptions<T> AllowingMismatchedTypes()
    {
        AllowMismatchedTypes = true;
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> IgnoringUnmatchedColumns()
    {
        IgnoreUnmatchedColumns = true;
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> UsingRowMatchMode(RowMatchMode rowMatchMode)
    {
        RowMatchMode = rowMatchMode;
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingOriginalData()
    {
        ExcludeOriginalData = true;
        return this;
    }

    public new IDataEquivalencyAssertionOptions<T> Excluding(Expression<Func<T, object>> expression)
    {
        base.Excluding(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataRelation, object>> expression)
    {
        ExcludeMemberOfRelatedTypeByGeneratedPredicate(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataTable, object>> expression)
    {
        ExcludeMemberOfRelatedTypeByGeneratedPredicate(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataColumn, object>> expression)
    {
        ExcludeMemberOfRelatedTypeByGeneratedPredicate(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<DataRow, object>> expression)
    {
        ExcludeMemberOfRelatedTypeByGeneratedPredicate(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<Constraint, object>> expression)
    {
        ExcludeMemberOfSubtypeOfRelatedTypeByGeneratedPredicate<Constraint, ForeignKeyConstraint, object>(expression);
        ExcludeMemberOfSubtypeOfRelatedTypeByGeneratedPredicate<Constraint, UniqueConstraint, object>(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<ForeignKeyConstraint, object>> expression)
    {
        ExcludeMemberOfRelatedTypeByGeneratedPredicate(expression);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingRelated(Expression<Func<UniqueConstraint, object>> expression)
    {
        ExcludeMemberOfRelatedTypeByGeneratedPredicate(expression);
        return this;
    }

    private void ExcludeMemberOfRelatedTypeByGeneratedPredicate<TDeclaringType, TPropertyType>(
        Expression<Func<TDeclaringType, TPropertyType>> expression)
    {
        Expression<Func<IMemberInfo, bool>> predicate = BuildMemberSelectionPredicate(
            typeof(TDeclaringType),
            GetMemberAccessTargetMember(expression.Body));

        Excluding(predicate);
    }

    private void ExcludeMemberOfSubtypeOfRelatedTypeByGeneratedPredicate<TDeclaringType, TInheritingType, TPropertyType>(
        Expression<Func<TDeclaringType, TPropertyType>> expression)
        where TInheritingType : TDeclaringType
    {
        Expression<Func<IMemberInfo, bool>> predicate = BuildMemberSelectionPredicate(
            typeof(TInheritingType),
            GetMemberAccessTargetMember(expression.Body));

        Excluding(predicate);
    }

    private static MemberInfo GetMemberAccessTargetMember(Expression expression)
    {
        if (expression is UnaryExpression unaryExpression
            && unaryExpression.NodeType == ExpressionType.Convert)
        {
            // If the expression is a value type, then accessing it will involve an
            // implicit boxing conversion to type object that we need to ignore.
            expression = unaryExpression.Operand;
        }

        if (expression is MemberExpression memberExpression)
        {
            return memberExpression.Member;
        }

        throw new ArgumentException("Expression must be a simple member access", nameof(expression));
    }

    private static Expression<Func<IMemberInfo, bool>> BuildMemberSelectionPredicate(Type relatedSubjectType,
        MemberInfo referencedMember)
    {
        ParameterExpression predicateMemberInfoArgument = Expression.Parameter(typeof(IMemberInfo));

        BinaryExpression typeComparison = Expression.Equal(
            Expression.MakeMemberAccess(
                predicateMemberInfoArgument,
                typeof(IMemberInfo).GetProperty(nameof(IMemberInfo.DeclaringType))),
            Expression.Constant(relatedSubjectType));

        BinaryExpression memberNameComparison = Expression.Equal(
            Expression.MakeMemberAccess(
                predicateMemberInfoArgument,
                typeof(IMemberInfo).GetProperty(nameof(IMemberInfo.Name))),
            Expression.Constant(referencedMember.Name));

        BinaryExpression predicateBody = Expression.AndAlso(
            typeComparison,
            memberNameComparison);

        return Expression.Lambda<Func<IMemberInfo, bool>>(
            predicateBody,
            predicateMemberInfoArgument);
    }

    public new IDataEquivalencyAssertionOptions<T> Excluding(Expression<Func<IMemberInfo, bool>> predicate)
    {
        base.Excluding(predicate);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingTable(string tableName)
    {
        return ExcludingTables(tableName);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingTables(params string[] tableNames)
    {
        return ExcludingTables((IEnumerable<string>)tableNames);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingTables(IEnumerable<string> tableNames)
    {
        excludeTableNames.UnionWith(tableNames);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumnInAllTables(string columnName)
    {
        return ExcludingColumnsInAllTables(columnName);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumnsInAllTables(params string[] columnNames)
    {
        return ExcludingColumnsInAllTables((IEnumerable<string>)columnNames);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumnsInAllTables(IEnumerable<string> columnNames)
    {
        excludeColumnNames.UnionWith(columnNames);
        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumn(DataColumn column)
    {
        return ExcludingColumn(column.Table.TableName, column.ColumnName);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumns(params DataColumn[] columns)
    {
        return ExcludingColumns((IEnumerable<DataColumn>)columns);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumns(IEnumerable<DataColumn> columns)
    {
        foreach (DataColumn column in columns)
        {
            ExcludingColumn(column);
        }

        return this;
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumn(string tableName, string columnName)
    {
        return ExcludingColumns(tableName, columnName);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumns(string tableName, params string[] columnNames)
    {
        return ExcludingColumns(tableName, (IEnumerable<string>)columnNames);
    }

    public IDataEquivalencyAssertionOptions<T> ExcludingColumns(string tableName, IEnumerable<string> columnNames)
    {
        if (!excludeColumnNamesByTableName.TryGetValue(tableName, out HashSet<string> excludeColumnNames))
        {
            excludeColumnNames = new HashSet<string>();
            excludeColumnNamesByTableName[tableName] = excludeColumnNames;
        }

        excludeColumnNames.UnionWith(columnNames);

        return this;
    }

    public bool ShouldExcludeColumn(DataColumn column)
    {
        if (excludeColumnNames.Contains(column.ColumnName))
        {
            return true;
        }

        if (excludeColumnNamesByTableName.TryGetValue(column.Table.TableName, out HashSet<string> excludeColumnsForTable)
            && excludeColumnsForTable.Contains(column.ColumnName))
        {
            return true;
        }

        return false;
    }
}
