using System.Collections.Generic;
using Dominion.LaborManagement.Dto.Approval;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    public interface IApprovalStatusService
    {
        IOpResult<SupervisorStatusResultDto> GetEmployeeExceptionStatus(int clientId, int payrollId, ApprovalOptionType approvalOption, int? userId = null, int? employeeId = null, bool isActiveOnly = true);
    }
}

