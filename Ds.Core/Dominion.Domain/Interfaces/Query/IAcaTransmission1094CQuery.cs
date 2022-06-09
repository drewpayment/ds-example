using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaTransmissionSubmissionQuery : IQuery<AcaTransmissionSubmission, IAcaTransmissionSubmissionQuery>
    {
        /// <summary>
        /// Filters by the specified year.
        /// </summary>
        /// <param name="year">Year to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionSubmissionQuery ByYear(int year);

        /// <summary>
        /// Filters by submission belonging to the specified transmission. Include year filter to hit DB indexing.
        /// </summary>
        /// <param name="transmissionId">Transmission to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionSubmissionQuery ByTransmissionId(int transmissionId);

        /// <summary>
        /// Filters by a specific submission. Make sure to include transmission ID to guarantee uniqueness.
        /// </summary>
        /// <param name="submissionId">Submission to filter by.</param>
        /// <returns></returns>
        IAcaTransmissionSubmissionQuery BySubmissionId(int submissionId);

        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="year">Employee to filter by.</param>
        /// <param name="transmissionId">Employee to filter by.</param>
        /// <param name="submissionId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAcaTransmissionSubmissionQuery ByYearTransmissionId1094Id(int year, int transmissionId, int submissionId);


    }
}

