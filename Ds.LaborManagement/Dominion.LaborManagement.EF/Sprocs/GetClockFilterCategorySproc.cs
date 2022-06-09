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
    internal class GetClockFilterCategorySproc : SprocBase<IEnumerable<GetClockFilterCategoryDto>, GetClockFilterCategoryArgsDto>
    {
        #region Constructors And Initializers

        public GetClockFilterCategorySproc(string connStr, GetClockFilterCategoryArgsDto args)
            : base(connStr, args)
        {
        }

        #endregion

        #region Methods

        public override IEnumerable<GetClockFilterCategoryDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClockFilterCategory]", reader =>

                {
                    var records = new List<GetClockFilterCategoryDto>();

                    while (reader.Read())
                    {

                        var record = new GetClockFilterCategoryDto()
                        {
                            ClockFilterID = Convert.ToInt16(reader["ClockFilterID"]),
                            Description = Convert.ToString(reader["Description"]),
                            WhereClause = Convert.ToString(reader["WhereClause"]),
                            idx = Convert.ToInt16(reader["idx"]),
                            value = Convert.ToString(reader["value"])
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
