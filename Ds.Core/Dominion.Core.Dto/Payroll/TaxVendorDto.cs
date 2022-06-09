using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class TaxVendorDto
    {
        public int TaxVendorId { get; set; }
        public string Name { get; set; }
        public int TaxTypeId { get; set; }
        public int? ForeignKeyId { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }
        public int CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; }
        public int TaxFrequencyId { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public string AchCodePayroll { get; set; }
        public string AchCodeMonthly { get; set; }
        public string AchCodeQuarterly { get; set; }
    }
}
