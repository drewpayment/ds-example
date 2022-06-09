using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicationSectionInstructionQuery : IQuery<ApplicationSectionInstruction, IApplicationSectionInstructionQuery>
    {
        IApplicationSectionInstructionQuery BySectionInstructionId(int sectionId);
        IApplicationSectionInstructionQuery ByClientIdAndSectionId(int clientId, int sectionId);
    }
}