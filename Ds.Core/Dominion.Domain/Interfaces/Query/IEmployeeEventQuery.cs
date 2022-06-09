using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeEventQuery : IQuery<EmployeeEvent, IEmployeeEventQuery>
    {
        IEmployeeEventQuery ByEmployeeId(int employeeId);
        IEmployeeEventQuery ByClientSubTopicId(int clientSubTopicId);
        IEmployeeEventQuery ByEmployeeEventId(int employeeEventId);
        IEmployeeEventQuery ByClientId(int clientId);
        IEmployeeEventQuery ByClientSubTopicIds(params int[] clientSubTopicIds);
    }
}
