using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Base class for containing all of the arguments for a specific Sproc class.
    /// A sproc class is any class the used the base <see cref="SprocBase"/>
    /// </summary>
    public class SprocParametersBase
    {
        #region Properties and Variables

        /// <summary>
        /// A dictionary of SqlParameter builders.
        /// </summary>
        protected IDictionary<string, ISqlParameterBuilder> Arguments { get; private set; }

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// Constructor.
        /// </summary>
        public SprocParametersBase()
        {
            Arguments = new Dictionary<string, ISqlParameterBuilder>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Generates the <see cref="SqlParameter"/> (s) needed for a sproc.
        /// </summary>
        /// <returns>All the stored in this class represented by <see cref="ISqlParameterBuilder"/>.</returns>
        public IEnumerable<SqlParameter> GenerateSqlParameters()
        {
            var list = new List<SqlParameter>();

            //lowFix: jay: can add conditional logic to skip on nullable params that are null
            Arguments.Values.ForEach(x => list.Add(x.Build()));

            return list;
        }

        /// <summary>
        /// A convenience method for adding a parameter.
        /// </summary>
        /// <typeparam name="TVal">The .NET value type for the param.</typeparam>
        /// <param name="paramName">The name for the param (starting with an '@'.</param>
        /// <param name="dbType">The sql type</param>
        /// <returns>The sql param builder it just added.</returns>
        protected SqlParameterBuilder<TVal> AddParameter<TVal>(string paramName, SqlDbType dbType)
        {
            return AddParameter(paramName, dbType, default(TVal));
        }

        /// <summary>
        /// highfix: jay: throw an exception if the same param name is being added.
        /// A convenience method for adding a parameter.
        /// </summary>
        /// <typeparam name="TVal">The .NET value type for the param.</typeparam>
        /// <param name="paramName">The name for the param (starting with an '@'.</param>
        /// <param name="dbType">The sql type</param>
        /// <param name="value">The value for the parameter.</param>
        /// <returns>The sql param builder it just added.</returns>
        protected SqlParameterBuilder<TVal> AddParameter<TVal>(string paramName, SqlDbType dbType, TVal value)
        {
            ISqlParameterBuilder value1;
            if(!Arguments.TryGetValue(paramName, out value1))
            {
                var obj = new SqlParameterBuilder<TVal>(paramName, dbType, value);
                Arguments.Add(paramName, obj);
                value1 = obj;
            }

            return value1 as SqlParameterBuilder<TVal>;
        }

        protected void SetParamValue<TVal>(ISqlParameter<TVal> pb, TVal val)
        {
            pb.Value = val;
        }

        protected TVal GetParamValue<TVal>(ISqlParameter<TVal> pb)
        {
            return pb.Value;
        }

        #endregion

    }
}
