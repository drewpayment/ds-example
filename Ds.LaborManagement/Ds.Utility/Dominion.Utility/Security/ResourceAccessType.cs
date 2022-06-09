using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Security
{
    /// <summary>
    /// Defines a resource's ownership.
    /// If it's Employee this means it's an employee's resource.
    /// </summary>
    public enum ResourceOwnership
    {
        Client = 1,
        User = 2,
        Employee = 3,
        Applicant = 4
    }
}
