using Dominion.Core.Dto.Labor;
using Dominion.Utility.ExtensionMethods;
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
    public class GetLeaveManagementEmployeeBalance : SprocBase<EmployeeLeaveManagementBalanceDto, GetLeaveManagementEmployeeBalance.Args>
    {
        public GetLeaveManagementEmployeeBalance(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override EmployeeLeaveManagementBalanceDto Execute()
        {
            return ExecuteSproc("[dbo].[spGetLeaveManagementEmployeeBalancestByEmployeeID_AccrualID]", dr =>
            {
                var result = dr.AsEnumerable<ResultDto>().FirstOrDefault();
                return Mapper.Instance.Map(result);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> employeeId;
            private readonly SqlParameterBuilder<int> clientAccrualId;
            private readonly SqlParameterBuilder<int?> requestTimeOffId;
            private readonly SqlParameterBuilder<DateTime> dateForComparison;

            public int EmployeeId
            {
                get { return employeeId.Value; }
                set { employeeId.Value = value; }
            }

            public int ClientAccrualId
            {
                get { return clientAccrualId.Value; }
                set { clientAccrualId.Value = value; }
            }

            public int? RequestTimeOffId
            {
                get { return requestTimeOffId.Value; }
                set { requestTimeOffId.Value = value; }
            }

            public DateTime DateForComparison
            {
                get { return dateForComparison.Value; }
                set { dateForComparison.Value = value; }
            }

            public Args()
            {
                employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                clientAccrualId = AddParameter<int>("@ClientAccrualID", SqlDbType.Int);
                requestTimeOffId = AddParameter<int?>("@RequestTimeOffID", SqlDbType.Int);
                dateForComparison = AddParameter<DateTime>("@DateForComparison", SqlDbType.DateTime2);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, EmployeeLeaveManagementBalanceDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, EmployeeLeaveManagementBalanceDto>> MapExpression
            {
                get
                {
                    return x => new EmployeeLeaveManagementBalanceDto
                    {
                        LastPeriodStart = x.LastPeriodStart,
                        LastPeriodEnd = x.LastPeriodEnd,
                        LastCheckDate = x.LastCheckDate,
                        CurrentBalance = x.CurrentBalance
                    };
                }
            }
        }

        internal class ResultDto
        {
            public DateTime LastPeriodStart { get; set; }
            public DateTime LastPeriodEnd { get; set; }
            public DateTime LastCheckDate { get; set; }
            public decimal CurrentBalance { get; set; }
        }
    }
}
