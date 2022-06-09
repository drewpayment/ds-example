namespace Dominion.Utility.Pdf.DocxToPdf
{
    public interface IDocxToPdfConverter
    {
        byte[] Convert(byte[] docxContent);
    }
}
