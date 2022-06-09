using System.Data;
using System.Data.SqlClient;

namespace Dominion.Utility.Query.AdoLogic
{
    public class ExecSqlCmd
    {
        #region Variables and Properties

        /// <summary>
        /// The connection string used to execute the stored proc.
        /// </summary>
        protected string ConnectionString { get; private set; }

        #endregion

        #region Constructors and Initializers

        public ExecSqlCmd(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #endregion

        #region Methods

        public virtual void Execute(IAdoLogic logic)
        {
            using (var sqlConn = new SqlConnection(ConnectionString))
            {

                if(logic.UseMessageEventHandler)
                    sqlConn.InfoMessage += logic.MessageEventHandler;

                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = logic.CmdText;
                    cmd.CommandType = logic.CmdType;
                    cmd.CommandTimeout = logic.CmdTimeoutSeconds ?? cmd.CommandTimeout;

                    if(logic.Parameters != null)
                        cmd.Parameters.AddRange(logic.Parameters);

                    cmd.Connection = sqlConn;

                    sqlConn.Open();

                    if(logic.UseDataReaderLogic)
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            logic.DataReaderLogic(reader);
                        }                        
                    }
                    else if(logic.UseDataSetLogic)
                    {
                        using (var da = new SqlDataAdapter(cmd))
                        {
                            // Fill the DataSet using default values for DataTable names, etc
                            var ds = new DataSet();
                            da.Fill(ds);

                            logic.Data = ds;
                        }                        
                    }
                    else
                    {
                        logic.NonQueryResult = cmd.ExecuteNonQuery();
                    }
                }
            }

        }

        #endregion

    }
}
