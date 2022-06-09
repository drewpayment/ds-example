using System;

namespace Dominion.Domain.Entities.Payroll
{
    public interface IVendorEntity
    {
        string AccountNumber { get; set; }
        int? AccountTypeId { get; set; }
        int AddressId { get; set; }
        int? BankId { get; set; }
        bool IsActive { get; set; }
        DateTime Modified { get; set; }
        int ModifiedBy { get; set; }
        string Name { get; set; }
        string PhoneExtension { get; set; }
        string PhoneNumber { get; set; }
        string RoutingNumber { get; set; }
        int TaxFrequencyId { get; set; }
        int VendorId { get; set; }
    }
}