using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dominion.Utility.Query
{
    public abstract class SprocBase<TResult, TArgs>
        where TResult : class
        where TArgs : SprocParametersBase
    {
        #region Variables and Properties

        /// <summary>
        /// The connection string used to execute the stored proc.
        /// </summary>
        protected string ConnectionString { get; private set; }

        /// <summary>
        /// The object that represents the parameters needed for a sproc.
        /// </summary> 
        protected TArgs Arguments { get; set; }

        /// <summary>
        /// Default is 180 seconds: 3 mins.
        /// </summary>
        protected int CommandTimeoutSeconds { get; set; }

        #endregion

        #region Constructors and Initializers

        protected SprocBase(string connectionString, TArgs args)
        {
            ConnectionString = connectionString;
            Arguments = args;
        }

        #endregion

        #region Methods

        public abstract TResult Execute();

        protected virtual TResult ExecuteSproc(string sprocName, Func<SqlDataReader, TResult> dataLogic)
        {
            TResult data;

            using (var sqlConn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = sprocName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = CommandTimeoutSeconds;
                    cmd.Parameters.AddRange(Arguments.GenerateSqlParameters().ToArray());
                    cmd.Connection = sqlConn;

                    sqlConn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        data = dataLogic(reader);
                    }
                }
            }

            return data;
        }

        #endregion

    }
}
