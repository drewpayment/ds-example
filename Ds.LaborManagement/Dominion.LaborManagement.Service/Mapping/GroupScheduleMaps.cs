using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.Utility.Mapping;
using FastMapper;
using Omu.ValueInjecter;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class GroupScheduleMaps
    {
        /// <summary>
        /// This is analogous to building a sproc that will get the basic data for us to return for the configuration of a group schedule.
        /// EXAMPLE (mapping with EF)
        ///    -review: note: jay: I tried this with AutoMapper.Project and FastMapper.Project and neither worked; must be due to the depth of the mappings.
        ///    -lowFix: jay: see if there is a trick to get this to work with framework mappers
        /// </summary>
        public class ToCostCenterGroupScheduleRawDto :
            ExpressionMapper<GroupSchedule, GroupScheduleConfigRawDto.GroupScheduleRawDto>
        {
            public override Expression<Func<GroupSchedule, GroupScheduleConfigRawDto.GroupScheduleRawDto>> MapExpression
            {
                get
                {
                    return gs => new GroupScheduleConfigRawDto.GroupScheduleRawDto
                    {
                        ClientId = gs.ClientId,
                        GroupScheduleId = gs.GroupScheduleId,
                        IsActive = gs.IsActive,
                        Name = gs.Name,
                        //------------------------------------
                        //group schedule shifts
                        //------------------------------------
                        GroupScheduleShifts =
                            gs.GroupScheduleShifts.Where(gss => !gss.IsDeleted).Select(
                                gss => new GroupScheduleConfigRawDto.GroupScheduleShiftRawDto()
                                {
                                    GroupScheduleShiftId = gss.GroupScheduleShiftId,
                                    DayOfWeek = gss.DayOfWeek,
                                    StartTime = gss.StartTime,
                                    EndTime = gss.EndTime,
                                    NumberOfEmployees = gss.NumberOfEmployees,
                                    //------------------------------------
                                    //schedule group map
                                    //------------------------------------
                                    ScheduleGroup = new GroupScheduleConfigRawDto.ScheduleGroupRawDto()
                                    {
                                        ScheduleGroupId = gss.ScheduleGroup.ScheduleGroupId,
                                        ScheduleGroupType = gss.ScheduleGroup.ScheduleGroupType,
                                        //------------------------------------
                                        //client cost center map
                                        //------------------------------------
                                        ClientCostCenter =
                                            new GroupScheduleConfigRawDto.ScheduleGroupClientCostCenterRawDto()
                                            {
                                                ClientCostCenterId =
                                                    gss.ScheduleGroup.ClientCostCenter.ClientCostCenterId,
                                                Code = gss.ScheduleGroup.ClientCostCenter.Code,
                                                Description = gss.ScheduleGroup.ClientCostCenter.Description
                                            }
                                    },
                                    //------------------------------------
                                    //schedule group shift name map
                                    //------------------------------------
                                    ScheduleGroupShiftName =
                                        new GroupScheduleConfigRawDto.ScheduleGroupShiftNameRawDto()
                                        {
                                            ScheduleGroupId = gss.GroupScheduleId,

                                            ScheduleGroupShiftNameId =
                                                gss.ScheduleGroupShiftName != null
                                                    ? gss.ScheduleGroupShiftName.ScheduleGroupShiftNameId
                                                    : default(int?),

                                            Name = gss.ScheduleGroupShiftName != null
                                                ? gss.ScheduleGroupShiftName.Name
                                                : default(string),
                                        },
                                    //ScheduleGroupShiftName = 
                                    //    gss.GroupScheduleShiftNameId != null
                                    //        ?   new GroupScheduleConfigRawDto.ScheduleGroupShiftNameRawDto()
                                    //            {
                                    //                ScheduleGroupId = gss.GroupScheduleId,
                                    //                ScheduleGroupShiftNameId = gss.ScheduleGroupShiftName.ScheduleGroupShiftNameId,
                                    //                Name = gss.ScheduleGroupShiftName.Name,
                                    //            } 
                                    //        : default(GroupScheduleConfigRawDto.ScheduleGroupShiftNameRawDto),
                                })
                    };
                }
            }
        }

        /// <summary>
        /// Get information on what objects to delete.
        /// </summary>
        public class ToGroupScheduleShiftReferenceDto :
            ExpressionMapper<GroupScheduleShift, GroupScheduleShiftReferencesDto>
        {
            public override Expression<Func<GroupScheduleShift, GroupScheduleShiftReferencesDto>> MapExpression
            {
                get
                {
                    return gs => new GroupScheduleShiftReferencesDto
                    {
                        GroupScheduleShiftId = gs.GroupScheduleShiftId,
                        EmployeeDefaultShiftIds = gs.EmployeeDefaultShifts.Select(x => x.EmployeeDefaultShiftId),
                        EmployeSchedulePreviewIds = gs.EmployeeSchedulePreviews.Select(x => x.EmployeeSchedulePreviewId),
                    };
                }
            }
        }

        /// <summary>
        /// Get all the shifts for a given schedule group id with the group schedule data too.
        /// </summary>
        public class ToGroupScheduleShiftLiteDto :
            ExpressionMapper<GroupScheduleShift, GroupScheduleConfigRawDto.GroupScheduleShiftLiteRawDto>
        {
            public override Expression<Func<GroupScheduleShift, GroupScheduleConfigRawDto.GroupScheduleShiftLiteRawDto>>
                MapExpression
            {
                get
                {
                    return gs => new GroupScheduleConfigRawDto.GroupScheduleShiftLiteRawDto
                    {
                        GroupSchedule = new GroupScheduleDtos.GroupScheduleDto()
                        {
                            Name            = gs.GroupSchedule.Name,
                            ClientId        = gs.GroupSchedule.ClientId,
                            GroupScheduleId = gs.GroupSchedule.GroupScheduleId,
                            IsActive        = gs.GroupSchedule.IsActive,
                        },

                        ScheduleGroupShiftName = new GroupScheduleConfigRawDto.ScheduleGroupShiftNameRawDto()
                        {
                            ScheduleGroupId = gs.GroupScheduleId,

                            ScheduleGroupShiftNameId =
                                gs.ScheduleGroupShiftName != null
                                    ? gs.ScheduleGroupShiftName.ScheduleGroupShiftNameId
                                    : default(int?),

                            Name = gs.ScheduleGroupShiftName != null
                                ? gs.ScheduleGroupShiftName.Name
                                : default(string),
                        },

                        GroupScheduleShiftId = gs.GroupScheduleShiftId,
                        DayOfWeek = gs.DayOfWeek,
                        StartTime = gs.StartTime,
                        EndTime = gs.EndTime,
                        NumberOfEmployees = gs.NumberOfEmployees
                    };
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class ToGroupScheduleDto : FastMapperDs<GroupSchedule, GroupScheduleDtos.GroupScheduleDto>
        {
        }

        /// <summary>
        /// This is used for the return value after updating/saving a group schedule.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto MapTo(
            GroupSchedule data)
        {
            var dto = new ToCostCenterGroupScheduleRawDto().MapExpression.Compile().Invoke(data);
            return MapTo(dto);
        }

        public static GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto MapTo(
            GroupScheduleConfigRawDto.GroupScheduleRawDto data)
        {
            var dto = new GroupScheduleDtos.GroupScheduleWithScheduleGroupsDto()
            {
                Name = data.Name,
                ClientId = data.ClientId,
                GroupScheduleId = data.GroupScheduleId,
                IsActive = data.IsActive,
    
                Groups = data
                    .GroupScheduleShifts
                    .GroupBy(x => x.ScheduleGroup.ScheduleGroupId)
                    .Select(x => new GroupScheduleDtos.ScheduleGroupDto()
                    {
                        ScheduleGroupId = x.Key,
                        SourceId = x.First().ScheduleGroup.ClientCostCenter.ClientCostCenterId,
                        Code = x.First().ScheduleGroup.ClientCostCenter.Code,
                        Description = x.First().ScheduleGroup.ClientCostCenter.Description,
                        ScheduleGroupType = x.First().ScheduleGroup.ScheduleGroupType,
                
                        Subgroups = x
                            .GroupBy (xx => xx.ScheduleGroupShiftName.ScheduleGroupShiftNameId.GetValueOrDefault())
                            .Select (xx => new GroupScheduleDtos.ScheduleGroupShiftNameWithShiftsDto()
                            {
                                ScheduleGroupShiftNameId = xx.Key == 0 ? default(int?) : xx.Key,
                                ScheduleGroupId = x.Key,
                                Name = 
                                    xx.Key > 0 
                                    ? xx.First().ScheduleGroupShiftName.Name 
                                    : default(string),
                            
                                Shifts = xx
                                .Select(z => new GroupScheduleDtos.GroupScheduleShiftLiteDto()
                                {
                                    GroupScheduleShiftId = z.GroupScheduleShiftId,
                                    DayOfWeek = z.DayOfWeek,
                                    StartTime = z.StartTime,
                                    EndTime = z.EndTime,
                                    NumberOfEmployees = z.NumberOfEmployees,
                                }).ToList(),                            
                            }).ToList(), 
                    }).ToList(),
            };

            return dto;
        }

        /// <summary>
        /// Map the raw shift data to a dto that represents a group schedule and it's shifts.
        /// This data is used for scheduling.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IEnumerable<GroupScheduleDtos.GroupScheduleForSchedulingDto> MapTo(
            IEnumerable<GroupScheduleConfigRawDto.GroupScheduleShiftLiteRawDto> data)
        {
            var qry = data
                .GroupBy(d => d.GroupSchedule.GroupScheduleId)
                .Select(d => new GroupScheduleDtos.GroupScheduleForSchedulingDto()
                {
                    GroupScheduleId = d.Key,
                    Name            = d.First().GroupSchedule.Name,

                    Subgroups = d.GroupBy (gs => gs.ScheduleGroupShiftName.ScheduleGroupShiftNameId.GetValueOrDefault())
                        .Select (gs => new GroupScheduleDtos.ScheduleGroupShiftNameWithShiftsDto()
                        {
                            ScheduleGroupShiftNameId = gs.Key,
                            ScheduleGroupId = d.Key,
                            Name = 
                                gs.Key > 0 
                                ? gs.First().ScheduleGroupShiftName.Name 
                                : default(string),
                            
                            Shifts = gs
                                .Select(z => new GroupScheduleDtos.GroupScheduleShiftLiteDto()
                                {
                                    GroupScheduleShiftId = z.GroupScheduleShiftId,
                                    DayOfWeek = z.DayOfWeek,
                                    StartTime = z.StartTime,
                                    EndTime = z.EndTime,
                                    NumberOfEmployees = z.NumberOfEmployees,                                
                                }).ToList(),                            
                        }).ToList(), 
                })
                .ToList();

            return qry;
        }
    }
}
