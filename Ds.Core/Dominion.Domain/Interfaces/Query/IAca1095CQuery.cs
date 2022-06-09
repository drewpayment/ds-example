using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAca1095CQuery : IQuery<Aca1095C, IAca1095CQuery>
    {
        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="employeeId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAca1095CQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters 1095C data by year.
        /// </summary>
        /// <param name="year">Year to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAca1095CQuery ByYear(int year);

        /// <summary>
        /// Filters by client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IAca1095CQuery ByClient(int clientId);

        /// <summary>
        /// Filters by clients.
        /// </summary>
        /// <param name="clientIds">ID(s) of client(s) to filter by.</param>
        /// <returns></returns>
        IAca1095CQuery ByClientIds(IEnumerable<int> clientIds);

        /// <summary>
        /// Get existing line items only matching the employee ids passed in.
        /// </summary>
        /// <param name="empIds">List of employee ids</param>
        /// <returns></returns>
        IAca1095CQuery ByEmployeeIds(IEnumerable<int> empIds);

        /// <summary>
        /// Filters by 1095-Cs belonging to employees with the specified employee numbers.
        /// </summary>
        /// <param name="employeeNumbers">Employee numbers to get 1095-Cs by.</param>
        /// <returns></returns>
        IAca1095CQuery ByEmployeeNumbers(IEnumerable<string> employeeNumbers);

        /// <summary>
        /// Filters by 1095-C exemption status.
        /// </summary>
        /// <param name="isExempt">If true, only exempt 1095-Cs will be included. If false, only non-exempt will
        /// be included.</param>
        /// <returns></returns>
        IAca1095CQuery ByIs1095CExempt(bool isExempt);

        /// <summary>
        /// Filters by 1095-C not previously transmitted and ready to transmit.
        /// </summary>
        /// <returns></returns>
        IAca1095CQuery ByReadyToTransmit();

        /// <summary>
        /// Filters by 1095-C not previously transmitted.
        /// </summary>
        /// <returns></returns>
        IAca1095CQuery ByNoPriorTransmission();

        /// <summary>
        /// Filters by 1095-C not previously transmitted.
        /// </summary>
        /// <returns></returns>
        IAca1095CQuery ByPriorTransmission();

        /// <summary>
        /// Filters by 1095-Cs whose last transmission was the one specified.
        /// </summary>
        /// <param name="lastTransmissionId">Transmission the 1095-C should have been last transmitted in.</param>
        /// <returns></returns>
        IAca1095CQuery ByLastTransmissionId(int lastTransmissionId);

        /// <summary>
        /// Filters by 1095-Cs that were part of the specified transmission (regardless if it was the last transmission 
        /// or not).
        /// </summary>
        /// <param name="transmissionId">Transmission the specified employee-1095 should be a part of.</param>
        /// <returns></returns>
        IAca1095CQuery ByTransmissionId(int transmissionId);

        /// <summary>
        /// Filters by 1095-C status is accepted.
        /// </summary>
        /// <returns></returns>
        IAca1095CQuery ByAccepted();

        /// <summary>
        /// Filters by 1095-C status is accepted.
        /// </summary>
        /// <returns></returns>
        IAca1095CQuery HasCorrection();

        /// <summary>
        /// Filters by validation waiver status.
        /// </summary>
        /// <param name="isWaived">True, will filter by waived 1095-Cs.</param>
        /// <returns></returns>
        IAca1095CQuery ByIsValidationWaived(bool isWaived);
    }
}
