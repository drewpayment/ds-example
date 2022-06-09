using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeExceptionHistoryByEmployeeIDArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientID;
        private readonly SqlParameterBuilder<int> _EmployeeID;
        private readonly SqlParameterBuilder<DateTime> _StartDate;
        private readonly SqlParameterBuilder<DateTime> _EndDate;

        public int ClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }
        public int EmployeeID
        {
            get { return _EmployeeID.Value; }
            set { _EmployeeID.Value = value; }
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

        public GetClockEmployeeExceptionHistoryByEmployeeIDArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);
            _EmployeeID = AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _StartDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _EndDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
        }
    }
}
