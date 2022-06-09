using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeAllocatedHoursDifferenceArgsDto : SprocParametersBase
    {
        private SqlParameterBuilder<int> _clientId { get; set; }
        private SqlParameterBuilder<int> _userId { get; set; }
        private SqlParameterBuilder<int> _employeeId { get; set; }
        private SqlParameterBuilder<DateTime> _startDate { get; set; }
        private SqlParameterBuilder<DateTime> _endDate { get; set; }

        public int ClientId { get { return _clientId.Value; } set { _clientId.Value = value; } }
        public int UserId { get { return _userId.Value; } set { _userId.Value = value; } }
        public int EmployeeId { get { return _employeeId.Value; } set { _employeeId.Value = value; } }
        public DateTime StartDate { get { return _startDate.Value; } set { _startDate.Value = value; } }
        public DateTime EndDate { get { return _endDate.Value; } set { _endDate.Value = value; } }

        public GetClockEmployeeAllocatedHoursDifferenceArgsDto()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _userId = AddParameter<int>("@UserID", SqlDbType.Int);
            _employeeId= AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _startDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _endDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
        }
    }
}
