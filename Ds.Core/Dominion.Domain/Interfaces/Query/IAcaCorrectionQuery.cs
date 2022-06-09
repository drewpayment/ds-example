using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaCorrectionQuery : IQuery<AcaCorrection, IAcaCorrectionQuery>
    {
        /// <summary>
        /// Filters by ACA reporting year.
        /// </summary>
        /// <param name="year">ACA reporting year the corrections should apply to.</param>
        /// <returns></returns>
        IAcaCorrectionQuery ByYear(int year);

        /// <summary>
        /// Filters by specified clients.
        /// </summary>
        /// <param name="clientIds">One or more client IDs the corrections should apply to.</param>
        /// <returns></returns>
        IAcaCorrectionQuery ByClients(params int[] clientIds);

        /// <summary>
        /// Filters by specified employees.
        /// </summary>
        /// <param name="employeeIds">One or more employee IDs corrections should apply to.</param>
        /// <returns></returns>
        IAcaCorrectionQuery ByEmployees(params int[] employeeIds);

        /// <summary>
        /// Filters by correction status.
        /// </summary>
        /// <param name="isCorrected">If true, only corrected corrections will be returned. If false, only corrections
        /// still in error will be returned.</param>
        /// <returns></returns>
        IAcaCorrectionQuery HasCorrection(bool isCorrected);

        /// <summary>
        /// Filters by corrections which must be corrected by the client (ie: <see cref="AcaCorrection.IsClientCorrection"/> = true).
        /// </summary>
        /// <returns></returns>
        IAcaCorrectionQuery IncludeClientCorrectionsOnly();
    }
}
