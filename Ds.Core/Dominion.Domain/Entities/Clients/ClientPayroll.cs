using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientPayroll : Entity<ClientPayroll>
    {
        public virtual int ClientId { get; set; }
        public virtual byte PayrollTypeHoursId { get; set; }
        public virtual byte PayrollCheckSeqId { get; set; }
        public virtual byte PayrollCheckFormatId { get; set; }
        public virtual byte PayrollSpecialReptId { get; set; }
        public virtual byte PayrollSpecialOptId { get; set; }
        public virtual byte PayrollSpecialCalcsId { get; set; }
        public virtual byte PayrollTipCreditId { get; set; }
        public virtual byte PayrollNachaOptId { get; set; }
        public virtual byte PayrollExemptionId { get; set; }
        public virtual byte? DominionVendorOpt { get; set; }
        public virtual byte PrintThirdPartyPayOnW2 { get; set; }
        public virtual byte FederalVendorOpt { get; set; }
        public virtual bool IsIncludeInW2ElectronicFile { get; set; }
        public virtual bool IsAvgRatePremium { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime LastCatchupDate { get; set; }

        public ClientPayroll()
        {
        }
    }
}
