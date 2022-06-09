using System.Data;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.W2
{
    public class Employee1099CountDto
    {
        public int ClientId { get; set; }
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> _clientId;
            private readonly SqlParameterBuilder<int> _employeeId;
            private readonly SqlParameterBuilder<int> _year;

            public int ClientId
            {
                get => _clientId.Value;
                set => _clientId.Value = value;
            }

            public int EmployeeId
            {
                get => _employeeId.Value;
                set => _employeeId.Value = value;
            }

            public int Year
            {
                get => _year.Value;
                set => _year.Value = value;
            }

            public Args()
            {
                _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
                _employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                _year = AddParameter<int>("@Year", SqlDbType.Int);
            }
        }
    }
}
