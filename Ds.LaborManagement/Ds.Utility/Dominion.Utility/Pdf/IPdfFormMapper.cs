using System.Collections.Generic;

namespace Dominion.Utility.Pdf
{
    public interface IPdfFormMapper<in TDto>
    {
        /// <summary>
        /// Fills all fields on a given PDF using the current mapping definitions.
        /// </summary>
        /// <param name="obj">Object to use to populate the PDF fields.</param>
        /// <param name="filler">PDF form filler used to fill a PDF file's fields.</param>
        void Map(TDto obj, IPdfFormFiller filler);

        /// <summary>
        /// Returns a dictionary of PDF fields names mapped to their proper values constructed from the given object.
        /// </summary>
        /// <param name="obj">Object to use to populate the PDF fields.</param>
        /// <returns></returns>
        IDictionary<string, string> GetFieldValues(TDto obj);
    }
}