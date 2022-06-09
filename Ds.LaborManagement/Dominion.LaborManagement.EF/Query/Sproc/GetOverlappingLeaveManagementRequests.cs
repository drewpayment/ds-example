using Dominion.Utility.Query;
using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dominion.Utility.Mapping;
using System.Linq.Expressions;
using Dominion.Utility.ExtensionMethods;
using Dominion.Core.Dto.LeaveManagement;

namespace Dominion.LaborManagement.EF.Query.Sproc
{
    public class GetOverlappingLeaveManagementRequests : SprocBase<IEnumerable<TimeOffRequestDto>, GetOverlappingLeaveManagementRequests.Args>
    {
        public GetOverlappingLeaveManagementRequests(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override IEnumerable<TimeOffRequestDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetOverlapingEmployeeLeaveManagementListByEmployeeIDNonCanceled]", dr =>
            {
                var results = dr.AsEnumerable<ResultDto>().ToArray();
                return Mapper.Instance.Map(results);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> employeeId;
            private readonly SqlParameterBuilder<DateTime> from;
            private readonly SqlParameterBuilder<DateTime> until;
            private readonly SqlParameterBuilder<int> requestTimeOffId;
            private readonly SqlParameterBuilder<decimal> hours;
            private readonly SqlParameterBuilder<int> clientAccrualId;

            public int EmployeeId
            {
                get { return employeeId.Value; }
                set { employeeId.Value = value; }
            }

            public DateTime From
            {
                get { return from.Value; }
                set { from.Value = value; }
            }

            public DateTime Until
            {
                get { return until.Value; }
                set { until.Value = value; }
            }

            public int RequestTimeOffId
            {
                get { return requestTimeOffId.Value; }
                set { requestTimeOffId.Value = value; }
            }

            public decimal Hours
            {
                get { return hours.Value; }
                set { hours.Value = value; }
            }

            public int ClientAccrualId
            {
                get { return clientAccrualId.Value; }
                set { clientAccrualId.Value = value; }
            }

            public Args()
            {
                employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                from = AddParameter<DateTime>("@From", SqlDbType.DateTime2);
                until = AddParameter<DateTime>("@Until", SqlDbType.DateTime2);
                requestTimeOffId = AddParameter<int>("@RequestTimeOffID", SqlDbType.Int);
                hours = AddParameter<decimal>("@Hours", SqlDbType.Float);
                clientAccrualId = AddParameter<int>("@ClientAccrualID", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, TimeOffRequestDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, TimeOffRequestDto>> MapExpression
            {
                get
                {
                    return x => new TimeOffRequestDto
                    {
                        TimeOffRequestId = x.RequestTimeOffId,
                        ClientAccrualId = x.ClientAccrualId,
                        EmployeeId = x.EmployeeId,
                        RequestFrom = x.RequestFrom,
                        RequestUntil = x.RequestUntill,
                        Status = x.Status, 
                        Hours = x.Hours
                    };
                }
            }
        }

        internal class ResultDto
        {
            public int RequestTimeOffId { get; set; }
            public int ClientAccrualId { get; set; }
            public int EmployeeId { get; set; }
            public DateTime RequestFrom { get; set; }
            public DateTime RequestUntill { get; set; }
            public TimeOffStatusType Status { get; set; }
            public int Hours { get; set; }
        }
    }
}
