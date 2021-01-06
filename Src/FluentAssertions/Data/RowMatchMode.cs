using System.Data;

namespace FluentAssertions.Data
{
    /// <summary>
    /// Indicates how <see cref="DataRow"/> objects from different <see cref="DataTable"/> objects should be matched
    /// up for equivalency comparisons.
    /// </summary>
    public enum RowMatchMode
    {
        /// <summary>
        /// Indicates that <see cref="DataRow"/> objects should be matched up by their index within the
        /// <see cref="DataTable.Rows"/> collection. This is the default.
        /// </summary>
        Index,

        /// <summary>
        /// Indicates that <see cref="DataRow"/> objects should be matched up by the values they have for
        /// the table's <see cref="DataTable.PrimaryKey"/>. For this to work, the rows must be from
        /// <see cref="DataTable"/> objects with exactly equivalent <see cref="DataTable.PrimaryKey"/>
        /// configuration.
        /// </summary>
        PrimaryKey,
    }
}
