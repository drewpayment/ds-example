namespace Dominion.Utility.Pdf
{
    /// <summary>
    /// Generates <see cref="IPdfBuilder"/>(s).
    /// </summary>
    public class PdfBuilderFactory : IPdfBuilderFactory
    {
        IPdfBuilder IPdfBuilderFactory.Create()
        {
            //use iTextSharp for now, but this can be swapped out to use whatever pdf library we want.
            return new iTextPdfBuilder();
        }
    }
}