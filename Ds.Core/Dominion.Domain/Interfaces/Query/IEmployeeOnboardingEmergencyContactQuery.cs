using System;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeOnboardingEmergencyContactQuery : IQuery<EmployeeEmergencyContact, IEmployeeOnboardingEmergencyContactQuery>
    {
        IEmployeeOnboardingEmergencyContactQuery ByEmployeeId(int employeeId);
    }
}
