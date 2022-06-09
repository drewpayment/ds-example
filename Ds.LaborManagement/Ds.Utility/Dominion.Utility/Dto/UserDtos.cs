using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Dto
{

    public class UserClientDto
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public bool IsClientAdmin { get; set; }
        public bool IsBenefitAdmin { get; set; }

        public string AdminEmailAddress { get; set; }
        public string AdminFirstName { get; set; }
        public string AdminLastName { get; set; }


    } // class UserDto

}
