using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeHoursComparisonArgsDto : SprocParametersBase
    {
        private SqlParameterBuilder<int> _clientId { get; set; }
        private SqlParameterBuilder<int> _userId { get; set; }
        private readonly SqlParameterBuilder<List<Microsoft.SqlServer.Server.SqlDataRecord>> _EmployeeIDs;
        private SqlParameterBuilder<DateTime> _startDate { get; set; }
        private SqlParameterBuilder<DateTime> _endDate { get; set; }
        private SqlParameterBuilder<int> _filterCategory { get; set; }
        private SqlParameterBuilder<int> _filter { get; set; }
        private SqlParameterBuilder<int> _payType { get; set; }
        private SqlParameterBuilder<int> _status { get; set; }

        public int ClientId { get { return _clientId.Value; } set { _clientId.Value = value; } }
        public int UserId { get { return _userId.Value; } set { _userId.Value = value; } }
        public List<Microsoft.SqlServer.Server.SqlDataRecord> EmployeeIDs
        {
            get { return _EmployeeIDs.Value.ToList(); }
            set { _EmployeeIDs.Value = value; }
        }
        public DateTime StartDate { get { return _startDate.Value; } set { _startDate.Value = value; } }
        public DateTime EndDate { get { return _endDate.Value; } set { _endDate.Value = value; } }
        public int FilterCategory { get { return _filterCategory.Value; } set { _filterCategory.Value = value; } }
        public int Filter { get { return _filter.Value; } set { _filter.Value = value; } }
        public int PayType { get { return _payType.Value; } set { _payType.Value = value; } }
        public int Status { get { return _status.Value; } set { _status.Value = value; } }

        public GetClockEmployeeHoursComparisonArgsDto()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _userId = AddParameter<int>("@UserID", SqlDbType.Int);
            _EmployeeIDs = AddParameter<List<Microsoft.SqlServer.Server.SqlDataRecord>>("@EmployeeIDs", SqlDbType.Structured);
            _startDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _endDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
            _filterCategory = AddParameter<int>("@FilterCategory", SqlDbType.Int);
            _filter = AddParameter<int>("@Filter", SqlDbType.Int);
            _payType = AddParameter<int>("@PayType", SqlDbType.NVarChar);
            _status = AddParameter<int>("@Status", SqlDbType.NVarChar);
        }
    }
}
