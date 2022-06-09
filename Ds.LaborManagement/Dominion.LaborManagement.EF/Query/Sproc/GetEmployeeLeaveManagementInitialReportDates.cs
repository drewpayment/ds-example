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
    public class GetEmployeeLeaveManagementInitialReportDates : SprocBase<EmployeeLeaveManagementLastPayrollDto, GetEmployeeLeaveManagementInitialReportDates.Args>
    {
        public GetEmployeeLeaveManagementInitialReportDates(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override EmployeeLeaveManagementLastPayrollDto Execute()
        {
            return ExecuteSproc("[dbo].[spGetEmployeeLeaveManagementInitialReportDates]", dr =>
            {
                var result = dr.AsEnumerable<ResultDto>().FirstOrDefault();
                return Mapper.Instance.Map(result);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> clientId;
            private readonly SqlParameterBuilder<int> payFrequencyId;

            public int ClientId
            {
                get { return clientId.Value; }
                set { clientId.Value = value; }
            }

            public int PayFrequencyId
            {
                get { return payFrequencyId.Value; }
                set { payFrequencyId.Value = value; }
            }

            public Args()
            {
                clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
                payFrequencyId = AddParameter<int>("@PayFrequencyID", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, EmployeeLeaveManagementLastPayrollDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, EmployeeLeaveManagementLastPayrollDto>> MapExpression
            {
                get
                {
                    return x => new EmployeeLeaveManagementLastPayrollDto
                    {
                        LastPayrollDate = x.LastPayrollDate,
                        YearBefore = x.YearBefore
                    };
                }
            }
        }

        internal class ResultDto
        {
            public DateTime LastPayrollDate { get; set; }
            public DateTime YearBefore { get; set; }
        }
    }

}
