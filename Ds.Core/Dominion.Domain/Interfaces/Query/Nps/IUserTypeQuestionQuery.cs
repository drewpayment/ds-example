using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Nps;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Nps
{
    public interface IUserTypeQuestionQuery : IQuery<UserTypeQuestion, IUserTypeQuestionQuery>
    {
        IUserTypeQuestionQuery ByUserType(UserType userType);
    }
}
