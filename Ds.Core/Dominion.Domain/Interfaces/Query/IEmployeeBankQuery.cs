using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeBankQuery : IQuery<EmployeeBank, IEmployeeBankQuery>
    {
        IEmployeeBankQuery ByClientId(int clientId);

        IEmployeeBankQuery ByEmployee(int employeeId);

        IEmployeeBankQuery ByRoutingNumber(string routingNumber);

        IEmployeeBankQuery ByAccountNumber(string accountNumber);

        IEmployeeBankQuery ByEmployeeBankId(int employeeBankId);

        IEmployeeBankQuery ByAccountType(EmployeeBankAccountType employeeBankAccountType);

        IEmployeeBankQuery ByEmployeeDeductionId(int employeeDeductionId);
    }
}
