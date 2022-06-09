using System.Web;

namespace Dominion.Utility
{
    public class ImageUtils
    {
        public static int MaxSignatureFileSizeBytes = 50000;
        public static int MaxLogoFileSizeBytes = 100000;

        public static bool IsPostedFileTooLarge(HttpPostedFile postedFile, int maxBytes)
        {
            if (postedFile.ContentLength > maxBytes)
                return true;

            return false;
        }
    }
}
