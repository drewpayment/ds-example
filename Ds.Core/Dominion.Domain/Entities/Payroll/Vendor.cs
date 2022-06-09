using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Contact;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Contact;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class Vendor : Entity<Vendor>, IHasModifiedData, IVendorEntity, IHasChangeHistoryEntityWithEnum<VendorChangeHistory>
    {
        public virtual int VendorId { get; set; }
        public virtual string Name { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual int TaxFrequencyId { get; set; }
        public virtual int? BankId { get; set; }
        public virtual string RoutingNumber { get; set; }
        public virtual int? AccountTypeId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public Vendor()
        {
        }
    }
}


