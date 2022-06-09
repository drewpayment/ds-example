using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaTransmissionRecordQuery : IQuery<AcaTransmissionRecord, IAcaTransmissionRecordQuery>
    {
        /// <summary>
        /// Filters by the specified year.
        /// </summary>
        /// <param name="year">Year to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionRecordQuery ByYear(int year);

        /// <summary>
        /// Filters by records belonging to the specified transmission. Include year filter to hit DB indexing.
        /// </summary>
        /// <param name="transmissionId">Transmission to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionRecordQuery ByTransmissionId(int transmissionId);

        /// <summary>
        /// Filters by a records belonging to a particlar submission. Make sure to include transmission ID to guarantee correct submission.
        /// </summary>
        /// <param name="submissionId">Submission to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionRecordQuery BySubmissionId(int submissionId);

        /// <summary>
        /// Filters by a specific record. Be sure to include transmission and submission IDs to guarantee uniqueness.
        /// </summary>
        /// <param name="recordId">Record to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionRecordQuery ByRecordId(int recordId);

        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="year">Employee to filter by.</param>
        /// <param name="transmissionId">Employee to filter by.</param>
        /// <param name="submissionId">Employee to filter by.</param>
        /// <param name="recordId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaTransmissionRecordQuery ByYearTranId1094CId1095CId(int year, int transmissionId, int submissionId, int recordId);


        /// <summary>
        /// Filters Year/Transmission/Submission(Client).
        /// </summary>
        /// <param name="year">Employee to filter by.</param>
        /// <param name="transmissionId">Employee to filter by.</param>
        /// <param name="submissionId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaTransmissionRecordQuery ByYearTranmissionSubmission(int year, int transmissionId, int submissionId);
    }
}