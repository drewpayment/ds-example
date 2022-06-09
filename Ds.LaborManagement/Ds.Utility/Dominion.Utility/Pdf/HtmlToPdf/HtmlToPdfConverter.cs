namespace Dominion.Utility.Pdf.HtmlToPdf
{
    public class HtmlToPdfConverter : IHtmlToPdfConverter
    {
        public byte[] Convert(string htmlContent)
        {
            var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
            return htmlToPdf.GeneratePdf(htmlContent);
        }
    }
}
