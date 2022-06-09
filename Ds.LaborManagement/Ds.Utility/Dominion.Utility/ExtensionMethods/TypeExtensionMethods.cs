using System;

namespace Dominion.Utility.ExtensionMethods
{
    public static class TypeExtensionMethods
    {
        /// <summary>
        /// Checks if the given type is the same as another type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>
        /// PER: http://stackoverflow.com/questions/9234009/c-sharp-type-comparison-type-equals-vs-operator
        /// </remarks>
        public static bool Equals<T>(this Type type)
        {
            return type.Equals(typeof(T));
        }

        /// <summary>
        /// This will get the type name for any object including nullable types.
        /// This means the underlying type in the case of nullable types, not the type name for Nullable.
        /// </summary>
        /// <param name="type">The type in question.</param>
        /// <returns>Name of the type or underlying type.</returns>
        public static string GetTypeName(this Type type)
        {
            return type.GetUnderlyingType().Name;
        }

        /// <summary>
        /// Checks if <see cref="type"/> is nullable and returns the underlying type. Otherwise, returns the original type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns></returns>
        /// <remarks>
        /// derived from: http://stackoverflow.com/questions/3531318/convert-changetype-fails-on-nullable-types
        /// </remarks>
        public static Type GetUnderlyingType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        /// <summary>
        /// Checks if <see cref="T"/> is nullable and returns the underlying type. Otherwise, returns the original type.
        /// </summary>
        /// <typeparam name="T">Type to check.</typeparam>
        /// <returns></returns>
        public static Type GetUnderlyingType<T>()
        {
            return typeof(T).GetUnderlyingType();
        }

        /// <summary>
        /// Returns the default value of the type. Similar to C#'s default() operator, but works for <see cref="Type"/>
        /// instances.
        /// </summary>
        /// <param name="type">Type to get default value of.</param>
        /// <returns></returns>
        /// <remarks>
        /// SRC: http://stackoverflow.com/questions/2490244/default-value-of-a-type-at-runtime
        /// </remarks>
        public static object GetDefaultValue(this Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Returns true if the specified type is nullable.
        /// </summary>
        /// <param name="type">Type to check for nullability.</param>
        /// <returns></returns>
        /// <remarks>
        /// SRC: http://stackoverflow.com/questions/6026824/detecting-a-nullable-type-via-reflection (2nd Answer)
        /// </remarks>
        public static bool IsNullableType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Returns true if the specified type is nullable.
        /// </summary>
        /// <typeparam name="T">Type to check for nullability.</typeparam>
        /// <returns></returns>
        public static bool IsNullableType<T>()
        {
            return IsNullableType(typeof(T));
        }
    }
}