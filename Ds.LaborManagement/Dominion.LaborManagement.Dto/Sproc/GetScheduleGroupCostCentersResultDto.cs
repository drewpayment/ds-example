//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Dominion.Utility.Query;

//namespace Dominion.LaborManagement.Dto.Sproc
//{
//    public class EmployeeTimeOffPolicyListArgsDto : SprocParametersBase
//    {
//        private readonly  SqlParameterBuilder<int> _userId;

//        public int? UserId
//        {
//            get { return _userId.Value; } 
//            set { _userId.Value = value.GetValueOrDefault(); }
//        }

//        public EmployeeTimeOffPolicyListArgsDto()
//        {
//            _userId = AddParameter<int>("@P_UserId",   SqlDbType.Int);
//        }
//    }
//}
