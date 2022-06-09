using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class ClockEmployeeBenefitHolidayArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int>      _employeeID;
        private readonly SqlParameterBuilder<int>      _clockClientHolidayID;
        private readonly SqlParameterBuilder<DateTime> _startDate;
        private readonly SqlParameterBuilder<DateTime> _endDate;
        private readonly SqlParameterBuilder<int>      _modifiedBy;
        private readonly SqlParameterBuilder<int>      _clockClientTimePolicyID;
        private readonly SqlParameterBuilder<bool>     _disregardEmployeeStatus;

        /// <summary>
        /// OPTIONAL (NULLABLE): This is the leave management policy. Example: PTO, Sick, Volunteer, etc.
        /// </summary>
        public int EmployeeID
        {
            get { return _employeeID.Value; }
            set { _employeeID.Value = value; }
        }

        /// <summary>
        /// OPTIONAL (NULLABLE): The status of the leave request.
        /// </summary>
        public int ClockClientHolidayID
        {
            get { return _clockClientHolidayID.Value; }
            set { _clockClientHolidayID.Value = value; }
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

        public int ModifiedBy
        {
            get { return _modifiedBy.Value; }
            set { _modifiedBy.Value = value; }
        }

        public int ClockClientTimePolicyID
        {
            get { return _clockClientTimePolicyID.Value; }
            set { _clockClientTimePolicyID.Value = value; }
        }

        public bool DisregardEmployeeStatus
        {
            get { return _disregardEmployeeStatus.Value; }
            set { _disregardEmployeeStatus.Value = value; }
        }

        public ClockEmployeeBenefitHolidayArgsDto()
        {
            _employeeID = AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _clockClientHolidayID = AddParameter<int>("@ClockClientHolidayID", SqlDbType.Int);
            _startDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _endDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
            _modifiedBy = AddParameter<int>("@ModifiedBy", SqlDbType.Int);
            _clockClientTimePolicyID = AddParameter<int>("@ClockClientTimePolicyID", SqlDbType.Int);
            _disregardEmployeeStatus = AddParameter<bool>("@DisregardEmployeeStatus", SqlDbType.Bit);

        }
    }
}
