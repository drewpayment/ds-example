using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    internal class GroupScheduleProvider : IGroupScheduleProvider
    {
        #region Variables and Properties
        
        private readonly IBusinessApiSession _session;
        
        #endregion

        #region Constructors and Initializers

        public GroupScheduleProvider(IBusinessApiSession session)
        {
            _session = session;
        }

        #endregion
        
        #region IGroupScheduleProvider

        /// <summary>
        /// Gets the raw data and flattens it.
        /// </summary>
        /// <param name="groupScheduleId">The id of the group sched.</param>
        /// <returns></returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> IGroupScheduleProvider.GetGroupScheduleGroupedData(int groupScheduleId)
        {
            var opResult = new OpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>();

            opResult.TryCatchIfSuccessful(() =>
            {
                var data = _session.UnitOfWork
                    .LaborManagementRepository
                    .GroupScheduleQuery()
                    .ByGroupScheduleId(groupScheduleId)
                    .ExecuteQueryAs(new GroupScheduleMaps.ToCostCenterGroupScheduleRawDto())
                    .FirstOrDefault();

                //data.SerializeJson(@"c:\+export\GroupScheduleProvider_GetGroupScheduleGroupedData_Raw.json");

                opResult.Data = GroupScheduleMaps.MapTo(data);

            });

            return opResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IOpResult<GroupSchedule> IGroupScheduleProvider.SaveOrUpdateGroupSchedule(GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto data)
        {
            var opResult = new OpResult<GroupSchedule>();
            var scheduleGroupObjects = new List<ScheduleGroup>();
            var scheduleGroupShiftNameObjects = new List<ScheduleGroupShiftName>();
            var tempScheduleGroupId = 0;

            //-----------------------------------------------------------
            //-----------  start the group schedule object    -----------
            //-----------    and register it as a new obj     -----------
            //-----------------------------------------------------------
            var groupScheduleObj = new GroupSchedule()
            {
                GroupScheduleId = data.GroupScheduleId
            };

            //register with EF
            if(groupScheduleObj.GroupScheduleId > 0)
                //_session.UnitOfWork.Register(groupScheduleObj);
                _session.UnitOfWork.RegisterModified(groupScheduleObj);
            else
                _session.UnitOfWork.RegisterNew(groupScheduleObj);

            groupScheduleObj.ClientId = data.ClientId;
            groupScheduleObj.Name = data.Name;
            groupScheduleObj.IsActive = data.IsActive; 
            groupScheduleObj.GroupScheduleShifts = new List<GroupScheduleShift>();
            _session.SetModifiedProperties(groupScheduleObj);

            //add to the return data
            opResult.Data = groupScheduleObj;

            //-----------------------------------------------------------
            //-----------       each sched group object       -----------
            //-----------------------------------------------------------
            foreach(var scheduleGroupDto in data.Groups)
            {
                //add schedule group object to list if it doesn't already exist
                if (scheduleGroupObjects.All(x => x.ClientCostCenterId != scheduleGroupDto.SourceId))
                    scheduleGroupObjects.Add(AddScheduleGroupObject(scheduleGroupDto));

                //retrieve the schedule group object from the list
                var scheduleGroup =
                    scheduleGroupObjects.FirstOrDefault(x => 
                        x.ClientCostCenterId == scheduleGroupDto.SourceId);

                if (scheduleGroup.ScheduleGroupId == 0)
                {
                    // if new schedule group, assign a unique temp ID 
                    // fixes the EF 'Unabled to determine the principal end of the relationship' issue
                    // see: http://stackoverflow.com/questions/25275832/unable-to-determine-the-principal-end-of-the-relationship-on-saving-order-ef6
                    scheduleGroup.ScheduleGroupId = tempScheduleGroupId;
                    tempScheduleGroupId--;
                }

                //-----------------------------------------------------------
                //-----------    each sched group shift name      -----------
                //-----------------------------------------------------------
                foreach(var scheduleGroupShiftNameDto in scheduleGroupDto.Subgroups)
                {
                    //retrieve the schedule group shift name object from the list
                    var scheduleGroupShiftName = scheduleGroupShiftNameObjects.FirstOrDefault(x =>
                            x.ScheduleGroupId == scheduleGroupShiftNameDto.ScheduleGroupId &&
                            x.Name == scheduleGroupShiftNameDto.Name);

                    //add schedule group object shift name object to list if it doesn't already exist
                    if(scheduleGroupShiftName == null)
                    {
                        scheduleGroupShiftName = AddScheduleGroupShiftNameObject(
                            data.ClientId,
                            scheduleGroupShiftNameDto,
                            scheduleGroup);
                        scheduleGroupShiftNameObjects.Add(scheduleGroupShiftName);
                    }

                    foreach(var shiftDto in scheduleGroupShiftNameDto.Shifts)
                    {
                        var gs = new GroupScheduleShift();

                        gs.GroupScheduleShiftId = shiftDto.GroupScheduleShiftId < 1
                            ? int.MinValue
                            : shiftDto.GroupScheduleShiftId;

                        //group schedule
                        gs.GroupScheduleId = groupScheduleObj.GroupScheduleId;

                        if(gs.GroupScheduleShiftId > 0)
                            _session.UnitOfWork.Register(gs);
                        else
                            _session.UnitOfWork.RegisterNew(gs);
                        
                        //schedule group
                        gs.ScheduleGroupId = scheduleGroup.ScheduleGroupId;
                        if(scheduleGroup.ScheduleGroupId < 1)
                            gs.ScheduleGroup = scheduleGroup;

                        //schedule group shift name
                        gs.ScheduleGroupShiftNameId = 
                            scheduleGroupShiftName.Name == null
                            ? null
                            : (int?)scheduleGroupShiftName.ScheduleGroupShiftNameId;

                        //only add the object if we are creating a new schedule group shift name object
                        if(scheduleGroupShiftName.ScheduleGroupShiftNameId < 1 && scheduleGroupShiftName.Name != null)
                            gs.ScheduleGroupShiftName = scheduleGroupShiftName;                            

                        //ordinary shift data
                        gs.NumberOfEmployees = shiftDto.NumberOfEmployees;
                        gs.DayOfWeek = shiftDto.DayOfWeek;
                        gs.StartTime = shiftDto.StartTime;
                        gs.EndTime = shiftDto.EndTime;
                        gs.IsDeleted = shiftDto.IsDeleted;

                        _session.SetModifiedProperties(gs);
                    }
                }
            }

            //mark shift records that were removed and possibly delete employee schedule preview records
            DeleteMissingShiftRecords(groupScheduleObj);

            //groupScheduleObj.SerializeJson(@"c:\+export\GroupScheduleProvider_SaveOrUpdateGroupSchedule_BeforeSave.json");
            opResult.CombineSuccessAndMessages(_session.UnitOfWork.Commit());

            //complete the object for proper mapping. We couldn't add them during the save process or EF would try and add them to the database.
            AddClientCostCentersToScheduleGroupObjects(scheduleGroupObjects, data);
            AddScheduleGroupObjects(groupScheduleObj, scheduleGroupObjects);
            AddScheduleGroupShiftNameObjects(groupScheduleObj, scheduleGroupShiftNameObjects);

            return opResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleGroupId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto>> IGroupScheduleProvider.GetGroupScheduleForScheduling(int scheduleGroupId)
        {
            var opResult = new OpResult<IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto>>();

            opResult.TryCatchIfSuccessful(() =>
            {
                var data = _session.UnitOfWork
                    .LaborManagementRepository
                    .GroupScheduleShiftQuery()
                    .ByGroupScheduleIsActive(true)
                    .ByIsNotDeleted()
                    .ByScheduleGroupId(scheduleGroupId)
                    .ExecuteQueryAs(new GroupScheduleMaps.ToGroupScheduleShiftLiteDto());

                opResult.Data = GroupScheduleMaps.MapTo(data); 
            });

            return opResult;
        }

        /// <summary>
        /// Sets each <see cref="GroupScheduleDtos.ScheduleGroupDto.IsReadOnly"/> property based on if the group is accessible 
        /// by the current user or not.
        /// </summary>
        /// <param name="schedule">Schedule to update group accessibility on.</param>
        /// <param name="accessibleScheduleGroupIds">Schedule group ID(s) the current user has access to.</param>
        /// <returns></returns>
        IOpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto> IGroupScheduleProvider.UpdateScheduleGroupAccess(GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto schedule, IEnumerable<int> accessibleScheduleGroupIds)
        {
            return new OpResult<GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto>().TrySetData(() =>
                {
                    foreach (var group in schedule.Groups.Where(g => g.ScheduleGroupId.HasValue))
                    {
                        group.IsReadOnly = accessibleScheduleGroupIds == null || !accessibleScheduleGroupIds.Contains(group.ScheduleGroupId.Value);
                    } 

                    return schedule;
                });
        }

        #endregion

        #region Pre Commit Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ScheduleGroup AddScheduleGroupObject(
            GroupScheduleDtos.ScheduleGroupDto data)
        {
            var scheduleGroup = new ScheduleGroup()
            {
                ScheduleGroupId = data.ScheduleGroupId.GetValueOrDefault(),
                ClientCostCenterId = data.SourceId,
                ScheduleGroupType = data.ScheduleGroupType,
            };

            return scheduleGroup;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="scheduleGroup"></param>
        /// <returns></returns>
        private ScheduleGroupShiftName AddScheduleGroupShiftNameObject(
            int clientId,
            GroupScheduleDtos.ScheduleGroupShiftNameWithShiftsDto data,
            ScheduleGroup scheduleGroup)
        {
            var obj = new ScheduleGroupShiftName();

            obj.ScheduleGroupShiftNameId = data.ScheduleGroupShiftNameId.GetValueOrDefault();

            if (!string.IsNullOrWhiteSpace(data.Name))
            {
                if (obj.ScheduleGroupShiftNameId < 1)
                    _session.UnitOfWork.RegisterNew(obj);
                else
                    _session.UnitOfWork.Register(obj);
            }

            obj.Name = data.Name;
            obj.ScheduleGroupId = scheduleGroup.ScheduleGroupId;
            obj.ClientId = clientId;

            //only add this if the schedule group is new
            if(scheduleGroup.ScheduleGroupId < 1)
                obj.ScheduleGroup = scheduleGroup;

            _session.SetModifiedProperties(obj);

            return obj;
        }

        /// <summary>
        /// Find out what group shift records need to be deleted.
        /// </summary>
        /// <param name="obj">The group schedule object we're evaluating.</param>
        private void DeleteMissingShiftRecords(
            GroupSchedule obj)
        {
            var data = _session
                .UnitOfWork
                .LaborManagementRepository
                .GroupScheduleShiftQuery()
                .ByGroupScheduleId(obj.GroupScheduleId)
                .ExecuteQueryAs(new GroupScheduleMaps.ToGroupScheduleShiftReferenceDto())
                .ToList();

            //find all the shift ids that are greater than zero in the group schedule object being evaluated
            var shiftsIdsInData = obj
                .GroupScheduleShifts
                .Where(x => x.GroupScheduleShiftId > 0) //ignore the new group shifts
                .Select(x => x.GroupScheduleShiftId)
                .ToList();

            //compare the shifts found the db to what is in the obj.
            var missingShiftData = data.Select(x => x)
                .Where(x => !shiftsIdsInData.Contains(x.GroupScheduleShiftId));

            foreach(var shiftReferenceData in missingShiftData)
            {
                //update the shift record's 'IsDeleted' flag (not deleting the record
                var shift = new GroupScheduleShift
                    {
                        GroupScheduleShiftId = shiftReferenceData.GroupScheduleShiftId
                    };
                _session.UnitOfWork.Register(shift);
                _session.SetModifiedProperties(shift);
                shift.IsDeleted = true;

                //delete the employee schedule preview records that reference the shift
                foreach(var id in shiftReferenceData.EmployeSchedulePreviewIds)
                {
                    var empSchedulePreview = new EmployeeSchedulePreview() {EmployeeSchedulePreviewId = id};
                    _session.UnitOfWork.RegisterDeleted(empSchedulePreview);
                }

                //delete the employee default shift records that reference the shift
                foreach(var id in shiftReferenceData.EmployeeDefaultShiftIds)
                {
                    var empDefaultShift = new EmployeeDefaultShift() { EmployeeDefaultShiftId = id };
                    _session.UnitOfWork.RegisterDeleted(empDefaultShift);
                }
            }

        }

        #endregion

        #region Post Commit Methods

        private void AddClientCostCentersToScheduleGroupObjects(
            IEnumerable<ScheduleGroup> scheduleGroups,
            GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto data)
        {
            foreach(var schedGroup in scheduleGroups)
            {
                if(schedGroup.ClientCostCenter == null)
                {
                    var grpData = data.Groups.Where(x => x.SourceId == schedGroup.ClientCostCenterId).FirstOrDefault();
                    schedGroup.ClientCostCenter = new ClientCostCenter()
                    {
                        Code = grpData.Code,
                        Description = grpData.Description,
                        ClientCostCenterId = grpData.SourceId,
                    };
                }
            }            
        }

        private void AddScheduleGroupObjects(
            GroupSchedule obj, 
            IEnumerable<ScheduleGroup> scheduleGroups)
        {
            foreach(var shift in obj.GroupScheduleShifts)
            {
                shift.ScheduleGroup =
                    shift.ScheduleGroup ??
                    scheduleGroups.FirstOrDefault(x => x.ScheduleGroupId == shift.ScheduleGroupId);
            }            
        }

        private void AddScheduleGroupShiftNameObjects(
            GroupSchedule obj, 
            IEnumerable<ScheduleGroupShiftName> scheduleGroupsShiftNames)
        {
            foreach(var shift in obj.GroupScheduleShifts)
            {
                shift.ScheduleGroupShiftName =
                    shift.ScheduleGroupShiftName ??
                    scheduleGroupsShiftNames.FirstOrDefault(x => x.ScheduleGroupShiftNameId == shift.ScheduleGroupShiftNameId);
            }              
        }


        #endregion
    }
}
