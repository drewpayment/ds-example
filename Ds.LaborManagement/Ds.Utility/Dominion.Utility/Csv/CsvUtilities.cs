using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvConfiguration = CsvHelper.Configuration.Configuration;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Csv
{
    using System.Data;

    /// <summary>
    /// Provides helper methods used to manipulate CSV data.
    /// </summary>
    public static class CsvUtilities
    {
        public static readonly IFileSystem FileSystem = new FileSystem();

        /// <summary>
        /// Gets the object representation of the data stored in the specified CSV file.
        /// </summary>
        /// <typeparam name="TMap">Type of object to map the CSV rows to.</typeparam>
        /// <param name="filename">Full path of the CSV file to load data from.</param>
        /// <param name="config"><see cref="CsvConfiguration"/> used to objectify the CSV.</param>
        /// <param name="fileSystem">The file system to use to access the file path. If null, the standard System.IO 
        /// tools will be used. This parameter is mainly for testing.</param>
        /// <returns>Task that will return a collection of objects once loaded.</returns>
        public static async Task<IOpResult<IEnumerable<TMap>>> LoadObjectsFromCsv<TMap>(string filename,
            Configuration config = null, IFileSystem fileSystem = null)
        {
            using (var stream = (fileSystem ?? FileSystem).File.OpenRead(filename))
            {
                return await LoadObjectsFromCsv<TMap>(stream, config);
            }
        }

        /// <summary>
        /// Gets the object representation of the data stored in the specified CSV file stream.
        /// </summary>
        /// <typeparam name="TMap">Type of object to map the CSV rows to.</typeparam>
        /// <param name="stream">File stream containing the CSV data to load from.</param>
        /// <param name="config"><see cref="Configuration"/> used to objectify the CSV.</param>
        /// <returns>Task that will return a collection of objects once loaded.</returns>
        public static async Task<IOpResult<IEnumerable<TMap>>> LoadObjectsFromCsv<TMap>(Stream stream,
            Configuration config = null)
        {
            using (var reader = new StreamReader(stream))
            {
                return await LoadObjectsFromCsv<TMap>(reader, config);
            }
        }

        /// <summary>
        /// Gets the object representation of the data stored in the specified CSV file stream.
        /// </summary>
        /// <typeparam name="TMap">Type of object to map the CSV rows to.</typeparam>
        /// <param name="reader">Text reader containing the CSV data to load from.</param>
        /// <param name="config"><see cref="Configuration"/> used to objectify the CSV.</param>
        /// <returns>Task that will return a collection of objects once loaded.</returns>
        public static async Task<IOpResult<IEnumerable<TMap>>> LoadObjectsFromCsv<TMap>(TextReader reader,
            Configuration config = null)
        {
            var result = new OpResult<IEnumerable<TMap>>();

            // create a default configuration if one was not specified
            if (config == null)
                config = new Configuration();

            var csv = new CsvReader(reader, config);

            try
            {
                result.Data = await new TaskFactory<IEnumerable<TMap>>().StartNew(csv.GetRecords<TMap>().ToList);
            }
            catch (CsvHelperException ex)
            {
                result.AddMessage(new CsvHelperExceptionMsg(ex)).SetToFail();
            }

            return result;
        }

        /// <summary>
        /// Converts the given <see cref="DataTable"/> into a CSV string.
        /// </summary>
        /// <param name="table">The table that should be converted into a CSV.</param>
        /// <param name="writeHeader">Whether the column headers should be written as the first line.</param>
        /// <returns></returns>
        public static string ToCsv(this DataTable table, bool writeHeader = true)
        {
            using (StringWriter writer = new StringWriter())
            {
                var csv = new CsvWriter(writer);
                if (writeHeader)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        csv.WriteField(column.ColumnName);
                    }
                    csv.NextRecord();
                }
                foreach (var row in table.AsEnumerable())
                {
                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        csv.WriteField(row[i]);
                    }
                    csv.NextRecord();
                }
                return writer.ToString();
            }
        }

        public static DataTable CsvToDataTable(string filePath)
        {
            var dt = new DataTable();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                // Do any configuration to `CsvReader` before creating CsvDataReader.
                using (var dr = new CsvDataReader(csv))
                {        
                    dt.Load(dr);
                }
            }
            return dt;
        }

        /// <summary>
        /// Reads a CSV into a DataTable.
        /// </summary>
        /// <remarks>See https://joshclose.github.io/CsvHelper/examples/data-table/ for more info.</remarks>
        /// <param name="csvFilePath">File path to the CSV to read into the data table.</param>
        /// <param name="columnTypes">Dictionary of pre-defined column names and their associate type.
        /// When specified, the columns will be loaded into the data table and converted to the given type.</param>
        /// <returns></returns>
        public static DataTable ToDataTable(string csvFilePath, IDictionary<string, Type> columnTypes = null)
        {
            using (var reader = new StreamReader(csvFilePath))
            {
                return ToDataTable(reader, columnTypes);
            }
        }

        /// <summary>
        /// Reads a CSV into a DataTable.
        /// </summary>
        /// <remarks>See https://joshclose.github.io/CsvHelper/examples/data-table/ for more info.</remarks>
        /// <param name="reader">Text reader pointing to the CSV data to read.</param>
        /// <param name="columnTypes">Dictionary of pre-defined column names and their associate type.
        /// When specified, the columns will be loaded into the data table and converted to the given type.</param>
        public static DataTable ToDataTable(TextReader reader, IDictionary<string, Type> columnTypes = null)
        {
            using (var csv = new CsvReader(reader))
            {
                // Do any configuration to `CsvReader` before creating CsvDataReader.
                using (var dr = new CsvDataReader(csv))
                {        
                    var dt = new DataTable();

                    if (columnTypes != null)
                    {
                        foreach(var col in columnTypes)
                        {
                            dt.Columns.Add(col.Key, col.Value);
                        }
                    }

                    dt.Load(dr);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Maps <paramref name="records"/> to a <see cref="string"/> suitable for output in a CSV file.
        /// </summary>
        /// <typeparam name="T">Type of dto to be mapped to CSV.</typeparam>
        /// <typeparam name="TMap"><see cref="ClassMap"/> for type <typeparamref name="T"/>.</typeparam>
        /// <param name="records">Records to be mapped to CSV rows.</param>
        /// <param name="csvHelperConfig">Defaults to using <see cref="CultureInfo.InvariantCulture"/>.</param>
        /// <returns>A <see cref="string"/> suitable for output in a CSV file.</returns>
        public static string GenerateCsvString<T, TMap>(
            IEnumerable<T> records,
            CsvConfiguration csvHelperConfig = null
        ) where TMap : ClassMap<T>
        {
            string result;

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            if (csvHelperConfig is null)
            {
                csvHelperConfig = new CsvConfiguration(CultureInfo.InvariantCulture);
            }

            using (var cw = new CsvHelper.CsvWriter(sw, csvHelperConfig))
            {
                cw.Configuration.RegisterClassMap<TMap>();
                cw.WriteRecords(records);
                result = sb.ToString();
            }

            return result;
        }
    }
}