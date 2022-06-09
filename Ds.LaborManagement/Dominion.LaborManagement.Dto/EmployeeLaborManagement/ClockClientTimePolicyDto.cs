using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using ClockClientRulesDto = Dominion.LaborManagement.Dto.Clock.ClockClientRulesDto;

namespace Dominion.LaborManagement.Dto.EmployeeLaborManagement
{
    [Serializable]
    public class ClockClientTimePolicyDtos
    {
       public class ClockClientTimePolicyDto {
        public virtual int ClockClientTimePolicyId { get; set; } 
        public virtual string Name { get; set; } 
        public virtual int ClientId { get; set; } 
        public virtual byte ClientStatusId { get; set; } 
        public virtual byte PayType { get; set; } 
        public virtual int? ClockClientRulesId { get; set; } 
        public virtual int? ClockClientExceptionId { get; set; } 
        public virtual int? ClockClientHolidayId { get; set; } 
        public virtual int ModifiedBy { get; set; } 
        public virtual DateTime Modified { get; set; } 
        public virtual int? ClientDepartmentId { get; set; } 
        public virtual int? ClientShiftId { get; set; } 
        public virtual int? TimeZoneId { get; set; } 
        public virtual bool IsAddToOtherPolicy { get; set; }
        public virtual ClockClientHolidayDto Holidays { get; set; }
        public virtual ClockClientRulesDto Rules { get; set; }
        public virtual ClockClientExceptionDto Exceptions { get; set; }
        public virtual ICollection<ClockClientLunchDto> Lunches { get; set; }
        public virtual ICollection<ClockClientOvertimeDto> Overtimes { get; set; }
        public virtual ICollection<ClientShiftDto> Shifts { get; set; }
        public virtual TimeZoneDto TimeZone { get; set; }

        }

       public class ClockClientTimePolicyListDto
        {
         public virtual int ClockClientTimePolicyId { get; set; } 

        public virtual string Name { get; set; } 

        public virtual int ClientId { get; set; } 
        }
    }

 

}
