using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class JobCostingProvider : IJobCostingProvider
    {
        private readonly IBusinessApiSession _session;

        public JobCostingProvider(IBusinessApiSession session)
        {
            _session = session;
        }

        public IOpResult<IEnumerable<ClientJobCostingDto>> GetClientJobCostingList(int clientId)
        {
            var result = new OpResult<IEnumerable<ClientJobCostingDto>>();
            result.TrySetData(() =>
            {
                var data = _session.UnitOfWork.JobCostingRepository.QueryClientJobCosting()
                    .ByClientId(clientId)
                    .ByIsEnabled(true)
                    .OrderBySequence()
                    .ExecuteQueryAs(x => new ClientJobCostingDto()
                    {
                        ClientJobCostingId = x.ClientJobCostingId,
                        ClientId = x.ClientId,
                        HideOnScreen = x.HideOnScreen,
                        IsRequired = x.IsRequired,
                        Sequence = x.Sequence,
                        JobCostingTypeId = x.JobCostingTypeId,
                        Description = x.Description,
                        Level = x.Level
                    })
                    .ToArray();

                foreach (var jc in data)
                {
                    jc.JobCostingTypeDescription = ((ClientJobCostingType)jc.JobCostingTypeId).ToString();
                    var jcResult = GetParentJobCostingIds(jc.ClientJobCostingId).MergeInto(result);
                    if (jcResult.Success)
                    {
                        jc.ParentJobCostingIds = jcResult.Data.ToArray();
                    }
                }

                return data;
            });
            return result;
        }

        public IOpResult<IEnumerable<ClientJobCostingAssignmentSprocDto>> GetEmployeeJobCostingAssignmentList(int clientId, int employeeId, int clientJobCostingId,
            string commaSeparatedParentJobCostingIds, string commaSeparatedParentJobCostingAssignmentIds, string searchText)
        {
            return this.GetEmployeeJobCostingAssignmentList(
                clientId,
                employeeId,
                clientJobCostingId,
                SplitIntoIds(commaSeparatedParentJobCostingIds),
                SplitIntoIds(commaSeparatedParentJobCostingAssignmentIds),
                searchText);
        }

        public IOpResult<IEnumerable<ClientJobCostingListDto>> GetEmployeeJobCostingAssignments(int clientId, int employeeId, AssociatedClientJobCostingDto[] jobCostings)
        {
            var result = new OpResult<IEnumerable<ClientJobCostingListDto>>();
            result.TrySetData(() => jobCostings
                .Select(jc => new
                {
                    ClientJobCostingId = jc.ClientJobCostingId,
                    Result = this.GetEmployeeJobCostingAssignmentList(
                        clientId,
                        employeeId,
                        jc.ClientJobCostingId,
                        jc.ParentJobCostingIds,
                        jc.ParentJobCostingAssignmentIds,
                        null)
                })
                .Do(r => r.Result.MergeInto(result))
                .Where(r => r.Result.Data != null)
                .Select(r => new ClientJobCostingListDto()
                {
                    ClientJobCostingId = r.ClientJobCostingId,
                    AvailableAssignments = r.Result.Data.ToArray()
                }));
            return result;
        }

        private IOpResult<IEnumerable<int>> GetParentJobCostingIds(int jobCostingId)
        {
            var result = new OpResult<IEnumerable<int>>();
            result.TrySetData(() => _session.UnitOfWork.JobCostingRepository
                .GetParentJobCostingIds(jobCostingId)
                .Select(dto => dto.ClientJobCostingId));
            return result;
        }

        private IOpResult<IEnumerable<ClientJobCostingAssignmentSprocDto>> GetEmployeeJobCostingAssignmentList(
            int clientId,
            int employeeId,
            int clientJobCostingId,
            int[] parentJobCostingIds,
            int[] parentJobCostingAssignmentIds,
            string searchText)
        {
            var result = new OpResult<IEnumerable<ClientJobCostingAssignmentSprocDto>>();
            result.TrySetData(() => _session.UnitOfWork.JobCostingRepository
                .GetEmployeeJobCostingAssignmentList(
                    clientId,
                    employeeId,
                    clientJobCostingId,
                    true,
                    parentJobCostingIds,
                    parentJobCostingAssignmentIds,
                    _session.LoggedInUserInformation.UserId,
                    searchText));

            return result;
        }

        private static int[] SplitIntoIds(string commaSeparatedParentJobCostingIds)
        {
            return commaSeparatedParentJobCostingIds.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .Where(i => !string.IsNullOrEmpty(i))
                .Select(int.Parse)
                .ToArray();
        }
    }
}