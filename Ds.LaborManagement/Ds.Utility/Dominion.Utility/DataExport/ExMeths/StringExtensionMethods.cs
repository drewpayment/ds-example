//using System.Data;
//using System.Data.SqlClient;
//using System.IO;

//namespace Dominion.Utility.DataExport.ExMeths
//{
//    public static class StringExtensionMethods
//    {
//        /// <summary>
//        /// Creates a directory for the path provdied if it doesn't exist.
//        /// </summary>
//        /// <param name="path"></param>
//        /// <returns></returns>
//        public static string IfNoDirectoryCreateIt(this string path)
//        {
//            var dirPath = new FileInfo(path).DirectoryName;
    
//            if(!Directory.Exists(dirPath))
//                Directory.CreateDirectory(dirPath);
    
//            return path;
//        }

//        /// <summary>
//        /// Get a data table from sql for a single result set sql statement.
//        /// </summary>
//        /// <param name="sql"></param>
//        /// <param name="connStr"></param>
//        /// <returns></returns>
//        public static DataTable ToDataTable(this string sql, string connStr, string tableName = "")
//        {
//            var dt = new DataTable(tableName);
//            var conn = new SqlConnection(connStr);
//            var cmd = new SqlCommand(sql, conn);
//            conn.Open();

//            // create data adapter
//            var da = new SqlDataAdapter(cmd);
        
//            // this will query your database and return the result to your datatable
//            da.Fill(dt);
//            conn.Close();
//            da.Dispose();

//            if(string.IsNullOrEmpty(tableName) && string.IsNullOrEmpty(dt.TableName))
//                dt.TableName = tableName;

//            return dt;
//        }

//        /// <summary>
//        /// Make sure the string has the same number of chars by padding the incoming value with spaces to the right.
//        /// </summary>
//        /// <param name="str"></param>
//        /// <param name="lengthDesired"></param>
//        /// <returns></returns>
//        public static string PadStringRight(this string str, int lengthDesired)
//        {
//            return str.PadRight(lengthDesired, ' ');
//        }
//    }
//}
