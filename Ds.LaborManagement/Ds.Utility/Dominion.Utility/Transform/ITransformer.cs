using System;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Non-typed tranformer interface.
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// Type the transformer applies to.
        /// </summary>
        Type TransformType { get; }

        /// <summary>
        /// Un-typed method which performs the defined transform action on the instance.
        /// </summary>
        /// <param name="instance">Instance to perform the transform on.</param>
        /// <returns>Transformed object.</returns>
        object Transform(object instance);
    }


    /// <summary>
    /// Represents an object that is capable of transforming a given type instance.
    /// </summary>
    /// <typeparam name="T">Type being transformed.</typeparam>
    public interface ITransformer<T> : ITransformer
    {
        /// <summary>
        /// Returns a transformed version of the given instance based on the transform configuration.
        /// </summary>
        /// <param name="instance">Instance to apply the transformation to.</param>
        /// <returns>Transformed version of the instance.</returns>
        T Transform(T instance);
    }
}