using System;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantCompanyJobBoardInfoDto
    {
       
        public int ClientId { get; set; }
        public string CompanyName { get; set; }
        public string ClientCode { get; set; }
        //add fields for the ABOUT COMPANY section of job board here
    }
}