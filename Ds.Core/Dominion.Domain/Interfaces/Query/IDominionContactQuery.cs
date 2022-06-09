using System.Collections.Generic;
using Dominion.Domain.Entities.Misc;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IDominionContactQuery : IQuery< DominionContact, IDominionContactQuery>
    {
        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="configurationId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IDominionContactQuery ByDominionContactId(int id);


    }
}