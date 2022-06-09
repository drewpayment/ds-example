using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Dominion.Utility.Containers
{
    public static class PropertyFactory
    {
        ///// <summary>
        ///// Gets the reflected PropertyInfo for the property specified in the given expression.
        ///// </summary>
        ///// <typeparam name="T">Type of object the property is part of.</typeparam>
        ///// <typeparam name="TProperty">Type of the property to get the PropertyInfo for.</typeparam>
        ///// <param name="propertyLambda">Expression defining what property to get the PropertyInfo for.</param>
        ///// <returns>PropertyInfo of the specified object's property. Throws an exception if the expression 
        ///// does not specify an immediate child property of the given type.</returns>
        ///// <remarks>
        ///// This method is based on the following StackOverflow Q&As:
        ///// http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression
        ///// http://stackoverflow.com/questions/2789504/get-the-property-as-a-string-from-an-expressionfunctmodel-tproperty/2789606#2789606
        ///// </remarks>
        //public static PropertyInfo GetPropertyInfo<T, TProperty>(this Expression<Func<T, TProperty>> propertyLambda)
        //    where T : class
        //{
        //    Type type = typeof (T);
        //    MemberExpression member;

        //    // if the compiler created an automatic conversion,
        //    // it'll look something like...
        //    // obj => Convert(obj.Property) [e.g., int -> object]
        //    // OR:
        //    // obj => ConvertChecked(obj.Property) [e.g., int -> long]
        //    // ...which are the cases checked in IsConversion
        //    if (IsConversion(propertyLambda.Body) && propertyLambda.Body is UnaryExpression)
        //    {
        //        var unaryExpression = (UnaryExpression) propertyLambda.Body;
        //        member = (MemberExpression) unaryExpression.Operand;
        //    }
        //    else
        //    {
        //        member = propertyLambda.Body as MemberExpression;
        //        if (member == null)
        //        {
        //            throw new ArgumentException(string.Format(
        //                "Expression '{0}' refers to a method, not a property.", 
        //                propertyLambda.ToString()));
        //        }
        //    }

        //    PropertyInfo propInfo = member.Member as PropertyInfo;
        //    if (propInfo == null)
        //    {
        //        throw new ArgumentException(string.Format(
        //            "Expression '{0}' refers to a field, not a property.", 
        //            propertyLambda.ToString()));
        //    }

        //    if (type != propInfo.ReflectedType &&
        //        !type.IsSubclassOf(propInfo.ReflectedType) &&
        //        !propInfo.ReflectedType.IsAssignableFrom(type))
        //    {
        //        throw new ArgumentException(string.Format(
        //            "Expression '{0}' refers to a property that is not from type {1}.", 
        //            propertyLambda.ToString(), 
        //            type));
        //    }

        //    return propInfo;
        //}

        public static PropertyInfo GetPropertyInfo<T, TReturn>(this Expression<Func<T, TReturn>> exp)
            where T : class
        {
            var type = typeof (T);
            var member = default(MemberExpression);
            var propInfo = default(PropertyInfo);

            if(exp.Body is BinaryExpression)
                member = ((BinaryExpression)exp.Body).Left as MemberExpression;
            
            if(exp.Body is UnaryExpression)
                member = ((UnaryExpression)exp.Body).Operand as MemberExpression;
            
            if(exp.Body is MemberExpression)
                member = exp.Body as MemberExpression;
            
            if (member != null)
            {
                propInfo = member.Member as PropertyInfo;
                if (propInfo == null)
                {
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a field, not a property.", 
                        exp));
                }
    
                if (type != propInfo.ReflectedType &&
                    !type.IsSubclassOf(propInfo.ReflectedType) &&
                    !propInfo.ReflectedType.IsAssignableFrom(type))
                {
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a property that is not from type {1}.", 
                        exp, 
                        type));
                }
            }
            else
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' This extension method doesn't support the expression passed to it.", 
                    exp));
            }

            return propInfo;
        }
        

        /// <summary>
        /// Checks if the given expression is the result of an automatic conversion.
        /// </summary>
        /// <param name="expr">Expression to check.</param>
        /// <returns>Indication if conversion occurs in the given expression.</returns>
        private static bool IsConversion(Expression expr)
        {
            return expr.NodeType == ExpressionType.Convert ||
                   expr.NodeType == ExpressionType.ConvertChecked;
        }

        // IsConversion()


        /// <summary>
        /// Gets the reflected PropertyInfo for the property specified in the given expression.
        /// </summary>
        /// <typeparam name="T">Type of object the property is part of.</typeparam>
        /// <typeparam name="TProperty">Type of the property to get the PropertyInfo for.</typeparam>
        /// <param name="propertyLambda">Expression defining what property to get the PropertyInfo for.</param>
        /// <param name="obj">Object to get the specified PropertyInfo from.</param>
        /// <returns>PropertyInfo of the specified object's property. Throws an exception if the expression does not specify an immediate child property of the given type.</returns>
        public static PropertyInfo GetPropertyInfo<T, TProperty>(this T obj, 
            Expression<Func<T, TProperty>> propertyLambda)
            where T : class
        {
            return GetPropertyInfo(propertyLambda);
        }

        // GetPropertyInfo()


        /// <summary>
        /// Gets the value of the property at the given index for the specified object.
        /// Returns null if no property exists at the specified index.
        /// </summary>
        /// <typeparam name="TProperty">Type the property value will be retrieved as.</typeparam>
        /// <param name="obj">The object to retrieve the property value of.</param>
        /// <param name="index">List index of the property to retrieve the value of.</param>
        /// <returns>The property value of the specified property.</returns>
        public static TProperty GetPropertyValue<T, TProperty>(this T obj, Expression<Func<T, TProperty>> propertyLambda)
            where T : class
        {
            var propertyInfo = obj.GetPropertyInfo(propertyLambda);

            if (propertyInfo != null)
            {
                object propertyValue = propertyInfo.GetValue(obj);

                if (propertyValue is TProperty)
                    return (TProperty) propertyValue;
            }

            return default(TProperty);
        }

        // GetPropertyValue()


        /// <summary>
        /// Provides a way to select a list of properties for a given object.
        /// </summary>
        /// <typeparam name="T">Type of object the properties will be selected from.</typeparam>
        /// <param name="obj">The object to select properties from.</param>
        /// <returns>New PropertyList object.</returns>
        public static PropertyList<T> CreatePropertyList<T>(this T obj)
            where T : class
        {
            return new PropertyList<T>();
        }

        // SelectProperties()    

        /// <summary>
        /// Builds a property expression for the property with the specified name. Throws exception if the specified 
        /// property could not be found.
        /// </summary>
        /// <typeparam name="T">Type containing the property.</typeparam>
        /// <param name="propertyName">Name of property to build expression for.</param>
        /// <returns>Expression representing the property of the given type.</returns>
        /// <exception cref="MissingMemberException">Throws exception propertyName is not a property of the given type.</exception>
        public static Expression<Func<T, object>> GetPropertyExpression<T>(string propertyName)
            where T : class
        {
            // attempt to get the property info 
            var propertyInfo = typeof (T).GetProperty(propertyName);

            // throw if property is not part of the given type
            if (propertyInfo == null)
            {
                throw new MissingMemberException(
                    "Property: " + propertyName + " is not a valid property of Type: " + typeof (T).Name);
            }

            // Build the property expression
            // x
            ParameterExpression param = Expression.Parameter(typeof (T), "x");

            // x.Property
            Expression property = Expression.Property(param, propertyInfo);

            // Convert(x.Property)
            Expression convert = Expression.Convert(property, typeof (object));

            // x => Convert(x.Property)
            Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(convert, param);

            return lambda;
        }

        /// <summary>
        /// Builds a property expression which maintains property type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static Expression<Func<T, TProperty>> GetPropertyExpression<T, TProperty>(string propertyName)
            where T : class
        {
            // attempt to get the property info 
            var propertyInfo = typeof (T).GetProperty(propertyName);

            // throw if property is not part of the given type
            if (propertyInfo == null)
            {
                throw new MissingMemberException(
                    "Property: " + propertyName + " is not a valid property of Type: " + typeof (T).Name);
            }

            // Build the property expression
            // x
            var param = Expression.Parameter(typeof (T), "x");

            // x.Property
            var property = Expression.Property(param, propertyInfo);

            // x => x.Property
            var lambda = Expression.Lambda<Func<T, TProperty>>(property, param);

            return lambda;
        }
    }
}