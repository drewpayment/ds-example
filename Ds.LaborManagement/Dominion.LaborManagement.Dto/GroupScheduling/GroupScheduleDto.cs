using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.GroupScheduling
{
    public class GroupScheduleDtos
    {
        //=============================================================
        public class GroupScheduleDto : IGroupScheduleDto
        {
            public virtual int      GroupScheduleId { get; set; } 
            public virtual int      ClientId        { get; set; } 
            public virtual string   Name            { get; set; } 
            public virtual bool     IsActive        { get; set; }
        }
        //=============================================================
        public class GroupScheduleWithScheduleGroupsDto : GroupScheduleDto
        {
            public List<ScheduleGroupDto> Groups { get; set; }
        }
        //=============================================================
            //=============================================================
            //=============================================================
            //=============================================================
            public class ScheduleGroupDto
            {
                public int?              ScheduleGroupId   { get; set; }
                public int               SourceId          { get; set; } //ie. will be client cost center if schedule group type is cost center
                public string            Code              { get; set; }
                public string            Description       { get; set; }
                public ScheduleGroupType ScheduleGroupType { get; set; }
                public bool              IsReadOnly        { get; set; }

                public IEnumerable<ScheduleGroupShiftNameWithShiftsDto> Subgroups { get; set; }
            }
            //=============================================================
                //=============================================================
                //=============================================================
                //=============================================================
                public class ScheduleGroupShiftNameDto
                {
                    public int?     ScheduleGroupShiftNameId    { get; set; }
                    public int?     ScheduleGroupId             { get; set; }
                    public string   Name                        { get; set; }
                }
                //=============================================================
                public class ScheduleGroupShiftNameWithShiftsDto : ScheduleGroupShiftNameDto
                {
                    public List<GroupScheduleShiftLiteDto> Shifts { get; set; }
                }
                //=============================================================
                    //=============================================================
                    //=============================================================
                    //=============================================================
                    public class GroupScheduleShiftLiteDto
                    {
                        public int          GroupScheduleShiftId     { get; set; }
                        public TimeSpan     StartTime                { get; set; } 
                        public TimeSpan     EndTime                  { get; set; } 
                        public DayOfWeek    DayOfWeek                { get; set; } 
                        public byte         NumberOfEmployees        { get; set; }
                        public bool         IsDeleted                { get; set; }
                    }


        public class GroupScheduleForSchedulingDto
        {
            public int GroupScheduleId { get; set; }
            public string Name { get; set; }
            public IEnumerable<ScheduleGroupShiftNameWithShiftsDto> Subgroups { get; set; } 
        }

        public class GroupScheduleShiftDto : GroupScheduleShiftLiteDto
        {
            //highFix: jay: this data is USED for correlation but more than likely isn't needed. Look into refactoring and consolidate dto's accordingly.
            public int               SourceId          { get; set; }
            public ScheduleGroupType ScheduleGroupType { get; set; }
        }

        public class ScheduleGroupWithShiftsDto : ScheduleGroupDto
        {
            public List<GroupScheduleShiftDto> Shifts { get; set; }
        }
    }
}
