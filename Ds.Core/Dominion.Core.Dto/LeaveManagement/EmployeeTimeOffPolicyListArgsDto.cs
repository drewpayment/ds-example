using System;
using System.Data;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class EmployeeTimeOffPolicyListArgsDto : SprocParametersBase
    {
        private readonly  SqlParameterBuilder<int>       _clientId;
        private readonly  SqlParameterBuilder<int?>      _employeeId;

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

        public EmployeeTimeOffPolicyListArgsDto()
        {
            _clientId     = AddParameter<int>       ("@ClientID",   SqlDbType.Int);
            _employeeId   = AddParameter<int?>      ("@EmployeeID", SqlDbType.Int);
        }
    }
}