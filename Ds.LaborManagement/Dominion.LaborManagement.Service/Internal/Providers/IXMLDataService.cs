using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <summary>
    /// Service used to retrieve the data to include in an XML file.
    /// </summary>
    public interface IXMLDataService
    {
        /// <summary>
        /// Gets the data to include in an xml feed to send to Indeed.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{IndeedXmlJobPost}"/> of job posts to include in the xml feed.</returns>
        IEnumerable<IndeedXmlJobPost> GetJobPostsForIndeed();
    }
}