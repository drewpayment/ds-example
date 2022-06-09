using System.Collections.Generic;
using System.Linq;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Query object providing extended filtering options (eg: 'AND' and 'OR').
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    public class Query<T, TQuery> : Query<T>, IQuery<T, TQuery>
        where TQuery : class, IQuery<T, TQuery>
    {
        #region Variables and Properties

        private readonly TQuery _typedQuery;

        /// <summary>
        /// Puts the query in 'AND'-mode. All subsequent filter calls will be 'AND'-ed with the existing query.
        /// </summary>
        public TQuery And
        {
            get
            {
                FilterMode = QueryFilterMode.And;
                return _typedQuery;
            }
        }

        /// <summary>
        /// Puts the query in 'OR'-mode. All subsequent filter calls will be 'Or'-ed with the existing query.
        /// </summary>
        public TQuery Or
        {
            get
            {
                FilterMode = QueryFilterMode.Or;
                return _typedQuery;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new <see cref="Query{T, TQuery}"/>.
        /// </summary>
        /// <param name="data">Data to build the query on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public Query(IEnumerable<T> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
            _typedQuery = this as TQuery;
        }

        #endregion

    }
}
