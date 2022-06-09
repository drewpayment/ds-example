using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Sprocs
{
    internal class GetClockEmployeeApproveHoursSettingsSproc : SprocBase<IEnumerable<GetClockEmployeeApproveHoursSettingsDto>, GetClockEmployeeApproveHoursSettingsArgsDto>
    {
        #region Constructors And Initializers

        public GetClockEmployeeApproveHoursSettingsSproc(string connStr, GetClockEmployeeApproveHoursSettingsArgsDto args)
            : base(connStr, args)
        {
        }

        #endregion

        #region Methods

        public override IEnumerable<GetClockEmployeeApproveHoursSettingsDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockEmployeeApproveHoursSettings]", reader =>

            {


                var records = new List<GetClockEmployeeApproveHoursSettingsDto>();

                while (reader.Read())
                {

                    var record = new GetClockEmployeeApproveHoursSettingsDto()
                    {
                        ClockEmployeeApproveHoursSettingsID = Convert.ToInt32(reader["ClockEmployeeApproveHoursSettingsID"]),
                        ClientId = Convert.ToInt32(reader["ClientID"]),
                        UserId = Convert.ToInt32(reader["UserID"]),
                        HideWeeklyTotals = Convert.ToBoolean(reader["HideWeeklyTotals"]),
                        HideGrandTotals = Convert.ToBoolean(reader["HideGrandTotals"]),
                        HideNoActivity = Convert.ToBoolean(reader["HideNoActivity"]),
                        HideActivity = Convert.ToBoolean(reader["HideActivity"]),
                        HideDailyTotals = Convert.ToBoolean(reader["HideDailyTotals"]),
                        ShowAllDays = Convert.ToBoolean(reader["ShowAllDays"]),
                        DefaultDaysFilter = reader["DefaultDaysFilter"] as int?, //== DBNull.Value ? 1 : Convert.ToInt16(reader["DefaultDaysFilter"]), // Convert.ToInt16(reader["DefaultDaysFilter"]),
                        EmployeesPerPage = reader["EmployeesPerPage"] as int?   //== DBNull.Value ? 10 : Convert.ToInt16(reader["EmployeesPerPage"])  //Convert.ToInt16(reader["EmployeesPerPage"])
                    };
                    records.Add(record);

                }

                return records;

            }

            );

        }
        //public override int Execute()
        //{

        //    var dto = ExecuteSprocBase("[dbo].[spGetClockPayrollListByClientIDPayrollRunID]");

        //    return dto;
        //}


        #endregion
    }
}
