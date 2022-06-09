using Dominion.Domain.Entities.Aca;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on a <see cref="FilingStatusInfo"/> data source.
    /// </summary>
    public interface IEmployee1095CConsentQuery : IQuery<Aca1095CEmployeeConsent, IEmployee1095CConsentQuery>
    {
        IEmployee1095CConsentQuery ByEmployeeId(int employeeId);
    }
}
