using iTextSharp.text.pdf;

using org.pdfclown.documents.interaction.forms;

namespace Dominion.Utility.Pdf
{
    public static class PdfExtensions
    {
        public static IPdfFormFiller AsPdfFormFiller(this PdfStamper stamper)
        {
            return new iTextFormFiller(stamper);
        }

        public static IPdfFormFiller AsPdfFormFiller(this Form form)
        {
            return new PdfClownFormFiller(form);
        }
    }
}