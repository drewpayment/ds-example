using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Sprocs
{
    public class GetClockEmployeeApproveHoursOptionsArgsDto : SprocParametersBase
    {
        private readonly SqlParameterBuilder<int>       _controlID;


        public int ControlID
        {
            get { return _controlID.Value; }
            set { _controlID.Value = value; }
        }

        public GetClockEmployeeApproveHoursOptionsArgsDto()
        {
            _controlID = AddParameter<int>("@ControlID", SqlDbType.Int);
        }

    }
}
