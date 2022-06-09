using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public partial class DominionVendorDto
    {
        public int DominionVendorId { get; set; }
        public string Name { get; set; }
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
        public string RoutingNumber { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
