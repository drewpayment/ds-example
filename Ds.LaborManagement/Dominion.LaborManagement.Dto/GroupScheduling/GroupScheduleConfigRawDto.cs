using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.GroupScheduling
{
    public class GroupScheduleConfigRawDto
    {
        public class GroupScheduleRawDto : GroupScheduleDtos.GroupScheduleDto, IGroupScheduleDto 
        {
            public IEnumerable<GroupScheduleShiftRawDto> GroupScheduleShifts { get; set; }
        }

        public class GroupScheduleShiftLiteRawDto : GroupScheduleDtos.GroupScheduleShiftLiteDto
        {
            public GroupScheduleDtos.GroupScheduleDto GroupSchedule          { get; set; }
            public ScheduleGroupShiftNameRawDto       ScheduleGroupShiftName { get; set; }
        }

        public class GroupScheduleShiftRawDto : GroupScheduleDtos.GroupScheduleShiftDto
        {
            public ScheduleGroupRawDto          ScheduleGroup           { get; set; }  
            public ScheduleGroupShiftNameRawDto ScheduleGroupShiftName  { get; set; }  
        }

        public class ScheduleGroupRawDto
        {
            public int ScheduleGroupId { get; set; } 
            public ScheduleGroupType ScheduleGroupType { get; set; } 

            public ScheduleGroupClientCostCenterRawDto ClientCostCenter { get; set; }        
        }

        public class ScheduleGroupShiftNameRawDto
        {
            public int?   ScheduleGroupShiftNameId { get; set; } 
            public int    ScheduleGroupId          { get; set; } 
            public string Name                     { get; set; } 
        }

        public class ScheduleGroupClientCostCenterRawDto
        {
            public int      ClientCostCenterId  { get; set; } 
            public string   Code                { get; set; } 
            public string   Description         { get; set; } 
        } 
    }
}