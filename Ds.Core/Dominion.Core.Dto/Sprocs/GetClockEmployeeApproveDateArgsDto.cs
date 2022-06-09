using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeApproveDateArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int>       _clientID;
        private readonly SqlParameterBuilder<int>       _userID;
        private readonly SqlParameterBuilder<int>       _employeeID;
        private readonly SqlParameterBuilder<DateTime>  _startDate;
        private readonly SqlParameterBuilder<DateTime>  _endDate;
        private readonly SqlParameterBuilder<bool>      _onlyPayToSchedule;

        public int ClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }
        public int UserID
        {
            get { return _userID.Value; }
            set { _userID.Value = value; }
        }
        public int  EmployeeID
        {
            get { return _employeeID.Value; }
            set { _employeeID.Value = value; }
        }

        public DateTime StartDate
        {
            get { return _startDate.Value; }
            set { _startDate.Value = value; }
        }

        public DateTime EndDate
        {
            get { return _endDate.Value; }
            set { _endDate.Value = value; }
        }

        public bool OnlyPayToSchedule
        {
            get { return _onlyPayToSchedule.Value; }
            set { _onlyPayToSchedule.Value = value; }
        }

        public GetClockEmployeeApproveDateArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);
            _userID = AddParameter<int>("@UserID", SqlDbType.Int);
            _employeeID = AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _startDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime); // _startDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _endDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
            _onlyPayToSchedule = AddParameter<bool>("@OnlyPayToSchedule", SqlDbType.Bit);

        }
    }
}
