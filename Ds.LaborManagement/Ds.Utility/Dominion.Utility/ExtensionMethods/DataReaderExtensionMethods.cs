using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Dominion.Utility.Mapping.DataReader;

using Newtonsoft.Json;

namespace Dominion.Utility.ExtensionMethods
{
    public static class DataReaderExtensionMethods
    {
        /// <summary>
        /// Maps a <see cref="IDataReader"/> to the specified type enumeration.  Return value is a true enumeration
        /// allowing additional expressions to be added to the results expression tree. Use .ToList() obtain the actual
        /// results.
        /// </summary>
        /// <typeparam name="TDest">Destination type.</typeparam>
        /// <param name="reader">Reader containing the data to convert.</param>
        /// <param name="mapper">Optional. Mapper to use to map the reader to objects.  Defaults to a new 
        /// <see cref="QuickDataReaderMapper{TDest}"/>.</param>
        /// <returns></returns>
        public static IEnumerable<TDest> AsEnumerable<TDest>(this IDataReader reader, IDataReaderMapper<TDest> mapper = null)
        {
            return (mapper ?? new QuickDataReaderMapper<TDest>()).Map(reader);
        }

        /// <summary>
        /// Convert the datareader to the object you specify.
        /// The table columns and the object's property names and types must match.
        /// </summary>
        /// <typeparam name="TObj">The object you are converting the datareader data to.</typeparam>
        /// <param name="reader">The datareader object.</param>
        /// <returns></returns>
        public static TObj ToObject<TObj>(this IDataReader reader)
        {
            var r = reader.SerializeToDictionary();
            var json = JsonConvert.SerializeObject(r, Formatting.Indented);
            var obj = JsonConvert.DeserializeObject<TObj>(json);
            return obj;
        }

        /// <summary>
        /// Serialize a data reader to a dictionary.
        /// </summary>
        /// <param name="reader">The datareader object.</param>
        /// <returns></returns>
        public static IEnumerable<Dictionary<string, object>> SerializeToDictionary(this IDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(reader.SerializeRowToDictionary(cols));

            return results;
        }

        /// <summary>
        /// Serialize a row from data reader to a dictionary.
        /// </summary>
        /// <param name="reader">The datareader object.</param>
        /// <param name="cols">List of columns names from the data reader.</param>
        /// <returns></returns>
        public static Dictionary<string, object> SerializeRowToDictionary(
            this IDataReader reader, 
            IEnumerable<string> cols)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        /// <summary>
        /// AddWithValue specifiying the <see cref="SqlDbType"/> of the parameter
        /// </summary>
        /// <param name="collection">The collection to add the parameter to</param>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="value">The value of the parameter</param>
        /// <param name="sqlDbType"><see cref="SqlDbType"/> which the SqlCommand is expecting for the created parameter</param>
        public static void AddSqlParamWithValue(this List<SqlParameter> collection, string parameterName, object value, SqlDbType sqlDbType)
        {
                var result = new SqlParameter(parameterName, sqlDbType) { Value = value ?? DBNull.Value };
                collection.Add(result);
        }

        /// <summary>
        /// Returns the value, of type T, from the <see cref="IDataReader"/>, accounting for both generic and non-generic types.
        /// This should be called within a <see cref="IDataReader.Read"/> loop when an actual record is available.
        /// </summary>
        /// <typeparam name="T">T, type applied</typeparam>
        /// <param name="reader">The <see cref="IDataReader"/> object that queried the database.</param>
        /// <param name="columnName">The column of data to retrieve a value from</param>
        /// <param name="defaultValue">Optional. Default value to return if the object is null or <see cref="DBNull"/>.</param>
        /// <returns>T, type applied; default value of type if database value is null</returns>
        /// <remarks>
        /// Derived From: http://stackoverflow.com/questions/18550769/sqldatareader-best-way-to-check-for-null-values-sqldatareader-isdbnull-vs-dbnul
        /// </remarks>
        public static T GetValueOrDefault<T>(this IDataReader reader, string columnName, T defaultValue = default(T))
        {
            // Read the value out of the reader by string (column name); returns object
            var value = reader[columnName];

            // Check for null value from the datasource 
            // If value was null in the datasource, return the default value for T; this will vary based on what T is (i.e. int has a default of 0)
            // otherwise convert & return
            return value != DBNull.Value ? value.ConvertTo(defaultValue: defaultValue) : defaultValue;

        }


        public static object GetValueOrDefault(this IDataReader reader, string columnName, Type convertType)
        {
            // Read the value out of the reader by string (column name); returns object
            var value = reader[columnName];

            // Check for null value from the datasource 
            // If value was null in the datasource, return the default value for T; this will vary based on what T is (i.e. int has a default of 0)
            // otherwise convert & return
            return value != DBNull.Value ? value.ConvertTo(convertType) : convertType.GetDefaultValue();

        }
    }
}