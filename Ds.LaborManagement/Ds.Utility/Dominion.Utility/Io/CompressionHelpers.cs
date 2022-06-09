using System.IO;
using System.IO.Compression;

namespace Dominion.Utility.Io
{
    public static class CompressionHelpers
    {
        /// <summary>
        /// Zip all of the supplied files into a single compressed memory stream that can then be used to write to a 
        /// file or added to an HTTP response.
        /// </summary>
        /// <param name="filePaths">Fully qualified path(s) to for each file to add to the ZIP archive.</param>
        /// <returns></returns>
        /// <remarks>
        /// REF: http://stackoverflow.com/questions/17232414/creating-a-zip-archive-in-memory-using-system-io-compression
        /// </remarks>
        public static MemoryStream ZipFiles(params string[] filePaths)
        {
            if(filePaths == null)
                return null;

            var ms = new MemoryStream();

            //create a new zip archive containing each file
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                foreach (var path in filePaths)
                {
                    var filename = Path.GetFileName(path);
                    var zipped = archive.CreateEntry(filename);

                    //zip each file
                    using (var zippedStream = zipped.Open())
                    using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        fileStream.CopyTo(zippedStream);
                    }
                }
            }

            //reset the position of the memory stream so it can be read into a file or processed as an HTTP response
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }
    }
}
