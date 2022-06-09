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
    public class GetEmployeeJobCostingAssignmentList_Search : SprocBase<IEnumerable<ClientJobCostingAssignmentSprocDto>, GetEmployeeJobCostingAssignmentList_Search.Args>
    {
        public GetEmployeeJobCostingAssignmentList_Search(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override IEnumerable<ClientJobCostingAssignmentSprocDto> Execute()
        {
            return ExecuteSproc("[dbo].[spGetEmployeeJobCostingAssignmentList_Search]", dr =>
            {
                var results = dr.AsEnumerable<ResultDto>().ToArray();
                return Mapper.Instance.Map(results);
            });
        }

        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int> clientId;
            private readonly SqlParameterBuilder<int> employeeId;
            private readonly SqlParameterBuilder<int> clientJobCostingId;
            private readonly SqlParameterBuilder<bool> isEnabled;
            private readonly SqlParameterBuilder<string> clientJobCostingIdString;
            private readonly SqlParameterBuilder<string> clientJobCostingAssignmentIdString;
            private readonly SqlParameterBuilder<int> userId;
            private readonly SqlParameterBuilder<string> searchText;

            public int ClientId
            {
                get { return clientId.Value; }
                set { clientId.Value = value; }
            }

            public int EmployeeId
            {
                get { return employeeId.Value; }
                set { employeeId.Value = value; }
            }

            public int ClientJobCostingId
            {
                get { return clientJobCostingId.Value; }
                set { clientJobCostingId.Value = value; }
            }

            public bool IsEnabled
            {
                get { return isEnabled.Value; }
                set { isEnabled.Value = value; }
            }

            public int[] ClientJobCostingIds
            {
                get { return clientJobCostingIdString.Value.Split(',').Select(int.Parse).ToArray(); }
                set { clientJobCostingIdString.Value = String.Join(",", value); }
            }

            public int[] ClientJobCostingAssignmentIds
            {
                get { return clientJobCostingAssignmentIdString.Value.Split(',').Select(int.Parse).ToArray(); }
                set { clientJobCostingAssignmentIdString.Value = String.Join(",", value); }
            }

            public int UserId
            {
                get { return userId.Value; }
                set { userId.Value = value; }
            }

            public string SearchText
            {
                get { return searchText.Value; }
                set { searchText.Value = value; }
            }

            public Args()
            {
                clientId = AddParameter<int>("@ClientID", SqlDbType.Int);
                employeeId = AddParameter<int>("@EmployeeID", SqlDbType.Int);
                clientJobCostingId = AddParameter<int>("@ClientJobCostingID", SqlDbType.Int);
                isEnabled = AddParameter<bool>("@IsEnabled", SqlDbType.Bit);
                clientJobCostingIdString = AddParameter<string>("@ClientJobCostingID_String", SqlDbType.VarChar);
                clientJobCostingAssignmentIdString = AddParameter<string>("@ClientJobCostingAssignmentID_String", SqlDbType.VarChar);
                userId = AddParameter<int>("@UserID", SqlDbType.Int);
                searchText = AddParameter<string>("@SearchText", SqlDbType.VarChar);
            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, ClientJobCostingAssignmentSprocDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, ClientJobCostingAssignmentSprocDto>> MapExpression
            {
                get
                {
                    return x => new ClientJobCostingAssignmentSprocDto()
                    {
                        ClientJobCostingAssignmentId = x.ID,
                        Description = x.Description,
                        Code = x.Code
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
            public int ID { get; set; }
            public string Description { get; set; }
            public string Code { get; set; }
        }

    }
}
