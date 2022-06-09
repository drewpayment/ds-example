using Dominion.Core.Dto.Client;
using System;

namespace Dominion.Core.Dto.User
{
    public class UserPinDto
    {
        public int UserPinId { get; set; }
        public int? UserId { get; set; }
        public int? ClientContactId { get; set; }
        public int ClientId { get; set; }
        public string Pin { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        // RELATIONSHIPS
        public UserDto User { get; set; }
        public ClientContactDto ClientContact { get; set; }
    }
}
