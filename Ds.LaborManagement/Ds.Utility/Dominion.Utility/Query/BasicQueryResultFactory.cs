using System;
using System.Linq;

using NLog;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Constructs query results using the base implementation <see cref="QueryResult{T}"/>.  For EF datasources query
    /// execution will NOT be thread safe using this factory type.
    /// </summary>
    public class BasicQueryResultFactory : IQueryResultFactory
    {
        #region Static Instance

        private static readonly Lazy<BasicQueryResultFactory> _instance = new Lazy<BasicQueryResultFactory>();

        public static IQueryResultFactory Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        #endregion

        /// <summary>
        /// Constructs a new <see cref="IQueryResult{T}"/> on the provided data query.
        /// </summary>
        /// <typeparam name="T">Data type being queried.</typeparam>
        /// <param name="dataQuery"></param>
        /// <returns></returns>
        IQueryResult<T> IQueryResultFactory.BuildResult<T>(IQueryable<T> dataQuery)
        {
            return new QueryResult<T>(dataQuery, this);
        }
    }
}
