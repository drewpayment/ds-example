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
    public class InsertRequestTimeOff : SprocBase<InsertRequestTimeOffResultDto, InsertRequestTimeOff.Args>
    {
        public InsertRequestTimeOff(string connectionString, Args args) : base(connectionString, args)
        { }

        public override InsertRequestTimeOffResultDto Execute()
        {
            return ExecuteSproc("[dbo].[spInsertRequestTimeOff]", dr =>
            {
                var result = dr.GetValue(0).ToString();
                return Mapper.Instance.Map(int.Parse(result));
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> clientAccrualId;
            private readonly SqlParameterBuilder<int> employeeId;
            private readonly SqlParameterBuilder<DateTime> requestFrom;
            private readonly SqlParameterBuilder<DateTime> requestUntil;
            private readonly SqlParameterBuilder<int> modifiedBy;
            private readonly SqlParameterBuilder<int> amountInOneDay;
            private readonly SqlParameterBuilder<string> requestorNotes;

            public int ClientAccrualId
            {
                get { return clientAccrualId.Value; }
                set { clientAccrualId.Value = value; }
            }

            public int EmployeeId
            {
                get { return employeeId.Value; }
                set { employeeId.Value = value; }
            }

            public DateTime RequestFrom
            {
                get { return requestFrom.Value; }
                set { requestFrom.Value = value; }
            }

            public DateTime RequestUntil
            {
                get { return requestUntil.Value; }
                set { requestUntil.Value = value; }
            }

            public int ModifiedBy
            {
                get { return modifiedBy.Value; }
                set { modifiedBy.Value = value; }
            }

            public int AmountInOneDay
            {
                get { return amountInOneDay.Value; }
                set { amountInOneDay.Value = value; }
            }

            public string RequestorNotes
            {
                get { return requestorNotes.Value; }
                set { requestorNotes.Value = value; }
            }

            public Args()
            {
                clientAccrualId = AddParameter<int>("@ClientAccrualPlanID", SqlDbType.Int);
                employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                requestFrom = AddParameter<DateTime>("@RequestFrom", SqlDbType.DateTime2);
                requestUntil = AddParameter<DateTime>("@RequestUntill", SqlDbType.DateTime2);
                modifiedBy = AddParameter<int>("@ModifiedBy", SqlDbType.Int);
                amountInOneDay = AddParameter<int>("@AmountInOneDay", SqlDbType.Int);
                requestorNotes = AddParameter<string>("@RequesterNotes", SqlDbType.VarChar);
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
