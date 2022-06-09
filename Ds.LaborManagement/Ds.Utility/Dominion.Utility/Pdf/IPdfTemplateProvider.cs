using System.IO;

namespace Dominion.Utility.Pdf
{
    public interface IPdfTemplateProvider
    {
        Stream GetPdfStream();
    }
}
