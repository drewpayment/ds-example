using Dominion.Core.Dto.Employee;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.OpResult;
using Dominion.Utility.OpTasks;
using Dominion.Utility.Query.LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.LeaveManagement;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class SchedulingProvider : ISchedulingProvider
    {
        #region Variables and Properties
        
        private readonly IBusinessApiSession _session;

        /// <summary>
        /// Give access to explicit interface for this provider.
        /// </summary>
        internal ISchedulingProvider Self { get; set; }

        private IScheduleGroupProvider _scheduleGroupProvider;
        
        #endregion

        #region Constructors and Initializers

        public SchedulingProvider(IBusinessApiSession session, IScheduleGroupProvider scheduleGroupProvider)
        {
            Self = this;

            _session = session;
            _scheduleGroupProvider = scheduleGroupProvider;
        }

        #endregion

        #region ISchedulingProvider

        /// <summary>
        /// Get the scheduled information based on the parameters passed in.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scheduleGroupId">The schedule group id.</param>
        /// <param name="scheduleGroupSourceId">The source id of the source group (cost center id).</param>
        /// <returns></returns>
        IOpResult<IEnumerable<EmployeeSchedulesDto>> ISchedulingProvider.GetEmployeeScheduleShifts(
            int clientId,
            DateTime startDate,
            DateTime endDate,
            int scheduleGroupId,
            int scheduleGroupSourceId)
        {
            var opResult = new OpResult<IEnumerable<EmployeeSchedulesDto>>();

            //get employees registered for cost center
            var empsForCC = _session
                .UnitOfWork
                .LaborManagementRepository
                .ClockEmployeeCostCenterQuery()
                .ByCostCenterId(scheduleGroupSourceId)
                .ExecuteQueryAs(new SchedulingMaps.ToEmployeesToScheduleForCostCenter());

            //get employee shifts from the preview schedule
            var preview = _session
                .UnitOfWork
                .LaborManagementRepository
                .EmployeeSchedulePreviewQuery()
                .ByScheduleGroupId(scheduleGroupId)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(new SchedulingMaps.ToPreviewSchedule()); 

            var previewShifts = _session.OpTasksFactory
                .ExecuteFunc<IEnumerable<ScheduleShiftDto>, IEnumerable<EmployeeSchedulesDto>>(
                    new FlattenScheduleData(), 
                    preview);

            //get employee shifts from the published schedule
            var published = _session
                .UnitOfWork
                .LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByEventDateRange(startDate, endDate)
                .ByScheduleGroupId(scheduleGroupId)
                .ExecuteQueryAs(new SchedulingMaps.ToClockEmployeeSchedule());

            var publishedShifts = _session.OpTasksFactory
                .ExecuteFunc<IEnumerable<ClockEmployeeScheduleShiftDto>, int?, IScheduleGroupProvider, IEnumerable<EmployeeSchedulesDto>>(
                        new SplitPublishedRawData(),
                        published,
                        scheduleGroupId,
                        _scheduleGroupProvider);

            var data = _session.OpTasksFactory.ExecuteFunc
                <IEnumerable<EmployeeSchedulesDto>, IEnumerable<EmployeeSchedulesDto>, IEnumerable<EmployeeSchedulesDto>,
                    IEnumerable<EmployeeSchedulesDto>>(
                        new CombineAllShiftData(),
                        previewShifts,
                        publishedShifts,
                        empsForCC).ToList();

            ////get shifts from other schedule groups for the employees that were found
            ////need to re-query here as any of the queries above could have included a new employee record
            var employeeIds = _session.OpTasksFactory
                .ExecuteFunc<IEnumerable<EmployeeSchedulesDto>, IEnumerable<int>>(
                    new ExtractEmployeeIds(),
                    data).ToList();

            var recurringShifts = _session
                .UnitOfWork
                .LaborManagementRepository
                .EmployeeDefaultShiftQuery()
                .ByEmployeesIds(employeeIds)
                .ByScheduleGroupId(scheduleGroupId)
                .ExecuteQueryAs(new SchedulingMaps.ToEmployeeDefaultShift());

            var previewOtherGroup = _session
                .UnitOfWork
                .LaborManagementRepository
                .EmployeeSchedulePreviewQuery()
                .ByEventDateRange(startDate, endDate)
                .ByScheduleGroupsOtherThanId(scheduleGroupId)
                .ByEmployeeIds(employeeIds)
                .ExecuteQueryAs(new SchedulingMaps.ToPreviewSchedule()); 

            var previewOtherGroupShifts = _session.OpTasksFactory
                .ExecuteFunc<IEnumerable<ScheduleShiftDto>, IEnumerable<EmployeeSchedulesDto>>(
                    new FlattenScheduleData(), 
                    previewOtherGroup);

            var publishedOtherGroup = _session
                .UnitOfWork
                .LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByEmployeeIds(employeeIds)
                .ByEventDateRange(startDate, endDate)
                .ByScheduleGroupsOtherThanId(scheduleGroupId)
                .ExecuteQueryAs(new SchedulingMaps.ToClockEmployeeSchedule());

            var publishedOtherGroupShifts = _session.OpTasksFactory
                .ExecuteFunc<IEnumerable<ClockEmployeeScheduleShiftDto>, int?, IScheduleGroupProvider, IEnumerable<EmployeeSchedulesDto>>(
                    new SplitPublishedRawData(),
                    publishedOtherGroup,
                    null,
                    _scheduleGroupProvider);

            _session.OpTasksFactory.ExecuteAction
                <int, IEnumerable<EmployeeSchedulesDto>, IEnumerable<EmployeeSchedulesDto>,
                    IEnumerable<EmployeeSchedulesDto>>(
                        new CombineAllOtherGroupShifts(),
                        scheduleGroupId,
                        data,
                        previewOtherGroupShifts,
                        publishedOtherGroupShifts);

            _session.OpTasksFactory
                .ExecuteAction<IEnumerable<EmployeeSchedulesDto>>(
                    new FixStartEndTimeForPublished(),
                    data);

            _session.OpTasksFactory.ExecuteAction<List<EmployeeSchedulesDto>, IEnumerable<ScheduleShiftDto>, 
                IEnumerable<EmployeeSchedulesDto>>(
                    new ProcessGetShiftData(), 
                    data, 
                    recurringShifts, 
                    empsForCC);

            //get employee benefits (time-off requests, vacation days and holidays)
            var benefitResults = Self.GetScheduledBenefits(clientId, employeeIds, startDate, endDate);
            opResult.CombineSuccessAndMessages(benefitResults);
            if (benefitResults.HasNoErrorAndHasData)
            {
                foreach (var ee in data)
                    ee.EmployeeScheduledBenefits = benefitResults.Data.Where(x => x.EmployeeId == ee.EmployeeId).ToList();
            }

            var canViewHourly = _session.CanPerformAction(ClientRateActionType.ViewHourlyRates).Success;

            var budgetingFeatureEnabled = _session.CanPerformAction(BudgetingActionType.BudgetingAdministrator).Success;

            if (canViewHourly && budgetingFeatureEnabled)
            {
                var rateInfo = _session
                .UnitOfWork
                .EmployeeRepository
                .EmployeeClientRateQuery()
                .ByEmployeeId(employeeIds.ToArray())
                .ByIsDefault()
                .ExecuteQueryAs(x => new
                {
                    x.EmployeeId,
                    x.Rate,
                    RateDescription = x.ClientRate.Description,
                    IsHourly = x.Employee.EmployeePayInfo.Any(y => y.Type == PayType.Hourly)
                });

                foreach (var ee in data)
                {
                    var rate = rateInfo.FirstOrDefault(x => x.EmployeeId == ee.EmployeeId);

                    if (rate != null)
                    {
                        ee.Rate = (decimal)rate.Rate;
                        ee.RateDescription = rate.RateDescription;
                        ee.IsHourly = rate.IsHourly;
                    }

                }
            }

            opResult.Data = data;
            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeeBenefitDto>> GetClockEmployeeBenefit(int clientId, DateTime startdate, DateTime endDate)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeeBenefitDto>>();

            var query = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeBenefitQuery().ByClientId(clientId).ByDateRange(startdate, endDate);

            opResult.TrySetData(() => query.ExecuteQueryAs(new ClockEmployeeBenefitMaps.ToClockEmployeeBenefitDto()));

            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetClockEmployeeSchedules(int clientId, DateTime startDate, DateTime endDate)
        {
            var opResult  = new OpResult<IEnumerable<ClockEmployeeScheduleDto>>();

            var query = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeScheduleQuery().ByClientId(clientId).ByEventDateRange(startDate, endDate);

            opResult.TrySetData(()=>  query.ExecuteQueryAs(new SchedulingMaps.ToClockEmployeeScheduleDto()));

            return opResult;
        }

        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> ISchedulingProvider.GetClockEmployeeSchedules(DateTime startDate, DateTime endDate, int employeeId)
        {
            return new OpResult<IEnumerable<ClockEmployeeScheduleDto>>().TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByEmployeeId(employeeId)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(new SchedulingMaps.ToClockEmployeeScheduleDto()));
        }

        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> ISchedulingProvider.GetClockEmployeeSchedulesByPunchTime(DateTime punchTime, int employeeId, short startGrace, short endGrace)
        {
            return new OpResult<IEnumerable<ClockEmployeeScheduleDto>>().TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByEmployeeId(employeeId)
                .ByPunchTime(punchTime, startGrace, endGrace)
                .ExecuteQueryAs(new SchedulingMaps.ToClockEmployeeScheduleDto()));
        }

        public IOpResult<IEnumerable<ClockClientScheduleDto>> GetClockClientSchedules(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientScheduleDto>>();

            var query = _session.UnitOfWork.LaborManagementRepository.ClockClientScheduleQuery().ByClient(clientId);

            opResult.TrySetData(() => query.ExecuteQueryAs(new ClockClientScheduleMaps.ToClockClientScheduleDto()));

            return opResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IOpResult ISchedulingProvider.SaveOrUpdateEmployeeScheduleShifts(EmployeeSchedulesPersistDto data)
        {
            var opResult = new OpResult();
            var shifts = data.EmployeeSchedulesDtos.SelectMany(d => d.EmployeeScheduleShifts).OrderBy(d => d.ShiftId).ThenBy(d => d.StartTime);
            var previewShifts = shifts.Where (s => s.IsPreview);
            var publishShifts = shifts.Where (s => !s.IsPreview);
            var recurringShifts = data.EmployeeSchedulesDtos.SelectMany(x => x.EmployeeRecurringShifts);

            if(previewShifts.Any())
            {
                _session.OpTasksFactory.ExecuteAction<IEnumerable<ScheduleShiftDto>>(
                    new SavePreview(_session),
                    previewShifts);
            }

            if(publishShifts.Any())
            {
                _session.OpTasksFactory.ExecuteAction<IEnumerable<ScheduleShiftDto>, int>(
                    new SavePublished(_session, new NormalizeShiftDateTimes()),
                    publishShifts,
                    _session.LoggedInUserInformation.ClientId.GetValueOrDefault());
            }

            if(recurringShifts.Any())
            {
                _session.OpTasksFactory.ExecuteAction<IEnumerable<ScheduleShiftDto>>(
                    new SaveRecurringShifts(_session),
                    recurringShifts);
            }

            opResult.CombineSuccessAndMessages(_session.UnitOfWork.Commit());
            return opResult;
        }

        /// <summary>
        /// Returns a combination of the unapproved time-off and approved benefits for the specified employees.
        /// </summary>
        /// <param name="clientId">Client associated with the employees.</param>
        /// <param name="employeeIds">Employees to get benefits for.</param>
        /// <param name="fromDate">Start of date range to get benefits for. If null, no from-date filter will be applied.</param>
        /// <param name="toDate">End of date range to get benefits for. If null, no to-date filter will be applied.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ScheduledBenefitDto>> ISchedulingProvider.GetScheduledBenefits(int clientId, IEnumerable<int> employeeIds, DateTime? fromDate, DateTime? toDate)
        {
            var result = new OpResult<IEnumerable<ScheduledBenefitDto>>();

            var unapprovedBenefitQuery = _session.UnitOfWork.LeaveManagementRepository
                .QueryTimeOffRequestDetails()
                .ByClientId(clientId)
                .ByEmployees(employeeIds)
                .ByStatus(TimeOffStatusType.Pending);

            var approvedBenefitQuery = _session.UnitOfWork.LeaveManagementRepository
                .QueryEmployeeBenefits()
                .ByClientId(clientId)
                .ByEmployees(employeeIds);

            if (fromDate.HasValue)
            {
                unapprovedBenefitQuery.ByDateRangeFrom(fromDate.Value);
                approvedBenefitQuery.ByDateRangeFrom(fromDate.Value);
            }

            if (toDate.HasValue)
            {
                unapprovedBenefitQuery.ByDateRangeTo(toDate.Value);
                approvedBenefitQuery.ByDateRangeTo(toDate.Value);
            }

            var unapprovedBenefitResults = unapprovedBenefitQuery.Result.MapTo(x => new ScheduledBenefitDto
            {
                EmployeeId = x.EmployeeId,
                EventDate = x.RequestDate,
                StartTimeDate = x.FromTime,
                EndTimeDate = x.ToTime,
                TotalHours = x.Hours,
                IsApproved = false,
                Description = x.TimeOffRequest.ClientAccrual.IsCombineByEarnings ? x.TimeOffRequest.ClientAccrual.ClientEarning.Description : x.TimeOffRequest.ClientAccrual.Description,
                IsHoliday = false
            });
            var approvedBenefitResults = approvedBenefitQuery.Result.MapTo(x => new ScheduledBenefitDto
            {
                EmployeeId = x.EmployeeId,
                EventDate = x.EventDate.Value,
                StartTimeDate = x.TimeOffRequestDetail != null ? x.TimeOffRequestDetail.FromTime : null,
                EndTimeDate = x.TimeOffRequestDetail != null ? x.TimeOffRequestDetail.ToTime : null,
                TotalHours = x.Hours ?? 0,
                IsApproved = x.IsApproved ?? true,
                Description = x.ClockClientHolidayDetail != null ? x.ClockClientHolidayDetail.ClientHolidayName : x.ClientEarning.Description,
                IsHoliday = x.ClockClientHolidayDetail != null
            });

            var benefits = result.TryGetData(unapprovedBenefitResults.Union(approvedBenefitResults).Execute().ToList);

            if (result.Success && benefits != null)
            {
                foreach (var b in benefits)
                {
                    b.DayOfWeek = b.EventDate.DayOfWeek;

                    if (b.EndTimeDate.HasValue)
                        b.EndTime = b.EndTimeDate.Value.TimeOfDay;
                    if (b.StartTimeDate.HasValue)
                        b.StartTime = b.StartTimeDate.Value.TimeOfDay;
                }

                result.Data = benefits;
            }

            return result;
        }

        #endregion

        #region Methods

        private void UpdateEmployeeDefaultShifts(IEnumerable<ScheduleShiftDto> shifts)
        {
            
        }

        #endregion

        #region Saving Actions and Funcs

        /// <summary>
        /// Save or delete recurring shifts.
        /// </summary>
        public class SaveRecurringShifts : IAdHocAction<IEnumerable<ScheduleShiftDto>>
        {
            #region Constructor and Variables

            private readonly IBusinessApiSession _session;

            public SaveRecurringShifts(IBusinessApiSession session)
            {
                _session = session;
            }

            #endregion

            /// <summary>
            /// 
            /// </summary>
            /// <param name="shifts"></param>
            /// <param name="sourceId"></param>
            /// <param name="clientId"></param>
            /// <returns></returns>
            public void Execute(IEnumerable<ScheduleShiftDto> shifts)
            {
                foreach(var shift in shifts)
                {
                    var obj = new EmployeeDefaultShift()
                    {
                        EmployeeId = shift.EmployeeId,
                        GroupScheduleShiftId = shift.GroupScheduleShiftId,
                    };

                    if(shift.IsDeleted && shift.ShiftId > 0 && !shift.IsAdded)
                    {
                        // DELETE only if not a new shift
                        obj.EmployeeDefaultShiftId = shift.ShiftId;
                        _session.UnitOfWork.RegisterDeleted(obj);
                    }
                    
                    if(shift.IsAdded)
                    {
                        // ADD/UPDATE shift as necessary
                        _session.SetModifiedProperties(obj);
                        _session.UnitOfWork.RegisterNew(obj);
                    }
                }
            }
        }

        /// <summary>
        /// Save published shifts.
        /// </summary>
        public class SavePublished : IAdHocAction<IEnumerable<ScheduleShiftDto>, int>
        {
            #region Constructor and Variables

            private readonly IBusinessApiSession _session;
            private readonly IAdHocAction<ScheduleShiftDto> _normalizeShiftDateTimes;

            public SavePublished(IBusinessApiSession session, IAdHocAction<ScheduleShiftDto> normalizeShiftDateTimes)
            {
                _session = session;
                _normalizeShiftDateTimes = normalizeShiftDateTimes;
            }

            #endregion

            /// <summary>
            /// 
            /// </summary>
            /// <param name="shifts"></param>
            /// <returns></returns>
            public void Execute(IEnumerable<ScheduleShiftDto> shifts, int clientId)
            {
                var groupedShifts = shifts.GroupBy(s => new
                {
                    s.EmployeeId,
                    s.EventDate
                });

                foreach(var grp in groupedShifts)
                {
                    var curShifts = grp.Select(g => g).ToList();
                    var allDeleted = curShifts.All(a => a.IsDeleted);
                    var allAdded = curShifts.All(a => a.IsAdded);
                    var shiftId = curShifts.Max(s => s.ShiftId);

                    var obj = new ClockEmployeeSchedule() {ClockEmployeeScheduleId = shiftId};

                    if(allDeleted)
                    {
                        _session.UnitOfWork.RegisterDeleted(obj);
                        continue;
                    }

                    if(allAdded)
                        _session.UnitOfWork.RegisterNew(obj);
                    else
                        _session.UnitOfWork.RegisterModified(obj);

                    obj.SetSchedulePropertiesToNull();
                    _session.SetModifiedProperties(obj);

                    //should already be ordered properly
                    var counter = 1;
                    foreach(var shift in curShifts)
                    {
                        if(shift.IsDeleted)
                            continue;

                        obj.EmployeeId = shift.EmployeeId;
                        obj.ClientId = clientId; //need the client id here
                        obj.EventDate = shift.EventDate;

                        _normalizeShiftDateTimes.Execute(shift);

                        if(counter == 1)
                        {
                          
                            obj.ClientCostCenterId1 = shift.Override_ScheduleGroupSourceId.HasValue ? shift.Override_ScheduleGroupSourceId : shift.ScheduleGroupSourceId;
                            obj.GroupScheduleShiftId1 = shift.GroupScheduleShiftId;
                            obj.StartTime1 = shift.StartTimeDate;
                            obj.EndTime1 = shift.EndTimeDate;
                        }

                        if(counter == 2)
                        {
                            obj.ClientCostCenterId2 = shift.Override_ScheduleGroupSourceId.HasValue ? shift.Override_ScheduleGroupSourceId : shift.ScheduleGroupSourceId;
                            obj.GroupScheduleShiftId2 = shift.GroupScheduleShiftId;
                            obj.StartTime2 = shift.StartTimeDate;
                            obj.EndTime2 = shift.EndTimeDate;
                        }

                        if(counter == 3)
                        {
                            obj.ClientCostCenterId3 = shift.Override_ScheduleGroupSourceId.HasValue ? shift.Override_ScheduleGroupSourceId : shift.ScheduleGroupSourceId;
                            obj.GroupScheduleShiftId3 = shift.GroupScheduleShiftId;
                            obj.StartTime3 = shift.StartTimeDate;
                            obj.EndTime3 = shift.EndTimeDate;
                        }

                        counter++;
                    }

                }
            }
        }

        /// <summary>
        /// Save preview shifts.
        /// </summary>
        public class SavePreview : IAdHocAction<IEnumerable<ScheduleShiftDto>>
        {
            #region Constructor and Variables

            private readonly IBusinessApiSession _session;

            public SavePreview(IBusinessApiSession session)
            {
                _session = session;
            }

            #endregion

            /// <summary>
            /// 
            /// </summary>
            /// <param name="shifts"></param>
            /// <param name="sourceId"></param>
            /// <param name="clientId"></param>
            /// <returns></returns>
            public void Execute(IEnumerable<ScheduleShiftDto> shifts)
            {
                var addedScheduleGroups = new List<ScheduleGroup>();

                foreach(var shift in shifts)
                {
                    var obj = new EmployeeSchedulePreview
                    {
                        EmployeeSchedulePreviewId = shift.ShiftId
                    };

                    if(shift.IsDeleted && shift.ShiftId > 0 && !shift.IsAdded)
                    {
                        // DELETE only if not a new shift
                        _session.UnitOfWork.RegisterDeleted(obj);
                    }
                    else
                    {
                        // ADD/UPDATE shift as necessary
                        obj.EmployeeId = shift.EmployeeId;
                        obj.EventDate = shift.EventDate;
                        obj.GroupScheduleShiftId = shift.GroupScheduleShiftId;
                        obj.Override_ScheduleGroupId = shift.Override_ScheduleGroupId;

                        // todo: review: This works for now. Maybe move to its own provider method??
                        if (!shift.Override_ScheduleGroupId.HasValue && shift.Override_ScheduleGroupSourceId.HasValue)
                        {
                            var previouslyAdded =
                                addedScheduleGroups.FirstOrDefault(
                                    g => g.ClientCostCenterId == shift.Override_ScheduleGroupSourceId);

                            if (previouslyAdded == null)
                            {
                                // ADD NEW SCHEDULE GROUP
                                previouslyAdded = new ScheduleGroup
                                {
                                    ScheduleGroupType = ScheduleGroupType.ClientCostCenter,
                                    ClientCostCenterId = shift.Override_ScheduleGroupSourceId.Value
                                };
                                _session.UnitOfWork.RegisterNew(previouslyAdded);
                                addedScheduleGroups.Add(previouslyAdded);
                            }

                            obj.Override_ScheduleGroup = previouslyAdded;
                        }

                        _session.SetModifiedProperties(obj);

                        if(shift.IsAdded)
                            _session.UnitOfWork.RegisterNew(obj);
                        else
                        {
                            _session.UnitOfWork.RegisterModified(obj);
                        }
                    }
                }
            }
        }

        #endregion

        #region Data Conversions

        /// <summary>
        /// There could be other shifts that happen for the date range but don't match the schedule group data.
        /// We need these shift as well.
        /// Need to filter out shifts that may be for the original group due to the Schedule1/2/3 structure of the.
        /// </summary>
        public class CombineAllOtherGroupShifts : IAdHocAction<
            int, 
            IEnumerable<EmployeeSchedulesDto>, 
            IEnumerable<EmployeeSchedulesDto>, 
            IEnumerable<EmployeeSchedulesDto>>
        {
            public void Execute(
                int originalScheduleGroupId,
                IEnumerable<EmployeeSchedulesDto> data,
                IEnumerable<EmployeeSchedulesDto> otherPreviewData,
                IEnumerable<EmployeeSchedulesDto> otherPublishData)
            {
                var otherPreviewShifts = otherPreviewData
                    .SelectMany(ee => ee.EmployeeScheduleShifts.Where(s => s.ScheduleGroupId != originalScheduleGroupId))
                    .ToList();

                // ClockEmployeeSchedule table
                var otherPublishShifts = otherPublishData
                    .SelectMany(ee => ee.EmployeeScheduleShifts.Where(s => s.ScheduleGroupId != originalScheduleGroupId))
                    .ToList();

                var otherShifts = otherPreviewShifts.AddRangeEx(otherPublishShifts);

                data.ForEach(ee => ee.EmployeeScheduleShifts.AddRange(otherShifts.Where(s => s.EmployeeId == ee.EmployeeId)));
            }
        }

        /// <summary>
        /// Extract employee ids from the shift data.
        /// </summary>
        public class ExtractEmployeeIds : IAdHocFunc<
            IEnumerable<EmployeeSchedulesDto>, 
            IEnumerable<int>>
        {
            public IEnumerable<int> Execute(IEnumerable<EmployeeSchedulesDto> data)
            {
                var ids = data
                    .Where(ee => !ee.IsTerminated || ee.EmployeeScheduleShifts.Any())
                    .Select(ee => ee.EmployeeId)
                    .ToList();

                return ids; 
            }
        }

        /// <summary>
        /// Take several different accounts of shift data and puy them into one list.
        /// </summary>
        public class CombineAllShiftData : IAdHocFunc<
            IEnumerable<EmployeeSchedulesDto>, 
            IEnumerable<EmployeeSchedulesDto>, 
            IEnumerable<EmployeeSchedulesDto>, 
            IEnumerable<EmployeeSchedulesDto>>
        {
            public IEnumerable<EmployeeSchedulesDto> Execute(
                IEnumerable<EmployeeSchedulesDto> preview,
                IEnumerable<EmployeeSchedulesDto> published,
                IEnumerable<EmployeeSchedulesDto> empCC)
            {
                Func<EmployeeSchedulesDto, EmployeeSchedulesDto, bool> comparison = (x,y) => x.EmployeeId == y.EmployeeId;
                var shiftsOnly = new List<ScheduleShiftDto>();
                var list = new List<EmployeeSchedulesDto>();
    
                //separate out the shifts
                shiftsOnly
                    .AddRangeEx(preview.SelectMany (p => p.EmployeeScheduleShifts))
                    .AddRangeEx(published.SelectMany (p => p.EmployeeScheduleShifts));  
    
                //remove shifts
                preview.ForEach(x => x.EmployeeScheduleShifts = null);
                published.ForEach(x => x.EmployeeScheduleShifts = null);
    
                var empRecords = (new List<EmployeeSchedulesDto>())
                    .AddRangeDistinct(preview, comparison)
                    .AddRangeDistinct(published, comparison)
                    .AddRangeDistinct(empCC, comparison);

                foreach (var emp in empRecords)
                    emp.EmployeeScheduleShifts = shiftsOnly.Where (o => o.EmployeeId == emp.EmployeeId).ToList();
    
                return empRecords;
            }
        }        

        /// <summary>
        /// Sets the shift's start date/time and end date/time to occur on the shift's event date. 
        /// Then adjusts for any third shift scenarios.
        /// The timespan start and end has to be set (published by default doesn't have the timespan properties set).
        /// </summary>
        public class NormalizeShiftDateTimes : IAdHocAction<
            ScheduleShiftDto>
        {
            public void Execute(ScheduleShiftDto shift)
            {
                // adjust shift times to occur on the event date
                shift.StartTimeDate = shift.StartTime.Value.ToDateTime(shift.EventDate);
                shift.EndTimeDate = shift.EndTime.Value.ToDateTime(shift.EventDate);

                // handle third shift scenarios
                // todo: will eventually need to tie into employee's time policy settings to determine whether to pull the
                // todo: start date up a day OR push the end date ahead a day
                // for now always push the end date ahead a day (ie: the start date will always occur on the event date)
                if(shift.EndTimeDate < shift.StartTimeDate)
                {
                    shift.EndTimeDate = shift.EndTimeDate.Value.AddDays(1);
                }
            }
        }

        /// <summary>
        /// Published data stored the start and end times as dates.
        /// This method will convert that data into timespans.
        /// Timespans are used with the SQL Server Time data type.
        /// lowfix: jay: update the published data to store as Time datatype in the db.
        /// </summary>
        public class FixStartEndTimeForPublished : IAdHocAction<
            IEnumerable<EmployeeSchedulesDto>>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            public void Execute(IEnumerable<EmployeeSchedulesDto> obj)
            {
                //for the published records convert the datetime to time span for both the start and end time for the shift.
                foreach(var data in obj)
                {
                    foreach(var shift in data.EmployeeScheduleShifts)
                    {
                        //if the shift times are null then the fix that. The published data uses dates but everything else is time span.
                        if(!shift.IsPreview)
                        {
                            if (!shift.StartTime.HasValue && shift.StartTimeDate.HasValue)
                            {
                                shift.StartTime = shift.StartTime ?? shift.StartTimeDate.Value.ToTimeSpan();
                                shift.StartTimeDate = null;
                            }

                            if (!shift.EndTime.HasValue && shift.EndTimeDate.HasValue)
                            {
                                shift.EndTime = shift.EndTime ?? shift.EndTimeDate.Value.ToTimeSpan();
                                shift.EndTimeDate = null;
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Break up the published shift data into individual shift objects.
        /// The published data can contain up to 3 different shifts.
        /// </summary>
        public class SplitPublishedRawData : IAdHocFunc<
            IEnumerable<ClockEmployeeScheduleShiftDto>, 
            int?, 
            IScheduleGroupProvider,
            IEnumerable<EmployeeSchedulesDto>>
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="rawPublishedShiftData"></param>
            /// <param name="scheduleGroupId"></param>
            /// <returns></returns>
            public IEnumerable<EmployeeSchedulesDto> Execute(
                IEnumerable<ClockEmployeeScheduleShiftDto> rawPublishedShiftData,
                int? scheduleGroupId,
                IScheduleGroupProvider scheduleGroupProvider)
            {
                var list = new List<EmployeeSchedulesDto>();
                var publishedShifts = rawPublishedShiftData.GroupBy(x => x.EmployeeId);

                //grouped published shifts by employee
                foreach(var rawPubShift in publishedShifts)
                {
                    var empShiftsDto = new EmployeeSchedulesDto();
                    empShiftsDto.EmployeeId = rawPubShift.First().EmployeeId;
                    empShiftsDto.FirstName = rawPubShift.First().EmployeeDataDto.FirstName;
                    empShiftsDto.LastName = rawPubShift.First().EmployeeDataDto.LastName;
                    empShiftsDto.IsTerminated = rawPubShift.First().EmployeeDataDto.IsTerminated;
                    empShiftsDto.JobTitle = rawPubShift.First().EmployeeDataDto.JobTitle;
                    empShiftsDto.JobProfileId = rawPubShift.First().EmployeeDataDto.JobProfileId;
                    empShiftsDto.EmployeeScheduleShifts = new List<ScheduleShiftDto>();

                    var obj = rawPubShift.FirstOrDefault(d => d.EmployeeId == rawPubShift.Key);

                    //for each raw published shift record
                    foreach(var shift in rawPubShift)
                    {
                        //if a schedule group ID was specified only create the shift if it was from the schedule group
                        //otherwise, if the group ID param is null create the shift regardless of group
                        if(shift.ScheduleGroupId1.GetValueOrDefault() == scheduleGroupId ||
                           (!scheduleGroupId.HasValue && shift.ScheduleGroupId1.HasValue))
                        {
                            empShiftsDto.EmployeeScheduleShifts.Add(new ScheduleShiftDto()
                            {
                                ShiftId = shift.ClockEmployeeScheduleId,
                                EmployeeId = empShiftsDto.EmployeeId,
                                EventDate = shift.EventDate,
                                StartTimeDate = shift.StartTimeDate1,
                                EndTimeDate = shift.EndTimeDate1,
                                GroupScheduleShiftId = shift.GroupScheduleShiftId1.GetValueOrDefault(),
                                ScheduleGroupId = shift.ScheduleGroupId1.GetValueOrDefault(),
                                ScheduleGroupDescription = shift.ScheduleGroupDescription1,
                                ScheduleGroupSourceId = shift.ScheduleGroupSourceId1.GetValueOrDefault(),
                                IsPreview = false,
                                //Override Decision Made here.  Needs to compare ScheduleGroup Based on shift, and one found based on SourceID
                                Override_ScheduleGroupSourceId = shift.ClientCostCenterId1 == shift.ScheduleGroupSourceId1.GetValueOrDefault() ? null : shift.ClientCostCenterId1, 
                            });
                        }

                        if(shift.ScheduleGroupId2.GetValueOrDefault() == scheduleGroupId ||
                           (!scheduleGroupId.HasValue && shift.ScheduleGroupId2.HasValue))
                        {
                            empShiftsDto.EmployeeScheduleShifts.Add(new ScheduleShiftDto()
                            {
                                ShiftId = shift.ClockEmployeeScheduleId,
                                EmployeeId = empShiftsDto.EmployeeId,
                                EventDate = shift.EventDate,
                                StartTimeDate = shift.StartTimeDate2,
                                EndTimeDate = shift.EndTimeDate2,
                                GroupScheduleShiftId = shift.GroupScheduleShiftId2.GetValueOrDefault(),
                                ScheduleGroupId = shift.ScheduleGroupId2.GetValueOrDefault(),
                                ScheduleGroupDescription = shift.ScheduleGroupDescription2,
                                ScheduleGroupSourceId = shift.ScheduleGroupSourceId2.GetValueOrDefault(),
                                IsPreview = false,
                                Override_ScheduleGroupSourceId =
                                    shift.ClientCostCenterId2 == shift.ScheduleGroupSourceId2.GetValueOrDefault()
                                        ? null
                                        : shift.ClientCostCenterId2,
                            });
                        }

                        if(shift.ScheduleGroupId3.GetValueOrDefault() == scheduleGroupId ||
                           (!scheduleGroupId.HasValue && shift.ScheduleGroupId3.HasValue))
                        {
                            empShiftsDto.EmployeeScheduleShifts.Add(new ScheduleShiftDto()
                            {
                                ShiftId = shift.ClockEmployeeScheduleId,
                                EmployeeId = empShiftsDto.EmployeeId,
                                EventDate = shift.EventDate,
                                StartTimeDate = shift.StartTimeDate3,
                                EndTimeDate = shift.EndTimeDate3,
                                GroupScheduleShiftId = shift.GroupScheduleShiftId3.GetValueOrDefault(),
                                ScheduleGroupId = shift.ScheduleGroupId3.GetValueOrDefault(),
                                ScheduleGroupDescription = shift.ScheduleGroupDescription3,
                                ScheduleGroupSourceId = shift.ScheduleGroupSourceId3.GetValueOrDefault(),
                                IsPreview = false,
                                Override_ScheduleGroupSourceId = shift.ClientCostCenterId3 == shift.ScheduleGroupSourceId3.GetValueOrDefault() ? null : shift.ClientCostCenterId3, 
                            });
                        }

                        //handle scenario when shift was deleted from outside the group schedule (e.g. from Punch Calendar)
                        //we still want to include the shift in case another shift is added on the same date
                        if (!shift.StartTimeDate1.HasValue)
                        {
                            empShiftsDto.EmployeeScheduleShifts.Add(new ScheduleShiftDto
                            {
                                ShiftId = shift.ClockEmployeeScheduleId,
                                EmployeeId = empShiftsDto.EmployeeId,
                                EventDate = shift.EventDate,
                                IsPreview = false,
                                IsDeleted = true
                            });
                        }
                    }

                    list.Add(empShiftsDto);
                }

                // get override schedule group IDs
                var employeeScheduleShiftsWithOverrides =
                    list.SelectMany(ee => ee.EmployeeScheduleShifts.Where(s => s.Override_ScheduleGroupSourceId != null))
                        .ToList();

                // only set overrides if necessary
                if (employeeScheduleShiftsWithOverrides.Any())
                {
                    var clientId = rawPublishedShiftData.First().ClientId;
                    var scheduleGroupResult = scheduleGroupProvider.GetScheduleGroupsByGroupType(false, clientId, ScheduleGroupType.ClientCostCenter);

                    foreach (var shift in employeeScheduleShiftsWithOverrides)
                    {
                        var group = scheduleGroupResult.Data.FirstOrDefault(g => g.SourceId == shift.Override_ScheduleGroupSourceId);

                        if (group != null)
                        {
                            shift.Override_ScheduleGroupId = group.ScheduleGroupId;
                            shift.Override_Description = group.Description;
                            shift.IsOverridden = true;
                        }
                    }
                }

                return list;

            }
        }

        /// <summary>
        /// Remove the employee data from the shifts.
        /// This data is temporarily used to correlate while gathering all the necessary data.
        /// </summary>
        public class FlattenScheduleData : IAdHocFunc<
            IEnumerable<ScheduleShiftDto>, 
            IEnumerable<EmployeeSchedulesDto>>
        {
            /// <summary>
            /// </summary>
            /// <param name="data"></param>
            /// <returns></returns>
            public IEnumerable<EmployeeSchedulesDto> Execute(IEnumerable<ScheduleShiftDto> data)
            {
                var list = new List<EmployeeSchedulesDto>();

                foreach(var emp in data.GroupBy(x => x.EmployeeId))
                {
                    var obj = new EmployeeSchedulesDto()
                    {
                        EmployeeId = emp.First().EmployeeId,
                        FirstName = emp.First().EmployeeDataDto.FirstName,
                        LastName = emp.First().EmployeeDataDto.LastName,
                        IsTerminated = emp.First().EmployeeDataDto.IsTerminated,
                        JobTitle = emp.First().EmployeeDataDto.JobTitle,
                        JobProfileId = emp.First().EmployeeDataDto.JobProfileId,
                        EmployeeScheduleShifts = emp.ToList(),
                    };

                    list.Add(obj);
                }

                //clear out the extra employee data so it's not sent to the client
                list.ToList().ForEach(x => x.EmployeeScheduleShifts.ForEach(y =>
                {
                    y.EmployeeDataDto = null;
                }));

                return list;
            }
        }

        /// <summary>
        /// Set data on the shift data.
        /// IsNotAssignedToSource flag.
        /// Remove employee data that has no shifts and the employee is terminated.
        /// Add recurring shift data.
        /// </summary>
        public class ProcessGetShiftData : IAdHocAction<
            List<EmployeeSchedulesDto>, 
            IEnumerable<ScheduleShiftDto>, 
            IEnumerable<EmployeeSchedulesDto>>
        {
            /// <summary>
            /// </summary>
            /// <param name="data">The data to modify.</param>
            /// <param name="recurringShifts">Recurring shift data.</param>
            /// <param name="clientCostCenterEmployees">Employees registered with the current cost center being processed.</param>
            public void Execute(
                List<EmployeeSchedulesDto> data,
                IEnumerable<ScheduleShiftDto> recurringShifts,
                IEnumerable<EmployeeSchedulesDto> clientCostCenterEmployees)
            {
                //if an employee wasn't assigned to a ScheduleGroup (CC) set a flag that denotes this to TRUE.
                data.ForEach(x => x.IsNotAssignedToSource = 
                    !clientCostCenterEmployees.Any(y => y.EmployeeId == x.EmployeeId));

                //remove any employee that shows in the list, has NO shifts and is terminated. 
                //they could be terminated but be included because they were registered to the cost center.
                data.RemoveAll(x => !x.EmployeeScheduleShifts.Any() && x.IsTerminated);

                //add recurring shifts
                foreach(var schedData in data)
                {
                    if(schedData.EmployeeRecurringShifts == null)
                        schedData.EmployeeRecurringShifts = new List<ScheduleShiftDto>();

                    schedData.EmployeeRecurringShifts.AddRange(
                        recurringShifts.Where(y => y.EmployeeId == schedData.EmployeeId));

                }
            }
        }

        #endregion

    }
}
