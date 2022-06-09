using System;
using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantPostingCategoriesQuery : Query<ApplicantPostingCategory, IApplicantPostingCategoriesQuery>, IApplicantPostingCategoriesQuery
    {
        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="data">Tax data the query will be performed on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public ApplicantPostingCategoriesQuery(IEnumerable<ApplicantPostingCategory> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantPostingCategoriesQuery IApplicantPostingCategoriesQuery.ByPostingCategoryId(int postingCategoryId)
        {
            FilterBy(x => x.PostingCategoryId == postingCategoryId);
            return this;
        }

        IApplicantPostingCategoriesQuery IApplicantPostingCategoriesQuery.ByPostingCategoryName(string postingCategoryName)
        {
            FilterBy(x => x.Name.ToLower() == postingCategoryName.ToLower());
            return this;
        }

        IApplicantPostingCategoriesQuery IApplicantPostingCategoriesQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantPostingCategoriesQuery IApplicantPostingCategoriesQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsEnabled == isActive);
            return this;
        }

        IApplicantPostingCategoriesQuery IApplicantPostingCategoriesQuery.OrderByName(SortDirection direction)
        {
            OrderBy(x => x.Name, direction);
            return this;
        }
        IApplicantPostingCategoriesQuery IApplicantPostingCategoriesQuery.OrderByPostingCount(SortDirection direction)
        {
            OrderBy(x => x.ApplicantPostings.Count, direction);
            return this;
        }
    }
}
