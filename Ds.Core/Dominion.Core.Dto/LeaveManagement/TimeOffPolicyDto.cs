using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffPolicyDto
    {
        /// <summary>
        /// The time off policy id.
        /// In some cases this can be a number other than an actually policy number.
        ///     - An earning ID can be substituted for policy id's in come business logic
        /// </summary>
        public int PolicyId { get; set; }

        /// <summary>
        /// The name of the time off policy.
        /// </summary>
        public string PolicyName { get; set; }
    }
}
