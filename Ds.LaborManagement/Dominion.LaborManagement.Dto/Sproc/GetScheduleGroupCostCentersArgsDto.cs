//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dominion.Utility.Query;

//namespace Dominion.LaborManagement.Dto.Sproc
//{
//    public class GetScheduleGroupCostCentersArgsDto : SprocParametersBase
//    {
//        private SqlParameterBuilder<int> _clientId;
//        private SqlParameterBuilder<bool> _isActive;
//        private SqlParameterBuilder<int?> _userId;
//        private SqlParameterBuilder<int?> _scheduleGroupId;
//        private SqlParameterBuilder<int?> _scheduleGroupTypeId;


//        public int ClientId
//        {
//            get { return _clientId.Value; } 
//            set { _clientId.Value = value; }
//        }

//        public bool IsActive
//        {
//            get { return _isActive.Value; } 
//            set { _isActive.Value = value; }
//        }

//        public int? UserId
//        {
//            get { return _userId.Value; } 
//            set { _userId.Value = value; }
//        }

//        public int? ScheduleGroupId
//        {
//            get { return _scheduleGroupId.Value; } 
//            set { _scheduleGroupId.Value = value; }
//        }

//        public int? ScheduleGroupTypeId
//        {
//            get { return _scheduleGroupTypeId.Value; } 
//            set { _scheduleGroupTypeId.Value = value; }
//        }

//        public GetScheduleGroupCostCentersArgsDto()
//        {
//            _clientId = AddParameter<int>("@P_ClientId",   SqlDbType.Int);
//            _userId = AddParameter<int?>("@P_UserId",   SqlDbType.Int);
//            _isActive = AddParameter<bool>("@P_IsActive",   SqlDbType.Bit);
//            _scheduleGroupId = AddParameter<int?>("@P_ScheduleGroupId",   SqlDbType.Int);
//            _scheduleGroupTypeId = AddParameter<int?>("@P_ScheduleGroupTypeId",   SqlDbType.Int);
//        }
//    }
//}
