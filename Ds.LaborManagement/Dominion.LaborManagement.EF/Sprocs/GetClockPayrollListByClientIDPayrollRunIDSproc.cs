using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Sprocs
{
    internal class GetClockPayrollListByClientIDPayrollRunIDSproc : SprocBase<IEnumerable<GetClockPayrollListByClientIDPayrollRunIDDto>, GetClockPayrollListByClientIDPayrollRunIDArgsDto>
    {
        #region Constructors And Initializers

        public GetClockPayrollListByClientIDPayrollRunIDSproc(string connStr, GetClockPayrollListByClientIDPayrollRunIDArgsDto args)
            : base(connStr, args)
        {
        }

        #endregion

        #region Methods

        public override IEnumerable<GetClockPayrollListByClientIDPayrollRunIDDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockPayrollListByClientIDPayrollRunID]", reader =>
            {
                var records = new List<GetClockPayrollListByClientIDPayrollRunIDDto>();
                while (reader.Read())
                {
                    var record = new GetClockPayrollListByClientIDPayrollRunIDDto()
                    {
                        PayrollId = Convert.ToInt32(reader["PayrollId"]),
                        Displayorder = Convert.ToInt16(reader["DisplayOrder"]),
                        checkdate = Convert.ToString(reader["CheckDate"]),
                        checkdateorder = Convert.ToString(reader["CheckDateOrder"]),
                        PayPeriod = Convert.ToString(reader["PayPeriod"])
                    };
                    records.Add(record);
                }
                return records;
            });
        }
        #endregion
    }
}
