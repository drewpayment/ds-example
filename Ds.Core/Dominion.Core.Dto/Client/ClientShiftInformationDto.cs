using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientShiftInformationDto
    {
        public Boolean HasAutoShiftByHrsWorkedOption { get; set; }
        public List<ClientEarningDto> Earnings { get; set; }
        public List<AdditionalAmountTypeInfoDto> Rates { get; set; }
        public List<ClientShiftDto> Shifts { get; set; }
        public List<int?> BlockedShifts { get; set; }

    }
}
