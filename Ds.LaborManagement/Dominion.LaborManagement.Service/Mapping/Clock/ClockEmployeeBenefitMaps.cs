using System;
using System.Linq.Expressions;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockEmployeeBenefitMaps
    {
        public class ToClockEmployeeBenefitDto : ExpressionMapper<ClockEmployeeBenefit, ClockEmployeeBenefitDto>
        {
            public override Expression<Func<ClockEmployeeBenefit, ClockEmployeeBenefitDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeeBenefitDto()
                    {
                        ClientId = x.ClientId,
                        EmployeeId = x.EmployeeId,
                        ClientDepartmentId = x.ClientDepartmentId,
                        ClientCostCenterId = x.ClientCostCenterId,
                        ClientShiftId = x.ClientShiftId,
                        ClientDivisionId = x.ClientDivisionId,
                        ClientEarningId = x.ClientEarningId,
                        ClientJobCostingAssignmentId1 = x.ClientJobCostingAssignmentId1,
                        ClientJobCostingAssignmentId2 = x.ClientJobCostingAssignmentId2,
                        ClientJobCostingAssignmentId3 = x.ClientJobCostingAssignmentId3,
                        ClientJobCostingAssignmentId4 = x.ClientJobCostingAssignmentId4,
                        ClientJobCostingAssignmentId5 = x.ClientJobCostingAssignmentId5,
                        ClientJobCostingAssignmentId6 = x.ClientJobCostingAssignmentId6,
                        ClockClientHolidayDetailId = x.ClockClientHolidayDetailId,
                        ClockEmployeeBenefitId = x.ClockEmployeeBenefitId,
                        EmployeeBenefitPay = x.EmployeeBenefitPay,
                        EmployeeClientRateId = x.EmployeeClientRateId,
                        RequestTimeOffDetailId = x.RequestTimeOffDetailId,
                        Subcheck = x.Subcheck,
                        IsWorkedHours = x.IsWorkedHours,
                        EmployeeComment = x.EmployeeComment,
                        Hours = x.Hours,
                        EventDate = x.EventDate,
                        IsApproved = x.IsApproved,
                        Comment = x.Comment,
                        ApprovedBy = x.ApprovedBy
                    };
                }
            }
        }

        /// <summary>
        /// Maps the Dto that replaces the data set in Sproc :  [dbo].[spGetClockEmployeeBenefitListByDate]
        /// This Mapper uses the logic from the Sproc to set the values for the Dto
        /// </summary>
        public class ToClockEmployeeBenefitListDto : ExpressionMapper<ClockEmployeeBenefit, ClockEmployeeBenefitListDto>
        {
            public override Expression<Func<ClockEmployeeBenefit, ClockEmployeeBenefitListDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeeBenefitListDto()
                    {
                        ClientCostCenterId = x.ClientCostCenterId ?? x.Employee.ClientCostCenterId, //ref Sproc Line 43
                        ClientDepartmentId = x.ClientDepartmentId ?? x.Employee.ClientDepartmentId, //ref Sproc Line 44
                        ClientDivisionId = x.ClientDivisionId,
                        ClientEarningId = x.ClientEarningId,
                        ClockEmployeeBenefitId = x.ClockEmployeeBenefitId,
                        EventDate = x.EventDate,
                        EmployeeId = x.EmployeeId,
                        Hours= x.Hours,
                        IsApproved = x.IsApproved ?? false, 
                        Comment = x.Comment,
                        ClientEarningCategoryId = x.ClientEarning.EarningCategoryId,
                        IsWorkedHours = x.ClientEarning.EarningCategoryId == ClientEarningCategory.Regular ? //ref Sproc Line 55
                                    x.IsWorkedHours ?? false : x.ClientEarning.IsIncludeInOvertimeCalcs,
                        Description = x.ClockClientHolidayDetailId > 0 ?  //ref Sproc Line 58
                                         x.ClockClientHolidayDetail.ClientHolidayName:
                                         x.ClientEarning.Description,
                        IsHoliday = x.ClockClientHolidayDetailId > 0 ? 1 : //ref Sproc Line 61
                                        x.ClientEarningId == x.ClockClientHolidayDetail.ClockClientHoliday.ClientEarningId ? 1 :  0
                    };
                }
            }
        }
    }
}