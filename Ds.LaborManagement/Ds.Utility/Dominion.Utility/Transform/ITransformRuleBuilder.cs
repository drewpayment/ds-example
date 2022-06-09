namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Interface providing the ability to add multiple transforms for the given type.
    /// </summary>
    /// <typeparam name="T">Type the transforms apply to.</typeparam>
    public interface ITransformBuilder<T>
    {
        /// <summary>
        /// Adds the specified transform to the transform builder.
        /// </summary>
        /// <param name="transform">Transform to add.</param>
        /// <returns>Transform rule containing the added transform.</returns>
        TransformRule<T> AddTransform(ITransformer<T> transform);
    }
}