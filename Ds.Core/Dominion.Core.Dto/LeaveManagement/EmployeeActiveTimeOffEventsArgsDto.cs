using System;
using System.Data;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class EmployeeActiveTimeOffEventsArgsDto : SprocParametersBase
    {
        private readonly  SqlParameterBuilder<int?>      _planId;
        private readonly  SqlParameterBuilder<int?>      _statusId;
        private readonly  SqlParameterBuilder<int>       _userId;
        private readonly  SqlParameterBuilder<int>       _clientId;
        private readonly  SqlParameterBuilder<int?>      _employeeId;
        private readonly  SqlParameterBuilder<DateTime?> _startDate;
        private readonly  SqlParameterBuilder<DateTime?> _endDate;
        private readonly  SqlParameterBuilder<int>       _filterTypeId;
        private readonly  SqlParameterBuilder<int>       _filterId;
        private readonly  SqlParameterBuilder<int>       _employeeStatusFilter;
        private readonly  SqlParameterBuilder<int?>      _payFrequencyId;

        /// <summary>
        /// OPTIONAL (NULLABLE): This is the leave management policy. Example: PTO, Sick, Volunteer, etc.
        /// </summary>
        public int? PlanId
        {
            get { return _planId.Value; } 
            set { _planId.Value = value; }
        }

        /// <summary>
        /// OPTIONAL (NULLABLE): The status of the leave request.
        /// </summary>
        public int? StatusId
        {
            get { return _statusId.Value; } 
            set { _statusId.Value = value; }
        }

        /// <summary>
        /// The id of the user executing the report. This could be a sys admin performing the report for an employee. This doesn't affect the report grid data, it's used to determine who is running the report.
        /// </summary>
        public int UserId
        {
            get { return _userId.Value; } 
            set { _userId.Value = value; }
        }

        /// <summary>
        /// The id of the client.
        /// </summary>
        public int ClientId
        {
            get { return _clientId.Value; } 
            set { _clientId.Value = value; }
        }

        /// <summary>
        /// OPTIONAL (NULLABLE): The id of the employee.
        /// </summary>
        public int? EmployeeId
        {
            get { return _employeeId.Value; } 
            set { _employeeId.Value = value; }
        }

        /// <summary>
        /// The start date of the date range.
        /// </summary>
        public DateTime? StartDate
        {
            get { return _startDate.Value; } 
            set { _startDate.Value = value; }
        }

        /// <summary>
        /// The end date of the date range.
        /// </summary>
        public DateTime? EndDate
        {
            get { return _endDate.Value; } 
            set { _endDate.Value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FilterTypeId
        {
            get { return _filterTypeId.Value; } 
            set { _filterTypeId.Value = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int FilterId
        {
            get { return _filterId.Value; } 
            set { _filterId.Value = value; }
        }

        /// <summary>
        /// Employee filter. -1 = All, 0 = Inactive Only, 1 = Active Only
        /// Since ESS is a single employee's view, we will always set this to -1 from ESS.
        /// </summary>
        public int EmployeeStatusFilter
        {
            get { return _employeeStatusFilter.Value; }
            set { _employeeStatusFilter.Value = value; }
        }

        public int? PayFrequencyId
        {
            get { return _payFrequencyId.Value; }
            set { _payFrequencyId.Value = value; }
        }

        public EmployeeActiveTimeOffEventsArgsDto()
        {
            _planId                 = AddParameter<int?>      ("@PlanID",           SqlDbType.Int);
            _statusId               = AddParameter<int?>      ("@StatusID",         SqlDbType.Int);
            _userId                 = AddParameter<int>       ("@UserID",           SqlDbType.Int);
            _clientId               = AddParameter<int>       ("@ClientID",         SqlDbType.Int);
            _employeeId             = AddParameter<int?>      ("@EmployeeID",       SqlDbType.Int);
            _startDate              = AddParameter<DateTime?> ("@StartDate",        SqlDbType.DateTime);
            _endDate                = AddParameter<DateTime?> ("@EndDate",          SqlDbType.DateTime);
            _filterTypeId           = AddParameter<int>       ("@FilterType",       SqlDbType.Int);
            _filterId               = AddParameter<int>       ("@FilterID",         SqlDbType.Int);
            _employeeStatusFilter   = AddParameter<int>       ("@EEStatusFilter",   SqlDbType.Int);
            _payFrequencyId         = AddParameter<int?>       ("@PayFrequencyID",   SqlDbType.Int);
        }
    }
}