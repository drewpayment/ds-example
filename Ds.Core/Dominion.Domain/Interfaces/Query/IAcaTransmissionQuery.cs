using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaTransmissionQuery : IQuery<AcaTransmission, IAcaTransmissionQuery>
    {
        /// <summary>
        /// Filters by specified ACA reporting year.
        /// </summary>
        /// <param name="year">ACA reproting year to filter transmissions by.</param>
        /// <returns></returns>
        IAcaTransmissionQuery ByYear(int year);

        /// <summary>
        /// Filters by a particular transmission (per internal system ID).
        /// </summary>
        /// <param name="transmissionId">Unique internal system generated transmission ID to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionQuery ByTransmissionId(int transmissionId);

        /// <summary>
        /// Filters by a particular set of transmissions (per internal system ID).
        /// </summary>
        /// <param name="transmissionIds">Unique internal system generated transmission ID(s) to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionQuery ByTransmissionIds(IEnumerable<int> transmissionIds);

        /// <summary>
        /// Filters by a particular transmission (per IRS-formatted unique ID).
        /// </summary>
        /// <param name="uniqueTransmissionId">Unique IRS-formatted transmission ID to fitler by.</param>
        /// <returns></returns>
        IAcaTransmissionQuery ByUniqueTransmissionId(string uniqueTransmissionId);

        /// <summary>
        /// Filters by the IRS issued Receipt ID.
        /// </summary>
        /// <param name="receiptId">IRS issued Receipt ID.</param>
        /// <returns></returns>
        IAcaTransmissionQuery ByReceiptId(string receiptId);
    }
}
