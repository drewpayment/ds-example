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
    public class GetLeaveManagementHolidays : SprocBase<IEnumerable<GetLeaveManagementHolidaysResultDto>, GetLeaveManagementHolidays.Args>
    {
        public GetLeaveManagementHolidays(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override IEnumerable<GetLeaveManagementHolidaysResultDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetLeaveManagementHolidays]", dr =>
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
            private readonly SqlParameterBuilder<int> policyId;

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

            public int PolicyId
            {
                get { return policyId.Value; }
                set { policyId.Value = value; }
            }

            public Args()
            {
                employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                from = AddParameter<DateTime>("@From", SqlDbType.DateTime2);
                until = AddParameter<DateTime>("@Until", SqlDbType.DateTime2);
                policyId = AddParameter<int>("@PolicyID", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, GetLeaveManagementHolidaysResultDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, GetLeaveManagementHolidaysResultDto>> MapExpression
            {
                get
                {
                    return x => new GetLeaveManagementHolidaysResultDto
                    {
                        HolidayName = x.HolidayName,
                        HolidayDate = x.HolidayDate,
                        HolidayHours = x.HolidayHours,
                        TotalHoursAvailable = x.TotalHoursAvailable
                    };
                }
            }
        }

        internal class ResultDto
        {
            public string HolidayName { get; set; }
            public DateTime HolidayDate { get; set; }
            public decimal HolidayHours { get; set; }
            public decimal TotalHoursAvailable { get; set; }
        }
    }
}
