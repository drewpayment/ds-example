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
    internal class GetClockEmployeePunchListByDateAndFilterSproc: SprocBase<IEnumerable<GetClockEmployeePunchListByDateAndFilterDto>, GetClockEmployeePunchListByDateAndFilterArgsDto>
    {
        public GetClockEmployeePunchListByDateAndFilterSproc(string connStr, GetClockEmployeePunchListByDateAndFilterArgsDto args)
            : base(connStr, args)
        {

        }

        public override IEnumerable<GetClockEmployeePunchListByDateAndFilterDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockEmployeePunchListByDateAndFilter]", reader =>
            {
                var records = new List<GetClockEmployeePunchListByDateAndFilterDto>();
                while (reader.Read())
                {
                    var record = new GetClockEmployeePunchListByDateAndFilterDto()
                    {
                        ClockClientTimePolicyID = Convert.ToInt32(DBNull.Value.Equals(reader["ClockClientTimePolicyID"]) ? null : reader["ClockClientTimePolicyID"]),
                        ClockClientTimePolicyName = Convert.ToString(DBNull.Value.Equals(reader["ClockClientTimePolicyName"]) ? null : reader["ClockClientTimePolicyName"]),
                        EmployeeHireDate = Convert.ToDateTime(reader["EmployeeHireDate"]),
                        EmployeeId = Convert.ToInt32(reader["EmployeeID"]),
                        EmployeeName = Convert.ToString(reader["EmployeeName"]),
                        EmployeeNumber = Convert.ToString(reader["EmployeeNumber"]),
                        EmployeeRehireDate = Convert.ToDateTime(reader["EmployeeRehireDate"]),
                        EmployeeSeparationDate = Convert.ToDateTime(reader["EmployeeSeparationDate"])
                    };
                    records.Add(record);
                }

                return records;
            });

        }
    }
}
