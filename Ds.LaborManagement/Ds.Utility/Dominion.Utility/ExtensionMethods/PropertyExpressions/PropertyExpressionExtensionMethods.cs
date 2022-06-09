using System;
using System.Linq.Expressions;

namespace Dominion.Utility.ExtensionMethods.PropertyExpressions
{
    /// <summary>
    /// lowFix: jay: I think this is duplicated code. Search for the PropertyFactory class.
    /// </summary>
    public static class PropertyExpressionExtensionMethods
    {
        /// <summary>
        /// Gets the name of the property name of the property this expression represents.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPropertyType">The property type.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <returns>Name of the property/member.</returns>
        public static string GetPropertyName<TEntity, TPropertyType>(this Expression<Func<TEntity, TPropertyType>> exp)
            where TEntity : class
        {
            return (exp.Body as MemberExpression).Member.Name;
        }

        /// <summary>
        /// Get the value from the object that the property expression represents.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPropertyType">The property type.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <param name="obj">The instanciated object.</param>
        /// <returns>The value the expression represents.</returns>
        public static TPropertyType GetMemeberValue<TEntity, TPropertyType>(
            this Expression<Func<TEntity, TPropertyType>> exp, TEntity obj) where TEntity : class
        {
            return exp.Compile().Invoke(obj);
        }

        /// <summary>
        /// Get the member type name from the expression.
        /// It gets the underlying type for nullable types.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPropertyType">The property type.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <returns>The type's name.</returns>
        public static string GetMemeberTypeName<TEntity, TPropertyType>(
            this Expression<Func<TEntity, TPropertyType>> exp) where TEntity : class
        {
            Type type = (exp.Body as MemberExpression).Type;
            return type.GetTypeName();
        }

        /// <summary>
        /// Get the member type from the expression.
        /// It gets the underlying type for nullable types.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPropertyType">The property type.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <returns>The type of the property.</returns>
        public static Type GetMemeberType<TEntity, TPropertyType>(this Expression<Func<TEntity, TPropertyType>> exp)
            where TEntity : class
        {
            Type type = (exp.Body as MemberExpression).Type;
            return type;
        }

        /// <summary>
        /// Find out if the member's type is nullable.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPropertyType">The property type.</typeparam>
        /// <param name="exp">The expression.</param>
        /// <returns>True if nullable.</returns>
        public static bool IsMemberTypeNullable<TEntity, TPropertyType>(
            this Expression<Func<TEntity, TPropertyType>> exp) where TEntity : class
        {
            Type type = (exp.Body as MemberExpression).Type;
            return type.IsNullableType();
        }
    }
}