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
    public class GetEmployeeActiveLeaveManagementListByClientID : SprocBase<IEnumerable<ClientAccrualDto>, GetEmployeeActiveLeaveManagementListByClientID.Args>
    {
        public GetEmployeeActiveLeaveManagementListByClientID(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override IEnumerable<ClientAccrualDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetEmployeeActiveLeaveManagementListByClientID]", dr =>
            {
                var results = dr.AsEnumerable<ResultDto>().ToArray();
                return Mapper.Instance.Map(results);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> employeeId;
            private readonly SqlParameterBuilder<int> clientId;

            public int EmployeeId
            {
                get { return employeeId.Value; }
                set { employeeId.Value = value; }
            }

            public int ClientId
            {
                get { return clientId.Value; }
                set { clientId.Value = value; }
            }

            public Args()
            {
                employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, ClientAccrualDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, ClientAccrualDto>> MapExpression
            {
                get
                {
                    return x => new ClientAccrualDto()
                    {
                        ClientAccrualId = x.ClientAccrualID,
                        Description = x.Description
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
            public int ClientAccrualID { get; set; }
            public string Description { get; set; }
        }
    }
}
