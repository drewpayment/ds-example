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
    internal class GetClockFilterIDsSproc : SprocBase<IEnumerable<GetClockFilterIdsDto>, GetClockFilterIdsArgsDto>
    {
        public GetClockFilterIDsSproc(string connStr, GetClockFilterIdsArgsDto args) : base(connStr, args)
        {

        }
        public override IEnumerable<GetClockFilterIdsDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockFilterIDs]", reader =>
            {
                var records = new List<GetClockFilterIdsDto>();

                while (reader.Read())
                {
                    var record = new GetClockFilterIdsDto()
                    {
                        Filter = Convert.ToString(reader["FILTER"]),
                        Id = Convert.ToInt32(reader["ID"])
                    };

                    records.Add(record);
                }

                return records;
                
            });
        }
    }
}
