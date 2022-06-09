using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEffectiveDateQuery : IQuery<EffectiveDate, IEffectiveDateQuery>
    {
        IEffectiveDateQuery ByEffectiveId(int effectiveId);
        IEffectiveDateQuery ByEmployeeId(int employeeId);
        IEffectiveDateQuery ByEffectiveDateTypeId(EffectiveDateTypes type);
        IEffectiveDateQuery ByDateEffective(DateTime effectiveId);
    }
}
