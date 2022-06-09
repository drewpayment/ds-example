using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeApproveHoursSettingsArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int>       _clientId;
        private readonly SqlParameterBuilder<int>       _userId;


        public int ClientID
        {
            get { return _clientId.Value; }
            set { _clientId.Value = value; }
        }
        public int UserID
        {
            get { return _userId.Value; }
            set { _userId.Value = value; }
        }

        public GetClockEmployeeApproveHoursSettingsArgsDto()
        {
            _clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            _userId = AddParameter<int>("@UserID", SqlDbType.Int);
        }

    }
}
