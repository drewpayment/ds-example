using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// A wrapper for a sql parameter.
    /// This a class for defining a sql parameter which will be 'built' when needed.
    /// </summary>
    /// <typeparam name="TVal">The .NET type this parameter represents.</typeparam>
    public class SqlParameterBuilder<TVal> : ISqlParameter<TVal>, ISqlParameterBuilder
    {
        #region Properties and Variables

        /// <summary>
        /// The name of the parameter as it is on the sproc.
        /// Include the '@' in front of the name.
        /// </summary>
        public string ParamName { get; private set; }

        /// <summary>
        /// The SQL type this parameter represents.
        /// </summary>
        public SqlDbType SqlType { get; private set; }

        /// <summary>
        /// The .NET type this parameter represents.
        /// </summary>
        public TVal Value { get; set; }

        #endregion

        #region Constructor

        public SqlParameterBuilder(string paramName, SqlDbType sqlType)
            : this(paramName, sqlType, default(TVal))
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="sqlType"></param>
        /// <param name="value"></param>
        public SqlParameterBuilder(string paramName, SqlDbType sqlType, TVal value)
        {
            ParamName = paramName;
            SqlType = sqlType;
            Value = value;
        }

        /// <summary>
        /// Builds the <see cref="SqlParameter"/>.
        /// A null value will be set to a <see cref="DBNull"/> value since that is what is expected for null values at the <see cref="SqlParameter"/> level.
        /// </summary>
        /// <returns>SqlParameter.</returns>
        public SqlParameter Build()
        {
            var isNullable = Value.IsTypeNullable();
            var param = new SqlParameter(ParamName, SqlType) {Value = (object)Value ?? DBNull.Value};
            param.IsNullable = isNullable;
            return param;
        }

        #endregion

    }
}
