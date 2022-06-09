using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Dominion.Utility.Mapping;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Base query that all other queries derive from.
    /// </summary>
    public interface IQuery
    {
    }

    /// <summary>
    /// Represents a query that can be constructed and executed on the given object type.
    /// </summary>
    /// <typeparam name="T">Type the query is built on.</typeparam>
    public interface IQuery<T> : IQuery
    {
        /// <summary>
        /// Return queryable of type T.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> AsQueryable();

        /// <summary>
        /// Executes the given query and returns the result set.
        /// </summary>
        /// <returns>Results of the query.</returns>
        IEnumerable<T> ExecuteQuery();

        /// <summary>
        /// Executes the given query and returns the result set as the specified type.  Assumes a mapping configuration
        /// such as AutoMapper has been configured to automatically project the source type to the destination type.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        IEnumerable<TDest> ExecuteQueryAs<TDest>();

        /// <summary>
        /// Executes the given query and returns the result set as the specified type using the given mapper.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        IEnumerable<TDest> ExecuteQueryAs<TDest>(IMapper<T, TDest> mapper);

        /// <summary>
        /// Executes the given query and returns the result set as the type specified in the selector.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <param name="selector">Expression describing how to map the source type to the destination type.</param>
        /// <returns>Result set mapped to the destination type.</returns>
        IEnumerable<TDest> ExecuteQueryAs<TDest>(Expression<Func<T, TDest>> selector);

        /// <summary>
        /// Executes the given query and returns the first in the result set as the specified type.  Assumes a mapping configuration
        /// such as AutoMapper has been configured to automatically project the source type to the destination type.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        TDest FirstOrDefaultAs<TDest>();

        /// <summary>
        /// Executes the given query and returns the first in the result set as the specified type using the given mapper.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <returns>Result set mapped to the destination type.</returns>
        TDest FirstOrDefaultAs<TDest>(IMapper<T, TDest> mapper);

        /// <summary>
        /// Executes the given query and returns the first in the result set as the type specified in the selector.
        /// </summary>
        /// <typeparam name="TDest">Type the result set will be mapped to.</typeparam>
        /// <param name="selector">Expression describing how to map the source type to the destination type.</param>
        /// <returns>Result set mapped to the destination type.</returns>
        TDest FirstOrDefaultAs<TDest>(Expression<Func<T, TDest>> selector);

        /// <summary>
        /// Returns the first result in the given query or the default value if no results are found.
        /// </summary>
        /// <returns></returns>
        T FirstOrDefault();

        /// <summary>
        /// A result object that allows the query set to be further manipulated before executing the query.
        /// </summary>
        IQueryResult<T> Result { get; }

    }

    /// <summary>
    /// Query providing advanced filtering (eg: 'AND', 'OR', etc).
    /// </summary>
    /// <typeparam name="T">Type of object being queried.</typeparam>
    /// <typeparam name="TQuery">Strong-type of the query object.</typeparam>
    public interface IQuery<T, TQuery> : IQuery<T>
        where TQuery : class, IQuery<T, TQuery>
    {
        /// <summary>
        /// Puts the query in 'AND'-mode. All subsequent filter calls will be 'AND'-ed with the existing query.
        /// </summary>
        TQuery And { get; }

        /// <summary>
        /// Puts the query in 'OR'-mode. All subsequent filter calls will be 'Or'-ed with the existing query.
        /// </summary>
        TQuery Or { get; }

    }
}