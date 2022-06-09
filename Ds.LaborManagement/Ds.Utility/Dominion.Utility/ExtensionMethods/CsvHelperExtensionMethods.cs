using System.Collections;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Dominion.Utility.ExtensionMethods
{
    public static class CsvHelperExtensionMethods
    {
        /// <summary>
        /// Build a csv string based on the list of objects.
        /// This uses the CsvHelper library.
        /// </summary>
        /// <param name="list">The data for the CSV file.</param>
        /// <param name="config">The CsvHelper configuration.</param>
        /// <returns></returns>
        public static string ToCsvString(this IEnumerable list, Configuration config)
        {
            var csv = string.Empty;

            using (var sw = new StringWriter())
            {
                using (var writer = new CsvWriter(sw, config))
                {
                    writer.WriteRecords(list);
                    csv = sw.ToString();
                }
            }

            return csv;
        }

        /// <summary>
        /// Build a csv string based on the list of objects.
        /// This uses the CsvHelper library.
        /// </summary>
        /// <param name="list">The data for the CSV file.</param>
        /// <returns></returns>
        public static string ToCsvString(this IEnumerable list)
        {
            var csv = string.Empty;

            using (var sw = new StringWriter())
            {
                using (var writer = new CsvWriter(sw))
                {
                    writer.WriteRecords(list);
                    csv = sw.ToString();
                }
            }

            return csv;
        }
    }
}