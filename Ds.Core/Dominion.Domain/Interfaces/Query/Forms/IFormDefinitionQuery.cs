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
    public interface IFormDefinitionQuery : IQuery<FormDefinition, IFormDefinitionQuery>
    {
        /// <summary>
        /// Filters by a single form definition.
        /// </summary>
        /// <param name="definitionId">ID of the form definition to get.</param>
        /// <returns></returns>
        IFormDefinitionQuery ByFormDefinitionId(int definitionId);
        IFormDefinitionQuery ByFormTypeId(int formTypeId);
        /// <summary>
        /// Filters by definitions valid as of a given date.
        /// </summary>
        /// <param name="date">Date the definition should be effective as of.</param>
        /// <returns></returns>
        IFormDefinitionQuery ByEffectiveAsOf(DateTime date);

        /// <summary>
        /// Filters by definitions belonging to the specified form types.
        /// </summary>
        /// <param name="identifiers">Identifiers will be OR'd together.</param>
        /// <returns></returns>
        IFormDefinitionQuery ByFormTypes(IEnumerable<FormTypeIdentifier> identifiers);


        /// <summary>
        /// Filters by Onboarding forms.
        /// </summary>
        /// <returns></returns>   
        IFormDefinitionQuery ByOnboardingForm();

        /// <summary>
        /// Filters by Form Type Ids.
        /// </summary>
        /// <returns></returns>   
        IFormDefinitionQuery ByFormTypeIds(IEnumerable<int?> formTypeIds);

        /// <summary>
        /// Filters by Form Definition Ids.
        /// </summary>
        /// <returns></returns>   
        IFormDefinitionQuery ByFormDefinitionIds(IEnumerable<int?> formFormDefinitionIds);
        IFormDefinitionQuery ByName(string name);
        IFormDefinitionQuery ByVersion(string name);
    }
}
