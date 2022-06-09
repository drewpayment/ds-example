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
    internal class GetClientJobCostingInfoByClientIDSproc : SprocBase<GetClientJobCostingInfoByClientIDResultsDto, GetClientJobCostingInfoByClientIDArgsDto>
    {
        #region Constructors And Initializers

        public GetClientJobCostingInfoByClientIDSproc(string connStr, GetClientJobCostingInfoByClientIDArgsDto args)
            : base(connStr, args)
        {
        }
        #endregion

        #region Methods

        public override GetClientJobCostingInfoByClientIDResultsDto Execute()
        {
            return ExecuteSproc("[dbo].[spGetClientJobCostingInfoByClientID]", reader =>

            {


                var records = new List<GetClientJobCostingInfoByClientIDDto>();
                var result = new GetClientJobCostingInfoByClientIDResultsDto()
                {
                    results1 = new List<GetClientJobCostingInfoByClientIDDto.table1>(),
                    results2 = new List<GetClientJobCostingInfoByClientIDDto.table2>(),
                    results3 = new List<GetClientJobCostingInfoByClientIDDto.table3>()
                };



                while (reader.Read())
                {
                    var record = new GetClientJobCostingInfoByClientIDDto.table1()
                    {
                        ClientJobCostingID = Convert.ToInt32(reader["ClientJobCostingID"]),
                        ClientID = Convert.ToInt32(reader["ClientID"]),
                        Description = Convert.ToString(reader["Description"]),
                        JobCostingTypeID = Convert.ToInt32(reader["JobCostingTypeID"]),
                        Sequence = Convert.ToInt32(reader["Sequence"]),
                        ModifiedBy = Convert.ToInt32(reader["ModifiedBy"]),
                        isEnabled = Convert.ToBoolean(reader["isEnabled"]),
                        HideOnScreen = Convert.ToBoolean(reader["HideOnScreen"]),
                        IsPostBack = Convert.ToBoolean(reader["IsPostBack"]),
                        Width = Convert.ToInt32(reader["Width"]),
                        Level = Convert.ToInt32(reader["Level"]),
                        IsRequired = Convert.ToBoolean(reader["IsRequired"])
                    };
                    result.results1.Add(record);

                }

                reader.NextResult();
                while (reader.Read())
                {

                    var record = new GetClientJobCostingInfoByClientIDDto.table2()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        JobCostID = Convert.ToInt32(reader["JobCostID"]),
                        Description = Convert.ToString(reader["Description"]),
                        ForeignKeyID = reader["ForeignKeyID"] as int?
                    };
                    result.results2.Add(record);

                }

                reader.NextResult();
                while (reader.Read())
                {

                    var record = new GetClientJobCostingInfoByClientIDDto.table3()
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        Description = Convert.ToString(reader["Description"]),
                        JobCostingTypeID = Convert.ToInt32(reader["JobCostingTypeID"]),
                    };
                    result.results3.Add(record);

                }
 
                return result;

            }

            );

        }

    }
    #endregion
}
