using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <summary>
    /// Service used to create an XML file using on the provided data.
    /// </summary>
    public interface IXMLGeneratorService
    {
        /// <summary>
        /// Creates an xml document with all the data required for job posts and Indeed Apply based on what is stored in our database.
        /// Requirements for xml document: <see href="http://opensource.indeedeng.io/api-documentation/docs/xml-feed-ia/"/> 
        /// </summary>
        /// <param name="posts">The jobs that we want to put into the feed</param>
        /// <param name="apiToken">I think this token is used by Indeed to determine who the feed is from: <see href="https://secure.indeed.com/account/apikeys"/> and then the "Client ID" property</param>
        /// <returns>An xml document</returns>
        IOpResult<XDocument> BuildIndeedXML(IEnumerable<IndeedXmlJobPost> posts, string apiToken, DateTime currentDateTime);
    }
}