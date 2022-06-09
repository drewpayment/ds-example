using System;

using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    using Dominion.Domain.Entities.Payroll;

    public partial class PreviewEmployeeBenefitImport : Entity<PreviewEmployeeBenefitImport>, IHasModifiedOptionalData
    {
        public virtual int?                              EmployeeDeductionId   { get; set; }
        public virtual int                               ClientDeductionId     { get; set; } 
        public virtual int?                              EmployeeBankId        { get; set; } 
        public virtual int?                              EmployeeBondId        { get; set; } 
        public virtual int                               EmployeeId            { get; set; } 
        public virtual int?                              ClientPlanId          { get; set; } 
        public virtual double?                           CurrentAmount         { get; set; } 
        public virtual double?                           NewAmount             { get; set; }
        public virtual EmployeeDeductionAmountType?      DeductionAmountTypeId { get; set; } 
        public virtual double?                           Max                   { get; set; } 
        public virtual EmployeeDeductionMaxType?         MaxType               { get; set; } 
        public virtual double?                           TotalMax              { get; set; } 
        public virtual int?                              ClientVendorId        { get; set; } 
        public virtual string                            AdditionalInfo        { get; set; } 
        public virtual bool                              IsActive              { get; set; } 
        public virtual byte?                             SubSequenceNum        { get; set; } 
        public virtual DateTime?                         Modified              { get; set; } 
        public virtual int?                              ModifiedBy            { get; set; } 
        public virtual int                               ClientId              { get; set; } 
        public virtual string                            ClientCode            { get; set; }
    }
}
