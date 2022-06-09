using System.Data;

using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    public class DataColumnNotFoundMsg : MsgBase<DataColumnNotFoundMsg>
    {
        /// <summary>
        /// Table missing the designated column.
        /// </summary>
        public DataTable Table { get; set; }

        /// <summary>
        /// Missing column.
        /// </summary>
        public string ColumnName { get; set; }

        public DataColumnNotFoundMsg(DataTable table, string columnName)
            : base(MsgLevels.Error, 0)
        {
            Table      = table;
            ColumnName = columnName;
        }

        protected override string BuildMsg()
        {
            return ColumnName + " column not found in table '" + Table.TableName + "'.";
        }
    }
}