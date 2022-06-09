using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientGLAssignmentQuery : IQuery<ClientGLAssignment, IClientGLAssignmentQuery>
    {
        IClientGLAssignmentQuery ByClientId(int clientId);
        IClientGLAssignmentQuery ByClientDivisionId(int? clientDivisionId);
        IClientGLAssignmentQuery ByClientDepartmentId(int? clientDepartmentId);
        IClientGLAssignmentQuery ByClientCostCenterId(int? clientCostCenterId);
        IClientGLAssignmentQuery ByClientGroupId(int? clientGroupId);
        IClientGLAssignmentQuery HasForeignKey(bool hasForeignKey);
        IClientGLAssignmentQuery GroupWithoutClassesTogether(int? clientCostCenterId, int? clientDepartmentId);
        IClientGLAssignmentQuery ByGeneralLedgerTypeIds(IEnumerable<int> generalLedgerTypeIds);
        IClientGLAssignmentQuery ByClientCostCenterIds(List<int> clientCostCenterIds);
        IClientGLAssignmentQuery ByForeignKeyId(int foreignKeyId);
        IClientGLAssignmentQuery ByLedgerId(int ledgerId);
        IClientGLAssignmentQuery ByDivisions(List<int?> divisionIds);
    }
}
