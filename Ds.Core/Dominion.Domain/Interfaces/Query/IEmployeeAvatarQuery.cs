using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeAvatarQuery : IQuery<EmployeeAvatar, IEmployeeAvatarQuery>
    {
        /// <summary>
        /// Filters EmployeeAvatar configs by EmployeeAvatarId
        /// </summary>
        /// <param name="employeeAvatarId"></param>
        /// <returns></returns>
        IEmployeeAvatarQuery ByEmployeeAvatarId(int employeeAvatarId);
        
        /// <summary>
        /// Filters EmployeeAvatar configs by ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IEmployeeAvatarQuery ByClientId(int clientId);

        /// <summary>
        /// Filters EmployeeAvatar configs by EmployeeId
        /// </summary>
        /// <param name="employddId"></param>
        /// <returns></returns>
        IEmployeeAvatarQuery ByEmployeeId(int employeeId);
    }
}
