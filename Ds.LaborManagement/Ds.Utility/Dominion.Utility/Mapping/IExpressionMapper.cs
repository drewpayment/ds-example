using System;
using System.Linq.Expressions;

namespace Dominion.Utility.Mapping
{
    /// <summary>
    /// Defines a mapping between two object types by defining a custom map expression. 
    /// </summary>
    /// <remarks>
    /// One current limitation of this interface, is that it can only be implemented
    /// once per class or "downstream" interface. Even though this can be implemented
    /// for arbitrary (<typeparamref name="TSource"/>, <typeparamref name="TDest"/>),
    /// because a class/interface can only have one <see cref="MapExpression"/> property.
    /// </remarks>
    /// <typeparam name="TSource">Source object type to be mapped FROM.</typeparam>
    /// <typeparam name="TDest">Destination object type to be mapped TO.</typeparam>
    public interface IHasMapExpression<TSource, TDest>
    {
        Expression<Func<TSource, TDest>> MapExpression { get; }
    }

    /// <summary>
    /// Defines a mapping between two object types by defining a custom map expression. 
    /// </summary>
    /// <typeparam name="TSource">Source object type to be mapped FROM.</typeparam>
    /// <typeparam name="TDest">Destination object type to be mapped TO.</typeparam>
    public interface IExpressionMapper<TSource, TDest> : IMapper<TSource, TDest>, IHasMapExpression<TSource, TDest>
    {
        // No new methods, is just a union of the implemented "upstream" interfaces.
    }
}