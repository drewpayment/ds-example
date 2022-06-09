using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientEmploymentClassQuery: IQuery<ClientEmploymentClass, IClientEmploymentClassQuery>
    {
        /// <summary>
        /// Filters the <see cref="ClientEmploymentClass"/> entities by the given client ID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientEmploymentClassQuery ByClientId(int clientId);

        /// <summary>
        /// Filters the <see cref="ClientEmploymentClass"/> entities by whether they are enabled.
        /// </summary>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        IClientEmploymentClassQuery ByIsEnabled(bool isEnabled);

        /// <summary>
        /// Filters the <see cref="ClientEmploymentClass"/> entities by the given ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IClientEmploymentClassQuery ById(int id);

        IClientEmploymentClassQuery SortByDescription();

        /// <summary>
        /// Filters the <see cref="ClientEmploymentClass"/> entities by the given code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IClientEmploymentClassQuery ByCode(string code);
    }
}
