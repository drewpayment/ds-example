using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.ClockEmployeBenefit record.
    /// </summary>
    public partial class ClockEmployeeBenefit : Entity<ClockEmployeeBenefit>, IHasModifiedData
    {
        public virtual int       ClockEmployeeBenefitId             { get; set; } 
        public virtual int       EmployeeId                    { get; set; } 
        public virtual int       ClientEarningId               { get; set; } 
        public virtual double?   Hours                         { get; set; } 
        public virtual DateTime? EventDate                     { get; set; } 
        public virtual int?      ClientCostCenterId            { get; set; } 
        public virtual int?      ClientDivisionId              { get; set; } 
        public virtual int?      ClientDepartmentId            { get; set; } 
        public virtual int?      ClientShiftId                 { get; set; } 
        public virtual bool?     IsApproved                    { get; set; } 
        public virtual int       ModifiedBy                    { get; set; } 
        public virtual DateTime  Modified                      { get; set; } 
        public virtual string    Comment                       { get; set; } 
        public virtual int?      ClockClientHolidayDetailId    { get; set; } 
        public virtual bool?     IsWorkedHours                 { get; set; } 
        public virtual int?      RequestTimeOffDetailId        { get; set; } 
        public virtual int       ClientId                      { get; set; } 
        public virtual string    Subcheck                      { get; set; } 
        public virtual int?      ClientJobCostingAssignmentId1 { get; set; } 
        public virtual int?      ClientJobCostingAssignmentId2 { get; set; } 
        public virtual int?      ClientJobCostingAssignmentId3 { get; set; } 
        public virtual int?      ClientJobCostingAssignmentId4 { get; set; } 
        public virtual int?      ClientJobCostingAssignmentId5 { get; set; } 
        public virtual int?      ClientJobCostingAssignmentId6 { get; set; } 
        public virtual string    EmployeeComment               { get; set; } 
        public virtual int?      EmployeeClientRateId          { get; set; } 
        public virtual decimal?  EmployeeBenefitPay            { get; set; } 
        public virtual int?      ApprovedBy                    { get; set; }

        public virtual Client               Client               { get; set; }
        public virtual Employee.Employee    Employee             { get; set; }
        public virtual ClientEarning        ClientEarning        { get; set; }
        public virtual ClientCostCenter     ClientCostCenter     { get; set; }
        public virtual ClientDepartment     ClientDepartment     { get; set; }
        public virtual ClockClientHolidayDetail  ClockClientHolidayDetail  { get; set; }
        public virtual TimeOffRequestDetail TimeOffRequestDetail { get; set; }
    }
}