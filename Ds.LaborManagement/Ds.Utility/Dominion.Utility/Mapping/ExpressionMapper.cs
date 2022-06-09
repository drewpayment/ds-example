using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dominion.Utility.Mapping
{
    /// <summary>
    /// Defines a mapping between two object types by defining a custom map expression. 
    /// </summary>
    /// <typeparam name="TSource">Source object type to be mapped FROM.</typeparam>
    /// <typeparam name="TDest">Destination object type to be mapped TO.</typeparam>
    public abstract class ExpressionMapper<TSource, TDest> : Mapper<TSource, TDest>, IExpressionMapper<TSource, TDest>
    {
        /// <summary>
        /// Expression defining how to map from the source-type to the destination-type.
        /// </summary>
        public abstract Expression<Func<TSource, TDest>> MapExpression { get; }

        private Func<TSource, TDest> _mapSelector;

        /// <summary>
        /// A compiled version of the <see cref="MapExpression"/>.  This is used to map single object instances and 
        /// <see cref="IEnumerable{TSource}"/> collections which are not <see cref="IQueryable{TSource}"/>.
        /// </summary>
        protected Func<TSource, TDest> MapSelector
        {
            get
            {
                return this._mapSelector ?? (this._mapSelector = this.MapExpression.Compile());
            }
        }

        /// <summary>
        /// Maps an instance of the source-type to the destination-type.
        /// </summary>
        /// <param name="obj">Object containing the source information to map from.</param>
        /// <returns>Object containing the mapped information.</returns>
        public override TDest Map(TSource obj)
        {
            return this.MapSelector(obj);
        }

        /// <summary>
        /// Maps a collection of source-type objects to a collection of destination-type objects.  If the 
        /// <see cref="source"/> is <see cref="IQueryable{TSource}"/> the returned collection will remain queryable.
        /// </summary>
        /// <param name="source">Object collection containing the information to map from.</param>
        /// <returns></returns>
        public override IEnumerable<TDest> Map(IEnumerable<TSource> source)
        {
            if(source is IQueryable<TSource>)
                return (source as IQueryable<TSource>).Select(this.MapExpression);

            return source.Select(this.MapSelector);
        }
    }
}