//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dominion.Utility.Mapping.DataReader;
//using NPOI.SS.UserModel;

//namespace Dominion.Utility.ExtensionMethods
//{
//    public static class NpoiExtentionMethods
//    {
//        /// <summary>
//        /// Get all the sheet names from the workbook
//        /// </summary>
//        /// <param name="wb"></param>
//        /// <returns></returns>
//        public static IEnumerable<string> GetSheetNames(this IWorkbook wb)
//        {
//            var list = new List<string>();
//            var loop = true;
//            var counter = 0;

//            do
//            {
//                try
//                {
//                    var sheetName = wb.GetSheetAt(counter).SheetName;
//                    list.Add(sheetName);
//                }
//                catch(Exception)
//                {
//                    loop = false;
//                }

//                counter++;
//            } while(loop);

//            return list;
//        }

//        /// <summary>
//        /// Will return the first matching or first sheet (if any exist).
//        /// Don't think I've ever met an excell workbook without a sheet.
//        /// </summary>
//        /// <param name="wb">Workbook instance.</param>
//        /// <param name="matchingNameCriteria">The sheet name matching criteria.</param>
//        /// <returns></returns>
//        public static string GetSheetNameMatchingOrFirst(this IWorkbook wb, Func<string, bool> matchingNameCriteria = null)
//        {
//            var list = wb.GetSheetNames().ToList();
//            var name = default(string);

//            //get a sheet name based on the criteria
//            name = matchingNameCriteria == null
//                ? null
//                : list.FirstOrDefault(matchingNameCriteria);
            
//            //get the first sheet if there were no matches and the list contains sheet names.
//            name = name == null && list.Any() 
//                ? list.First() 
//                : null;

//            return name;
//        }

//        /// <summary>
//        /// Convert a workbook's sheet data to a collection of objects.
//        /// This assumes a header row and all cell value types are string; which means that the object's values must be string too (quick and dirty edition).
//        /// You can provide a mapper to convert the strings to a different type if you need; again quick and dirty for now.
//        /// This will first convert the excel data to a datatable, then convert it to a list of object; it seems performant so no worries.
//        /// lowfix: jay: have the conversion support other types as well as string.
//        /// lowfix: jay: can there be a direct conversion to object; not having to convert to datatable?
//        /// </summary>
//        /// <typeparam name="T">Type to map the dataset to.</typeparam>
//        /// <param name="wb">Workbook instance.</param>
//        /// <param name="sheetName">The sheet name you want the data from.</param>
//        /// <returns></returns>
//        public static IEnumerable<T> To<T>(this IWorkbook wb, string sheetName, IDataReaderMapper<T> mapper = null)
//             where T : class
//        {
//            var dtos = wb.ToDataTable(sheetName).AsEnumerable<T>();
//            return dtos;
//        }

//        /// <summary>
//        /// http://stackoverflow.com/questions/17615672/npoi-with-asp-net-c
//        /// Convert a sheet to a datatable, all types will be string (quick and dirty)
//        /// Assuming that there is a header row, if not it's only one row of data (quick and dirty)
//        /// lowfix: jay: have the conversion support other types as well as string.
//        /// </summary>
//        /// <param name="wb"></param>
//        /// <param name="sheetName"></param>
//        /// <returns></returns>
//        public static DataTable ToDataTable(this IWorkbook wb, string sheetName)
//        {
//            var dt = new DataTable();
//            var sheet = wb.GetSheet(sheetName);
//            var rows = sheet.GetRowEnumerator();

//            while(rows.MoveNext())
//            {
//                var row = (IRow)rows.Current;

//                //we're assuming that there is a header row, if not it's only one row of data
//                if(dt.Columns.Count == 0)
//                {
//                    for(int j = 0; j < row.LastCellNum; j++)
//                        dt.Columns.Add(row.GetCell(j).ToString(), typeof(string));

//                    continue;
//                }

//                var dr = dt.NewRow();
//                for(int i = 0; i < row.LastCellNum; i++)
//                {
//                    var cell = row.GetCell(i);

//                    if(cell == null)
//                    {
//                        dr[i] = null;
//                    }
//                    else
//                    {
//                        dr[i] = cell.ToString();
//                    }
//                }
//                dt.Rows.Add(dr);
//            }

//            return dt;
//        }

//    }

//}
