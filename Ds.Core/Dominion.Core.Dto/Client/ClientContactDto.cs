using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientContactDto
    {
        public virtual int ClientContactId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Title { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string PhoneExtension { get; set; }
        public virtual string MobilePhoneNumber { get; set; }
        public virtual string Fax { get; set; }
        public virtual bool IsPrimary { get; set; }
        public virtual bool IsDelivery { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int? UserPinId { get; set; }
    }
}
