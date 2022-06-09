using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Containers
{
    /// <summary>
    /// Class representing a list of properties expressed as lambdas for the given type.
    /// </summary>
    /// <typeparam name="T">Type of object the properties will be selected from.</typeparam>
    /// <remarks>
    /// This class can be used in the following way:
    ///        var props = new PropertyList&lt;MyClass&gt;
    ///            {
    ///                x => x.Prop1,
    ///                x => x.Prop2,
    ///                x => x.Prop3
    ///            }; 
    /// </remarks>
    public class PropertyList<T> : List<Expression<Func<T, object>>>
        where T : class
    {
        private HashSet<Type> _systemTypes;

        /// <summary>
        /// Gets the current executing Assembly's System Types.
        /// </summary>
        private HashSet<Type> SystemTypes => _systemTypes ?? (_systemTypes = typeof(Assembly).Assembly.GetExportedTypes().ToHashSet());
        

        /// <summary>
        /// Iterates through each public property on an entity and compares them to the entity passed
        /// into the method and returns a PropertyList. This does not change the PrimaryKey (id) from the 
        /// curr record to the entity and should happen prior to calling this method.
        /// 
        /// TODO: MUCH SLOWER THAN JUST DOING IF STATEMENTS --- DO NOT USE
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="entity"></param>
        /// <param name="curr"></param>
        /// <returns></returns>
        public PropertyList<T> ResolvePropertyList<T1>(T1 entity, T1 curr)
        {
            var entityType = entity.GetType();
            var entityProps = entityType.GetProperties();
            var changedProps = new List<string>();
            for (var i = 0; i < entityProps.Length; i++)
            {
                var p = entityProps[i];
                var propType = p.Name.ToLower()?.IndexOf("null") > -1 ? p.PropertyType.GetGenericArguments()[0] : p.PropertyType;
                if (p.Name.ToLower() == "modified" || p.Name.ToLower() == "modifiedby") continue;
                if (!p.CanRead || !SystemTypes.Contains(propType)) continue;
                var entityValue = p.GetValue(entity);
                var currValue = p.GetValue(curr);
                if (entityValue == null && currValue == null) continue;
                if (entityValue == null || currValue == null || !entityValue.Equals(currValue))
                {
                    changedProps.Add(p.Name);
                }
            }

            this.IncludeIf(changedProps, changedProps.Any());

            return this;
        }


        public PropertyList<T> IncludeIf(Expression<Func<T, object>> propertyExpression, bool include)
        {
            if (include)
                this.Include(propertyExpression);

            return this;
        }

        public PropertyList<T> IncludeIf(IEnumerable<string> propertiesToInclude, bool include)
        {
            if (include)
                this.Include(propertiesToInclude);

            return this;
        }

        /// <summary>
        /// Adds the given property expression to the list and returns the list to be 
        /// further manipulated using fluent syntax.
        /// </summary>
        /// <param name="propertyExpression">The property expression to add.</param>
        /// <returns>The list of properties.</returns>
        public PropertyList<T> Include(Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression != null &&
                IsValidProperty(propertyExpression) &&
                !this.ContainsProperty(propertyExpression))
            {
                this.Add(propertyExpression);
            }

            return this;
        }

        // Include(Expression<>)


        /// <summary>
        /// Adds an existing property list to the current list and returns the list to be 
        /// further manipulated using fluent syntax.
        /// </summary>
        /// <param name="propertyExpression">The property list to add.</param>
        /// <returns>The list of properties.</returns>
        public PropertyList<T> Include(PropertyList<T> propertiesToInclude)
        {
            if (propertiesToInclude != null)
            {
                foreach (var propertyExpression in propertiesToInclude)
                    this.Include(propertyExpression);
            }

            return this;
        }

        // Include(PropertyList<T>)


        /// <summary>
        /// Adds the specified properties to the PropertyList and returns the list to be 
        /// further manipulated using fluent syntax.
        /// </summary>
        /// <param name="propertyNames">Names of the properties to be added.</param>
        /// <returns>The list of properties.</returns>
        public PropertyList<T> Include(IEnumerable<string> propertyNames)
        {
            if (propertyNames != null)
            {
                foreach (var propertyName in propertyNames)
                {
                    this.Include(propertyName);
                }
            }

            return this;
        }

        // Include(IEnumerable<string>)


        /// <summary>
        /// Adds the specified property to the PropertyList and returns the list to be
        /// further manipulated using fluent syntax.
        /// </summary>
        /// <param name="propertyName">Name of property to include.</param>
        /// <returns>The list of properties.</returns>
        public PropertyList<T> Include(string propertyName)
        {
                // this will throw an exception if the specified property is not 
                // part of the 
                var lambda = PropertyFactory.GetPropertyExpression<T>(propertyName);

                this.Include(lambda);

            return this;
        }

        // Include(string)


        /// <summary>
        /// Adds properties from a type with similar property names (typically from a mapped object). Only matching 
        /// property names will be included.
        /// </summary>
        /// <typeparam name="TMapped">Type to get the list of property names from.</typeparam>
        /// <param name="propertiesToInclude">Collection of property names from the mapped type to include. 
        /// Will include all properties from the mapped type if null or empty.</param>
        /// <returns>Property list to be further manipulated fluently.</returns>
        public PropertyList<T> IncludeFromMappedType<TMapped>(IEnumerable<string> propertiesToInclude = null)
            where TMapped : class
        {
            var mappedPropertyList = new PropertyList<TMapped>();

            if (propertiesToInclude == null || !propertiesToInclude.Any())
                this.Include(mappedPropertyList.IncludeAllProperties().GetPropertyNames());
            else
                this.Include(mappedPropertyList.Include(propertiesToInclude).GetPropertyNames());

            return this;
        }


        /// <summary>
        /// Populates the PropertyList with properties that satisfy the specified criteria and is of the given Type.
        /// </summary>
        /// <typeparam name="TProperty">Include only properties of this type.</typeparam>
        /// <param name="withBindingFlags">Include properties with these bindings.</param>
        /// <param name="canRead">Include properties that can be read.</param>
        /// <param name="canWrite">Include properties that can be written to.</param>
        /// <param name="hasPublicGetter">Include properties that have public getters.</param>
        /// <param name="hasPublicSetter">Include properties that have public setters.</param>
        /// <returns>The property list to be further manipulated via fluent syntax.</returns>
        public PropertyList<T> IncludeAllProperties<TProperty>(
            BindingFlags withBindingFlags = BindingFlags.Public | BindingFlags.Instance, 
            bool? canRead = true, 
            bool? canWrite = true, 
            bool? hasPublicGetter = true, 
            bool? hasPublicSetter = true)
        {
            return IncludeAllProperties(typeof (TProperty), withBindingFlags, canRead, canWrite, hasPublicGetter, 
                hasPublicSetter);
        }

        /// <summary>
        /// Populates the PropertyList with properties that satisfy the specified criteria.
        /// </summary>
        /// <param name="ofType">Include only properties of this type.</param>
        /// <param name="withBindingFlags">Include properties with these bindings.</param>
        /// <param name="canRead">Include properties that can be read.</param>
        /// <param name="canWrite">Include properties that can be written to.</param>
        /// <param name="hasPublicGetter">Include properties that have public getters.</param>
        /// <param name="hasPublicSetter">Include properties that have public setters.</param>
        /// <returns>The property list to be further manipulated via fluent syntax.</returns>
        public PropertyList<T> IncludeAllProperties(
            Type ofType = null, 
            BindingFlags withBindingFlags = BindingFlags.Public | BindingFlags.Instance, 
            bool? canRead = true, 
            bool? canWrite = true, 
            bool? hasPublicGetter = true, 
            bool? hasPublicSetter = true)
        {
            // get all properties with the specified BindingFlags
            PropertyInfo[] properties = typeof (T).GetProperties(withBindingFlags);

            foreach (PropertyInfo p in properties)
            {
                // only work with specified type if given
                if (ofType != null && p.PropertyType != ofType)
                    continue;

                // only properties that are readable (either privately or publicly)
                if (canRead != null && p.CanRead != canRead)
                    continue;

                // only properties that are writable (either privately or publicly)
                if (canWrite != null && p.CanWrite != canWrite)
                    continue;

                // limit to only public getters if specified
                if (hasPublicGetter != null)
                {
                    MethodInfo mget = p.GetGetMethod(!hasPublicGetter.Value);
                    if (mget == null)
                        continue;
                }

                // limit to only public setters if specified
                if (hasPublicSetter != null)
                {
                    MethodInfo mset = p.GetSetMethod(!hasPublicSetter.Value);
                    if (mset == null)
                        continue;
                }

                // if this code is reached the property has satisfied all of the given criteria
                // so add it to the list
                this.Include(p.Name);
            }

            return this;
        }

        // IncludeAllProperties()


        /// <summary>
        /// Remove the specified property from the list if present and return the list to be 
        /// further manipulated using fluent syntax.
        /// </summary>
        /// <param name="propertyExpression">The property expression to exclude.</param>
        /// <returns>The list of properties.</returns>
        public PropertyList<T> Exclude(Expression<Func<T, object>> propertyExpression)
        {
            if (propertyExpression != null &&
                IsValidProperty(propertyExpression) &&
                this.ContainsProperty(propertyExpression))
            {
                this.RemoveAt(IndexOfProperty(propertyExpression).Value);
            }

            return this;
        }

        // Include(Expression<>)


        /// <summary>
        /// Gets the PropertyInfo for the property expression at the given index. 
        /// Returns null if no property exists at the specified index.
        /// </summary>
        /// <param name="index">List index of the property to retrieve info for.</param>
        /// <returns>PropertyInfo of the specified property expression.</returns>
        public PropertyInfo GetPropertyInfo(int index)
        {
            var propertyExpression = this.ElementAtOrDefault(index);

            if (propertyExpression != null)
                return propertyExpression.GetPropertyInfo();

            return null;
        }

        // GetPropertyInfo()


        /// <summary>
        /// Gets the name of the property expression at the given index. 
        /// Returns null if no property exists at the specified index.
        /// </summary>
        /// <param name="index">List index of the property to retrieve the name of.</param>
        /// <returns>PropertyInfo of the specified property expression.</returns>
        public string GetPropertyName(int index)
        {
            var propertyInfo = GetPropertyInfo(index);

            if (propertyInfo != null)
                return propertyInfo.Name;

            return null;
        }

        // GetPropertyName()


        /// <summary>
        /// Gets the value of the property at the given index for the specified object.
        /// Returns null if no property exists at the specified index.
        /// </summary>
        /// <param name="obj">The object to retrieve the property value of.</param>
        /// <param name="index">List index of the property to retrieve the value of.</param>
        /// <returns>The property value of the specified property.</returns>
        public object GetPropertyValue(T obj, int index)
        {
            var propertyInfo = GetPropertyInfo(index);

            if (propertyInfo != null)
                return propertyInfo.GetValue(obj);

            return null;
        }

        // GetPropertyValue()


        /// <summary>
        /// Gets the value of the property at the given index for the specified object.
        /// Returns null if no property exists at the specified index.
        /// </summary>
        /// <typeparam name="TProperty">Type the property value will be retrieved as.</typeparam>
        /// <param name="obj">The object to retrieve the property value of.</param>
        /// <param name="index">List index of the property to retrieve the value of.</param>
        /// <returns>The property value of the specified property.</returns>
        public TProperty GetPropertyValue<TProperty>(T obj, int index)
        {
            var propertyInfo = GetPropertyInfo(index);

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
        /// Gets the property names of the properties in the list.
        /// </summary>
        /// <returns>Enumeration of property names.</returns>
        public IEnumerable<string> GetPropertyNames()
        {
            foreach (var propertyInfo in this.Select(expr => expr.GetPropertyInfo()))
                yield return propertyInfo.Name;
        }

        // GetPropertyNames()


        /// <summary>
        /// Returns the properties that are of the specified type.
        /// </summary>
        /// <typeparam name="TProperty">Type of property to retrieve.</typeparam>
        /// <returns>Collection of property expressions that will result in the given type.</returns>
        public IEnumerable<Expression<Func<T, TProperty>>> GetPropertiesOfType<TProperty>()
        {
            foreach (var property in this)
                if (property.GetPropertyInfo().PropertyType is TProperty)
                    yield return property as Expression<Func<T, TProperty>>;
        }

        // GetPropertiesOfType()


        /// <summary>
        /// Checks if the PropertyList contains the given property.
        /// </summary>
        /// <typeparam name="TProperty">Type of Property.</typeparam>
        /// <param name="propertyExpression">The property expression to check for.</param>
        /// <returns>True: Property exists in the PropertyList | False: Property does not exist in the collection.</returns>
        public bool ContainsProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return IndexOfProperty(propertyExpression) != null;
        }

        // ContainsProperty()


        /// <summary>
        /// Checks if the PropertyList contains the given property.
        /// </summary>
        /// <param name="propertyName">The property name to check for.</param>
        /// <returns>True: Property exists in the PropertyList | False: Property does not exist in the collection.</returns>
        public bool ContainsProperty(string propertyName)
        {
            return IndexOfProperty(propertyName) != null;
        }

        // ContainsProperty()


        /// <summary>
        /// Returns the index of the specified property expression or null if property was not found.
        /// </summary>
        /// <typeparam name="TProperty">Type of Property.</typeparam>
        /// <param name="propertyExpression">The property to find in the list.</param>
        /// <returns>Index of the property if found; otherwise, null.</returns>
        public int? IndexOfProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            try
            {
                var propertyName = propertyExpression.GetPropertyInfo().Name;

                return IndexOfProperty(propertyName);
            }
            catch (ArgumentException ex)
            {
                return null;
            }
        }

        // IndexOfProperty()


        /// <summary>
        /// Validates if the specified expression represents a valid property on the given type.
        /// </summary>
        /// <typeparam name="TProperty">Type of property.</typeparam>
        /// <param name="propertyExpression">Expression to be evaluated.</param>
        /// <returns>
        /// True:  Expression respresents a property. 
        /// False: Property does NOT represent a valid property.</returns>
        public bool IsValidProperty<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            try
            {
                var propertyName = propertyExpression.GetPropertyInfo().Name;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the index of the specified property expression or null if property was not found.
        /// </summary>
        /// <param name="propertyName">Name of the property to find.</param>
        /// <returns>Index of the property if found; otherwise, null.</returns>
        public int? IndexOfProperty(string propertyName)
        {
            for (int i = 0; i < this.Count(); i++)
                if (GetPropertyName(i) == propertyName)
                    return i;

            return null;
        }

        // IndexOfProperty()
    }
}