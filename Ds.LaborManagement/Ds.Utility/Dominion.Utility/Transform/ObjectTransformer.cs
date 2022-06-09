using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Utility.Containers;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Provides the ability to define a tranformer around the given class type.
    /// </summary>
    /// <typeparam name="T">Class type the transformer applies to.</typeparam>
    public class ObjectTransformer<T> : Transformer<T>, ITransformBuilder<T> where T : class
    {
        private readonly List<ITransformer<T>> _transformers;

        /// <summary>
        /// Instantiates a new ObjectTransformer instance.
        /// </summary>
        public ObjectTransformer()
        {
            _transformers = new List<ITransformer<T>>();
        }

        /// <summary>
        /// Defines a transformer for the specified property.
        /// </summary>
        /// <typeparam name="TProperty">Type of the property.</typeparam>
        /// <param name="property">Property expression (eg: x => x.Property1).</param>
        /// <returns>Property transformer that multiple transforms can be attached to.</returns>
        public PropertyTransformer<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> property)
        {
            var propertyName = property.GetPropertyInfo().Name;

            var propertyTransformer = _transformers.OfType<PropertyTransformer<T, TProperty>>()
                .FirstOrDefault(x => x.PropertyName == propertyName);

            if (propertyTransformer == null)
            {
                propertyTransformer = new PropertyTransformer<T, TProperty>(property);
                _transformers.Add(propertyTransformer);
            }

            return propertyTransformer;
        }

        /// <summary>
        /// Adds the specified object-level transformer to the collection of transformers and returns a variant in which
        /// rules can be applied.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public TransformRule<T> AddTransform(ITransformer<T> transform)
        {
            var rule = new TransformRule<T>(transform);

            _transformers.Add(rule);

            return rule;
        }

        /// <summary>
        /// Transforms the given instance based on the currently defined transformers.
        /// </summary>
        /// <param name="instance">Object to transform.</param>
        /// <returns>Transformed version of the object.</returns>
        public override T Transform(T instance)
        {
            foreach (var transformer in _transformers)
                transformer.Transform(instance);

            return instance;
        }
    }
}