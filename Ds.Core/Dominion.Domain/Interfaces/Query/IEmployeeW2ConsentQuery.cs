using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on a <see cref="FilingStatusInfo"/> data source.
    /// </summary>
    public interface IEmployeeW2ConsentQuery : IQuery<EmployeeW2Consent, IEmployeeW2ConsentQuery>
    {
        IEmployeeW2ConsentQuery ByEmployeeId(int employeeId);
    }
}
