using System.Text;

namespace Dominion.Utility.ExtensionMethods
{
    public static class StringBuilderExtensionMethods
    {
        /// <summary>
        /// Specify how many lines you want to append.
        /// </summary>
        /// <param name="sb">The string builder you're adding lines to.</param>
        /// <param name="lineCount">How many lines you want.</param>
        /// <returns>The string builder object for the chaingin affect.</returns>
        public static StringBuilder AppendLines(this StringBuilder sb, int lineCount)
        {
            for (int i = 0; i < lineCount; i++)
                sb.AppendLine();

            return sb;
        }
    }
}