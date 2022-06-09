using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ISchoolDistrictQuery : IQuery<SchoolDistrict, ISchoolDistrictQuery>
    {
        ISchoolDistrictQuery ByStateId(int stateId);

    }
}
