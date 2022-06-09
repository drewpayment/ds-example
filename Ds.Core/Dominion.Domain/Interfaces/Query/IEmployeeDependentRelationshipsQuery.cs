using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeDependentRelationshipsQuery : IQuery<EmployeeDependentRelationships, IEmployeeDependentRelationshipsQuery>
    {
    }
}
