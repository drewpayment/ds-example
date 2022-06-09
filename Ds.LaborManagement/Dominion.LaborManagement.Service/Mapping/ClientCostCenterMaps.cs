using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.GroupScheduling;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ClientCostCenterMaps
    {
        public class ToScheduleGroup :
            ExpressionMapper<ClientCostCenter, GroupScheduleDtos.ScheduleGroupDto>
        {
            public override Expression<Func<ClientCostCenter, GroupScheduleDtos.ScheduleGroupDto>> MapExpression
            {
                get
                {
                    return x => new GroupScheduleDtos.ScheduleGroupDto()
                    {
                        //ScheduleGroupId   = x.ScheduleGroupId,
                        ScheduleGroupId   = 
                            x.ScheduleGroups.Any() 
                            ? x.ScheduleGroups.FirstOrDefault().ScheduleGroupId 
                            : default(int?),

                        SourceId          = x.ClientCostCenterId,
                        Code              = x.Code,
                        Description       = x.Description,
                        ScheduleGroupType = ScheduleGroupType.ClientCostCenter,
                    };
                }

            }
        }

        public class ToScheduleGroupWithSubGroups :
            ExpressionMapper<ClientCostCenter, GroupScheduleDtos.ScheduleGroupDto>
        {
            public override Expression<Func<ClientCostCenter, GroupScheduleDtos.ScheduleGroupDto>> MapExpression
            {
                get
                {
                    return x => new GroupScheduleDtos.ScheduleGroupDto()
                    {
                        //ScheduleGroupId   = x.ScheduleGroupId,
                        ScheduleGroupId   = 
                            x.ScheduleGroups.Any() 
                            ? x.ScheduleGroups.FirstOrDefault().ScheduleGroupId 
                            : default(int?),

                        SourceId          = x.ClientCostCenterId,
                        Code              = x.Code,
                        Description       = x.Description,
                        ScheduleGroupType = ScheduleGroupType.ClientCostCenter,
                        Subgroups         =
                            x.ScheduleGroups.FirstOrDefault()
                                .ScheduleGroupShiftNames.Select(
                                    y => new GroupScheduleDtos.ScheduleGroupShiftNameWithShiftsDto()
                                    {
                                        Name = y.Name,
                                        ScheduleGroupId = y.ScheduleGroupId,
                                        ScheduleGroupShiftNameId = y.ScheduleGroupShiftNameId,
                                    }),
                    };
                }

            }
        }

        public class DefaultClientCostCenterMap : ExpressionMapper<ClientCostCenter, ClientCostCenterDto>
        {
            public override Expression<Func<ClientCostCenter, ClientCostCenterDto>> MapExpression
            {
                get
                {
                    return c => new ClientCostCenterDto()
                    {
                        ClientId = c.ClientId,
                        Modified = c.Modified,
                        ClientCostCenterId = c.ClientCostCenterId,
                        Description = c.Description,
                        Code = c.Code,
                        IsActive = c.IsActive,
                        IsDefaultGlCostCenter = c.IsDefaultGlCostCenter
                    };
                }
            }
        }
    }
}
