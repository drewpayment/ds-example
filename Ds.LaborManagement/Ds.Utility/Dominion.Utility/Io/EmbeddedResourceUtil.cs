using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Io
{
    public class EmbeddedFileResource
    {
        #region Properties and Variables

        public const int DEFAULT_BUFFERE_SIZE = 32768;

        public TwoPartEmbeddedPath PathInfo { get; private set; }

        /// <summary>
        /// Used when you need to write the embedded resource to the file system.
        /// Some files such as an excel file cannot be opened with a stream ... may be a 3rd party util somewhere though.
        /// Default is: 32768 bytes.
        /// </summary>
        public int BufferSize { get; set; }

        #endregion

        #region Constructors

        public EmbeddedFileResource(string filePath)
        {
            
        }

        public EmbeddedFileResource(Type type, string fileName) :
            this(new TwoPartEmbeddedPath(type, fileName))
        {
        }

        public EmbeddedFileResource(string nameSpace, string fileName) :
            this(new TwoPartEmbeddedPath(nameSpace, fileName))
        {
        }

        public EmbeddedFileResource(TwoPartEmbeddedPath pathInfo)
        {
            PathInfo = pathInfo;
            BufferSize = DEFAULT_BUFFERE_SIZE;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This object represents an embedded file.
        /// If the embedded file is a json file, use this method to convert to the object you're expecting from it.
        /// </summary>
        /// <typeparam name="T">The object type your are expecting the json to represent.</typeparam>
        /// <returns></returns>
        public T DeserializeJsonFile<T>()
        {
            var obj = default(T);
            var assembly = Assembly.GetCallingAssembly();

            using (var embResourceStream = assembly.GetManifestResourceStream(PathInfo.ResourcePath))
                obj = embResourceStream.DeserializeJson<T>();

            return obj;
        }

        /// <summary>
        /// Read the string contents of the file.
        /// ie. Useful for reading JSON from a file.
        /// </summary>
        /// <returns>A string representing the contents of the file.</returns>
        public string ReadAllText()
        {
            var fileContents = string.Empty;
            var assembly = Assembly.GetCallingAssembly();

            using (var embResourceStream = assembly.GetManifestResourceStream(PathInfo.ResourcePath))
            {
                using (StreamReader reader = new StreamReader(embResourceStream))
                    fileContents = reader.ReadToEnd();
            }

            return fileContents;
        }

        #endregion
    }
}
