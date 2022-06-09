using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Base object used to construct and execute a query.
    /// </summary>
    /// <typeparam name="T">Type being queried.</typeparam>
    public class Query<T> : IQuery<T>
    {
        protected enum QueryFilterMode
        {
            And,
            Or
        }

        #region Variables and Properties

        private readonly IQueryResultFactory _resultFactory;

        public IQueryable<T> AsQueryable()
        {
            return this.Build();
        }

        /// <summary>
        /// A result object that allows the query set to be further manipulated before executing the query.
        /// </summary>
        public IQueryResult<T> Result
        {
            get
            {
                this.Build();
                return _resultFactory.BuildResult(this.DataQuery);
            }
        }

        /// <summary>
        /// Data being queried.
        /// </summary>
        protected IQueryable<T> DataQuery
        {
            get;
            set;
        }

        /// <summary>
        /// Setting indicating if the filteres are currently being 'AND-ed' or 'OR-ed' together.
        /// </summary>
        protected QueryFilterMode FilterMode
        {
            get; 
            set;
        }

        /// <summary>
        /// Predicate containing all filters that have been applied so far.
        /// </summary>
        protected Expression<Func<T, bool>> FilterPredicate
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of currently defined sort criteria for the query.  These will be applied to the query upon
        /// execution.
        /// </summary>
        protected IList<ISorter<T>> Sorters
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Instantiates a new <see cref="Query{T}"/>.
        /// </summary>
        /// <param name="data">Object set to build and execute the query on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public Query(IEnumerable<T> data, IQueryResultFactory resultFactory = null)
        {
            this.DataQuery        = data.AsQueryable();
            this._resultFactory   = resultFactory ?? BasicQueryResultFactory.Instance;

            this.FilterMode       = QueryFilterMode.And;
            this.FilterPredicate  = null;
        }

        #endregion

        #region IQuery Implementation

        public virtual void Load<TEntity, TPropType>(Expression<Func<T, TPropType>> selector)
        {
            
        }

        /// <summary>
        /// Executes the given query and returns the result set.
        /// </summary>
        /// <returns>Results of the query.</returns>
        public virtual IEnumerable<T> ExecuteQuery()
        {
            return Result.Execute();
        }

        /// <summary>
        /// Executes the given query and returns the result set as the specified type.  Assumes a mapping configuration
        /// such as AutoMapper has been configured to automatically project the source type to the destination type.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        public virtual IEnumerable<TDest> ExecuteQueryAs<TDest>()
        {
            return Result.MapTo<TDest>().Execute();
        }

        /// <summary>
        /// Executes the given query and returns the result set as the specified type using the given mapper.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        public virtual IEnumerable<TDest> ExecuteQueryAs<TDest>(IMapper<T, TDest> mapper)
        {
            return Result.MapTo(mapper).Execute();
        }

        /// <summary>
        /// Executes the given query and returns the first in the result set as the type specified in the selector.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <param name="selector">Expression describing how to map the source type to the destination type.</param>
        /// <returns>Result set mapped to the destination type.</returns>
        public virtual TDest FirstOrDefaultAs<TDest>(Expression<Func<T, TDest>> selector)
        {
            return Result.MapTo(selector).FirstOrDefault();
        }

        /// <summary>
        /// Executes the given query and returns the first in the result set as the specified type.  Assumes a mapping configuration
        /// such as AutoMapper has been configured to automatically project the source type to the destination type.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        public virtual TDest FirstOrDefaultAs<TDest>()
        {
            return Result.MapTo<TDest>().FirstOrDefault();
        }

        /// <summary>
        /// Executes the given query and returns the first in the result set as the specified type using the given mapper.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        public virtual TDest FirstOrDefaultAs<TDest>(IMapper<T, TDest> mapper)
        {
            return Result.MapTo(mapper).FirstOrDefault();
        }

        /// <summary>
        /// Executes the given query and returns the result set as the type specified in the selector.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <param name="selector">Expression describing how to map the source type to the destination type.</param>
        /// <returns>Result set mapped to the destination type.</returns>
        public virtual IEnumerable<TDest> ExecuteQueryAs<TDest>(Expression<Func<T, TDest>> selector)
        {
            return Result.MapTo(selector).Execute();
        }

        /// <summary>
        /// Returns the first result in the given query or the default value if no results are found.
        /// </summary>
        /// <returns></returns>
        public virtual T FirstOrDefault()
        {
            return Result.FirstOrDefault();
        }

        #endregion

        #region Private Helpers
        
        /// <summary>
        /// Applies all filters and ordering to the <see cref="DataQuery"/>.
        /// </summary>
        /// <returns>Data with all filters and ordering applied.</returns>
        protected IQueryable<T> Build()
        {
            this.AddCriteria();
            this.AddOrdering();
            return this.DataQuery;
        }

        /// <summary>
        /// Adds the given filter to the query.  Filter will be 'AND'-ed or 'OR'-ed based on the the current 
        /// filter-mode state of the query.
        /// </summary>
        /// <param name="filterExpression">Expression representing the filter to apply to the query.</param>
        /// <returns>Query to be further maniplulated.</returns>
        protected void FilterBy(Expression<Func<T, bool>> filterExpression)
        {
            if(this.FilterPredicate == null) this.FilterPredicate = filterExpression;
            else
            {
                switch(FilterMode)
                {
                    case QueryFilterMode.And:
                        this.FilterPredicate = this.FilterPredicate.And(filterExpression);
                        break;
                    case QueryFilterMode.Or:
                        this.FilterPredicate = this.FilterPredicate.Or(filterExpression);
                        break;
                }
            }
        }

        /// <summary>
        /// Adds a sorter to the query with the given order-expression.  Upon execution, query will be ordered accordingly.
        /// </summary>
        /// <typeparam name="TCriteria">Type the query will be ordered by.</typeparam>
        /// <param name="orderExpression">Expression containing the criteria to order by.</param>
        /// <param name="direction">Indication if the results should be ordered in ascending or descending order.</param>
        protected void OrderBy<TCriteria>(Expression<Func<T, TCriteria>> orderExpression, SortDirection direction = SortDirection.Ascending)
        {
            if (Sorters == null)
            {
                Sorters = new List<ISorter<T>>();
            }

            Sorters.Add(new SortExpression<T, TCriteria>(orderExpression, direction));
        }

        /// <summary>
        /// This will be caused before the query is executed. 
        /// It will add any criteria you specified via the And/Or FilterBy clauses.
        /// </summary>
        private void AddCriteria()
        {
            if(this.FilterPredicate != null)
            {
                this.DataQuery = this.DataQuery
                    .Where(this.FilterPredicate.Expand());
                
                // clear the filters now that they have been applied to the data
                this.FilterPredicate = null;
            }
        }

        /// <summary>
        /// Adds ordering to the query before being executed.
        /// </summary>
        private void AddOrdering()
        {
            if(Sorters == null)
                return;

            var first = true;
            foreach(var sorter in Sorters)
            {
                this.DataQuery = sorter.Sort(this.DataQuery, first);
                first = false;
            }

            // clear the sorters now that they have been applied
            Sorters = null;
        }

        #endregion
    }
}