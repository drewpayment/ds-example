using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientWorkNumberPolicyDto
    {
        public int ClientWorkNumberPolicyId { get; set; }
        public int ClientId { get; set; }
        public int AcceptedBy { get; set; }
        public DateTime Modified { get; set; }
        public Boolean IsAccepted { get; set; }

        public User.UserDto User {
            get;
            set; 
        }
    }
}
