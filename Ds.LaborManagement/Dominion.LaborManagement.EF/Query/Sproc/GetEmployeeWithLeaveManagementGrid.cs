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
    public class GetEmployeeWithLeaveManagementGrid : SprocBase<IEnumerable<EmployeeLeaveManagementInfoDto>, GetEmployeeWithLeaveManagementGrid.Args>
    {
        public GetEmployeeWithLeaveManagementGrid(string connectionString, Args args) : base(connectionString, args)
        { }

        public override IEnumerable<EmployeeLeaveManagementInfoDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetEmployeeWithLeaveManagementGrid]", dr =>
            {
                var results = dr.AsEnumerable<EmployeeWithLeaveManagementGridDto>().ToArray();
                return Mapper.Instance.Map(results);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int?> planId;
            private readonly SqlParameterBuilder<int?> statusId;
            private readonly SqlParameterBuilder<int> userId;
            private readonly SqlParameterBuilder<int> clientId;
            private readonly SqlParameterBuilder<int?> employeeId;
            private readonly SqlParameterBuilder<DateTime?> startDate;
            private readonly SqlParameterBuilder<DateTime?> endDate;
            private readonly SqlParameterBuilder<int> filterType;
            private readonly SqlParameterBuilder<int> filterId;
            private readonly SqlParameterBuilder<int> eeStatusFilter;
            private readonly SqlParameterBuilder<bool?> requireLeaveManagement;
            private readonly SqlParameterBuilder<int> payFrequencyId;

            public int? PlanId
            {
                get { return planId.Value; }
                set { planId.Value = value; }
            }

            public int? StatusId
            {
                get { return statusId.Value; }
                set { statusId.Value = value; }
            }

            public int UserId
            {
                get { return userId.Value; }
                set { userId.Value = value; }
            }

            public int ClientId
            {
                get { return clientId.Value; }
                set { clientId.Value = value; }
            }

            public int? EmployeeId
            {
                get { return employeeId.Value; }
                set { employeeId.Value = value; }
            }

            public DateTime? StartDate
            {
                get { return startDate.Value; }
                set { startDate.Value = value; }
            }

            public DateTime? EndDate
            {
                get { return endDate.Value; }
                set { endDate.Value = value; }
            }

            public int FilterType
            {
                get { return filterType.Value; }
                set { filterType.Value = value; }
            }

            public int FilterId
            {
                get { return filterId.Value; }
                set { filterId.Value = value; }
            }

            public int EEStatusFilter
            {
                get { return eeStatusFilter.Value; }
                set { eeStatusFilter.Value = value; }
            }

            public bool? RequireLeaveManagement
            {
                get { return requireLeaveManagement.Value; }
                set { requireLeaveManagement.Value = value; }
            }

            public int PayFrequencyId
            {
                get { return payFrequencyId.Value; }
                set { payFrequencyId.Value = value; }
            }

            public Args()
            {
                planId = AddParameter<int?>("@PlanID", SqlDbType.Int);
                statusId = AddParameter<int?>("@StatusID", SqlDbType.Int);
                userId = AddParameter<int>("@UserID", SqlDbType.Int);
                clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
                employeeId = AddParameter<int?>("@EmployeeID", SqlDbType.Int);
                startDate = AddParameter<DateTime?>("@StartDate", SqlDbType.DateTime);
                endDate = AddParameter<DateTime?>("@EndDate", SqlDbType.DateTime);
                filterType = AddParameter<int>("@FilterType", SqlDbType.Int);
                filterId = AddParameter<int>("@FilterID", SqlDbType.Int);
                eeStatusFilter = AddParameter<int>("@EEStatusFilter", SqlDbType.Int);
                requireLeaveManagement = AddParameter<bool?>("@RequireLeaveManagement", SqlDbType.Bit);
                payFrequencyId = AddParameter<int>("@PayFrequencyID", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<EmployeeWithLeaveManagementGridDto, EmployeeLeaveManagementInfoDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<EmployeeWithLeaveManagementGridDto, EmployeeLeaveManagementInfoDto>> MapExpression
            {
                get
                {
                    return x => new EmployeeLeaveManagementInfoDto
                    {
                        RequestTimeOffId = x.RequestTimeOffId,
                        ClientAccrualId = x.ClientAccrualId,
                        ClientEarningId = x.ClientEarningId,
                        EmployeeName = x.Employee,
                        EmployeeId = x.EmployeeId,
                        PlanDescription = x.PlanDescription,
                        RequestHoursPrior = x.RequestBefore,
                        RequestHoursAfter = x.RequestAfter,
                        HoursInDay = x.HoursInDay,
                        Units = x.Units,
                        RequestFrom = x.RequestFrom,
                        Until = x.Until,
                        DateRequested = x.DateRequested,
                        Status = x.Status,
                        StatusDescription = x.StatusDescription,
                        Hours = x.Hours,
                        PayrollId = x.payrollId,
                        PeriodEnded = x.PeriodEnded,
                        ClockEmployeeBenefitId = x.ClockEmployeeBenefitId,
                        LeaveManagementPendingAwardId = x.LeaveManagementPendingAwardId,
                        PendingAwardType = x.PendingAwardType,
                        AwardOrder = x.AwardOrder
                    };
                }
            }
        }
    }
}
