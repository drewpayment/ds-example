using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dominion.Utility.Containers
{
    /// <summary>
    /// This class represents a list of object properties expressed as lambdas.
    /// </summary>
    /// <remarks>
    /// Example:
    ///   Say that you have an object, myObj, with properties Prop1 and Prop2. Creating a list
    ///   that contains these properties would look like this:
    ///   
    ///   var props = new PropertyList
    ///   {
    ///     () => myObj.Prop1,
    ///     () => myObj.Prop2
    ///   }
    /// </remarks>
    public class PropertyList : List<Expression<Func<object>>>
    {
        /// <summary>
        /// Get the name of the object property at the given index.
        /// </summary>
        /// <param name="listIndex">The list index of the target property.</param>
        /// <returns>The name of the object property at the given index, or null if the index
        /// does not exist.</returns>
        public string GetName(int listIndex)
        {
            if (listIndex >= this.Count)
                return null;

            return GetPropertyName(this[listIndex]);
        }

        /// <summary>
        /// Get the value of the object property at the given index.
        /// </summary>
        /// <param name="listIndex">The list index of the target property.</param>
        /// <returns>The value of the object property at the given index, or null if the index
        /// does not exist.</returns>
        public object GetValue(int listIndex)
        {
            if (listIndex >= this.Count)
                return null;

            return GetPropertyValue(this[listIndex]);
        }

        /// <summary>
        /// Adds the specified property to the list and then returns the list 
        /// so additional properties can be specified using "fluent" syntax.
        /// </summary>
        /// <param name="propertyExpression">The property to add.</param>
        /// <returns>The PropertyList so additional properties can be included fluently.</returns>
        /// <remarks>
        /// The following method allows the syntax below:
        ///        var props = new PropertyList()
        ///            .Include( () => obj.Prop1 )
        ///            .Include( () => obj.Prop2 );
        /// </remarks>
        public PropertyList Include(Expression<Func<object>> propertyExpression)
        {
            if (propertyExpression != null)
                this.Add(propertyExpression);

            return this;
        }

        // Include()

        /// <summary>
        /// Get the name of the given property.
        /// </summary>
        /// <param name="property">The property who's name is to be retrieved.</param>
        /// <returns>The name of the given property.</returns>
        public static string GetPropertyName(Expression<Func<object>> property)
        {
            var lambda = (LambdaExpression) property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression) lambda.Body;
                memberExpression = (MemberExpression) unaryExpression.Operand;
            }
            else
            {
                memberExpression = (MemberExpression) lambda.Body;
            }

            return memberExpression.Member.Name;
        }

        // GetPropertyName()

        /// <summary>
        /// Get the value of the given property.
        /// </summary>
        /// <param name="property">The property who's value is to be retrieved.</param>
        /// <returns>The value of the given object property.</returns>
        public static object GetPropertyValue(Expression<Func<object>> property)
        {
            return property.Compile()();
        }
    }
}