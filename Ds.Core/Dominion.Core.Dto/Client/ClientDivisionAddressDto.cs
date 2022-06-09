using Dominion.Core.Dto.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientDivisionAddressDto
    {
        public virtual int ClientDivisionAddressId { get; set; }
        public virtual int ClientDivisionId { get; set; }
        public virtual int ClientContactId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Zip { get; set; }
        public virtual int CountryId { get; set; }
        public virtual string StateAbbreviation { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        //FK
        public virtual ClientDivisionDto ClientDivision { get; set; }
        public virtual ClientContactDto ClientContact { get; set; }
        public virtual StateDto State { get; set; }
    }
}
