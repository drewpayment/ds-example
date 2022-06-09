using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping.DataReader;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;

namespace Dominion.Utility.Excel
{
    public class ExcelNpoi : IExcelNpoi
    {
        #region Static

        /// <summary>
        /// Set this if you wish for testing purposes.
        /// If you set this, everytime you call this will be used to get the workbook object.
        /// </summary>
        internal static IWorkbook WorkbookTestHook;

        /// <summary>
        /// Gets a workbook object from a file path.
        /// This is a READONLY object.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Workbook object (interface).</returns>
        internal  static IWorkbook GetNpoiWorkbookFromFile(string filePath)
        {
            var wb = default(IWorkbook);

            //if the test hook has been set return that
            if(WorkbookTestHook != null)
                return WorkbookTestHook;

            //build a workbook using a real excel file; welcome to the future
            using(var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                wb = WorkbookFactory.Create(fs);

            return wb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static IExcelNpoi Create(string filePath)
        {
            return new ExcelNpoi(GetNpoiWorkbookFromFile(filePath));
        }

        /// <summary>
        /// Constructs a new excel object used to manipulate the excel workbook in the provided stream. NOTE: Make sure
        /// stream is disposed of properly (ie: wrap Create call in using statement).
        /// </summary>
        /// <param name="stream">Stream containing the excel file to maniplulate. </param>
        /// <returns></returns>
        public static IExcelNpoi Create(Stream stream)
        {
            return new ExcelNpoi(WorkbookFactory.Create(stream));
        }

        #endregion

        #region Properties and Variables

        protected IWorkbook Workbook { get; private set; }

        internal IExcelNpoi Self
        {
            get { return this; }
        }

        #endregion

        #region Constructor and Initializers

        /// <summary>
        /// Use the static factory method 'Create(filePath)' to build this object.
        /// </summary>
        /// <param name="workbook">Workbook of excel file to work with.</param>
        internal ExcelNpoi(IWorkbook workbook)
        {
            Workbook = workbook;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all the sheet names from the workbook
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        IEnumerable<string> IExcelNpoi.GetSheetNames()
        {
            var list = new List<string>();

            for(int i = 0; i < this.Workbook.NumberOfSheets; i++)
                list.Add(this.Workbook.GetSheetAt(i).SheetName);                

            return list;
        }

        /// <summary>
        /// Will return the first matching or first sheet (if any exist).
        /// Don't think I've ever met an excell workbook without a sheet.
        /// </summary>
        /// <param name="wb">Workbook instance.</param>
        /// <param name="matchingNameCriteria">The sheet name matching criteria.</param>
        /// <returns></returns>
        string IExcelNpoi.GetSheetNameMatchingOrFirst(Func<string, bool> matchingNameCriteria)
        {
            var list = Self.GetSheetNames().ToList();

            //get a sheet name based on the criteria; if the criteria was included.
            var name = matchingNameCriteria == null
                ? null
                : list.FirstOrDefault(matchingNameCriteria);

            //get the first sheet if there were no matches and the list contains sheet names.
            name = name == null && list.Any()
                ? list.First()
                : name;

            return name;
        }

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
        IEnumerable<T> IExcelNpoi.To<T>(Func<string, bool> matchingNameCriteria, IDataReaderMapper<T> mapper, IEnumerable<string> headerColumnNames)
        {
            var sheetName = Self.GetSheetNameMatchingOrFirst(matchingNameCriteria);
            var dtos = Self.ToDataTable(sheetName, headerColumnNames).AsEnumerable<T>(mapper);
            return dtos.ToList();
        }

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
        DataTable IExcelNpoi.ToDataTable(string sheetName, IEnumerable<string> headerColumnNames)
        {
            var dt = new DataTable();
            var sheet = Workbook.GetSheet(sheetName);
            var rows = sheet.GetRowEnumerator();
            var lastColumnIndex = 0;

            headerColumnNames = headerColumnNames.NullCheckToList();
           
            while(rows.MoveNext())
            {
                var row = (IRow)rows.Current;

                //read the header row
                if(dt.Columns.Count == 0)
                {
                    if (headerColumnNames == null)
                    {
                        //derive column headers from first row
                        //for each cell in the first row
                        for (var j = 0; j < row.LastCellNum; j++)
                        {
                            //only add a column if the cell is not a whitespace or null value
                            if(!string.IsNullOrWhiteSpace(row.GetCell(j).ToString()))
                            {
                                dt.Columns.Add(
                                    row.GetCell(j).ToString().RemoveWhiteSpace(), 
                                    typeof(string)); //adding everything as string; ha ha ha

                                lastColumnIndex++;
                            }
                        }
                        continue;
                    }
                    
                    //otherwise load column headers from specified names
                    lastColumnIndex = headerColumnNames.Count();
                    for (var j = 0; j < lastColumnIndex; j++)
                    {
                        dt.Columns.Add(
                            headerColumnNames.ElementAt(j), 
                            typeof(string));
                    }
                }

                //read the data rows
                var dr = dt.NewRow();
                for(var i = 0; i < lastColumnIndex; i++)
                {
                    var cell = row.GetCell(i);
                    dr[i] = cell.ToStringWithNullCheck().NullCheckTrim();
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        #endregion
    }
}

