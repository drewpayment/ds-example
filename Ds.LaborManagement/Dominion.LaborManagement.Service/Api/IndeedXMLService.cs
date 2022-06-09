using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.Utility.OpResult;
using Renci.SshNet;

namespace Dominion.LaborManagement.Service.Api
{
    public class IndeedXMLService : IIndeedXMLService
    {
        private readonly IXMLDataService _xmlDataService;
        private readonly IXMLGeneratorService _xmlGeneratorService;
        private readonly IBusinessApiSession _apiSession;
        private const int IndeedApiTokenId = 2;
        public IndeedXMLService(IXMLDataService xmlDataService, IXMLGeneratorService xmlGeneratorService, IBusinessApiSession apiSession)
        {
            _xmlDataService = xmlDataService;
            _xmlGeneratorService = xmlGeneratorService;
            _apiSession = apiSession;
        }

        public IOpResult<XDocument> GetIndeedXMLFeed()
        {

            return _xmlGeneratorService.BuildIndeedXML(_xmlDataService.GetJobPostsForIndeed(),
                _apiSession.UnitOfWork.ApiRepository.GetApiAccountQuery().ByApiAccountId(IndeedApiTokenId).ExecuteQuery().First().ApiKey, DateTime.Now);
        }

        public void SendFile(ConnectionInfo info, IOpResult result, string fileName, string zippedFileDir)
        {
            var localFilePath = zippedFileDir + fileName;
            result.TryCatch(() =>
            {
                using (var client = new SftpClient(info))
                {
                    using (var fs = new FileStream(localFilePath, FileMode.Open))
                    {
                        client.Connect();
                        client.UploadFile(fs, fileName);
                    }
                }
            });
        }
    }
}
