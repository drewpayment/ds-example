using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query on <see cref="CustomBenefitField"/> data.
    /// </summary>
    public interface ICustomBenefitFieldQuery : IQuery<CustomBenefitField, ICustomBenefitFieldQuery>
    {
        /// <summary>
        /// Filter by a single field.
        /// </summary>
        /// <param name="fieldId">ID of the field to return.</param>
        /// <returns></returns>
        ICustomBenefitFieldQuery ByCustomFieldId(int fieldId);

        /// <summary>
        /// Filters custom fields by a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        ICustomBenefitFieldQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by a field's archived status.
        /// </summary>
        /// <param name="isArchived">If true, query will only return archived. If false, 
        /// will return only active fields.</param>
        /// <returns></returns>
        ICustomBenefitFieldQuery ByIsArchived(bool isArchived);

        /// <summary>
        /// Excludes archived custom fields from the query results.
        /// </summary>
        /// <returns></returns>
        ICustomBenefitFieldQuery ExcludeArchived();
    }
}
