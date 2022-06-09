using System;
using System.Linq.Expressions;
using Dominion.Utility.Query;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.Transform
{
    public interface ITransformRule<T>
    {
        TransformRule<T> When(Expression<Func<T, bool>> rule);
    }

    /// <summary>
    /// Holds rule information for when a transform should be applied to a given type.
    /// </summary>
    /// <typeparam name="T">Type the transform is applied to.</typeparam>
    public class TransformRule<T> : Transformer<T>, ITransformRule<T>
    {
        #region VARIABLES & PROPERTIES

        private Expression<Func<T, bool>> _rule;
        private readonly ITransformer<T> _transformer;

        #endregion

        #region CONSTRUCTOR(S)

        /// <summary>
        /// Instantiates a new TransformRule object.
        /// </summary>
        /// <param name="transformer">Transform to apply rules to.</param>
        public TransformRule(ITransformer<T> transformer)
        {
            _transformer = transformer;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Adds a rule to the transform that must be satisified for the transform to be applied.
        /// </summary>
        /// <param name="rule">Rule to check before applying the transform.</param>
        /// <returns>The current transform rule object to be further manipulated.</returns>
        public TransformRule<T> When(Expression<Func<T, bool>> rule)
        {
            And(rule);

            return this;
        }

        /// <summary>
        /// ORs any existing rules with the given rule.
        /// </summary>
        /// <param name="rule">Rule to OR with any existing rules.</param>
        /// <returns>The current transform rule object to be further manipulated.</returns>
        public TransformRule<T> Or(Expression<Func<T, bool>> rule)
        {
            _rule = _rule == null ? rule : _rule.Or(rule);

            return this;
        }

        /// <summary>
        /// ANDs any existing rules with the given rule.
        /// </summary>
        /// <param name="rule">Rule to AND with any existing rules.</param>
        /// <returns>The current transform rule object to be further manipulated.</returns>
        public TransformRule<T> And(Expression<Func<T, bool>> rule)
        {
            _rule = _rule == null ? rule : _rule.And(rule);

            return this;
        }

        /// <summary>
        /// Returns a transformed version of the given instance if either the rules has been satisfied or no rules have 
        /// been specified. Note: 
        /// </summary>
        /// <param name="instance">Original version to transform.</param>
        /// <returns>Transformed version of given value.</returns>
        public override T Transform(T instance)
        {
            if (_rule == null || (_rule != null && _rule.Invoke(instance)))
                return _transformer.Transform(instance);

            return instance;
        }

        #endregion
    }
}