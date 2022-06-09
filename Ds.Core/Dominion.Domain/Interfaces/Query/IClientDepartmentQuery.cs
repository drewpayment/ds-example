using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientDepartmentQuery : IQuery<ClientDepartment, IClientDepartmentQuery>
    {
        /// <summary>
        /// Filters by departments belonging to a particular client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientDepartmentQuery ByClientId(int clientId);
        /// <summary>
        /// Filters by departments with the given 'active' status.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IClientDepartmentQuery ByIsActive(bool isActive);
        IClientDepartmentQuery ByDivisionId(int divisionId);
        IClientDepartmentQuery ByUserTypeAndEmployeeId(int userTypeId, int employeeId);
        IClientDepartmentQuery OrderByName();
        IClientDepartmentQuery ByCode(string code);
        IClientDepartmentQuery ByName(string name);

        /// <summary>
        /// Filters by one or more particular departments.
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        IClientDepartmentQuery ByDepartment(params int[] departmentIds);
        IClientDepartmentQuery ExcludeDeparment(params int[] departmentIds);
    }
}
