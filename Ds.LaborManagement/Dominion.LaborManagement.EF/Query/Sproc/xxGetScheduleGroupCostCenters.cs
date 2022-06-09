//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dominion.LaborManagement.Dto.GroupScheduling;
//using Dominion.LaborManagement.Dto.Sproc;
//using Dominion.Utility.ExtensionMethods;
//using Dominion.Utility.Query;

//namespace Dominion.LaborManagement.EF.Query.Sproc
//{
//    internal class xxGetScheduleGroupCostCenters : SprocBase<IEnumerable<GroupScheduleDtos.ScheduleGroupDto>, GetScheduleGroupCostCentersArgsDto>
//    {
//        #region Constructors And Initializers

//        public xxGetScheduleGroupCostCenters(string connStr, GetScheduleGroupCostCentersArgsDto args) 
//            : base(connStr, args)
//        {
//        }

//        #endregion


//        public override IEnumerable<GroupScheduleDtos.ScheduleGroupDto> Execute()
//        {
//            var dto = ExecuteSproc(
//                "[labor].[xxGetScheduleGroupCostCenters]",
//                dr =>
//                {
//                    var data = dr.ToObject<List<GroupScheduleDtos.ScheduleGroupDto>>();
//                    return data;
//                });


//            return dto;
//        }


//    }
//}
