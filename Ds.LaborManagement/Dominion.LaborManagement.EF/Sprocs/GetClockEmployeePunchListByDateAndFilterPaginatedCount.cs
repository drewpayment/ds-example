using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.EF.Sprocs
{
    internal class GetClockEmployeePunchListByDateAndFilterPaginatedCount : SprocBase<EmployeePunchListCountAndResultLengthDto, ClockEmployeePunchListByDateAndFilterPaginatedCountArgs>
    {
        public GetClockEmployeePunchListByDateAndFilterPaginatedCount(string connStr, ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args) : base(connStr, args)
        {
        }

        public override EmployeePunchListCountAndResultLengthDto Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockEmployeePunchListByDateAndFilterPaginatedCount]", reader =>
            {
                var result = new EmployeePunchListCountAndResultLengthDto();
                while (reader.Read())
                {
                    result.EmployeeCount = Convert.ToInt32(DBNull.Value.Equals(reader["EmployeeCount"]) ? 0 : reader["EmployeeCount"]);
                    result.TotalPages = Convert.ToInt32(DBNull.Value.Equals(reader["PageCount"]) ? 0 : reader["PageCount"]);
                }
                return result;
            });
        }
    }

}
