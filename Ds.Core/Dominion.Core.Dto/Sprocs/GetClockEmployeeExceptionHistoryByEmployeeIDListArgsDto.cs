using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeExceptionHistoryByEmployeeIDListArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientID;
        //Need to change this for employee List 
        private readonly SqlParameterBuilder<List<Microsoft.SqlServer.Server.SqlDataRecord>> _EmployeeIDs;
        private readonly SqlParameterBuilder<DateTime> _StartDate;
        private readonly SqlParameterBuilder<DateTime> _EndDate;

        public int ClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }
        //Fix Me for list
        public List<Microsoft.SqlServer.Server.SqlDataRecord> EmployeeIDs
        {
            get { return _EmployeeIDs.Value.ToList(); }
            set { _EmployeeIDs.Value = value; }
        }
        public DateTime StartDate
        {
            get { return _StartDate.Value; }
            set { _StartDate.Value = value; }
        }
        public DateTime EndDate
        {
            get { return _EndDate.Value; }
            set { _EndDate.Value = value; }
        }

        public GetClockEmployeeExceptionHistoryByEmployeeIDListArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);
            //fix me for list
            _EmployeeIDs = AddParameter<List<Microsoft.SqlServer.Server.SqlDataRecord>>("@EmployeeIDs", SqlDbType.Structured);
            _StartDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _EndDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
        }
    }
}
