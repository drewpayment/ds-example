namespace Dominion.Utility.Pdf.HtmlToPdf
{
    public interface IHtmlToPdfConverter
    {
        byte[] Convert(string htmlContent);
    }
}
