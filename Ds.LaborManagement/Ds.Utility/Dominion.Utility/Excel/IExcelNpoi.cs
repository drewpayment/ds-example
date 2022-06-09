using System;
using System.Collections.Generic;
using System.Data;
using Dominion.Utility.Mapping.DataReader;
using NPOI.SS.UserModel;

namespace Dominion.Utility.Excel
{
    public interface IExcelNpoi
    {
        /// <summary>
        /// Get all the sheet names from the workbook
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        IEnumerable<string> GetSheetNames();

        /// <summary>
        /// Will return the first matching or first sheet (if any exist).
        /// Don't think I've ever met an excell workbook without a sheet.
        /// </summary>
        /// <param name="wb">Workbook instance.</param>
        /// <param name="matchingNameCriteria">The sheet name matching criteria.</param>
        /// <returns></returns>
        string GetSheetNameMatchingOrFirst(Func<string, bool> matchingNameCriteria = null);

        /// <summary>
        /// Convert a workbook's sheet data to a collection of objects.
        /// This assumes a header row and all cell value types are string; which means that the object's values must be string too (quick and dirty edition).
        /// You can provide a mapper to convert the strings to a different type if you need; again quick and dirty for now.
        /// This will first convert the excel data to a datatable, then convert it to a list of object; it seems performant so no worries.
        /// lowfix: jay: have the conversion support other types as well as string.
        /// lowfix: jay: can there be a direct conversion to object; not having to convert to datatable?
        /// </summary>
        /// <typeparam name="T">Type to map the dataset to.</typeparam>
        /// <param name="wb">Workbook instance.</param>
        /// <param name="sheetName">The sheet name you want the data from.</param>
        /// <returns></returns>
        IEnumerable<T> To<T>(Func<string, bool> matchingNameCriteria = null, IDataReaderMapper<T> mapper = null, IEnumerable<string> headerColumnNames = null)
            where T : class;

        /// <summary>
        /// http://stackoverflow.com/questions/17615672/npoi-with-asp-net-c
        /// Convert a sheet to a datatable, all types will be string (quick and dirty)
        /// Assuming that there is a header row, if not it's only one row of data (quick and dirty)
        /// lowfix: jay: have the conversion support other types as well as string.
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sheetName"></param>
        /// <param name="headerColumnNames">Header column names. Names must be included in correct column order. 
        /// If null (default), column headers will be derived from first row of sheet.</param>
        /// <returns></returns>
        DataTable ToDataTable(string sheetName, IEnumerable<string> headerColumnNames = null);

    }
}
