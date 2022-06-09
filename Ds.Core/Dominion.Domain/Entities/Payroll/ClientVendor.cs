using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class ClientVendor : Entity<ClientVendor>
    {
        public virtual int ClientVendorId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Name { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual int CountryId { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual string FipsCode { get; set; }
        public virtual string CaseNumber { get; set; }
        public virtual string Msi { get; set; }
        public virtual string AbsentParentName { get; set; }
        public virtual string AbsentParentSsn { get; set; }
        public virtual int? BankId { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int TaxFrequencyId { get; set; }
        public virtual int? AccountTypeId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int? EmployeeId { get; set; }

        #region Foreign Keys
        public virtual Employee.Employee Employee { get; set; }
        public virtual State State { get; set; }
        public virtual Country Country { get; set; } 
        #endregion

        public ClientVendor()
        {
        }
    }
}