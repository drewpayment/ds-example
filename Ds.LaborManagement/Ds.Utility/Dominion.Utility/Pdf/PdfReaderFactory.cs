using System.IO;
using System.Reflection;
using Dominion.Utility.Io;

namespace Dominion.Utility.Pdf
{
    public class PdfReaderFactory
    {
        public static IPdfTemplateProvider FromFile(string filename)
        {
            return new FilePdfTemplateProvider(filename);
        }

        public static IPdfTemplateProvider FromEmbeddedResource<T>(T assemblyObject, string resourceName)
        {
            return new EmbeddedResourcePdfTemplateProvider<T>(resourceName);
        }

        private class FilePdfTemplateProvider : IPdfTemplateProvider
        {
            private readonly string _filename;

            public FilePdfTemplateProvider(string filename)
            {
                _filename = filename;
            } 

            public Stream GetPdfStream()
            {
                return new FileStream(_filename, FileMode.Open, FileAccess.ReadWrite);
            }
        }

        private class EmbeddedResourcePdfTemplateProvider<T> : EmbeddedResourceLoader<T>, IPdfTemplateProvider
        {
            public EmbeddedResourcePdfTemplateProvider(string resourceName) : base(resourceName)
            { }

            public Stream GetPdfStream()
            {
                return this.GetStream();
            }
        }
    }
}