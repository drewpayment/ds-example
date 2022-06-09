using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.LeaveManagement
{

    public interface IClientAccrualBasicDto
    {
        int       ClientAccrualId   { get; set; } 
        int       ClientId          { get; set; } 
        string    Description       { get; set; } 
    }

    public class ClientAccrualBasicDto : IClientAccrualBasicDto
    {
        public int       ClientAccrualId    { get; set; } 
        public int       ClientId           { get; set; } 
        public string    Description        { get; set; } 
    }

    public class ClientAccrualDto : IClientAccrualBasicDto
    {
        public int       ClientAccrualId                   { get; set; } 
        public int       ClientId                          { get; set; } 
        public int       ClientEarningId                   { get; set; } 
        public string    Description                       { get; set; } 
        public int       EmployeeStatusId                  { get; set; } 
        public PayType       EmployeeTypeId                { get; set; } 
        public bool      IsShowOnStub                      { get; set; } 
        public DateTime  Modified                          { get; set; } 
        public string    ModifiedBy                        { get; set; } 
        public int       AccrualBalanceOptionId            { get; set; } 
        public int       ServiceReferencePointId           { get; set; } 
        public int       PlanType                          { get; set; } 
        public int       Units                             { get; set; } 
        public bool      IsShowPreviewMassages             { get; set; } 
        public string    Notes                             { get; set; } 
        public int?      BeforeAfterId                     { get; set; } 
        public DateTime? BeforeAfterDate                   { get; set; } 
        public int?      CarryOverToId                     { get; set; } 
        public bool?     IsUseCheckDates                   { get; set; } 
        public bool?     IsLeaveManagment                  { get; set; } 
        public int?      LeaveManagmentAdministrator       { get; set; } 
        public double?   RequestMinimum                    { get; set; } 
        public double?   RequestMaximum                    { get; set; } 
        public double?   HoursInDay                        { get; set; } 
        public string    AtmInterfaceCode                  { get; set; } 
        public bool?     IsPeriodEnd                       { get; set; } 
        public bool?     IsPeriodEndPlusOne                { get; set; } 
        public DateTime? ShowAccrualBalanceStartingFrom    { get; set; } 
        public bool?     IsAccrueWhenPaid                  { get; set; } 
        public int       AccrualCarryOverOptionId          { get; set; } 
        public bool      IsEmailSupervisorRequests         { get; set; } 
        public bool      IsCombineByEarnings               { get; set; } 
        public bool      IsLeaveManagementUseBalanceOption { get; set; } 
        public bool      IsRealTimeAccruals                { get; set; } 
        public bool      IsActive                          { get; set; } 
        public int?      AccrualClearOptionId              { get; set; } 
        public bool      IsStopLastPay                     { get; set; } 
        public double    RequestIncrement                  { get; set; } 
        public string    PolicyLink                        { get; set; } 
        public double    ProjectAmount                     { get; set; } 
        public byte      ProjectAmountType                 { get; set; } 
        public bool      IsEmpEmailRequest                 { get; set; } 
        public bool      IsEmpEmailApproval                { get; set; } 
        public bool      IsEmpEmailDecline                 { get; set; } 
        public bool      IsSupEmailRequest                 { get; set; } 
        public bool      IsSupEmailApproval                { get; set; } 
        public bool      IsSupEmailDecline                 { get; set; } 
        public bool      IsCaEmailRequest                  { get; set; } 
        public bool      IsCaEmailApproval                 { get; set; } 
        public bool      IsCaEmailDecline                  { get; set; } 
        public bool      IsLmaEmailRequest                 { get; set; } 
        public bool      IsLmaEmailApproval                { get; set; } 
        public bool      IsLmaEmailDecline                 { get; set; } 
        public double    BalanceOptionAmount               { get; set; } 
        public double?    LmMinAllowedBalance              { get; set; } 
        public bool      IsDisplay4Decimals                { get; set; }
        public double?   WaitingPeriodValue                { get; set; }
        public int?      WaitingPeriodTypeId               { get; set; }
        public int?      WaitingPeriodReferencePoint       { get; set; }
        public bool AllowAllDays { get; set; }
        public bool      IsPaidLeaveAct                    { get; set; }
        public double?   HoursPerWeekAct                   { get; set; }
        public bool AllowHoursRollOver { get; set; }
        public int? ProratedServiceReferencePointOverrideId { get; set; }
        public ProratedAccrualWhenToAwardType? ProratedWhenToAwardTypeId { get; set; }
        public bool CarryOverToBalanceLimit            { get; set; }


        public ClientEarningDto ClientEarning { get; set; }
        public IEnumerable<ClientAccrualEarningDto> ClientAccrualEarnings { get; set; }
        public IEnumerable<ClientAccrualScheduleDto>  ClientAccrualSchedules { get; set; }
        public IEnumerable<ClientAccrualProratedScheduleDto>  ClientAccrualProratedSchedules { get; set; }
        public IEnumerable<EmployeeAccrualDto> EmployeeAccruals { get; set; }
        public IEnumerable<JobProfileAccrualsDto> JobProfileAccruals { get; set; }
        public IEnumerable<LeaveManagementPendingAwardDto> LeaveManagementPendingAwards { get; set; }

        public AccrualBalanceOptionDto AccrualBalanceOption { get; set; }
    }
}


