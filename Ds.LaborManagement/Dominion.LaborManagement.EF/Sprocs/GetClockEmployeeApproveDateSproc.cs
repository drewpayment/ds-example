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
    internal class GetClockEmployeeApproveDateSproc : SprocBase<IEnumerable<GetClockEmployeeApproveDateDto>, GetClockEmployeeApproveDateArgsDto>
    {
        #region Constructors And Initializers

        public GetClockEmployeeApproveDateSproc(string connStr, GetClockEmployeeApproveDateArgsDto args)
            : base(connStr, args)
        {
        }

        #endregion

        #region Methods

        public override IEnumerable<GetClockEmployeeApproveDateDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockEmployeeApproveDate]", reader =>

            {
                var records = new List<GetClockEmployeeApproveDateDto>();

                while (reader.Read())
                {

                    var record = new GetClockEmployeeApproveDateDto()
                    {
                        EmployeeID = Convert.ToInt32(reader["employeeID"]),
                        Eventdate = Convert.ToDateTime(reader["eventdate"]),
                        IsApproved = Convert.ToBoolean(reader["IsApproved"]),
                        ClientCostCenterID = reader["ClientCostCenterID"] as int?,
                        ClientEarningID = reader["ClientEarningID"] as int?,
                        PayToSchedule = Convert.ToBoolean(reader["PayToSchedule"]),
                        ClockClientNoteID = reader["ClockClientNOteID"] as int?,
                        Note = Convert.ToString(reader["Note"]),
                        ApprovingUser = Convert.ToString(reader["ApprovingUser"])

                    };
                    records.Add(record);

                }

                return records;
            }

            );

        }
        #endregion
    }
}
