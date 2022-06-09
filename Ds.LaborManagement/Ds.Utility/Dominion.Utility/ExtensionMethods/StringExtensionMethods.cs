using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dominion.Utility.Constants;

namespace Dominion.Utility.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        /// <summary>
        /// Default strings that will be evaluated as true.
        /// </summary>
        public static readonly string[] DefaultTruthyStrings = { "1", "y", "yes", "true" };

        /// <summary>
        /// Return the string followed with ": "
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string CreateSummaryLabel(this string fieldName)
        {
            return fieldName + ": ";
        }

        /// <summary>
        /// Treat a string like a directory and delete all files in that directory that match the search pattern.
        /// </summary>
        /// <param name="dirPath">The string that represents a path.</param>
        /// <param name="searchPattern">The pattern that determines what files are deleted.</param>
        /// <returns></returns>
        public static void DirectoryDeleteAllFiles(
            this string dirPath, 
            string searchPattern, 
            SearchOption searchOptions = SearchOption.TopDirectoryOnly)
        {
            var list = Directory.GetFiles(dirPath, searchPattern, searchOptions);

            foreach (var item in list)
                File.Delete(item);
        }

        /// <summary>
        /// Attempts to convert the given string into the specified type.  If the coversion is fails, the specified
        /// default value will be returned.
        /// </summary>
        /// <typeparam name="T">Type to convert the string to.</typeparam>
        /// <param name="textToConvert">String to convert.</param>
        /// <param name="defaultValue">Default value to return if the conversion fails.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertToOrDefault<T>(this string textToConvert, T defaultValue = default(T))
        {
            try
            {
                return textToConvert.ConvertTo(defaultValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Does a lot of the split string options in one easier call.
        /// </summary>
        /// <param name="str">The string to split.</param>
        /// <param name="includeNullOrwWhiteSpaceItems">False if you don't want any.</param>
        /// <param name="separator">The separator to split with.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitEx(
            this string str, 
            bool includeNullOrWhiteSpaceItems = true,
            char separator = ',')
        {
            if(str != null)
            {
                var items = str.Split(separator);
                return items.Where(x => includeNullOrWhiteSpaceItems || !String.IsNullOrWhiteSpace(x));
            }

            return new List<string>();
        }

        /// <summary>
        /// Creates a directory for the path provdied if it doesn't exist.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string IfNoDirectoryCreateIt(this string path)
        {
            var dirPath = new FileInfo(path).DirectoryName;
    
            if(!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
    
            return path;
        }

        /// <summary>
        /// Get a data table from sql for a single result set sql statement.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string sql, string connStr, string tableName = "")
        {
            var dt = new DataTable(tableName);
            var conn = new SqlConnection(connStr);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();

            // create data adapter
            var da = new SqlDataAdapter(cmd);
        
            // this will query your database and return the result to your datatable
            da.Fill(dt);
            conn.Close();
            da.Dispose();

            if(String.IsNullOrEmpty(tableName) && String.IsNullOrEmpty(dt.TableName))
                dt.TableName = tableName;

            return dt;
        }

        /// <summary>
        /// Make sure the string has the same number of chars by padding the incoming value with spaces to the right.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lengthDesired"></param>
        /// <returns></returns>
        public static string PadStringRight(this string str, int lengthDesired)
        {
            return str.PadRight(lengthDesired, ' ');
        }

        public static string Ellipsis(this string s, int charsToDisplay)
        {
            if (!String.IsNullOrWhiteSpace(s))
                return s.Length <= charsToDisplay-1 ? s : new string(s.Take(charsToDisplay-3).ToArray()) + "...";
            return String.Empty;
        }

        /// <summary>
        /// Converts several string values to bool, instead of just 'true' or '1'.
        /// If string is null it will return false (has a null check).
        /// String is trimmed before being evaluated.
        /// I added this for an import that accepted 'yes/no' for bool field.
        /// </summary>
        /// <param name="str">The string that should represent a bool.</param>
        /// <param name="truthyValues">String values that will be evaluated as 'true'. 
        /// Values are compared w/ <see cref="StringComparison.OrdinalIgnoreCase"/>. 
        /// Default values are [1, y, yes, true] (see <see cref="DefaultTruthyStrings"/>).</param>
        /// <returns></returns>
        public static bool ConvertToBool(this string str, IEnumerable<string> truthyValues = null)
        {
            var answer = false;

            truthyValues = truthyValues ?? DefaultTruthyStrings;

            if(str != null)
            {
                var val = str.Trim();
                answer = truthyValues.Any(t => t.Equals(val, StringComparison.OrdinalIgnoreCase));
            }
            
            return answer;
        }

        /// <summary>
        /// http://stackoverflow.com/a/26435053
        /// </summary>
        /// <returns></returns>
        public static string NullCheckTrim(this string s)
        {
            return s == null ? null : s.Trim();
        }

        /// <summary>
        /// Cuts the string to the given length. If the string is shorter than the given length, then it is returned without modification.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="maxLength">The maximum length that the returned string should be.</param>
        /// <returns></returns>
        public static string Truncate(this string s, int maxLength)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            return s.Substring(0, Math.Min(maxLength, s.Length));
        }
		
        /// <summary>
        /// Removes all white space from a string.
        /// http://stackoverflow.com/a/26435053
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string RemoveWhiteSpace(this string str)
        {
            return new string(str.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }

        public static string TryTrim(this string str, bool returnEmptyStringIfNull = false)
        {
            return string.IsNullOrEmpty(str) 
                ? (returnEmptyStringIfNull) ? (str ?? string.Empty) : str
                : str.Trim();
        }

        /// <summary>
        /// Returns the decimal representation of a string. Common special characters are removed 
        /// prior to conversion (e.g. ',').  If conversion fails, null is returned.
        /// </summary>
        /// <param name="str">String to convert to decimal.</param>
        /// <returns></returns>
        public static decimal? ToDecimal(this string str)
        {
            var scrubbed = string.IsNullOrWhiteSpace(str) ? CommonConstants.EMPTY_STRING : str.Replace(",","");
            decimal d;
            return decimal.TryParse(scrubbed, out d) ? d : default(decimal?);
        }


        public static Stream ToStream(this string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string ToJsStringOrNull(this string str)
        {
            return str == null ? "null" : $"'{str}'";
        }

        /// <summary>
        /// Adds HTML tags to beginning and end of string to italicize the string.
        /// </summary>
        /// <param name="str">The string to be italicized.</param>
        /// <returns></returns>
        public static string Italicize(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Insert(str.Length, NotificationConstants.EMPHASIS_END_TAG).Insert(0, NotificationConstants.EMPHASIS_TAG);
        }

        /// <summary>
        /// Adds HTML tags to beginning and end of string to bolden the string.
        /// </summary>
        /// <param name="str">The string to be boldened.</param>
        /// <returns></returns>
        public static string Bold(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Insert(str.Length, NotificationConstants.BOLD_END_TAG).Insert(0, NotificationConstants.BOLD_TAG);
        }

        /// <summary>
        /// Adds HTML tags to beginning and end of string to underline the string.
        /// </summary>
        /// <param name="str">The string to be underlined.</param>
        /// <returns></returns>
        public static string Underline(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Insert(str.Length, "</u>").Insert(0, "<u>");
        }

        /// <summary>
        /// Adds HTML tags to beginning and end of string to reduce the font size
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MakeSmall(this string str)
        {
            return string.IsNullOrEmpty(str)
                ? str
                : str.Insert(str.Length, NotificationConstants.SMALL_END_TAG).Insert(0, NotificationConstants.SMALL_TAG);
        }
    }
}