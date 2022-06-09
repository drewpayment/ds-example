using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    public class PayrollEmployeeOverride : Entity<PayrollEmployeeOverride>
    {
        public virtual int PayrollEmployeeOverrideId { get; set; }
        public virtual int? PayrollPayDataId { get; set; }
        public virtual int? PayrollAdjustmentId { get; set; }
        public virtual int? ClientWorkersCompId { get; set; }
        public virtual int? ClientGroupId { get; set; }
        public virtual int? ClientDivisionId { get; set; }
        public virtual int? ClientDepartmentId { get; set; }
        public virtual int? SutaStateClientTaxId { get; set; }
        public virtual int? ClientCostCenterId { get; set; }
        public virtual int? ClientShiftId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string Modifiedby { get; set; }
        public virtual int? ClientRateId { get; set; }
        public virtual decimal? OverrideRateAmount { get; set; }
        public virtual bool? IsModifiedRecently { get; set; }
        public virtual bool? IsClientRateIdUpdated { get; set; }
        public virtual int? TaxFactorId { get; set; }
        public virtual bool? IsStopSalary { get; set; }
        public virtual int? ClientPayDataInterfaceId { get; set; }
        public virtual byte? PayrollPayDataInterfaceId { get; set; }
        public virtual string SourceId { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual bool? IsTippedEmployee { get; set; }

        // FOREIGN KEYS
        public virtual PayrollPayData PayrollPayData { get; set; }
        public virtual ClientCostCenter ClientCostCenter { get; set; }
        public virtual PayrollAdjustment PayrollAdjustment { get; set; }
    }
}