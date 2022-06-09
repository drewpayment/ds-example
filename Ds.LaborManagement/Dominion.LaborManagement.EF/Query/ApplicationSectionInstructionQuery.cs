using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicationSectionInstructionQuery : Query<ApplicationSectionInstruction, IApplicationSectionInstructionQuery>, IApplicationSectionInstructionQuery
    {
        public ApplicationSectionInstructionQuery(IEnumerable<ApplicationSectionInstruction> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicationSectionInstructionQuery IApplicationSectionInstructionQuery.BySectionInstructionId(int sectionInstructionId)
        {
            FilterBy(x => x.SectionInstructionId == sectionInstructionId);
            return this;
        }

        IApplicationSectionInstructionQuery IApplicationSectionInstructionQuery.ByClientIdAndSectionId(int clientId, int sectionId)
        {
            FilterBy(x => x.ClientId == clientId && x.SectionId == sectionId);
            return this;
        }
    }
}