using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.ExtensionMethods
{
    public static class ByteArrExtensionMethods
    {
        /// <summary>
        /// Convert a byte[] into a string.
        /// This of course expects that the byte[] is a binary representation of a string.
        /// ie. Reading a file resource which is stored as a byte[].
        /// </summary>
        /// <param name="arr">The array you are converting into a string.</param>
        /// <returns>A string.</returns>
        public static string ReadAllText(this byte[] arr)
        {
            var str = string.Empty;

            using(var ms = new MemoryStream(arr))
                str = Encoding.ASCII.GetString(ms.ToArray());

            return str;
        }

        public static byte[] ToByteArray(this Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
