using Dominion.Domain.Entities.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Service.Internal
{
    public class IndeedXmlJobPost
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int ReferenceNumber { get; set; }
        public string Company { get; set; }
        public string SourceName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Description { get; set; }
        public string Salary { get; set; }
        public string Education { get; set; }
        public string JobType { get; set; }
        public string Category { get; set; }
        public int Experience { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientEmail { get; set; }
        public int ResumeRequiredId { get; set; }
        public int ApplicationId { get; set; }
        public ICollection<string> Responsibilities { get; set; }
        public ICollection<string> Skills { get; set; }
        public string WorkingConditions { get; set; }
        public string Benefits { get; set; }
    }
}
