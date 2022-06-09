using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicationQuestionSectionQuery : IQuery<ApplicationQuestionSection, IApplicationQuestionSectionQuery>
    {
        IApplicationQuestionSectionQuery BySectionId(int sectionId);
        IApplicationQuestionSectionQuery ByClientIdWithDefaultClient(int clientId);
        IApplicationQuestionSectionQuery HasQuestionsByClientId(int clientId);
        IApplicationQuestionSectionQuery ByIsActive(bool flag);
        IApplicationQuestionSectionQuery OrderByDisplayOrder();
        IApplicationQuestionSectionQuery ByClientId(int clientId);

    }
}