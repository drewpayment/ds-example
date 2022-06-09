using System;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Extension methods for various Transform objects.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Adds a social security mask with the given settings to the builder.
        /// </summary>
        /// <param name="transformBuilder">Transform builder to add the social security mask to.</param>
        /// <param name="settings">Settings defining how to mask the social security number.</param>
        /// <returns></returns>
        public static TransformRule<string> AddSocialSecurityNumberMask(
            this ITransformBuilder<string> transformBuilder, 
            SocialSecurityNumberMaskSettings settings)
        {
            return transformBuilder.AddTransform(new SocialSecurityNumberMask(settings));
        }

        /// <summary>
        /// Adds a generic transformer with the given transform action to the builder.
        /// </summary>
        /// <typeparam name="T">Type the transformer will apply to.</typeparam>
        /// <param name="transformBuilder">Builder to add the transform action to.</param>
        /// <param name="transformAction">Action defining how to transform the object.</param>
        /// <returns></returns>
        public static TransformRule<T> AddGenericTransform<T>(
            this ITransformBuilder<T> transformBuilder, 
            Func<T, T> transformAction)
        {
            return transformBuilder.AddTransform(new GenericTransformer<T>(transformAction));
        }

        /// <summary>
        /// Adds a <see cref="TrimTransform"/> action to the builder.
        /// </summary>
        /// <param name="builder">The builder to add the trim transform to.</param>
        /// <param name="ruleBuilder">A function that constructs the rule conditions as needed.</param>
        /// <returns></returns>
        public static ITransformBuilder<string> AddTrim(this ITransformBuilder<string> builder, Func<ITransformRule<string>, ITransformRule<string>> ruleBuilder = null)
        {
            var rule = builder.AddTransform(new TrimTransform());
            ruleBuilder?.Invoke(rule);
            return builder;
        }

        /// <summary>
        /// Adds a <see cref="EmptyToNullTransform"/> action to the builder.
        /// </summary>
        /// <param name="builder">The builder to add the trim transform to.</param>
        /// <param name="ruleBuilder">A function that constructs the rule conditions as needed.</param>
        /// <returns></returns>
        public static ITransformBuilder<string> AddEmptyToNull(this ITransformBuilder<string> builder, Func<ITransformRule<string>, ITransformRule<string>> ruleBuilder = null)
        {
            var rule = builder.AddTransform(new EmptyToNullTransform());
            ruleBuilder?.Invoke(rule);
            return builder;
        }

        /// <summary>
        /// Executes the given setup action on the given object and then returns the original object.
        /// </summary>
        /// <typeparam name="T">The type of the object to setup.</typeparam>
        /// <param name="builder">The object to setup.</param>
        /// <param name="setupAction">The setup action you want to perform.</param>
        /// <returns></returns>
        public static T Setup<T>(
            this T builder,
            Action<T> setupAction)
        {
            setupAction(builder);
            return builder;
        }
    }
}