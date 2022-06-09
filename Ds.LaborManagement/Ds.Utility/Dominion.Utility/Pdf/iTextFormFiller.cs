using iTextSharp.text.pdf;

namespace Dominion.Utility.Pdf
{
    public class iTextFormFiller : IPdfFormFiller
    {
        private readonly PdfStamper _stamper;

        public iTextFormFiller(PdfStamper stamper)
        {
            _stamper = stamper;
        }

        void IPdfFormFiller.SetField(string key, string value)
        {
            _stamper.AcroFields.SetField(key, value);
        }
    }
}