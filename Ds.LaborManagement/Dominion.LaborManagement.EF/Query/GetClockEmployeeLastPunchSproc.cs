using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query;
// ReSharper disable InconsistentNaming

namespace Dominion.LaborManagement.EF.Query
{
    internal class GetClockEmployeeLastPunchSproc :
        SprocBase<ClockEmployeeLastPunchDto, GetClockEmployeeLastPunchSproc.Args>
    {

        public const string SprocName = "[dbo].[spGetClockEmployeePunchLast]";

        public GetClockEmployeeLastPunchSproc(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override ClockEmployeeLastPunchDto Execute()
        {
            var dto = ExecuteSproc(SprocName, dr =>
            {
                var result = dr.AsEnumerable<ResultDto>();
                var item = result.FirstOrDefault();
                return item != null ? Mapper.Instance.Map(item) : null;
            });

            return dto;
        }

        #region Utility Classes
		
        /// <summary>
        /// Args class for the sproc.  The sproc cannot must be provided a client or employeeId.
        /// </summary>
        public class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int?> _clientId;
            private readonly SqlParameterBuilder<int?> _employeeId;
            private readonly SqlParameterBuilder<DateTime?> _punchDateTime;
            private readonly SqlParameterBuilder<int?> _clientCostCenterId;
            private readonly SqlParameterBuilder<int?> _clientDivisionId;
            private readonly SqlParameterBuilder<int?> _clientDepartmentId;
            private readonly SqlParameterBuilder<int?> _jobCostingAssignment1Id;
            private readonly SqlParameterBuilder<int?> _jobCostingAssignment2Id;
            private readonly SqlParameterBuilder<int?> _jobCostingAssignment3Id;
            private readonly SqlParameterBuilder<int?> _jobCostingAssignment4Id;
            private readonly SqlParameterBuilder<int?> _jobCostingAssignment5Id;
            private readonly SqlParameterBuilder<int?> _jobCostingAssignment6Id;
            private readonly SqlParameterBuilder<int?> _clockClientLunchID;

            public int? ClientId
            {
                get { return _clientId.Value; }
                set { _clientId.Value = value; }
            }
			
            public int? EmployeeId
            {
                get { return _employeeId.Value; }
                set { _employeeId.Value = value; }
            }
			
            public int? ClientCostCenterId
            {
                get { return _clientCostCenterId.Value; }
                set { _clientCostCenterId.Value = value; }
            }

            public int? ClientDivisionId
            {
                get { return _clientDivisionId.Value; }
                set { _clientDivisionId.Value = value; }
            }
			
            public int? ClientDepartmentId
            {
                get { return _clientDivisionId.Value; }
                set { _clientDepartmentId.Value = value; }
            }
			
            public int? JobAssignment1Id
            {
                get { return _jobCostingAssignment1Id.Value; }
                set { _jobCostingAssignment1Id.Value = value; }
            }

            public int? JobAssignment2Id
            {
                get { return _jobCostingAssignment2Id.Value; }
                set { _jobCostingAssignment2Id.Value = value; }
            }

            public int? JobAssignment3Id
            {
                get { return _jobCostingAssignment3Id.Value; }
                set { _jobCostingAssignment3Id.Value = value; }
            }

            public int? JobAssignment4Id
            {
                get { return _jobCostingAssignment4Id.Value; }
                set { _jobCostingAssignment4Id.Value = value; }
            }

            public int? JobAssignment5Id
            {
                get { return _jobCostingAssignment5Id.Value; }
                set { _jobCostingAssignment5Id.Value = value; }
            }

            public int? JobAssignment6Id
            {
                get { return _jobCostingAssignment6Id.Value; }
                set { _jobCostingAssignment6Id.Value = value; }
            }

            public int? ClockClientLunchId
            {
                get { return _clockClientLunchID.Value; }
                set { _clockClientLunchID.Value = value; }
            }

            public DateTime? PunchDateTime
            {
                get { return _punchDateTime.Value; }
                set { _punchDateTime.Value = value; }
            }

            public Args()
            {
                _clientId = AddParameter<int?>("@ClientId", SqlDbType.Int);
                _employeeId = AddParameter<int?>("@employeeId", SqlDbType.Int);
                _punchDateTime = AddParameter<DateTime?>("PunchDateTime", SqlDbType.DateTime);
                _clientCostCenterId = AddParameter<int?>("@ClientCostCenterID", SqlDbType.Int);
                _clientDivisionId = AddParameter<int?>("@ClientDivisionID", SqlDbType.Int);
                _clientDepartmentId = AddParameter<int?>("@ClientDepartmentID", SqlDbType.Int);
                _jobCostingAssignment1Id = AddParameter<int?>("@ClientJobCostingAssignmentID_1", SqlDbType.Int);
                _jobCostingAssignment2Id = AddParameter<int?>("@ClientJobCostingAssignmentID_2", SqlDbType.Int);
                _jobCostingAssignment3Id = AddParameter<int?>("@ClientJobCostingAssignmentID_3", SqlDbType.Int);
                _jobCostingAssignment4Id = AddParameter<int?>("@ClientJobCostingAssignmentID_4", SqlDbType.Int);
                _jobCostingAssignment5Id = AddParameter<int?>("@ClientJobCostingAssignmentID_5", SqlDbType.Int);
                _jobCostingAssignment6Id = AddParameter<int?>("@ClientJobCostingAssignmentID_6", SqlDbType.Int);
                _clockClientLunchID = AddParameter<int?>("@ClockClientLunchID", SqlDbType.Int);            }
        }

        internal class Mapper : ExpressionMapper<ResultDto, ClockEmployeeLastPunchDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, ClockEmployeeLastPunchDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeeLastPunchDto()
                    {
                        IsTransferPunch = x.TransferPunch.ToUpperInvariant() == "TRUE",
                        ClockEmployeePunchId = x.ClockEmployeePunchID,
                        ClientCostCenterId = x.ClientCostCenterID,
                        ClientDivisionId =  x.ClientDivisionID,
                        ClientDepartmentId = x.ClientDepartmentID,
                        JobCostingAssignment1 = x.ClientJobCostingAssignmentID_1,
                        JobCostingAssignment2 = x.ClientJobCostingAssignmentID_2,
                        JobCostingAssignment3 = x.ClientJobCostingAssignmentID_3,
                        JobCostingAssignment4 = x.ClientJobCostingAssignmentID_4,
                        JobCostingAssignment5 = x.ClientJobCostingAssignmentID_5,
                        JobCostingAssignment6 = x.ClientJobCostingAssignmentID_6,
                        ClockClientLunchId = x.ClockClientLunchID,
                        ModifiedPunch =  x.ModifiedPunch
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
            public string TransferPunch { get; set; }
            public int? ClockEmployeePunchID { get; set; }
            public int? ClientCostCenterID { get; set; }
            public int? ClientDivisionID { get; set; }
            public int? ClientDepartmentID { get; set; }
            public int? ClientJobCostingAssignmentID_1 { get; set; }
            public int? ClientJobCostingAssignmentID_2 { get; set; }
            public int? ClientJobCostingAssignmentID_3 { get; set; }
            public int? ClientJobCostingAssignmentID_4 { get; set; }
            public int? ClientJobCostingAssignmentID_5 { get; set; }
            public int? ClientJobCostingAssignmentID_6 { get; set; }
            public int? ClockClientLunchID { get; set; }
            public DateTime ModifiedPunch { get; set; }
        }

        #endregion
    }
}