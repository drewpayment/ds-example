using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientShiftQuery : IQuery<ClientShift, IClientShiftQuery>
    {
        /// <summary>
        /// Filters to shifts for a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientShiftQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by one or more particular shifts.
        /// </summary>
        /// <param name="shiftIds"></param>
        /// <returns></returns>
        IClientShiftQuery ByShifts(params int[] shiftIds);

        IClientShiftQuery OrderByClientShiftId();
        IClientShiftQuery OrderByDescription();

        /// <summary>
        /// Filter by client shift id.
        /// </summary>
        /// <param name="clientShiftId"></param>
        /// <returns></returns>
        IClientShiftQuery ByClientShift(int clientShiftId);

        /// <summary>
        /// Filter by client shift description.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        IClientShiftQuery ByClientShiftDescription(string description);
    }
}
