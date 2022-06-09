using Dominion.Core.Dto.Labor;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.EF.Query.Sproc
{
    public class InsertRequestTimeOffDetail : SprocBase<InsertRequestTimeOffResultDto, InsertRequestTimeOffDetail.Args>
    {
        public InsertRequestTimeOffDetail(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override InsertRequestTimeOffResultDto Execute()
        {
            return ExecuteSproc("[dbo].[spInsertRequestTimeOffDetail]", dr =>
            {
                var result = dr.GetInt32(0);
                return Mapper.Instance.Map(result);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> requestTimeOffId;
            private readonly SqlParameterBuilder<DateTime> requestFrom;
            private readonly SqlParameterBuilder<int> modifiedBy;
            private readonly SqlParameterBuilder<decimal> amountInOneDay;
            private readonly SqlParameterBuilder<DateTime> fromTime;
            private readonly SqlParameterBuilder<DateTime> toTime;

            public int RequestTimeOffId
            {
                get { return requestTimeOffId.Value; }
                set { requestTimeOffId.Value = value; }
            }

            public DateTime RequestFrom
            {
                get { return requestFrom.Value; }
                set { requestFrom.Value = value; }
            }

            public int ModifiedBy
            {
                get { return modifiedBy.Value; }
                set { modifiedBy.Value = value; }
            }

            public decimal AmountInOneDay
            {
                get { return amountInOneDay.Value; }
                set { amountInOneDay.Value = value; }
            }

            public DateTime FromTime
            {
                get { return fromTime.Value; }
                set { fromTime.Value = value; }
            }

            public DateTime ToTime
            {
                get { return toTime.Value; }
                set { toTime.Value = value; }
            }

            public Args()
            {
                requestTimeOffId = AddParameter<int>("@RequestTimeOffID", SqlDbType.Int);
                requestFrom = AddParameter<DateTime>("@RequestFrom", SqlDbType.DateTime2);
                modifiedBy = AddParameter<int>("@ModifiedBy", SqlDbType.Int);
                amountInOneDay = AddParameter<decimal>("@AmountInOneDay", SqlDbType.Float);
                fromTime = AddParameter<DateTime>("@FromTime", SqlDbType.DateTime2);
                toTime = AddParameter<DateTime>("@ToTime", SqlDbType.DateTime2);
            }
        }

        internal class Mapper : ExpressionMapper<int, InsertRequestTimeOffResultDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<int, InsertRequestTimeOffResultDto>> MapExpression
            {
                get
                {
                    return x => new InsertRequestTimeOffResultDto
                    {
                        RequestTimeOffId = x
                    };
                }
            }
        }
    }
}
