using Dominion.Core.Dto.Benefits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;

namespace Dominion.Core.Dto.Client
{
    public class ClientDeductionInfoDto
    {
        public bool HasBankInfoSetup { get; set; }
        public int? ACHBankID { get; set; }
        public bool NoPreNote { get; set; }
        public List<PlanDeductionDto> PlanList { get; set; }
        public List<MaxTypeDto> MaxTypeList { get; set; }
        public List<VendorDeductionDto> VendorList { get; set; }
        public List<ClientCostCenterDto> CostCenterList { get; set; }
        public ClientEssOptionsDto ClientEssOptions { get; set; }
    }
}
