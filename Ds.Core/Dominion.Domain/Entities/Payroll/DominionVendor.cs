using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class DominionVendor
    {
        public virtual int DominionVendorId { get; set; }
        public virtual string Name { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual int CountryId { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual int? BankId { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
    }
}
