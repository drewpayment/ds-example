using Dominion.Utility.Query;
using System;
using System.Data;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class ClockEmployeePunchListByDateAndFilterPaginatedCountArgs : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientId;
        private readonly SqlParameterBuilder<int> _userId;
        private readonly SqlParameterBuilder<int> _employeeId;
        private readonly SqlParameterBuilder<DateTime> _startDate;
        private readonly SqlParameterBuilder<DateTime> _endDate;
        private readonly SqlParameterBuilder<int> _filter;
        private readonly SqlParameterBuilder<int> _filterCategory;
        private readonly SqlParameterBuilder<int> _specialOption;
        private readonly SqlParameterBuilder<int> _filterCategory2;
        private readonly SqlParameterBuilder<int> _filter2;
        private readonly SqlParameterBuilder<int> _size;

        public int ClientId
        {
            get { return _clientId.Value; }
            set { _clientId.Value = value; }
        }

        public int UserId
        {
            get { return _userId.Value; }
            set { _userId.Value = value; }
        }

        public int EmployeeId
        {
            get { return _employeeId.Value; }
            set { _employeeId.Value = 0; }
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

        public int Filter
        {
            get { return _filter.Value; }
            set { _filter.Value = value; }
        }

        public int FilterCategory
        {
            get { return _filterCategory.Value; }
            set { _filterCategory.Value = value; }
        }

        public int SpecialOption
        {
            get { return _specialOption.Value; }
            set { _specialOption.Value = value; }
        }

        public int Filter2
        {
            get { return _filter2.Value; }
            set { _filter2.Value = value; }
        }

        public int FilterCategory2
        {
            get { return _filterCategory2.Value; }
            set { _filterCategory2.Value = value; }
        }

        public int PageSize
        {
            get { return _size.Value; }
            set { _size.Value = value; }
        }

        public ClockEmployeePunchListByDateAndFilterPaginatedCountArgs()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _userId = AddParameter<int>("@UserID", SqlDbType.Int);
            _employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
            _startDate = AddParameter<DateTime>("@StartDate", SqlDbType.DateTime);
            _endDate = AddParameter<DateTime>("@EndDate", SqlDbType.DateTime);
            _filter = AddParameter<int>("@Filter", SqlDbType.Int);
            _filterCategory = AddParameter<int>("@FilterCategory", SqlDbType.Int);
            _specialOption = AddParameter<int>("@SpecialOption", SqlDbType.Int);
            _filterCategory2 = AddParameter<int>("@FilterCategory2", SqlDbType.Int);
            _filter2 = AddParameter<int>("@Filter2", SqlDbType.Int);
            _size = AddParameter<int>("@Size", SqlDbType.Int);

            _employeeId.Value = 0;
        }
    }
}
