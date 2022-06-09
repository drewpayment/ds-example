using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query.JobCosting
{
    public class GetClientJobCostingIdWithPostBackSproc : SprocBase<IEnumerable<ClientJobCostingIdDto>, GetClientJobCostingIdWithPostBackSproc.Args>
    {
        public GetClientJobCostingIdWithPostBackSproc(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override IEnumerable<ClientJobCostingIdDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetClientJobCostingIDWithPostBack]", dr =>
            {
                var results = dr.AsEnumerable<ResultDto>().ToArray();
                return Mapper.Instance.Map(results);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> clientJobCostingId;

            public int ClientJobCostingId
            {
                get { return clientJobCostingId.Value; }
                set { clientJobCostingId.Value = value; }
            }

            public Args()
            {
                clientJobCostingId = AddParameter<int>("@ClientJobCostingID", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, ClientJobCostingIdDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, ClientJobCostingIdDto>> MapExpression
            {
                get
                {
                    return x => new ClientJobCostingIdDto()
                    {
                        ClientJobCostingId = x.ClientJobCostingID
                    };
                }
            }
        }

        /// <summary>
        /// DTO with a 1-to-1 Mapping with the sproc result.  Column names match result set
        /// exactly how they appear from the sproc
        /// </summary>
        internal class ResultDto
        {
            public int ClientJobCostingID { get; set; }
        }

    }
}
