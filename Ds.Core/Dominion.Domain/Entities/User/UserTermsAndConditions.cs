using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.User
{
    public class UserTermsAndConditions : Entity<UserTermsAndConditions>
    {
        public int UserTermsAndConditionsID { get; set; }
        public int UserId { get; set; }
        public DateTime? AcceptDate { get; set; }
        public bool UserAccepted { get; set; }
        public int? TermsAndConditionsVersionId { get; set; }

    }
}
