using Dominion.Domain.Entities.User;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IUserTermsAndConditionsQuery : IQuery<UserTermsAndConditions, IUserTermsAndConditionsQuery>
    {
        IUserTermsAndConditionsQuery ByUserId(int userId);
    }
}
