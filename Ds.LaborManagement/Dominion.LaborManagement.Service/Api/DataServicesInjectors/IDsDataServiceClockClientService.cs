using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Service.Api.DataServicesInjectors
{
    public interface IDsDataServiceClockClientService
    {

        /// <summary>
        /// Auto applies the time policy updates and inserts a new record into ClockEmployee table. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="userId"></param>
        /// <param name="whatToApply"></param>
        /// <param name="clockClientTimePolicyId"></param>
        /// <returns></returns>
        int AutoApplyClockSchedulesAndTimePolicies(int clientId, int userId, int whatToApply, int clockClientTimePolicyId); 

    }
}
