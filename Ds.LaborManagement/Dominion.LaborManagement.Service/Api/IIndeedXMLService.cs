using System.Xml.Linq;
using Dominion.Utility.OpResult;
using Renci.SshNet;

namespace Dominion.LaborManagement.Service.Api
{
    /// <summary>
    /// Service used to create an xml feed to send to Indeed using data in the connected database.
    /// </summary>
    public interface IIndeedXMLService
    {
        IOpResult<XDocument> GetIndeedXMLFeed();
        void SendFile(ConnectionInfo info, IOpResult result, string fileName, string zippedFileDir);
    }
}