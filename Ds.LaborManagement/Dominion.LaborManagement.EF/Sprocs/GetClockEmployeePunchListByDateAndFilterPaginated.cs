using Dominion.Core.Dto.TimeCard.Result;
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
    internal class GetClockEmployeePunchListByDateAndFilterPaginated : SprocBase<PunchActivitySprocResults, ClockEmployeePunchListByDateAndFilterPaginatedCountArgs>
    {
        public GetClockEmployeePunchListByDateAndFilterPaginated(string connStr, ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args) : base(connStr, args)
        {
        }

        public override PunchActivitySprocResults Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockEmployeePunchListByDateAndFilterPaginatedCount]", reader =>
            {
                var rows = new List<EmployeePunchActivityDto>();
                while (reader.Read())
                {
                    rows.Add(new EmployeePunchActivityDto
                    {
                        EmployeeId = Convert.ToInt32(DBNull.Value.Equals(reader["EmployeeID"]) ? 0 : reader["EmployeeID"]),
                        EmployeeName = Convert.ToString(reader["EmployeeName"]),
                        EmployeeNumber = Convert.ToString(reader["EmployeeNumber"]),
                        ClockEmployeePunchId = Convert.ToInt32(DBNull.Value.Equals(reader["ClockEmployeePunchId"]) ? 0 : reader["ClockEmployeePunchId"]),
                        ClockClientLunchId = Convert.ToInt32(DBNull.Value.Equals(reader["ClockClientLunchId"]) ? 0 : reader["ClockClientLunchId"]),
                        DateOfPunch = Convert.ToDateTime(DBNull.Value.Equals(reader["DateOfPunch"]) ? default(DateTime) : reader["DateOfPunch"]),
                        ShiftDateTime = Convert.ToString(reader["ShiftDateTime"]),
                        ShiftDateString = Convert.ToString(reader["ShiftDateString"]),
                        OriginalShiftDate = Convert.ToDateTime(DBNull.Value.Equals(reader["OriginalShiftDate"]) ? default(DateTime) : reader["OriginalShiftDate"]),
                        Comment = Convert.ToString(reader["Comment"]),
                        ClockName = Convert.ToString(reader["ClockName"]),
                        TimeZoneId = Convert.ToInt32(DBNull.Value.Equals(reader["TimeZoneId"]) ? 0 : reader["TimeZoneId"]),
                        ClientCostCenterId = Convert.ToInt32(DBNull.Value.Equals(reader["ClientCostCenterId"]) ? 0 : reader["ClientCostCenterId"]),
                        IsPendingBenefit = Convert.ToBoolean(reader["IsPendingBenefit"]),
                        EmployeeComment = Convert.ToString(reader["EmployeeComment"]),
                        ClientDepartmentId = Convert.ToInt32(DBNull.Value.Equals(reader["ClientDepartmentId"]) ? 0 : reader["ClientDepartmentId"]),
                        ClientDivisionId = Convert.ToInt32(DBNull.Value.Equals(reader["ClientDivisionId"]) ? 0 : reader["ClientDivisionid"]),
                        ClientJobCostingAssignmentId1 = Convert.ToInt32(DBNull.Value.Equals(reader["ClientJobCostingAssignmentID_1"]) ? 0 : reader["ClientJobCostingAssignmentID_1"]),
                        ClientJobCostingAssignmentId2 = Convert.ToInt32(DBNull.Value.Equals(reader["ClientJobCostingAssignmentID_2"]) ? 0 : reader["ClientJobCostingAssignmentID_2"]),
                        ClientJobCostingAssignmentId3 = Convert.ToInt32(DBNull.Value.Equals(reader["ClientJobCostingAssignmentID_3"]) ? 0 : reader["ClientJobCostingAssignmentID_3"]),
                        ClientJobCostingAssignmentId4 = Convert.ToInt32(DBNull.Value.Equals(reader["ClientJobCostingAssignmentID_4"]) ? 0 : reader["ClientJobCostingAssignmentID_4"]),
                        ClientJobCostingAssignmentId5 = Convert.ToInt32(DBNull.Value.Equals(reader["ClientJobCostingAssignmentID_5"]) ? 0 : reader["ClientJobCostingAssignmentID_5"]),
                        ClientJobCostingAssignmentId6 = Convert.ToInt32(DBNull.Value.Equals(reader["ClientJobCostingAssignmentID_6"]) ? 0 : reader["ClientJobCostingAssignmentID_6"])
                    });
                }

                reader.NextResult();

                var emps = new List<PunchActivityEmployeeInfoDto>();
                while(reader.Read())
                {
                    emps.Add(new PunchActivityEmployeeInfoDto
                    {
                        EmployeeId = Convert.ToInt32(DBNull.Value.Equals(reader["EmployeeID"]) ? 0 : reader["EmployeeID"]),
                        EmployeeName = Convert.ToString(DBNull.Value.Equals(reader["EmployeeName"]) ? 0 : reader["EmployeeName"]),
                        EmployeeNumber = Convert.ToString(DBNull.Value.Equals(reader["EmployeeNumber"]) ? 0 : reader["EmployeeNumber"]),
                        EmployeeActivity = Convert.ToInt32(DBNull.Value.Equals(reader["EmployeeActivity"]) ? 0 : reader["EmployeeActivity"]),
                        EmployeeHireDate = Convert.ToDateTime(DBNull.Value.Equals(reader["EmployeeHireDate"]) ? 0 : reader["EmployeeHireDate"]),
                        EmployeeSeparationDate = Convert.ToDateTime(DBNull.Value.Equals(reader["EmployeeSeparationDate"]) ? 0 : reader["EmployeeSeparationDate"]),
                        EmployeeRehireDate = Convert.ToDateTime(DBNull.Value.Equals(reader["EmployeeRehireDate"]) ? 0 : reader["EmployeeRehireDate"]),
                        ClockClientTimePolicyId = Convert.ToInt32(DBNull.Value.Equals(reader["ClockClientTimePolicyID"]) ? 0 : reader["ClockClientTimePolicyID"]),
                        ClockClientTimePolicyName = Convert.ToString(reader["ClockClientTimePolicyName"])
                    });
                }

                return new PunchActivitySprocResults() 
                { 
                    Activity = rows,
                    Employees = emps
                };
            });
        }
    }

}
