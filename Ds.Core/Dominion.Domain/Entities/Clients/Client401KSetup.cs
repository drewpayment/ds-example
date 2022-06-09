using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public partial class Client401KSetup : Entity<Client401KSetup>
    {
        public object Client;

        public virtual int Client401KSetupId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int Client401KProviderId { get; set; }
        public virtual string ContractPlanNumber { get; set; }
        public virtual int FrequencyId { get; set; }
        public virtual string FileName { get; set; }
        public virtual int? TaxYear { get; set; }
        public virtual int? BankId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual int EmployeeOptionId { get; set; }
        public virtual string SubPlanNumber { get; set; }
        public virtual int EmployeeTypeToInclude { get; set; }
        public virtual string ContractPlanName { get; set; }
        public virtual string RecordVersionNumber { get; set; }
        public virtual string TransmissionNumber { get; set; }
        public virtual string DemographicLocationCode { get; set; }
        public virtual string DemographicSubsetCode { get; set; }
        public virtual string ContributionPlanCode { get; set; }
        public virtual string VendorName { get; set; }
        public virtual bool IsMep { get; set; }
        public virtual string DemographicLanguageCode { get; set; }
        public virtual int? DepositMethod { get; set; }
        public virtual string AchNumber { get; set; }
        public virtual bool IsExcludeIneligible { get; set; }
        public virtual bool IsIncludeForfeituresByDivision { get; set; }

        public Client401KSetup()
        {
        }
    }
}
