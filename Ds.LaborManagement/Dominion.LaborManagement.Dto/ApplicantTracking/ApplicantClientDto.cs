using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    /// <summary>
    /// Defines a class that represents a client with information that can be sent through the API.
    /// </summary>
    public class ApplicantClientDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string CorrespondenceEmailAddress { get; set; }
        public string JobBoardTitle { get; set; }
        public bool ShowAboutPage { get; set; }
        public string CompanyURL { get; set; }
        public AzureViewDto Logo { get; set; }
        public string LogoFileName { get; set; }
        public AzureViewDto Photo { get; set; }
        public string PhotoFileName { get; set; }
        public string PhotoCaption { get; set; }
        public string AboutUs { get; set; }
    }
}
