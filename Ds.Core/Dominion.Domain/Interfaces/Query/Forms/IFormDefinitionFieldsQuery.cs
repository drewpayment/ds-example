using System;
using System.Collections.Generic;

using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Forms;
using Dominion.Domain.Entities.Forms;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Forms
{
    /// <summary>
    /// Query <see cref="FormDefinition"/>(s).
    /// </summary>
    public interface IFormDefinitionFieldsQuery : IQuery<FormDefinitionFields, IFormDefinitionFieldsQuery>
    {
        /// <summary>
        /// Filters by a single form definition.
        /// </summary>
        /// <param name="definitionId">ID of the form definition to get.</param>
        /// <returns></returns>
        IFormDefinitionFieldsQuery ByFormDefinitionId(int definitionId);
        
    }
}
