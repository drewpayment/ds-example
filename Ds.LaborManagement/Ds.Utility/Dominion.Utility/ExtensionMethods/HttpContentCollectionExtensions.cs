using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Containers;
using Dominion.Utility.Query.LinqKit;
using org.pdfclown.documents.contents;

namespace Dominion.Utility.ExtensionMethods
{
    public static class HttpContentCollectionExtensions
    {
        public static async Task<byte[]> GetByteArrayContentByNameAsync(this ICollection<HttpContent> contents, string contentName)
        {
            var result = new byte[0];
            var content = contents.FirstOrDefault(con =>
                con.Headers.ContentDisposition.Name.Replace("\"", string.Empty) == contentName);
            if (content != null)
            {
                result = await content.ReadAsByteArrayAsync();
            }

            return result;
        }

        public static string GetFileNameByName(this ICollection<HttpContent> contents, string contentName)
        {
            var content = contents.FirstOrDefault(con =>
                con.Headers.ContentDisposition.Name.Replace("\"", string.Empty) == contentName);
            if (content != null)
                return content.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            return "";
        }

        public static async Task<string> GetStringObjectByName(this ICollection<HttpContent> contents, string contentName)
        {
            var content = contents.FirstOrDefault(con =>
                con.Headers.ContentDisposition.Name.Replace("\"", string.Empty) == contentName);
            string result = "";
            if (content != null)
                result = await content.ReadAsStringAsync();

            return result;
        }

        public static async Task<Int32> GetIntegerContentByNameAsync(this ICollection<HttpContent> contents, string contentName)
        {
            var content = contents.FirstOrDefault(con =>
                con.Headers.ContentDisposition.Name.Replace("\"", string.Empty) == contentName);
            var stringResult = await content.ReadAsStringAsync();
            int result = Int32.MinValue;
            if (stringResult != null && stringResult != "")
            {
                Int32.TryParse(stringResult, out result);
            }

            return result;
        }
    }
}
