using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.Payroll
{
    public class PayrollAdjustmentDetail : Entity<PayrollAdjustmentDetail>, IHasModifiedUserNameData //IHasModifiedData
    {
        public virtual int       PayrollAdjustmentDetailId { get; set; }
        public virtual int       PayrollAdjustmentId       { get; set; }
        //public virtual int       PayrollAdjustmentTypeId   { get; set; }
        public virtual PayrollAdjustmentType PayrollAdjustmentTypeId { get; set; }
        public virtual int?      ForeignKeyId              { get; set; }
        public virtual decimal?  Amount                    { get; set; }
        public virtual double?   Hours                     { get; set; }
        public virtual string    AccountNumber             { get; set; }
        public virtual string    RoutingNumber             { get; set; }
        public virtual byte?     AccountType               { get; set; } //tinyint
        public virtual int?      AdditionalAmountTypeId    { get; set; }
        public virtual DateTime  Modified                  { get; set; }
        public virtual string    ModifiedBy                { get; set; }
        public virtual int?      ClientId                  { get; set; }
        //public virtual double?   TaxableWages              { get; set; }
        public virtual decimal?   TaxableWages              { get; set; }

        // Foreign keys
        public virtual PayrollAdjustment PayrollAdjustment { get; set; }
        public virtual Client Client { get; set; }
    }
}
