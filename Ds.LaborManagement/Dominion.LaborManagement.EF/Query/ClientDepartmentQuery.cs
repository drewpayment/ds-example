using System;
using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClientDepartmentQuery : Query<ClientDepartment, IClientDepartmentQuery>, IClientDepartmentQuery
    {
        #region Constructor
        public ClientDepartmentQuery(IEnumerable<ClientDepartment> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filters by departments belonging to a particular client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientDepartmentQuery IClientDepartmentQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IClientDepartmentQuery IClientDepartmentQuery.ByCode(string code)
        {
            FilterBy(x => x.Code == code);
            return this;
        }

        IClientDepartmentQuery IClientDepartmentQuery.ByName(string name)
        {
            FilterBy(x => x.Name == name);
            return this;
        }

        /// <summary>
        /// Filters by departments with the given 'active' status.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IClientDepartmentQuery IClientDepartmentQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsActive == isActive);
            return this;
        }

        IClientDepartmentQuery IClientDepartmentQuery.ByDivisionId(int divisionId)
        {
            FilterBy(x => x.ClientDivisionId == divisionId);
            return this;
        }

        IClientDepartmentQuery IClientDepartmentQuery.OrderByName()
        {
            OrderBy(x => x.Name);
            return this;
        }

        public IClientDepartmentQuery ByUserTypeAndEmployeeId(int userTypeId, int employeeId)
        {
            FilterBy(x => userTypeId == 1 || (userTypeId > 1 && (x.IsActive || x.Employees.Any(y => y.EmployeeId == employeeId))));
            return this;
        }

        /// <summary>
        /// Filters by one or more particular departments.
        /// </summary>
        /// <param name="departmentIds"></param>
        /// <returns></returns>
        IClientDepartmentQuery IClientDepartmentQuery.ByDepartment(params int[] departmentIds)
        {
            if(departmentIds != null)
            {
                if(departmentIds.Length == 1)
                {
                    var id = departmentIds[0];
                    FilterBy(x => x.ClientDepartmentId == id);
                }
                else
                {
                    FilterBy(x => departmentIds.Contains(x.ClientDepartmentId));
                }
            }
            return this;
        }

        IClientDepartmentQuery IClientDepartmentQuery.ExcludeDeparment(params int[] departmentIds)
        {
            if(departmentIds != null)
            {
                if(departmentIds.Length == 1)
                {
                    var id = departmentIds[0];
                    FilterBy(x => x.ClientDepartmentId != id);
                }
                else
                {
                    FilterBy(x => !departmentIds.Contains(x.ClientDepartmentId));
                }
            }
            return this;
        }

    }
}