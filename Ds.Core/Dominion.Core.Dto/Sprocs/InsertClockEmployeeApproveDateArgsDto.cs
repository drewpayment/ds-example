using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class InsertClockEmployeeApproveDateArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _employeeID;
        private readonly SqlParameterBuilder<DateTime> _eventDate;
        private readonly SqlParameterBuilder<int> _modifiedBy;
        private readonly SqlParameterBuilder<bool> _isApproved;
        private readonly SqlParameterBuilder<int> _clientCostCenterID;
        private readonly SqlParameterBuilder<int> _clockClientNoteId;
        private readonly SqlParameterBuilder<bool> _payToSchedule;

        public int EmployeeID
        {
            get { return _employeeID.Value; }
            set { _employeeID.Value = value; }
        }

        public DateTime? EventDate
        {
            get { return _eventDate.Value; }
            set { _eventDate.Value = value ?? DateTime.Parse("1/1/1900"); }
        }

        public int ModifiedBy
        {
            get { return _modifiedBy.Value; }
            set { _modifiedBy.Value = value; }
        }

        public Boolean IsApproved
        {
            get { return _isApproved.Value; }
            set { _isApproved.Value = value; }
        }

        public int ClientCostCenterID
        {
            get { return _clientCostCenterID.Value; }
            set { _clientCostCenterID.Value = value; }
        }

        public int ClockClientNoteID
        {
            get { return _clockClientNoteId.Value; }
            set { _clockClientNoteId.Value = value; }
        }

        public bool PayToSchedule
        {
            get { return _payToSchedule.Value; }
            set { _payToSchedule.Value = value; }
        }

        public InsertClockEmployeeApproveDateArgsDto()
        {
            _employeeID = AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _eventDate = AddParameter<DateTime>("@EventDate", SqlDbType.DateTime);
            _modifiedBy = AddParameter<int>("@ModifiedBy", SqlDbType.Int);
            _isApproved = AddParameter<bool>("@IsApproved", SqlDbType.Bit);
            _clockClientNoteId = AddParameter<int>("@ClockClientNoteID", SqlDbType.Int);
            _payToSchedule = AddParameter<bool>("@PaytoSchedule", SqlDbType.Bit);
            _clientCostCenterID = AddParameter<int>("@ClientCostCenterID", SqlDbType.Int);
        }
    }
}
