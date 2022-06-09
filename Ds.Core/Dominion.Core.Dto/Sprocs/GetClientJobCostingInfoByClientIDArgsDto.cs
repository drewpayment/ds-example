using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClientJobCostingInfoByClientIDArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int>      _clientID;

        public int ClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }

        public GetClientJobCostingInfoByClientIDArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);

        }
    }
}
