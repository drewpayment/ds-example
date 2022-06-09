using Dominion.Utility.Query;
using System.Data;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetPayrollListByClientIDPayrollRunIDArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientId;
        private readonly SqlParameterBuilder<int> _payrollRunId;
        private readonly SqlParameterBuilder<int> _w2Year;

        public int ClientID
        {
            get { return _clientId.Value; }
            set { _clientId.Value = value; }
        }
        public int PayrollRunID
        {
            get { return _payrollRunId.Value; }
            set { _payrollRunId.Value = value; }
        }
        public int W2Year
        {
            get { return _w2Year.Value; }
            set { _w2Year.Value = value; }
        }

        public GetPayrollListByClientIDPayrollRunIDArgsDto()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _payrollRunId = AddParameter<int>("@PayrollRunID", SqlDbType.Int);
            _w2Year = AddParameter<int>("@W2Year", SqlDbType.Int);
        }
    }
}