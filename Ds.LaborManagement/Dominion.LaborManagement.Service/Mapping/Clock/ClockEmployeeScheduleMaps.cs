using System;
using System.Configuration;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockEmployeeScheduleMaps
    {
        public class ToClockEmployeeScheduleListDto :
            ExpressionMapper<ClockEmployeeSchedule, ClockEmployeeScheduleListDto>
        {
            public override Expression<Func<ClockEmployeeSchedule, ClockEmployeeScheduleListDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeeScheduleListDto()
                    {
                        ClientEmployeeScheduleId = x.ClockEmployeeScheduleId,
                        ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                        EventDate = x.EventDate,
                        StartTime = x.StartTime1,
                        StopTime = x.EndTime1,
                        EmployeeId = x.EmployeeId,
                        Modified = x.Modified,
                        ModifiedBy = x.ModifiedBy,
                        ClockClientSchedule_ChangeHistory_ChangeId = x.ClockClientScheduleChangeHistoryChangeId,
                        ClientId = x.ClientId,
                        ClientCostCenterId = x.ClientCostCenterId1,
                        ClientDepartmentId = x.Employee.ClientDepartmentId,
                        Schedule2StartTime = x.StartTime2,
                        Schedule2StopTime = x.EndTime2,
                        Schedule2ClientCostCenterId = x.ClientCostCenterId2,
                        Schedule2ClientDepartmentId = x.ClientDepartmentId2,
                        Schedule3StartTime = x.StartTime3,
                        Schedule3StopTime = x.EndTime3,
                        Schedule3ClientDepartmentId = x.ClientDepartmentId3,
                        Schedule3ClientCostCenterId = x.ClientCostCenterId3,
                        GroupScheduleShiftId = x.GroupScheduleShiftId1,
                        Schedule2GroupScheduleShiftId = x.GroupScheduleShiftId2,
                        Schedule3GroupScheduleShiftId = x.GroupScheduleShiftId3,
                        FirstName = x.Employee.FirstName,
                        LastName = x.Employee.LastName,
                        MiddleInitial = x.Employee.MiddleInitial,
                        Address1 = x.Employee.AddressLine1,
                        Address2 = x.Employee.AddressLine2,
                        City = x.Employee.City,
                        StateId = x.Employee.StateId,
                        ZipCode = x.Employee.PostalCode,
                        CountryId = x.Employee.CountryId,
                        CountyId = x.Employee.CountyId,
                        HomePhone = x.Employee.HomePhoneNumber,
                        SocialSecurityNumber = x.Employee.SocialSecurityNumber,
                        Gender = x.Employee.Gender,
                        DateOfBirth = x.Employee.BirthDate,
                        EmployeeNumber = x.Employee.EmployeeNumber,
                        JobTitle = x.Employee.JobTitle,
                        JobClass = x.Employee.JobClass,
                        ClientDivisionId = x.Employee.ClientDivisionId,
                        ClientGroupId = x.Employee.ClientGroupId,
                        ClientWorkersCompId = x.Employee.ClientWorkersCompId,
                        IsW2Pension = x.Employee.IsW2Pension,
                        HireDate = x.Employee.HireDate,
                        RehireDate = x.Employee.RehireDate,
                        SeparationDate = x.Employee.SeparationDate,
                        AnniversaryDate = x.Employee.AnniversaryDate,
                        EligibilityDate = x.Employee.EligibilityDate,
                        IsActive = x.Employee.IsActive, //not sure is we use this one (employee active is determined on EP)
                        Email = x.Employee.EmailAddress,
                        PayStubOption = x.Employee.PayStubOption,
                        Notes = x.Employee.Notes,
                        CostCenterType = x.Employee.CostCenterType,
                        NewHireDataSent = x.Employee.IsNewHireDataSent,
                        MaritalStatusId = x.Employee.MaritalStatusId,
                        EEOCRaceId = x.Employee.EeocRaceId,
                        EEOCJobCategoryId = x.Employee.EeocJobCategoryId,
                        EEOCLocationId = x.Employee.EeocLocationId,
                        CellPhone = x.Employee.CellPhoneNumber,
                        JobProfileId = x.Employee.JobProfileId,
                        //PsdCode = x.Employee.PsdCode //see ClockEmployeeMapSchedule
                        //IsInOnboarding = x.Employee.IsInOnBoarding,
                        DepartmentName = x.ClientDepartment1.Name,
                        Schedule2DepartmentName = x.ClientDepartment2.Name,
                        Schedule3DepartmentName = x.ClientDepartment3.Name,
                        Schedule2CostCenterName = x.ClientCostCenter2.Description,
                        Schedule3CostCenterName = x.ClientCostCenter3.Description,
                    };
                }
            }
        }
    }
}