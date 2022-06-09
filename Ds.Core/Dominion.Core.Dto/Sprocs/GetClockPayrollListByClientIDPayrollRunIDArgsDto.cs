using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockPayrollListByClientIDPayrollRunIDArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientId;
        private readonly SqlParameterBuilder<int> _payrollRunId;
        private readonly SqlParameterBuilder<bool> _hideCustomDateRange;


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
        public bool HideCustomDateRange
        {
            get { return _hideCustomDateRange.Value; }
            set { _hideCustomDateRange.Value = value; }
        }

        public GetClockPayrollListByClientIDPayrollRunIDArgsDto()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _payrollRunId = AddParameter<int>("@PayrollRunID", SqlDbType.Int);
            _hideCustomDateRange = AddParameter<bool>("@HideCustomDateRange", SqlDbType.Bit);
        }

    }
}
