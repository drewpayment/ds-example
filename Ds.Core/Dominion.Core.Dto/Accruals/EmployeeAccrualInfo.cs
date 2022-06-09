using System;

namespace Dominion.Core.Dto.Accruals
{
    public class EmployeeAccrualListDto
    {
        public int                    EmployeeId              { get; set; }
        public int                    ClientAccrualId         { get; set; }
        public ServicePlanTypeType    PlanType                { get; set; }
        public ServiceBeforeAfterType BeforeAfterId           { get; set; }
        public DateTime               BeforeAfterDate         { get; set; }
        public string                 Description             { get; set; }
        public bool                   AllowScheduledAwards    { get; set; }
        public bool                   IsActive                { get; set; }
        public bool                   Display4Decimals        { get; set; }
        public ServiceReferencePointType  ServiceReferencePointId { get; set; }
        public DateTime?              ServiceReferenceDate    { get; set; }
    }

    public class EmployeeAccrualInfoDto
    {
        public int EmployeeAccrualId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientAccrualId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int NumDecimals { get; set; }
        public bool AllowScheduledAwards { get; set; }
        public decimal Rate { get; set; }
        public decimal Balance { get; set; }
        public bool IsVisible { get; set; }
        public bool ShowReferenceDate { get; set; }
        public DateTime? ReferenceDate { get; set; }
        public string ReferencePoint { get; set; }
    }

    public class GenPayCheckAccrualHistoryDto
    {
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal GenCredit { get; set; }
        public decimal GenDebit { get; set; }
        public decimal GenTotalAmount { get; set; }
    }
}