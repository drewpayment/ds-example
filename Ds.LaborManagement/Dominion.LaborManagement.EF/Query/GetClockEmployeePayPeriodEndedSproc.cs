using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.Query;
using Dominion.Utility.Query.LinqKit;

// ReSharper disable InconsistentNaming

namespace Dominion.LaborManagement.EF.Query
{
    /// <summary>
    ///  "[dbo].[spGetClockEmployeePayPeriodEnded]" stored procedure definition
    ///  See <see cref="GetClockEmployeePayPeriodEndedSproc.Args"/> for parameter info
    /// </summary>
    internal class GetClockEmployeePayPeriodEndedSproc :
        SprocBase<IEnumerable<ClockEmployeePayPeriodEndedDto>, GetClockEmployeePayPeriodEndedSproc.Args>
    {
        /// <summary>
        /// Name of sproc:  "[dbo].[spGetClockEmployeePayPeriodEnded]"
        /// </summary>
        public const string SprocName = "[dbo].[spGetClockEmployeePayPeriodEnded]";

        public GetClockEmployeePayPeriodEndedSproc(string connectionString, Args args) : base(connectionString, args)
        {
        }

        public override IEnumerable<ClockEmployeePayPeriodEndedDto> Execute()
        {
            var dto = ExecuteSproc(SprocName, dr =>
            {
                var results = dr.AsEnumerable<ResultDto>();

                var dtos = new List<ClockEmployeePayPeriodEndedDto>();

                results.ForEach(r => dtos.Add(Mapper.Instance.Map(r)));

                return dtos;
            });

            return dto;
        }

        #region Utility Classes

        /// <summary>
        /// Args class for the sproc.  The sproc cannot must be provided a client or employeeId.
        /// </summary>
        internal class Args : SprocParametersBase
        {
            private readonly SqlParameterBuilder<int?> _clientId;
            private readonly SqlParameterBuilder<int?> _employeeId;
            private readonly SqlParameterBuilder<int?> _userId;

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

            public int? UserId
            {
                get { return _userId.Value; }
                set { _userId.Value = value; }
            }

            public Args()
            {
                _clientId = AddParameter<int?>("@ClientId", SqlDbType.Int);
                _employeeId = AddParameter<int?>("@EmployeeId", SqlDbType.Int);
                _userId = AddParameter<int?>("@UserId", SqlDbType.Int);
            }
        }

        /// <summary>
        /// Maps the sproc result to the DTO object
        /// </summary>
        internal class Mapper : ExpressionMapper<ResultDto, ClockEmployeePayPeriodEndedDto>
        {
            private static readonly Lazy<Mapper> _mapper = new Lazy<Mapper>();

            public static Mapper Instance => _mapper.Value;

            public override Expression<Func<ResultDto, ClockEmployeePayPeriodEndedDto>> MapExpression
            {
                get
                {
                    return x => new ClockEmployeePayPeriodEndedDto()
                    {
                        EmployeeId = x.EmployeeId,
                        PeriodEnded = x.PeriodEnded,
                        WarningMessageClosed = x.WarningMessage_Closed,
                        WarningMessageLocked = x.WarningMessage_Locked,
                        AllowScheduleEdits = x.AllowScheduleEdits,
                        PeriodStartLocked = x.PeriodStart_Locked
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
            public int EmployeeId { get; set; }
            public DateTime? PeriodEnded { get; set; }
            public DateTime? PeriodStart_Locked { get; set; }
            public string WarningMessage_Locked { get; set; }
            public string WarningMessage_Closed { get; set; }
            public int AllowScheduleEdits { get; set; }
        }

        #endregion
    }
}