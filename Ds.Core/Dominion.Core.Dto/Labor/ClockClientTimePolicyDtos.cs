using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Labor
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
        public bool HasCombinedOtFrequencies { get; set; }
        public virtual ClockClientHolidayDto Holidays { get; set; }
        public virtual ClockClientRulesDto Rules { get; set; }
        public virtual ClockClientExceptionDto Exceptions { get; set; }
        public virtual ICollection<ClockClientLunchDto> Lunches { get; set; }
        public virtual IEnumerable<ClockClientLunchSelectedDto> LunchSelected { get; set; }
        public virtual ICollection<ClockClientAddHoursDto> AddHours { get; set; }
        public virtual IEnumerable<ClockClientAddHoursSelectedDto> AddHoursSelected { get; set; }
        public virtual ICollection<ClockClientOvertimeDto> Overtimes { get; set; }
        public virtual IEnumerable<ClockClientOvertimeSelectedDto> OvertimeSelected { get; set; }
        public virtual IEnumerable<ClientShiftDto> Shifts { get; set; }
        public virtual IEnumerable<ClientShiftSelectedDto> ShiftSelected { get; set; }
        public TimeZoneDto TimeZone { get; set; }
        public virtual bool AutoPointsEnabled { get; set; }
        public virtual bool ShowTCARatesEnabled { get; set; }
        public virtual bool GeofenceEnabled { get; set; }
    }

       public class ClockClientTimePolicyListDto
        {
            public virtual int ClockClientTimePolicyId { get; set; } 
            public virtual string Name { get; set; } 
            public virtual int ClientId { get; set; }
            public virtual bool GeofenceEnabled { get; set; }
            public virtual int EmployeeCount { get; set; }
        }

        public class ClockClientTimePolicyViewModels
        {
            public List<ClockClientTimePolicyDto> TimePolicies { get; set; }
            public List<ClockClientRulesDto> CompanyRules { get; set; }
            public List<ClockClientExceptionDto> Exceptions { get; set; }
            public List<ClockClientHolidayDto> Holidays { get; set; }

            public List<TimeZoneDto> TimeZones { get; set; }
            public bool AutoPointsSwitchEnabled { get; set; }
            public bool AutoPointsSwitchVisible { get; set; }
        }
    }

 

}
