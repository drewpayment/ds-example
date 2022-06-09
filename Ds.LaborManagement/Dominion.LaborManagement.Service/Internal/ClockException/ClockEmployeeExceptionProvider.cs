using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Constants;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.LaborManagement.Service.Internal.ClockException
{
    public class ClockEmployeeExceptionProvider : IClockEmployeeExceptionProvider
    {
        private readonly IBusinessApiSession _session;

        internal IClockEmployeeExceptionProvider Self { get; set; }

        public ClockEmployeeExceptionProvider(IBusinessApiSession session)
        {
            Self = this;

            _session = session;
        }

        public IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetClockEmployeeExceptionsByEmployeeId(int employeeId)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>>();

            var user = _session.LoggedInUserInformation;

            result.Data = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeExceptionHistoryQuery()
                .ByClientId(user.ClientId.Value)
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(x => new ClockEmployeeExceptionHistoryDto
                {
                    ClientId = x.ClientId,
                    ClockClientExceptionDetailId = x.ClockClientExceptionDetailId,
                    ClockClientLunchId = x.ClockClientLunchId,
                    ClockEmployeeExceptionHistoryId = x.ClockEmployeeExceptionHistoryId,
                    ClockEmployeePunchId = x.ClockEmployeePunchId,
                    ClockExceptionTypeId = x.ClockExceptionTypeId,
                    EmployeeId = x.EmployeeId,
                    Hours = x.Hours,
                }).ToList();

            return result;
        }

        public IOpResult<ClockEmployeeExceptionHistoryDto> AddClockEmployeeException(ClockEmployeeExceptionHistoryDto exception)
        {
            var result = new OpResult<ClockEmployeeExceptionHistoryDto>();

            if (exception == null) return result;

            var newEx = new ClockEmployeeExceptionHistory
            {
                ClientId = exception.ClientId,
                ClockClientExceptionDetailId = exception.ClockClientExceptionDetailId,
                ClockClientLunchId = exception.ClockClientLunchId,
                ClockEmployeeExceptionHistoryId = CommonConstants.NEW_ENTITY_ID,
                ClockExceptionTypeId = exception.ClockExceptionTypeId,
                ClockEmployeePunchId = exception.ClockEmployeePunchId,
                ClockEmployeeBenefitID = exception.ClockEmployeeBenefitId,
                Hours = exception.Hours,
                EmployeeId = exception.EmployeeId,
                EventDate = exception.EventDate,
            };

            _session.UnitOfWork.RegisterNew(newEx);
            _session.UnitOfWork
                .RegisterPostCommitAction(() => exception.ClockEmployeeExceptionHistoryId = newEx.ClockEmployeeExceptionHistoryId);
            _session.UnitOfWork.Commit().MergeInto(result);

            result.SetDataOnSuccess(new ClockEmployeeExceptionHistoryDto
            {
                ClientId = newEx.ClientId,
                ClockClientExceptionDetailId = newEx.ClockClientExceptionDetailId,
                ClockClientLunchId = newEx.ClockClientLunchId,
                ClockEmployeeExceptionHistoryId = newEx.ClockEmployeeExceptionHistoryId,
                ClockEmployeePunchId = newEx.ClockEmployeePunchId,
                ClockExceptionTypeId = newEx.ClockExceptionTypeId,
                ClockEmployeeBenefitId = newEx.ClockEmployeeBenefitID,
                EmployeeId = newEx.EmployeeId,
                Hours = newEx.Hours,
            });

            return result;
        }

        public IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetEmployeePunchExceptionsByPunchIds(IEnumerable<int> punchIds)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>>();

            var user = _session.LoggedInUserInformation;

            result.Data = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeExceptionHistoryQuery()
                .ByClientId(user.ClientId.Value)
                .ByPunchIds(punchIds)
                .ExecuteQueryAs(x => new ClockEmployeeExceptionHistoryDto
                {
                    ClientId = x.ClientId,
                    ClockClientExceptionDetailId = x.ClockClientExceptionDetailId,
                    ClockClientLunchId = x.ClockClientLunchId,
                    ClockEmployeeExceptionHistoryId = x.ClockEmployeeExceptionHistoryId,
                    ClockEmployeePunchId = x.ClockEmployeePunchId,
                    ClockExceptionTypeId = x.ClockExceptionTypeId,
                    EmployeeId = x.EmployeeId,
                    Hours = x.Hours,
                }).ToList();

            return result;
        }

        public IOpResult<bool> DeleteClockEmployeeException(IEnumerable<ClockExceptionType> clockExceptionIds, int punchId)
        {
            var result = new OpResult<bool>();

            IList<int> punchList = new List<int>();
            punchList.Add(punchId);
            var user = _session.LoggedInUserInformation;

            var exceptions = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeExceptionHistoryQuery()
                .ByClientId(user.ClientId.Value)
                .ByPunchIds(punchList)
                .ByExceptionTypeIds(clockExceptionIds)
                .ExecuteQueryAs(x => new ClockEmployeeExceptionHistoryDto
                {
                    ClientId = x.ClientId,
                    ClockClientExceptionDetailId = x.ClockClientExceptionDetailId,
                    ClockClientLunchId = x.ClockClientLunchId,
                    ClockEmployeeExceptionHistoryId = x.ClockEmployeeExceptionHistoryId,
                    ClockEmployeePunchId = x.ClockEmployeePunchId,
                    ClockExceptionTypeId = x.ClockExceptionTypeId,
                    EmployeeId = x.EmployeeId,
                    Hours = x.Hours,
                }).ToList();

            if (exceptions.Count == 0) return result;

            exceptions.ForEach(exception =>
            {
                var delEx = new ClockEmployeeExceptionHistory
                {
                    ClockEmployeeExceptionHistoryId = exception.ClockEmployeeExceptionHistoryId,
                };

                _session.UnitOfWork.RegisterDeleted(delEx);
            });

            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }
    }
}