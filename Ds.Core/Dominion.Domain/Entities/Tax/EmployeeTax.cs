using System;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.EntityViews;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Tax
{
    public class EmployeeTax :
        Entity<EmployeeTax>, 
        IHasEmployeeId,
        IHasIsActive,
        IHasModifiedData, 
        IEmployeeOwnedEntity<EmployeeTax>,
        IEmployeeTax,
        IHasChangeHistoryEntityWithEnum<EmployeeTaxChangeHistory>
    {
        public virtual int EmployeeTaxId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual int? ClientTaxId { get; set; }
        public virtual ClientTax ClientTax { get; set; }
        public virtual FilingStatus FilingStatus { get; set; }
        public virtual FilingStatusInfo FilingStatusInfo { get; set; }
        public virtual byte NumberOfExemptions { get; set; }
        public virtual byte NumberOfDependents { get; set; }

        public virtual byte AdditionalTaxAmountTypeId { get; set; }

        public virtual double AdditionalTaxPercent { get; set; }
        public virtual double AdditionalTaxAmount { get; set; }

        public virtual bool IsResident { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual string Description { get; set; }

        public virtual int ClientId { get; set; }
        public virtual Client Client { get; set; }
        public virtual int? ResidentId { get; set; }
        public virtual bool Reimburse { get; set; }
        public virtual decimal TaxCredit { get; set; }
        public virtual decimal OtherTaxableIncome { get; set; }
        public virtual decimal WageDeduction { get; set; }
        public virtual bool HasMoreThanOneJob { get; set; }
        public virtual bool Using2020FederalW4Setup { get; set; }
        //public virtual int? MaritalStatusId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }


        #region IEmployeeOwnedEntity IMPLEMENTATION

        /// <summary>
        /// Gets a view that selects only the Employee ID from an employee tax entity.
        /// </summary>
        /// <returns></returns>
        public Expression<Func<EmployeeTax, EmployeeTax>> GetEmployeeIdView()
        {
            return EmployeeIdView;
        }

        /// <summary>
        /// View used to select only the Employee ID from the employee tax entity.
        /// </summary>
        public static Expression<Func<EmployeeTax, EmployeeTax>> EmployeeIdView
        {
            get { return x => new EmployeeTaxEntityView {EmployeeId = x.EmployeeId}; }
        }

        #endregion

        #region Filters

        /// <summary>
        /// Predicate definition used to limit based on a specific employee Tax.
        /// </summary>
        /// <param name="employeeId">The employe Tax id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeTax, bool>> HasEmployeeTaxId(int employeeTaxId)
        {
            return x => x.EmployeeTaxId == employeeTaxId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific employee.
        /// </summary>
        /// <param name="employeeId">The employee's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeTax, bool>> ForEmployee(int employeeId)
        {
            return x => x.EmployeeId == employeeId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific client.
        /// </summary>
        /// <param name="employeeId">The client's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeTax, bool>> ForClient(int clientId)
        {
            return x => x.ClientId == clientId;
        }

        /// <summary>
        /// Predicate definition used to limit based on the employee tax active status.
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<EmployeeTax, bool>> IsActiveTax()
        {
            return x => x.IsActive;
        }

        /// <summary>
        /// Limits employee tax results to employee configurable taxes. 
        /// Currently, excludes Unemployment (SUTA), Disability 
        /// and Employer Paid taxes.
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<EmployeeTax, bool>> IsEmployeeConfigurable()
        {
            return x => (x.ClientTax == null) ||
                        (x.ClientTax.LegacyTaxTypeInfo.TaxGroupCode != TaxGroups.Unemployment.TaxGroupCode &&
                         x.ClientTax.LegacyTaxTypeInfo.TaxGroupCode != TaxGroups.Disability.TaxGroupCode &&
                         x.ClientTax.LegacyTaxTypeInfo.TaxGroupCode != TaxGroups.EmployerPaid.TaxGroupCode);
        }

        #endregion

    }
}