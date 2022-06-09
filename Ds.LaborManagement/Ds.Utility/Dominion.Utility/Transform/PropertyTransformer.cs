using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Utility.Containers;
using Dominion.Utility.Query;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Defines permission rules for a given object type's property.
    /// </summary>
    /// <typeparam name="T">Type of object the property exists on.</typeparam>
    /// <typeparam name="TProperty">Property type the permissions are being applied to.</typeparam>
    public class PropertyTransformer<T, TProperty> : Transformer<T>, ITransformBuilder<TProperty> where T : class
    {
        #region PROPERTIES & VARIABLES

        private Expression<Func<T, bool>> _rule = null;

        private readonly List<TransformRule<TProperty>> _transformRules;

        private readonly Expression<Func<T, TProperty>> _property;

        /// <summary>
        /// Name of the property this permission applies to.
        /// </summary>
        public string PropertyName
        {
            get { return _property.GetPropertyInfo().Name; }
        }

        #endregion

        #region CONSTRUCTOR(S)

        /// <summary>
        /// Instantiates a new PropertyPermission object.
        /// </summary>
        /// <param name="property">Property expression the permissions will be defined for.</param>
        public PropertyTransformer(Expression<Func<T, TProperty>> property)
        {
            _property = property;

            _transformRules = new List<TransformRule<TProperty>>();
        }

        #endregion

        /// <summary>
        /// Adds a new transform rule to the property permissions.
        /// </summary>
        /// <param name="transform">Transformation object defined what type of transform to apply.</param>
        /// <returns>Newly added TransformRule that can be further manipulated.</returns>
        public TransformRule<TProperty> AddTransform(ITransformer<TProperty> transform)
        {
            var rule = new TransformRule<TProperty>(transform);

            _transformRules.Add(rule);

            return rule;
        }


        /// <summary>
        /// Transforms the given object's property based on the transform rules applied to the permission object.  
        /// Transforms will only be applied if their associated rules are satisfied.
        /// </summary>
        /// <param name="instance">Instance to tranform the property of.</param>
        public override T Transform(T instance)
        {
            if (instance != null && (_rule == null || _rule.Invoke(instance)))
            {
                foreach (var rule in _transformRules)
                {
                    // get the current property's value
                    var propertyValue = _property.Invoke(instance);

                    // transform the current value
                    // note: the value will only be transformed if the rule is satisfied
                    var transformedValue = rule.Transform(propertyValue);

                    _property.GetPropertyInfo().SetValue(instance, transformedValue);
                }
            }

            return instance;
        }

        public PropertyTransformer<T, TProperty> When(Expression<Func<T, bool>> rule)
        {
            if (_rule == null)
                _rule = rule;

            return this;
        }
    }
}