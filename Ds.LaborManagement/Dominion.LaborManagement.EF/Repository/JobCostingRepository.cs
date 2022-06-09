using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.EF.Abstract;
using Dominion.Core.EF.Interfaces;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.LaborManagement.EF.Query;
using Dominion.LaborManagement.EF.Query.JobCosting;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Repository
{
    public class JobCostingRepository : RepositoryBase, IJobCostingRepository
    {
        public JobCostingRepository(IDominionContext context, IQueryResultFactory queryResultFactory = null) : base(context, queryResultFactory)
        {
        }

        public IClientJobCostingQuery QueryClientJobCosting()
        {
            return new ClientJobCostingQuery(this._context.ClientJobCosting);
        }

        public IClientJobCostingAssignmentQuery GetJobCostingAssignmentQuery()
        {
            return new ClientJobCostingAssignmentQuery(_context.ClientJobCostingAssignments);
        }

        public IClientJobCostingAssignmentSelectedQuery GetJobClientJobCostingAssignmentSelectedQuery()
        {
            return new ClientJobCostingAssignmentSelectedQuery(_context.ClientJobCostingAssignmentsSelected);
        }

        public IEnumerable<ClientJobCostingIdDto> GetParentJobCostingIds(int jobCostingId)
        {
            var args = new GetClientJobCostingIdWithPostBackSproc.Args()
            {
                ClientJobCostingId = jobCostingId
            };
            var sproc = new GetClientJobCostingIdWithPostBackSproc(_context.ConnectionString, args);
            return sproc.Execute().ToArray();
        }

        public IEnumerable<ClientJobCostingAssignmentSprocDto> GetEmployeeJobCostingAssignmentList(int clientId, int employeeId, int clientJobCostingId, bool isEnabled,
            int[] clientJobCostingIds, int[] clientJobCostingAssignmentIds, int userId, string searchText = null)
        {
            var args = new GetEmployeeJobCostingAssignmentList_Search.Args()
            {
                ClientId = clientId,
                EmployeeId = employeeId,
                ClientJobCostingId = clientJobCostingId,
                IsEnabled = isEnabled,
                UserId = userId,
                ClientJobCostingIds = clientJobCostingIds,
                ClientJobCostingAssignmentIds = clientJobCostingAssignmentIds,
                SearchText = searchText
            };
            var sproc = new GetEmployeeJobCostingAssignmentList_Search(_context.ConnectionString, args);
            return sproc.Execute().ToArray();
        }
    }
}
