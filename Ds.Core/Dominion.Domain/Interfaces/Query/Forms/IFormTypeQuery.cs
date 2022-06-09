using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Forms;
using Dominion.Domain.Entities.Forms;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Forms
{
    /// <summary>
    /// Query on <see cref="FormType"/>(s).
    /// </summary>
    public interface IFormTypeQuery : IQuery<FormType, IFormTypeQuery>
    {
        /// <summary>
        /// Filters by a single form-type.
        /// </summary>
        /// <param name="id">ID of the form type to get.</param>
        /// <returns></returns>
        IFormTypeQuery ByFormTypeId(int id);

        /// <summary>
        /// Filters by the specified system-defined form type.
        /// </summary>
        /// <param name="type">System defined form type to fitler by.</param>
        /// <returns></returns>
        IFormTypeQuery BySystemType(SystemFormType? type);

        /// <summary>
        /// Filters by a specific locality.
        /// </summary>
        /// <param name="locality">Locality type (e.g. Federal, State, Local, etc). If null, only form types without a
        /// locality defined will be included.</param>
        /// <param name="localityId">Locality ID (e.g. State ID) the form type must be associated with. If null, only 
        /// localities without an ID will be included (e.g. Federal -> null).</param>
        /// <returns></returns>
        IFormTypeQuery ByLocality(LocalityType? locality, int? localityId);

        IFormTypeQuery ByLocalityType(LocalityType? locality);
    }
}
