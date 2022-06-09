using System;
using System.Linq.Expressions;
using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.EntityViews;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;

namespace Dominion.Domain.Entities.Tax
{
    public class EmployeeTaxChangeHistory : 
        Entity<EmployeeTaxChangeHistory>,        
        IHasEmployeeId,
        IHasIsActive,
        IEmployeeTax,
        IHasChangeHistoryDataWithEnum
    {
        public virtual int                      ChangeId { get; set; }
        public ChangeModeType  ChangeMode { get; set; }
        public virtual DateTime                 ChangeDate { get; set; }

        public virtual int                      EmployeeTaxId { get; set; }
        public virtual int                      EmployeeId { get; set; }
        public virtual Employee.Employee        Employee { get; set; }
        public virtual int?                     ClientTaxId { get; set; }
        public virtual ClientTax                ClientTax { get; set; }
        public virtual FilingStatus             FilingStatus { get; set; }
        public virtual FilingStatusInfo         FilingStatusInfo { get; set; }
        public virtual byte                     NumberOfExemptions { get; set; }
        public virtual byte                     NumberOfDependents { get; set; }

        public virtual byte                     AdditionalTaxAmountTypeId { get; set; }
        public virtual double                   AdditionalTaxPercent { get; set; }
        public virtual double                   AdditionalTaxAmount { get; set; }
                                                
        public virtual bool                     IsResident { get; set; }
        public virtual bool                     IsActive { get; set; }

        public virtual string                   Description { get; set; }

        public virtual int                      ClientId { get; set; }
        public virtual Client                   Client { get; set; }

        //public virtual int?                     ResidentId { get; set; }

        public virtual decimal TaxCredit { get; set; }
        public virtual decimal OtherTaxableIncome { get; set; }
        public virtual decimal WageDeduction { get; set; }
        public virtual bool HasMoreThanOneJob { get; set; }
        public virtual bool Using2020FederalW4Setup { get; set; }

        public int ModifiedBy { get; set; }
    }
}