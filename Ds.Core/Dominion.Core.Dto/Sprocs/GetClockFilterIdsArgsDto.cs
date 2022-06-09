using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockFilterIdsArgsDto : SprocParametersBase
    {

        private readonly SqlParameterBuilder<int> _clientID;
        private readonly SqlParameterBuilder<int> _clockFilterID;

        public int ClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }
        public int ClockFilterID
        {
            get { return _clockFilterID.Value; }
            set { _clockFilterID.Value = value; }
        }

        public GetClockFilterIdsArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);
            _clockFilterID = AddParameter<int>("@ClockFilterID", SqlDbType.Int);

        }
    }
}
