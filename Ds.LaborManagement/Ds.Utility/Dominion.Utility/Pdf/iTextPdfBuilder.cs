using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Dominion.Utility.Pdf
{
    public class iTextPdfBuilder : IPdfBuilder
    {
        private Document _document;
        private MemoryStream _stream;
        public  PdfSmartCopy Copier { get; private set; }

        IPdfBuilder IPdfBuilder.Start()
        {
            _stream = new MemoryStream();
            _document = new Document();
            Copier = new PdfSmartCopy(_document, _stream) { CloseStream = false };
            _document.Open();

            Copier.Info.Put(new PdfName("Producer"), new PdfString("Dominion Systems Inc."));

            return this;
        }

        IPdfBuilder IPdfBuilder.Add(IPdf pdf, params int[] pages)
        {
            pdf.AddToBuilder(this, pages);

            return this;
        }

        MemoryStream IPdfBuilder.End()
        {
            Copier.Close();
            _document.Close();
           
            _stream.Position = 0;
            return _stream;
        }
    }
}