using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.ClientAccrual record.
    /// </summary>
    public partial class ClientAccrual : Entity<ClientAccrual>, IHasModifiedUserNameData
    {
        public virtual int ClientAccrualId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int ClientEarningId { get; set; }
        public virtual string Description { get; set; }
        public virtual int EmployeeStatusId { get; set; }
        public virtual PayType EmployeeTypeId { get; set; }
        public virtual bool IsShowOnStub { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual int AccrualBalanceOptionId { get; set; }
        public virtual int ServiceReferencePointId { get; set; }
        public virtual int PlanType { get; set; }
        public virtual int Units { get; set; }
        public virtual bool IsShowPreviewMassages { get; set; }
        public virtual string Notes { get; set; }
        public virtual int? BeforeAfterId { get; set; }
        public virtual DateTime? BeforeAfterDate { get; set; }
        public virtual int? CarryOverToId { get; set; }
        public virtual bool? IsUseCheckDates { get; set; }
        public virtual bool? IsLeaveManagment { get; set; }
        public virtual int? LeaveManagmentAdministrator { get; set; }
        public virtual double? RequestMinimum { get; set; }
        public virtual double? RequestMaximum { get; set; }
        public virtual double? HoursInDay { get; set; }
        public virtual string AtmInterfaceCode { get; set; }
        public virtual bool? IsPeriodEnd { get; set; }
        public virtual bool? IsPeriodEndPlusOne { get; set; }
        public virtual DateTime? ShowAccrualBalanceStartingFrom { get; set; }
        public virtual bool? IsAccrueWhenPaid { get; set; }
        public virtual int AccrualCarryOverOptionId { get; set; }
        public virtual bool IsEmailSupervisorRequests { get; set; }
        public virtual bool IsCombineByEarnings { get; set; }
        public virtual bool IsLeaveManagementUseBalanceOption { get; set; }
        public virtual bool IsRealTimeAccruals { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int? AccrualClearOptionId { get; set; }
        public virtual bool IsStopLastPay { get; set; }
        public virtual double RequestIncrement { get; set; }
        public virtual string PolicyLink { get; set; }
        public virtual double ProjectAmount { get; set; }
        public virtual byte ProjectAmountType { get; set; }
        public virtual bool IsEmpEmailRequest { get; set; }
        public virtual bool IsEmpEmailApproval { get; set; }
        public virtual bool IsEmpEmailDecline { get; set; }
        public virtual bool IsSupEmailRequest { get; set; }
        public virtual bool IsSupEmailApproval { get; set; }
        public virtual bool IsSupEmailDecline { get; set; }
        public virtual bool IsCaEmailRequest { get; set; }
        public virtual bool IsCaEmailApproval { get; set; }
        public virtual bool IsCaEmailDecline { get; set; }
        public virtual bool IsLmaEmailRequest { get; set; }
        public virtual bool IsLmaEmailApproval { get; set; }
        public virtual bool IsLmaEmailDecline { get; set; }
        public virtual double BalanceOptionAmount { get; set; }
        public virtual double? LmMinAllowedBalance { get; set; }
        public virtual bool IsDisplay4Decimals { get; set; }
        public virtual double? WaitingPeriodValue { get; set; }
        public virtual int? WaitingPeriodTypeId { get; set; }
        public virtual int? WaitingPeriodReferencePoint { get; set; }
        public virtual bool AllowAllDays { get; set; }
        public virtual bool IsPaidLeaveAct { get; set; }
        public virtual double? HoursPerWeekAct { get; set; }
        public virtual bool AllowHoursRollOver { get; set; }
        public virtual int? ProratedServiceReferencePointOverrideId { get; set; }
        public virtual ProratedAccrualWhenToAwardType? ProratedWhenToAwardTypeId { get; set; }
        public virtual bool CarryOverToBalanceLimit { get; set; }

        public virtual Client        Client        { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }
        public virtual ICollection<ClientAccrualEarning> ClientAccrualEarnings { get; set; }
        public virtual ICollection<ClientAccrualSchedule> ClientAccrualSchedules { get; set; }
        public virtual ICollection<ClientAccrualProratedSchedule> ClientAccrualProratedSchedules { get; set; }
        public virtual ICollection<JobProfileAccruals> JobProfileAccruals { get; set; }
        public virtual ICollection<EmployeeAccrual> EmployeeAccrual { get; set; }
        public virtual ICollection<LeaveManagementPendingAward> LeaveManagementPendingAwards { get; set; }


        public virtual AccrualBalanceOption AccrualBalanceOption { get; set; }

    }
}
