using System.Collections.Generic;

namespace Dominion.Utility.DataGeneration
{
    /// <summary>
    /// Helper class used to create random objects used for testing.
    /// </summary>
    public static class TestObjectGenerator
    {
        /// <summary>
        /// Uses the Dominion.Web.Areas.ApiDocs ObjectGenerator to generate a random object of the given type;
        /// populating all properties with random values.
        /// </summary>
        /// <typeparam name="T">Type of object to create.</typeparam>
        /// <returns>New object of the specified type with all properties populated.</returns>
        public static T GenerateObject<T>()
        {
            return (T)new ObjectGenerator().GenerateObject(typeof(T));

        }// GenerateObject()

        /// <summary>
        /// Uses the Dominion.Web.Areas.ApiDocs ObjectGenerator to generate a collection of random objects of the 
        /// given type. Note: Use only when validity of property values is not a concern.
        /// </summary>
        /// <typeparam name="T">Type of object to generate.</typeparam>
        /// <param name="objectCount">Number of objects to create.</param>
        /// <returns>Collection of objects to return.</returns>
        public static IEnumerable<T> GenerateObjects<T>(int objectCount)
        {
            return new ObjectGenerator().GenerateObjectCollection<T>(objectCount);

        }// GenerateObject()
    }
}
