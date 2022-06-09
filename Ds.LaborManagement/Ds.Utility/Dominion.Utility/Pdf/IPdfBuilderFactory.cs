namespace Dominion.Utility.Pdf
{
    /// <summary>
    /// Generates <see cref="IPdfBuilder"/>(s).
    /// </summary>
    public interface IPdfBuilderFactory
    {
        /// <summary>
        /// Instantiates a new <see cref="IPdfBuilder"/> using the default builder type (i.e. iTextSharp, PdfClown, etc).
        /// </summary>
        /// <returns></returns>
        IPdfBuilder Create();
    }
}
