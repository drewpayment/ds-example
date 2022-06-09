using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantPostingCategoriesQuery : IQuery<ApplicantPostingCategory, IApplicantPostingCategoriesQuery>
    {
        IApplicantPostingCategoriesQuery ByPostingCategoryId(int postingCategoryId);
        IApplicantPostingCategoriesQuery ByPostingCategoryName(string postingCategoryName);
        IApplicantPostingCategoriesQuery ByClientId(int clientId);
        IApplicantPostingCategoriesQuery ByIsActive(bool isActive);
        
        IApplicantPostingCategoriesQuery OrderByName(SortDirection direction);
        IApplicantPostingCategoriesQuery OrderByPostingCount(SortDirection direction);

        //IApplicantPostingCategoriesQuery OrderByDescription();
        //IApplicantPostingCategoriesQuery ByClientIds(List<int> clientIds);
    }
}
