using System.Text.RegularExpressions;
using Dominion.Utility.Constants;

namespace Dominion.Utility.Transform
{
    /// <summary>
    /// Transformer that transforms a string using the Regex.Replace method. 
    /// </summary>
    public class RegexTransformer : Transformer<string>
    {
        protected string FindFormat;
        protected string ReplacementFormat;

        /// <summary>
        /// Instantiates a new RegexTransformer. 
        /// </summary>
        /// <param name="formatToFind">Regex to match in the given string.</param>
        /// <param name="formatToReplaceWith">Regex to replace the found string with.</param>
        public RegexTransformer(string formatToFind, string formatToReplaceWith = CommonConstants.EMPTY_STRING)
        {
            FindFormat = formatToFind;
            ReplacementFormat = formatToReplaceWith;
        }

        /// <summary>
        /// Transforms the given string using the Regex.Replace method.
        /// </summary>
        /// <param name="instance">String to search for a regex match & replace.</param>
        /// <returns>String with the regex match replaced.</returns>
        public override string Transform(string instance)
        {
            string maskedResult = instance;

            if (instance != null)
                maskedResult = Regex.Replace(instance, FindFormat, ReplacementFormat);

            return maskedResult;
        }
    }
}