using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicationQuestionSectionQuery : Query<ApplicationQuestionSection, IApplicationQuestionSectionQuery>, IApplicationQuestionSectionQuery
    {
        public ApplicationQuestionSectionQuery(IEnumerable<ApplicationQuestionSection> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicationQuestionSectionQuery IApplicationQuestionSectionQuery.ByClientIdWithDefaultClient(int clientId)
        {
            FilterBy(x => x.ClientId == clientId || x.ClientId==0);
            return this;
        }

        IApplicationQuestionSectionQuery IApplicationQuestionSectionQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicationQuestionSectionQuery IApplicationQuestionSectionQuery.BySectionId(int sectionId)
        {
            FilterBy(x => x.SectionId == sectionId);
            return this;
        }

        IApplicationQuestionSectionQuery IApplicationQuestionSectionQuery.ByIsActive(bool flag)
        {
            FilterBy(x => x.IsEnabled == flag);
            return this;
        }

        IApplicationQuestionSectionQuery IApplicationQuestionSectionQuery.OrderByDisplayOrder()
        {
            OrderBy(x => x.DisplayOrder);
            return this;
        }

        public IApplicationQuestionSectionQuery HasQuestionsByClientId(int clientId)
        {
            FilterBy(x => x.ApplicantQuestionControl.Any(y => y.ClientId == clientId && y.IsEnabled==true));
            return this;
        }
    }
}