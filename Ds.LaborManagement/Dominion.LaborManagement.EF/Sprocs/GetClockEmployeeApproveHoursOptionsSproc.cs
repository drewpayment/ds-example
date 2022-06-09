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
    internal class GetClockEmployeeApproveHoursOptionsSproc : SprocBase<IEnumerable<GetClockEmployeeApproveHoursOptionsDto>, GetClockEmployeeApproveHoursOptionsArgsDto>
    {
        #region Constructors And Initializers

        public GetClockEmployeeApproveHoursOptionsSproc(string connStr, GetClockEmployeeApproveHoursOptionsArgsDto args)
            : base(connStr, args)
        {
        }

        #endregion

        #region Methods

        public override IEnumerable<GetClockEmployeeApproveHoursOptionsDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockEmployeeApproveHoursOptions]", reader =>

                {


                    var records = new List<GetClockEmployeeApproveHoursOptionsDto>();

                    while (reader.Read())
                    {

                        var record = new GetClockEmployeeApproveHoursOptionsDto()
                        {
                            Label = Convert.ToString(reader["Label"]),
                            Value = Convert.ToInt16(reader["value"])
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
