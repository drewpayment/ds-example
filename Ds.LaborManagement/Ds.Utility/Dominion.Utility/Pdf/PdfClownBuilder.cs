using System;
using System.IO;

using org.pdfclown.files;
using org.pdfclown.tools;

namespace Dominion.Utility.Pdf
{
    public class PdfClownBuilder : IPdfBuilder
    {
        private MemoryStream            _outputStream;
        private org.pdfclown.files.File _outputFile;

        public PageManager Manager { get; private set; }

        IPdfBuilder IPdfBuilder.Start()
        {
            _outputStream = new MemoryStream();
            _outputFile   = new org.pdfclown.files.File();

            Manager       = new PageManager(_outputFile.Document);

            return this;
        }

        IPdfBuilder IPdfBuilder.Add(IPdf pdf, params int[] pages)
        {
            pdf.AddToBuilder(this, pages);
            return this;
        }

        MemoryStream IPdfBuilder.End()
        {
            _outputFile.Configuration.XRefMode = XRefModeEnum.Compressed;
            _outputFile.Save(_outputStream, SerializationModeEnum.Standard);
            _outputFile.Dispose();
            _outputStream.Position = 0;

            return _outputStream;
        }
    }
}
