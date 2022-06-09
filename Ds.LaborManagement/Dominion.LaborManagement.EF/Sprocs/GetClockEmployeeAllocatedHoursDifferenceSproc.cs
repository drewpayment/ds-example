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
    public class GetClockEmployeeAllocatedHoursDifferenceSproc : SprocBase<IEnumerable<GetClockEmployeeAllocatedHoursDifferenceDto>, GetClockEmployeeAllocatedHoursDifferenceArgsDto>
    {
        public GetClockEmployeeAllocatedHoursDifferenceSproc(string connStr, GetClockEmployeeAllocatedHoursDifferenceArgsDto args) : base(connStr, args)
        {

        }

        public override IEnumerable<GetClockEmployeeAllocatedHoursDifferenceDto> Execute()
        {
            return ExecuteSproc("spGetClockEmployeeAllocatedHoursDifference", reader =>
            {
                var records = new List<GetClockEmployeeAllocatedHoursDifferenceDto>();
                while (reader.Read())
                {
                    var record = new GetClockEmployeeAllocatedHoursDifferenceDto()
                    {
                        AllocatedHoursDifference = Convert.ToInt32(reader["AllocatedHoursDifference"]),
                        EmployeeId = Convert.ToInt32(reader["employeeid"]),
                        EventDate = Convert.ToDateTime(reader["eventdate"])
                    };
                    records.Add(record);
                }
                    return records;
            });
        }
    }
}
