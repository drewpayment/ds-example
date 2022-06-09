using System;
using System.Data;
using System.Data.SqlClient;

namespace Dominion.Utility.Query.AdoLogic
{
    public interface IAdoLogic
    {
        int? CmdTimeoutSeconds { get; }
        string CmdText { get;  }
        CommandType CmdType { get; }
        string Messages { get; }
        
        bool UseDataReaderLogic { get; }
        bool UseDataSetLogic { get; }
        bool UseMessageEventHandler { get; }

        SqlParameter[] Parameters { get; }

        Action<SqlDataReader> DataReaderLogic { get; }

        int NonQueryResult { get; set; }
        object Data { get; set; }
        void MessageEventHandler(object sender, SqlInfoMessageEventArgs e);
    }
}
