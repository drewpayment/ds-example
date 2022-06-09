using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeCostCenterQuery : Query<ClockEmployeeCostCenter, IClockEmployeeCostCenterQuery>, IClockEmployeeCostCenterQuery
    {
        #region Constructor

        /// <summary>
        /// Intitializes a new <see cref="GroupScheduleQuery"/>.
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public ClockEmployeeCostCenterQuery(IEnumerable<ClockEmployeeCostCenter> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        IClockEmployeeCostCenterQuery IClockEmployeeCostCenterQuery.ByCostCenterId(int clientCostCenterId)
        {
            FilterBy(x => x.ClientCostCenterId == clientCostCenterId);
            return this;
        }

        public IClockEmployeeCostCenterQuery ByClientId(int clientId)
        {
            FilterBy(x => x.CostCenter.ClientId == clientId);
            
            return this;
        }

        public IClockEmployeeCostCenterQuery ByEmployeeId(int employeeId)
        {

            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }
    }
}
