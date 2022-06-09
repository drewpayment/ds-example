using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Constants;
using Dominion.Core.Dto.Contact;

namespace Dominion.Core.Dto.Payroll
{
    public partial class VendorDto: AddressDto
    {
        public virtual int VendorId { get; set; }
        public virtual string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneExtension { get; set; }
        public string AccountNumber { get; set; }
        public int TaxFrequencyId { get; set; }
        public int? BankId { get; set; }
        public bool IsActive { get; set; }
        public string RoutingNumber { get; set; }
        public int? AccountTypeId { get; set; }
    }
}
