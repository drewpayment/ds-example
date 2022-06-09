using System.Collections.Generic;

using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaTransmissionErrorQuery : IQuery<AcaTransmissionError, IAcaTransmissionErrorQuery>
    {
        /// <summary>
        /// Filters errors by those belonging to a specific transmission.
        /// </summary>
        /// <param name="transmissionId">ID of transmission to filter errors by.</param>
        /// <returns></returns>
        IAcaTransmissionErrorQuery ByTransmissionId(int transmissionId);

        /// <summary>
        /// Filters errors by those belonging to a specific set of transmissions.
        /// </summary>
        /// <param name="transmissionIds">ID of transmission to filter errors by.</param>
        /// <returns></returns>
        IAcaTransmissionErrorQuery ByTransmissionIds(IEnumerable<int> transmissionIds);

        /// <summary>
        /// Filters errors by those associated with the provided <see cref="AcaCorrection"/> IDs.
        /// </summary>
        /// <param name="correctionIds">Correction IDs to filter errors by.</param>
        /// <returns></returns>
        IAcaTransmissionErrorQuery ByCorrectionIds(IEnumerable<int> correctionIds);

        /// <summary>
        /// Filters errors by those belonging to the specified transmission entry levels.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        IAcaTransmissionErrorQuery ByTransmissionLevel(EFileTransmissionLevelType level);

        /// <summary>
        /// Filters errors by those with the specified correction status.
        /// </summary>
        /// <param name="isCorrected">If true will return only corrected errors. If false will return only those still
        /// in-error.</param>
        /// <returns></returns>
        IAcaTransmissionErrorQuery ByCorrectionStatus(bool isCorrected);
    }
}
