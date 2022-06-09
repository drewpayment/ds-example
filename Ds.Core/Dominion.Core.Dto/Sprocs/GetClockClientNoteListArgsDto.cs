using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockClientNoteListArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int> _clientID;

        public int ClientID
        {
            get { return _clientID.Value; }
            set { _clientID.Value = value; }
        }

        public GetClockClientNoteListArgsDto()
        {
            _clientID = AddParameter<int>("@ClientID", SqlDbType.Int);
        }
    }
}
