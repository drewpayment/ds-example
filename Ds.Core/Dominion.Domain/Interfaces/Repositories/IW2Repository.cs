using System.Collections.Generic;
using Dominion.Core.Dto.W2;

namespace Dominion.Domain.Interfaces.Repositories
{


    public interface IW2Repository
    {
        /// <summary>
        /// (Re)Calculates the W2 employee history for the given client ID and employee ID.
        /// </summary>
        /// <param name="year">The year that the W2s should be (re)generated for.</param>
        /// <param name="clientId">The Client ID that the W2s should be (re)generated for.</param>
        /// <param name="employeeId">The Optional Employee ID that the W2s should be (re)generated for.</param>
        /// <returns></returns>
        int CalculateGenW2EmployeeHistory(int year, int clientId, int? employeeId);

        IEnumerable<W2ProcessingDto> GetW2ProcessingListByYearAndFilterSproc(W2ProcessingDto.Args args);

        Employee1099CountDto GetEmployeeW2Count1099Sproc(Employee1099CountDto.Args args);
    }
}