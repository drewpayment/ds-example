using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockClientTimePolicyListQuery : IQuery<ClockClientTimePolicy, IClockClientTimePolicyListQuery>
    {
   
        /// <summary>
        /// pass:1
        /// Filter results by client id.
        /// </summary>
        /// <param name="clientId">The client id.</param>
        /// <returns></returns>
        IClockClientTimePolicyListQuery ByClientId(int clientId);

        /// <summary>
        /// Filter results by clock client exception id.
        /// </summary>
        /// <param name="clockClientExceptionId">The clock client exception id.</param>
        /// <returns></returns>
        IClockClientTimePolicyListQuery ByClockClientExceptionId(int clockClientExceptionId);
    }
}
