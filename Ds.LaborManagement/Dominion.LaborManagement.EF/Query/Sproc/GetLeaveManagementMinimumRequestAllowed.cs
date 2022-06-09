using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.LeaveManagement;
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
    public class GetLeaveManagementMinimumRequestAllowed : SprocBase<LeaveManagementMinimumHoursResultDto, GetLeaveManagementMinimumRequestAllowed.Args>
    {
        public GetLeaveManagementMinimumRequestAllowed(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override LeaveManagementMinimumHoursResultDto Execute()
        {
            return ExecuteSproc("[dbo].[spGetLeaveManagementMinimumRequestAllowed]", dr =>
            {
                var result = dr.AsEnumerable<ResultDto>();
                return Mapper.Instance.Map(result.FirstOrDefault());
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> clientAccrualId;
            private readonly SqlParameterBuilder<int> clientId;

            public int ClientAccrualId
            {
                get { return clientAccrualId.Value; }
                set { clientAccrualId.Value = value; }
            }

            public int ClientId
            {
                get { return clientId.Value; }
                set { clientId.Value = value; }
            }

            public Args()
            {
                clientAccrualId = AddParameter<int>("@ClientAccrualId", SqlDbType.Int);
                clientId = AddParameter<int>("@ClientId", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, LeaveManagementMinimumHoursResultDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, LeaveManagementMinimumHoursResultDto>> MapExpression
            {
                get
                {
                    return x => new LeaveManagementMinimumHoursResultDto
                    {
                        LMMinAllowedBalance = x.LMMinAllowedBalance
                    };
                }
            }
        }

        internal class ResultDto
        {
            public decimal LMMinAllowedBalance { get; set; }
        }
    }
}
