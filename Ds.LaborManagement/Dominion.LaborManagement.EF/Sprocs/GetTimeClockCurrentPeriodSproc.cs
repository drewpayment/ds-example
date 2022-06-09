using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.EF.Sprocs
{
    public class GetTimeClockCurrentPeriodSproc : SprocBase<GetTimeClockCurrentPeriodDto, GetTimeClockCurrentPeriodArgsDto>
    {
        public GetTimeClockCurrentPeriodSproc(string connStr, GetTimeClockCurrentPeriodArgsDto args) : base(connStr, args)
        {

        }

        public override GetTimeClockCurrentPeriodDto Execute()
        {
            return ExecuteSproc("[dbo].[spGetTimeClockCurrentPeriod]", reader =>
            {
                reader.Read();
                return new GetTimeClockCurrentPeriodDto
                {
                    StartDate = Convert.ToDateTime(reader["StartDate"]),
                    EndDate = Convert.ToDateTime(reader["EndDate"])
                };
            });
        }
    }
}
