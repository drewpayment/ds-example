using System.IO;

namespace Dominion.Utility.Pdf
{
    public interface IPdfBuilder
    {
        IPdfBuilder Start();
        IPdfBuilder Add(IPdf pdf, params int[] pages);
        MemoryStream End();
    }
}
