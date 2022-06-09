
using Dominion.Core.Services.Interfaces;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.Utility.Security;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.TimeCard;

namespace Dominion.LaborManagement.Service.Api
{
    public class TimeCardAuthorizationService : ITimeCardAuthorizationService
    {
        private readonly ITimeCardAuthorizationProvider _timeCardAuthorizationProvider;
        private readonly IClientService _clientService;
        private readonly IDsDataServicesClockCalculateActivityService _clockCalculateActivityService;
        private readonly IDsDataServicesClockAutomatedPointsLogicService _ClockAutomatedPointsLogicService;
        private readonly IBusinessApiSession _session;
        private readonly IUserManager _userProvider;

        public TimeCardAuthorizationService(
            ITimeCardAuthorizationProvider timeCardAuthorizationProvider,
            IClientService clientService,
            IDsDataServicesClockCalculateActivityService clockCalculateActivityService,
            IDsDataServicesClockAutomatedPointsLogicService ClockAutomatedPointsLogicService,
            IBusinessApiSession session,
            IUserManager userProvider)
        {
            _timeCardAuthorizationProvider = timeCardAuthorizationProvider;
            _clientService = clientService;
            _clockCalculateActivityService = clockCalculateActivityService;
            _ClockAutomatedPointsLogicService = ClockAutomatedPointsLogicService;
            _session = session;
            _userProvider = userProvider;
        }

        public IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int noOfPayPeriodOptions, int payPeriod, string payPeriodText, int clientId)
        {
            return Authorize(() =>
            {
                var result = new OpResult<IEnumerable<GetClockFilterIdsDto>>();
                _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);
                if (result.HasError) return result;
                var data = _timeCardAuthorizationProvider.FillFilterIDDropdown(filterCategory, noOfPayPeriodOptions, payPeriod, payPeriodText, clientId).MergeInto(result);
                if (result.HasError) return result;
                result.Data = data.Data;
                return result;
            }, _session, _userProvider);
        }

        IOpResult<GetTimeCardAuthorizationDataResult> ITimeCardAuthorizationService.GetData(TimeCardAuthorizationDataArgs args)
        {
            //TODO: perform Action Type Check (does the user have permission to this call?)
            //TODO: perform any necessary Resource Check (does the user have access to the resource?)

            //IF either authorization check above fails, return failed OpResult
            return Authorize(() => { return _timeCardAuthorizationProvider.GetData(args); }, _session, _userProvider);
        }

        IOpResult<IEnumerable<GetClockEmployeeApproveDateDto>> ITimeCardAuthorizationService.SaveTimeCardAuthorizationData(TimeCardAuthorizationSaveArgs args)
        {
            //TODO: perform Action Type Check (does the user have permission to this call?)
            //_session.CanPerformAction();
            //TODO: perform any necessary Resource Check (does the user have access to the resource?)
            //_session.ResourceAccessChecks.CheckAccessById()

            //IF either authorization check above fails, return failed OpResult
            return Authorize(() =>
            {
                var result = new OpResult<IEnumerable<GetClockEmployeeApproveDateDto>>();

                args.UserId = _session.LoggedInUserInformation.UserId;
                args.ModifiedBy = -_session.LoggedInUserInformation.UserId;

                if (!args.DataEntries.Any()) return result;

                var entries = args.DataEntries
                    .GroupBy(x => new { 
                        x.ClockEmployeeApproveDate.EmployeeID, 
                        x.ClockEmployeeApproveDate.ClientCostCenterID, 
                        x.ClockEmployeeApproveDate.EventDate 
                    })
                    .Select(x => x.First())?
                    .ToList();

                if (!entries.Any()) return result;

                var employeeIds = entries.Select(e => e.ClockEmployeeApproveDate.EmployeeID);
                var homeCostCenters = _session.UnitOfWork.EmployeeRepository
                    .QueryEmployees()
                    .ByEmployeeIds(employeeIds)
                    .ExecuteQueryAs(x => new EmployeeBasicDto
                    {
                        EmployeeId = x.EmployeeId,
                        ClientCostCenterId = x.ClientCostCenterId
                    })
                    ?.ToList();

                var clientOption = _session.UnitOfWork.ClientAccountFeatureRepository
                    .ClientOptionQuery()
                    .ByClientId(args.ClientId)
                    .ByOption(Core.Dto.Misc.AccountOption.TimeClock_ApprovalOptions)
                    .ExecuteQueryAs(x => new
                    {
                        x.Value,
                        items = x.AccountOptionInfo.AccountOptionItems.Select(a => new AccountOptionItemDto
                        {
                            AccountOption = a.AccountOption,
                            AccountOptionItemId = a.AccountOptionItemId,
                            Value = a.Value
                        })
                    })
                    .FirstOrDefault();

                var approvalOption = clientOption?.items
                    ?.FirstOrDefault(aoi => aoi.AccountOption == Core.Dto.Misc.AccountOption.TimeClock_ApprovalOptions
                        && aoi.AccountOptionItemId.ToString() == clientOption.Value);

                var isApproveByCostCenter = approvalOption != null && (approvalOption.Value.GetValueOrDefault() == (int)ApprovalOptionType.Cost_Center);

                var approvalDates = new List<ClockEmployeeApproveDateDto>();

                foreach (var entry in entries)
                {
                    var cead = entry.ClockEmployeeApproveDate;
                    var dto = new ClockEmployeeApproveDateDto
                    {
                        ClientId = args.ClientId, //_session.LoggedInUserInformation.ClientId.GetValueOrDefault(),
                        ClientCostCenterId = cead.ClientCostCenterID < 1 
                            ? homeCostCenters
                                .FirstOrDefault(h => h.EmployeeId == entry.ClockEmployeeApproveDate.EmployeeID).ClientCostCenterId 
                            : cead.ClientCostCenterID,
                        EmployeeId = cead.EmployeeID,
                        EventDate = cead.EventDate.GetValueOrDefault(),
                        IsApproved = cead.IsApproved,
                        IsPayToSchedule = cead.PayToSchedule,
                        ClockClientNoteId = cead.ClockClientNoteID,
                        ApprovedBy = cead.IsApproved ? args.UserId : default(int),
                        ApprovedDate = cead.IsApproved ? DateTime.Now : default(DateTime)
                    };

                    _timeCardAuthorizationProvider.SaveTimeCardApprovalRow(dto, isApproveByCostCenter).MergeInto(result);
                    if (result.HasError) return result;
                    approvalDates.Add(dto);

                    entry.ClockEmployeeApproveDate.ModifiedBy = _session.LoggedInUserInformation.UserId;
                }

                if (approvalDates.Any()) _session.UnitOfWork.Commit().MergeInto(result);
                if (result.HasError) return result;

                // Map entries to dictionary of <key: employeeId, value: entries_for_that_employee> 
                var entriesByEmployee = entries.GroupBy(e => e.ClockEmployeeApproveDate.EmployeeID).ToDictionary(x => x.Key, x => x.ToList());

                foreach(var kvPair in entriesByEmployee)
                {
                    // Use the earliest eventDate for this employee as startDate for point calculations.
                    var startDate = kvPair.Value.Min(e => e.ClockEmployeeApproveDate.EventDate).GetValueOrDefault(args.StartDate);
                    // Use the latest eventDate for this employee as punchDate for point calculations.
                    var punchDate = kvPair.Value.Max(e => e.ClockEmployeeApproveDate.EventDate).GetValueOrDefault(args.EndDate);

                    // Only do one points calc per employee.
                    // The only thing we loose doing it this way (instead of per e in entries) is running the points calc with each of the punchDates...
                    // Its fine though, this already happens the same way elsewhere in the app.
                    if (kvPair.Value.Any(e => e.DoRecalcPoints))
                    {
                        _ClockAutomatedPointsLogicService.StartPointCalcs(args.ClientId, kvPair.Key, args.UserId, startDate, args.EndDate, true, punchDate, false, _clientService, null);
                    }
                }
                
                _timeCardAuthorizationProvider.RecalculateWeeklyActivityByApproveDates(args).MergeInto(result);

                return result;
            }, _session, _userProvider);
        }

        public IOpResult<IEnumerable<GetClockFilterIdsDto>> FillFilterIDDropdown(int filterCategory, int clientId)
        {
            return Authorize(() =>
            {
                var result = new OpResult<IEnumerable<GetClockFilterIdsDto>>();
                _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);
                if (result.HasError) return result;
                var data = _timeCardAuthorizationProvider.FillFilterIDDropdown(filterCategory, clientId).MergeInto(result);
                if (result.HasError) return result;
                result.Data = data.Data;
                return result;
            }, _session, _userProvider);
        }

        public IOpResult<GetClockEmployeeApproveHoursSettingsDto> SaveDisplaySettings(GetClockEmployeeApproveHoursSettingsDto dto)
        {
            return Authorize(() =>
            {
                var result = new OpResult<GetClockEmployeeApproveHoursSettingsDto>();

                _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, dto.ClientId).MergeInto(result);
                if (result.HasError) return result;

                result.TrySetData(() => _timeCardAuthorizationProvider.SaveDisplaySettings(dto).MergeInto(result).Data);
                if (result.HasError) return result;

                return result;
            }, _session, _userProvider);
        }

        public static IOpResult<T> Authorize<T>(Func<IOpResult<T>> onSuccess, IBusinessApiSession _session, IUserManager _userProvider)
        {

            var result = new OpResult<T>();

            if (!_session.LoggedInUserInformation.ClientId.HasValue)
            {
                result.SetToFail("A client must be selected.");
                return result;
            }

            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(_session.LoggedInUserInformation.ClientId.Value).MergeInto(result);
            var user = _userProvider.GetUser(_session.LoggedInUserInformation.UserId);
            if (user == null)
            {
                result.SetToFail("Unknown user.");
                return result;
            }

            _session.CanPerformAction(LaborManagementActionType.ReadTimeCardAuthorization).MergeInto(result);

            if (result.HasError) return result;

            return onSuccess();
        }
    }
}
