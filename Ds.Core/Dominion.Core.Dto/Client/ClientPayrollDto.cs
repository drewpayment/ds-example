using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientPayrollDto
    {
        public int ClientId { get; set; }
        public byte PayrollTypeHoursId { get; set; }
        public byte PayrollCheckSeqId { get; set; }
        public byte PayrollCheckFormatId { get; set; }
        public byte PayrollSpecialReptId { get; set; }
        public byte PayrollSpecialOptId { get; set; }
        public byte PayrollSpecialCalcsId { get; set; }
        public byte PayrollTipCreditId { get; set; }
        public byte PayrollNachaOptId { get; set; }
        public byte PayrollExemptionId { get; set; }
        public byte? DominionVendorOpt { get; set; }
        public byte PrintThirdPartyPayOnW2 { get; set; }
        public byte FederalVendorOpt { get; set; }
        public bool IsIncludeInW2ElectronicFile { get; set; }
        public bool IsAvgRatePremium { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime LastCatchupDate { get; set; }
    }
}
