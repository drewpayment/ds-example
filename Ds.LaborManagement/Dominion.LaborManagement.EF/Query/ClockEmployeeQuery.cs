using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeQuery : Query<ClockEmployee, IClockEmployeeQuery>, IClockEmployeeQuery
    {
        private int? _clientId;

        public ClockEmployeeQuery(IEnumerable<ClockEmployee> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByBadgeNumber(string badgeNumber)
        {
            FilterBy(c => c.BadgeNumber.Equals(badgeNumber));
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByClientId(int clientId, bool includeAllClientsInOrganization)
        {

            if (includeAllClientsInOrganization)
            {
                FilterBy(c => c.ClientId == clientId || c.Client.ClientRelations.SelectMany(r => r.Clients).Any(y => y.ClientId == clientId));
            }
            else
            {
                FilterBy(c => c.ClientId == clientId);
                //store for other filters
                _clientId = clientId;
            }
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByEmployeeId(int employeeId)
        {
            FilterBy(c => c.EmployeeId == employeeId);
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByActiveEmployee(bool isActive)
        {
            if(_clientId != null)
            {
                //hit indexes
                FilterBy(c => c.Employee.ClientId == _clientId && c.Employee.EmployeePayInfo.Any(ep => ep.ClientId == _clientId && ep.EmployeeStatus.IsActive == isActive));
            }
            else
            {
                FilterBy(c => c.Employee.EmployeePayInfo.Any(ep => ep.EmployeeStatus.IsActive == isActive));
            }
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByEmployeePin(string employeePin)
        {
            FilterBy(c => c.EmployeePin == employeePin);
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByClientIds(List<int> clientIds)
        {
            FilterBy(x => clientIds.Contains(x.ClientId));
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByClockClientTimePolicyIds(List<int> timePolicyIds)
        {
            FilterBy(x => timePolicyIds.Contains(x.ClockClientTimePolicyId.Value));
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByGeofenceEnabled()
        {
            FilterBy(c => c.GeofenceEnabled);
            return this;
        }

        IClockEmployeeQuery IClockEmployeeQuery.ByTimePolicyId(int timePolicyId)
        {
            FilterBy(c => c.ClockClientTimePolicyId == timePolicyId);
            return this;
        }
    }
}