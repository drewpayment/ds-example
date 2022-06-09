using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Utility.Mapping;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class SchedulingMaps
    {
        public class ToEmployeesToScheduleForCostCenter : ExpressionMapper<ClockEmployeeCostCenter, EmployeeSchedulesDto>
        {
            public override Expression<Func<ClockEmployeeCostCenter, EmployeeSchedulesDto>> MapExpression
            {
                get
                {
                    return gs => new EmployeeSchedulesDto
                    {
                        EmployeeId = gs.EmployeeId,
                        FirstName = gs.Employee.FirstName,
                        LastName = gs.Employee.LastName,
                        //IsTerminated = gs.Employee.IsActive, //highFix: review: jay: I need to add the employeePay and employeeStatus tables as entities to get this true answer to this question.
                        
                        // Crazy cast is to prevent entity framework from erroring out when an employee has no pay info.
                        // But when an employee doesn't have a pay record, they should be considered terminated.
                        IsTerminated = !(((bool?)gs.Employee.EmployeePayInfo.FirstOrDefault().EmployeeStatus.IsActive) ?? false),
                        JobProfileId = gs.Employee.JobProfileId,
                        JobTitle = gs.Employee.JobTitleInfo != null ? gs.Employee.JobTitleInfo.Description : null
                    };
                }
            }
        }

        public class ToPreviewSchedule : ExpressionMapper<EmployeeSchedulePreview, ScheduleShiftDto>
        {
            public override Expression<Func<EmployeeSchedulePreview, ScheduleShiftDto>> MapExpression
            {
                get
                {
                    return x => new ScheduleShiftDto
                    {
                        EmployeeId = x.EmployeeId,

                        EmployeeDataDto = new SchedulesEmployeeDataDto()
                        {
                            EmployeeId = x.EmployeeId,
                            FirstName = x.Employee.FirstName,
                            LastName = x.Employee.LastName,
                            IsTerminated = !x.Employee.EmployeePayInfo.FirstOrDefault().EmployeeStatus.IsActive,      
                            JobProfileId = x.Employee.JobProfileId,
                            JobTitle = x.Employee.JobTitleInfo != null ? x.Employee.JobTitleInfo.Description : null
                        },

                        StartTime = x.GroupScheduleShift.StartTime,
                        EndTime = x.GroupScheduleShift.EndTime,
                        EventDate = x.EventDate,
                        GroupScheduleShiftId = x.GroupScheduleShiftId,
                        IsPreview = true,
                        ShiftId = x.EmployeeSchedulePreviewId,
                        ScheduleGroupId = x.GroupScheduleShift.ScheduleGroupId,
                        ScheduleGroupDescription = x.GroupScheduleShift.ScheduleGroup.ClientCostCenter.Description,
                        ScheduleGroupSourceId = x.GroupScheduleShift.ScheduleGroup.ClientCostCenterId ?? 0,
                        Override_ScheduleGroupId = x.Override_ScheduleGroupId,
                        Override_ScheduleGroupSourceId = x.Override_ScheduleGroup.ClientCostCenterId,
                        Override_Description = x.Override_ScheduleGroup.ClientCostCenter.Description,
                        IsOverridden = x.Override_ScheduleGroupId.HasValue && x.Override_ScheduleGroupId != x.GroupScheduleShift.ScheduleGroupId, //if it has a value and that value is not what the shift is for it is overridden
                    };
                }
            }
        }

        public class ToClockEmployeeSchedule : ExpressionMapper<ClockEmployeeSchedule, ClockEmployeeScheduleShiftDto>
        {
            public override Expression<Func<ClockEmployeeSchedule, ClockEmployeeScheduleShiftDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeeScheduleShiftDto
                    {
                        EmployeeId = x.EmployeeId,
                        ClientId = x.ClientId,

                        EmployeeDataDto = new SchedulesEmployeeDataDto()
                        {
                            EmployeeId = x.EmployeeId,
                            FirstName = x.Employee.FirstName,
                            LastName = x.Employee.LastName,
                            IsTerminated = !x.Employee.EmployeePayInfo.FirstOrDefault().EmployeeStatus.IsActive,     
                            JobProfileId = x.Employee.JobProfileId,
                            JobTitle = x.Employee.JobTitleInfo != null ? x.Employee.JobTitleInfo.Description : null                       
                        },

                        ClockEmployeeScheduleId = x.ClockEmployeeScheduleId,
                        EventDate = x.EventDate,

                        StartTimeDate1 = x.StartTime1,
                        EndTimeDate1 = x.EndTime1,
                        GroupScheduleShiftId1 = x.GroupScheduleShiftId1,
                        ScheduleGroupId1 = x.GroupScheduleShiftId1.HasValue ? x.GroupScheduleShift1.ScheduleGroupId : default(int?),
                        ScheduleGroupDescription1 = x.GroupScheduleShiftId1.HasValue ? x.GroupScheduleShift1.ScheduleGroup.ClientCostCenter.Description : null,
                        ScheduleGroupSourceId1 = x.GroupScheduleShiftId1.HasValue ? x.GroupScheduleShift1.ScheduleGroup.ClientCostCenterId : default(int?),
                        EmployeeDefaultShiftId1 = 
                            x.GroupScheduleShiftId1.HasValue 
                            ?  x.GroupScheduleShift1.EmployeeDefaultShifts
                                    .Where(y => y.EmployeeId == x.EmployeeId)
                                    .Select(y => y.EmployeeDefaultShiftId)
                                    .FirstOrDefault()
                            : default(int?),
                        ClientCostCenterId1 = x.ClientCostCenterId1,

                        StartTimeDate2 = x.StartTime2,
                        EndTimeDate2 = x.EndTime2,
                        GroupScheduleShiftId2 = x.GroupScheduleShiftId2,
                        ScheduleGroupId2 = x.GroupScheduleShiftId2.HasValue ? x.GroupScheduleShift2.ScheduleGroupId : default(int?),
                        ScheduleGroupDescription2 = x.GroupScheduleShiftId2.HasValue ? x.GroupScheduleShift2.ScheduleGroup.ClientCostCenter.Description : null,
                        ScheduleGroupSourceId2 = x.GroupScheduleShiftId2.HasValue ? x.GroupScheduleShift2.ScheduleGroup.ClientCostCenterId : default(int?),
                        EmployeeDefaultShiftId2 = 
                            x.GroupScheduleShiftId2.HasValue 
                            ?  x.GroupScheduleShift2.EmployeeDefaultShifts
                                    .Where(y => y.EmployeeId == x.EmployeeId)
                                    .Select(y => y.EmployeeDefaultShiftId)
                                    .FirstOrDefault()
                            : default(int?),
                        ClientCostCenterId2 = x.ClientCostCenterId2,

                        StartTimeDate3 = x.StartTime3,
                        EndTimeDate3 = x.EndTime3,
                        GroupScheduleShiftId3 = x.GroupScheduleShiftId3,
                        ScheduleGroupId3 = x.GroupScheduleShiftId3.HasValue ? x.GroupScheduleShift3.ScheduleGroupId : default(int?),
                        ScheduleGroupDescription3 = x.GroupScheduleShiftId3.HasValue ? x.GroupScheduleShift3.ScheduleGroup.ClientCostCenter.Description : null,
                        ScheduleGroupSourceId3 = x.GroupScheduleShiftId3.HasValue ? x.GroupScheduleShift3.ScheduleGroup.ClientCostCenterId : default(int?),
                        EmployeeDefaultShiftId3 = 
                            x.GroupScheduleShiftId2.HasValue 
                            ?  x.GroupScheduleShift2.EmployeeDefaultShifts
                                    .Where(y => y.EmployeeId == x.EmployeeId)
                                    .Select(y => y.EmployeeDefaultShiftId)
                                    .FirstOrDefault()
                            : default(int?),
                        ClientCostCenterId3 = x.ClientCostCenterId3,
                    };
                }
            }
        }

        public class ToEmployeeDefaultShift : ExpressionMapper<EmployeeDefaultShift, ScheduleShiftDto>
        {
            public override Expression<Func<EmployeeDefaultShift, ScheduleShiftDto>> MapExpression
            {
                get
                {
                    return x => new ScheduleShiftDto
                    {
                        EmployeeId = x.EmployeeId,
                        GroupScheduleShiftId = x.GroupScheduleShiftId,
                        ShiftId = x.EmployeeDefaultShiftId,
                        ScheduleGroupId = x.GroupScheduleShift.ScheduleGroupId,
                        ScheduleGroupDescription = x.GroupScheduleShift.ScheduleGroup.ClientCostCenter.Description,
                        ScheduleGroupSourceId = x.GroupScheduleShift.ScheduleGroup.ClientCostCenterId ?? 0,

                        DayOfWeek = x.GroupScheduleShift.DayOfWeek,
                        StartTime = x.GroupScheduleShift.StartTime,
                        EndTime = x.GroupScheduleShift.EndTime,
                    };
                }
            }
        }

        public class ToClockEmployeeScheduleDto : ExpressionMapper<ClockEmployeeSchedule, ClockEmployeeScheduleDto>
        {
            public override Expression<Func<ClockEmployeeSchedule, ClockEmployeeScheduleDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeeScheduleDto()
                    {
                        ClientId = x.ClientId,
                        EmployeeId = x.EmployeeId,
                        ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                        ClientCostCenterId1 = x.ClientCostCenterId1,
                        ClientCostCenterId2 = x.ClientCostCenterId2,
                        ClientCostCenterId3 = x.ClientCostCenterId3,
                        ClientDepartmentId1 = x.ClientDepartmentId1,
                        ClientDepartmentId2 = x.ClientDepartmentId2,
                        ClientDepartmentId3 = x.ClientDepartmentId3,
                        ClockEmployeeScheduleId = x.ClockEmployeeScheduleId,
                        StartTime1 = x.StartTime1,
                        EndTime1 = x.EndTime1,
                        StartTime2 = x.StartTime2,
                        EndTime2 =  x.EndTime2,
                        StartTime3 = x.StartTime3,
                        EndTime3 = x.EndTime3,
                        EventDate = x.EventDate,
                    };
                }
            }
        }
    }
}
