using System.Collections.Generic;
using System.Linq;
using System;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Service.Internal.ClockException;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.Constants;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;

namespace Dominion.Core.Services.Api.ClockException
{
    public class ClockEmployeeExceptionService : IClockEmployeeExceptionService
    {
        #region Variables and Properties

        private readonly IBusinessApiSession _session;
        private readonly IClockEmployeeExceptionProvider _clockEmployeeExceptionProvider;
        private readonly IGeoProvider _geoProvider;

        internal IClockEmployeeExceptionService Self { get; set; }

        #endregion

        #region Constructors and Initializers

        public ClockEmployeeExceptionService(
            IBusinessApiSession session,
            IClockEmployeeExceptionProvider clockEmployeeExceptionProvider,
            IGeoProvider geoProvider
        )
        {
            Self = this;

            _session = session;
            _clockEmployeeExceptionProvider = clockEmployeeExceptionProvider;
            _geoProvider = geoProvider;
        }

        public IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetClockEmployeeExceptionsByEmployeeId(int employeeId)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.ReadUser).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this clock employee");

            var exceptionsResult = _clockEmployeeExceptionProvider.GetClockEmployeeExceptionsByEmployeeId(employeeId);
            exceptionsResult.MergeInto(result);
            result.SetDataOnSuccess(exceptionsResult.Data);

            return result;
        }

        public IOpResult<ClockEmployeeExceptionHistoryDto> AddClockEmployeeException(RealTimePunchRequest punch, RealTimePunchResultDto savedPunch)
        {
            var result = new OpResult<ClockEmployeeExceptionHistoryDto>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You cannot edit this clock employee");

            var inLocation = _geoProvider.CheckPunchLocation(punch.PunchLocation);

            if ((!inLocation.Data || inLocation.Data == false) && savedPunch?.PunchId != null)
            {
                var lunchPunchInfo = _session.UnitOfWork.TimeClockRepository
                    .GetClockEmployeePunchQuery()
                    .ByClockEmployeePunchId(savedPunch.PunchId.Value)
                    .ExecuteQueryAs(x => new ClockEmployeePunchDto
                    {
                        EmployeeId = x.EmployeeId,
                        ClockEmployeePunchId = x.ClockEmployeePunchId,
                        ClockClientLunchId = x.ClockClientLunchId,
                    }).FirstOrDefault();

                var exception = new ClockEmployeeExceptionHistoryDto
                {
                    ClientId = user.ClientId.Value,
                    ClockClientExceptionDetailId = null,
                    ClockClientLunchId = lunchPunchInfo?.ClockClientLunchId,
                    ClockEmployeeExceptionHistoryId = CommonConstants.NEW_ENTITY_ID,
                    ClockEmployeePunchId = savedPunch.PunchId,
                    EmployeeId = user.EmployeeId,
                    ClockExceptionTypeId = punch.PunchLocation == null ? ClockExceptionType.NoLocation : ClockExceptionType.BadLocation,
                    EventDate = punch.OverridePunchTime != null ? punch.OverridePunchTime : DateTime.Now,
                    Hours = null
                };

                var exceptionResult = _clockEmployeeExceptionProvider.AddClockEmployeeException(exception);
                exceptionResult.MergeInto(result);
                result.SetDataOnSuccess(exceptionResult.Data);
            }

            return result;
        }

        public IOpResult<ClockEmployeeExceptionHistoryDto> AddClockEmployeeExceptionForInputRequest(InputHoursPunchRequest request, InputHoursPunchRequestResult savedPunch)
        {
            var result = new OpResult<ClockEmployeeExceptionHistoryDto>();
            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You cannot edit this clock employee");

            var inLocation = _geoProvider.CheckPunchLocation(request.PunchLocation);

            if (!inLocation.Data && savedPunch.PunchId != null)
            {
                var exception = new ClockEmployeeExceptionHistoryDto
                {
                    ClientId = user.ClientId.Value,
                    ClockClientExceptionDetailId = null,
                    ClockClientLunchId = null,
                    ClockEmployeeExceptionHistoryId = CommonConstants.NEW_ENTITY_ID,
                    ClockEmployeeBenefitId = savedPunch.PunchId,
                    EmployeeId = user.EmployeeId,
                    ClockExceptionTypeId = request.PunchLocation == null ? ClockExceptionType.NoLocation : ClockExceptionType.BadLocation,
                    EventDate = request.Data.EventDate,
                    Hours = null
                };

                var exceptionResult = _clockEmployeeExceptionProvider.AddClockEmployeeException(exception);
                exceptionResult.MergeInto(result);
                result.SetDataOnSuccess(exceptionResult.Data);
            }

            return result;
        }

        public IOpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>> GetEmployeePunchExceptionsByPunchIds(IEnumerable<int> punchIds)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeExceptionHistoryDto>>();

            var user = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You do not have access to this client");

            // result.SetDataOnSuccess(_clockEmployeeExceptionProvider.GetEmployeePunchExceptionsByPunchIds(punchIds).Data);

            var exceptionResult = _clockEmployeeExceptionProvider.GetEmployeePunchExceptionsByPunchIds(punchIds);
            exceptionResult.MergeInto(result);
            result.SetDataOnSuccess(exceptionResult.Data);

            return result;
        }

        public IOpResult<bool> DeleteClockEmployeeException(IEnumerable<ClockExceptionType> clockExceptionId, int punchId)
        {
            var result = new OpResult<bool>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            var user = _session.LoggedInUserInformation;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError) return result.SetToFail("You cannot edit this clock employee");

            var exceptionResult = _clockEmployeeExceptionProvider.DeleteClockEmployeeException(clockExceptionId, punchId);
            exceptionResult.MergeInto(result);

            return result;
        }

        #endregion
    }
}