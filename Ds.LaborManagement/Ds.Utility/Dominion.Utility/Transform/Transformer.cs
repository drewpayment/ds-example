using System;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Base tranformer class used to define a new Transformer object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Transformer<T> : ITransformer<T>
    {
        Type ITransformer.TransformType
        {
            get { return typeof (T); }
        }

        public abstract T Transform(T instance);

        //public virtual T TransformAndValidate(T instance)
        //{
        //    return Transform(instance);
        //}

        /// <summary>
        /// Transforms the specified instance if it is of the same type as handled by the Transformer.
        /// </summary>
        /// <param name="instance">Object to transform.</param>
        /// <returns>Transformed version of the object.</returns>
        object ITransformer.Transform(object instance)
        {
            // apply the transform only if it is of a valid type
            return instance is T ? Transform((T) instance) : instance;
        }

    }
}