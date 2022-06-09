using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class GetClockClientRulesByEmployeeIdArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientId;
        private readonly SqlParameterBuilder<int> _employeeId;

        public int ClientId
        {
            get { return _clientId.Value; }
            set { _clientId.Value = value; }
        }

        public int EmployeeId
        {
            get { return _employeeId.Value; }
            set { _employeeId.Value = value; }
        }

        public GetClockClientRulesByEmployeeIdArgsDto()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
        }
    }
}
