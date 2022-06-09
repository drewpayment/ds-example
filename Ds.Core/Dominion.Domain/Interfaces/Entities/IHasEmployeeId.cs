using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Entities
{
    /// <summary>
    /// Common in entities.
    /// </summary>
    public interface IHasEmployeeId
    {
        int      EmployeeId          { get; set; }
    }

    /// <summary>
    /// Common in entities.
    /// </summary>
    public interface IHasIsActive
    {
        bool IsActive { get; set; }
    }

    /// <summary>
    /// Common in change history entities.
    /// </summary>
    public interface IHasChangeDate
    {
        DateTime ChangeDate { get; set; }
    }

}
