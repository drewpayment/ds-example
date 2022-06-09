using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query.Sproc
{
    public class GetClockClientRulesByEmployeeID : SprocBase<IEnumerable<GetClockClientRulesByEmployeeIdResultDto>, GetClockClientRulesByEmployeeIdArgsDto>
    {
        public GetClockClientRulesByEmployeeID(string connectionString, GetClockClientRulesByEmployeeIdArgsDto args) : base(connectionString, args)
        {
        }

        public override IEnumerable<GetClockClientRulesByEmployeeIdResultDto> Execute() => 
            ExecuteSproc(
                "[dbo].[spGetClockClientRulesByEmployeeID]",
                dr => dr.ToObject<List<GetClockClientRulesByEmployeeIdResultDto>>());
    }
}
