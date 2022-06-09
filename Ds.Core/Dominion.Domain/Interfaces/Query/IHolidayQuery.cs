using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IHolidayQuery : IQuery<Holiday, IHolidayQuery>
    {
        IHolidayQuery ByHolidayId(int holidayId);
    }
}
