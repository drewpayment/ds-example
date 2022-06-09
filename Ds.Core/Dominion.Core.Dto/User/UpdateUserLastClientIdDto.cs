using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// Defines a DTO that represents a request to update a user's last client ID.
    /// </summary>
    public class UpdateUserLastClientIdDto
    {
        /// <summary>
        /// The new client ID that the user should be viewing.
        /// </summary>
        public int ClientId { get; set; }
        public int? EmployeeId { get; set; }
    }
}
