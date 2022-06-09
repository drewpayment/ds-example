using Dominion.Core.EF.Abstract;
using Dominion.Core.EF.Interfaces;
using Dominion.Core.EF.Query;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.EF.Query;
using Dominion.Utility.Query;
using System.Collections.Generic;
using System.Linq;
using TimeZone = Dominion.Domain.Entities.Misc.TimeZone;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.EF.Query.TimeClock;
using Dominion.LaborManagement.EF.Sprocs;
using System;

namespace Dominion.LaborManagement.EF.Repository
{
    /// <summary>
    /// Repository providing methods to retrieve TimeClock entities from a datastore.
    /// </summary>
    public class TimeClockRepository : RepositoryBase, ITimeClockRepository
    {
        #region CONSTRUCTOR(S) & INIT

        /// <summary>
        /// Instantiates a new TimeClockRepository.
        /// </summary>
        /// <param name="context">Context containing the TimeClock datastore.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public TimeClockRepository(IDominionContext context, IQueryResultFactory resultFactory = null)
            : base(context, resultFactory)
        {
        }

        #endregion

        #region CLOCK EMPLOYEES

        /// <summary>
        /// Retrieves the ClockEmployees which match the specified query criteria from the datastore.
        /// </summary>
        /// <typeparam name="TResult">Type the results will be mapped to.</typeparam>
        /// <param name="query">Query containing the filter, sort and result-map criteria to be performed.</param>
        /// <returns>Collection of objects which match the specified query.</returns>
        public IEnumerable<TResult> GetClockEmployees<TResult>(QueryBuilder<ClockEmployee, TResult> query)
            where TResult : class
        {
            return this.QueryBuilderExecuter(query);
        }


        /// <summary>
        /// Query that returns employees punches.  If you need to add to the query, access the interface and implemenation
        /// that allow for inline fluent syntax.
        /// </summary>
        /// <returns></returns
        IClockEmployeePunchQuery ITimeClockRepository.GetClockEmployeePunchQuery()
        {
            return new ClockEmployeePunchQuery(_context.ClockEmployeePunches);
        }
       
        /// <summary>
        /// Query that returns clock employees
        /// </summary>
        /// <returns></returns>
        public IClockEmployeeQuery GetClockEmployeeQuery()
        {
            return new ClockEmployeeQuery(_context.ClockEmployees, QueryResultFactory);
        }

        public IClockClientHolidayQuery GetClockClientHolidayQuery()
        {
            return new ClockClientHolidayQuery(_context.ClockClientHolidays, QueryResultFactory);
        }

        public IClockClientHardwareQuery QueryClockClientHardware()
        {
            return new ClockClientHardwareQuery(_context.ClockClientHardwares, QueryResultFactory);
        }

        public IClockClientLunchQuery GetClockClientLunchQuery()
        {
            return new ClockClientLunchQuery(_context.ClockClientLunches, QueryResultFactory);
        }

        public IClockClientAddHoursQuery GetClockClientAddHoursQuery()
        {
            return new ClockClientAddHoursQuery(_context.ClockClientAddHours, QueryResultFactory);
        }

        public IClockClientRulesQuery GetClockClientRules()
        {
            return new ClockClientRulesQuery(_context.ClockClientRules, QueryResultFactory);
        }

        public IClockClientTimePolicyQuery GetClockClientTimePolicyQuery()
        {
            return new ClockClientTimePolicyQuery(_context.ClockClientTimePolicies, QueryResultFactory);
        }

        public IDayListSavingsTimeQuery DayListSavingsTimeQuery()
        {
            return new DayLightSavingsTimeQuery(_context.DayLightSavingsTimes, QueryResultFactory);
        }

        public IClockRoundingTypeQuery ClockRoundingTypeQuery()
        {
            return new ClockRoundingTypeQuery(_context.ClockRoundingTypes, QueryResultFactory);
        }

        /// <summary>
        /// returns the most recent pay period ended.  If providing only a client ID, the result will contain the pay
        /// period ended for all employees- which is a large query.  By supplying and employee id, it will return only the
        /// pay period ended for the specified employee. 
        /// </summary>
        /// <param name="clientId">id for client</param>
        /// <param name="employeeId">id for employee</param>
        /// <returns></returns>
        public IEnumerable<ClockEmployeePayPeriodEndedDto> ClockEmployeePayPeriodEndedSproc(int? clientId = null, int? employeeId = null)
        {
            var args = new GetClockEmployeePayPeriodEndedSproc.Args()
            {
                EmployeeId = employeeId,
                ClientId = clientId
            };
            var sproc = new GetClockEmployeePayPeriodEndedSproc(_context.ConnectionString, args);

            return sproc.Execute();
        }

        /// <summary>
        /// Returns the most recent / last punch for an employee.  This is needed to determine if a transfer punch is necessary. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="employeeId">Id for Employee</param>
        /// <param name="costCenterId">Id for ClientCostCenter</param>
        /// <param name="divisionId">Id for ClientDivision</param>
        /// <param name="departmentId">Id for ClientDepartment</param>
        /// <param name="jobCostingAssignment1Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment2Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment3Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment4Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment5Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment6Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="clockClientLunchId"></param>
        /// name="clientId">Id for Client/param>
        /// <returns></returns>
        public ClockEmployeeLastPunchDto ClockEmployeeLastPunchSproc(int clientId, int employeeId, DateTime punchDateTime, int? costCenterId = null,
            int? divisionId = null, int? departmentId = null, int? jobCostingAssignment1Id = null,
            int? jobCostingAssignment2Id = null, int? jobCostingAssignment3Id = null, int? jobCostingAssignment4Id = null,
            int? jobCostingAssignment5Id = null, int? jobCostingAssignment6Id = null, int? clockClientLunchId = null)
        {
            var args = new GetClockEmployeeLastPunchSproc.Args()
            {
                ClientId = clientId,
                EmployeeId = employeeId,
                PunchDateTime = punchDateTime,
                ClientCostCenterId = costCenterId,
                ClientDivisionId = divisionId,
                ClientDepartmentId = departmentId,
                JobAssignment1Id = jobCostingAssignment1Id,
                JobAssignment2Id = jobCostingAssignment2Id,
                JobAssignment3Id = jobCostingAssignment3Id,
                JobAssignment4Id = jobCostingAssignment4Id,
                JobAssignment5Id = jobCostingAssignment5Id,
                JobAssignment6Id = jobCostingAssignment6Id,
                ClockClientLunchId = clockClientLunchId
            };  
            var sproc = new GetClockEmployeeLastPunchSproc(_context.ConnectionString, args);

            return sproc.Execute();
        }

        /// <summary>
        /// returns the time as set on the Dominion Sql Server
        /// </summary>
        /// <returns></returns>
        public SqlServerTimeDto GetSqlServerTimeSproc()
        {
            var sproc = new SqlServerDateTimeSproc(_context.ConnectionString, new SqlServerDateTimeSproc.Args());

            return sproc.Execute();
        }

        public IEnumerable<TimeZone> GetTimeZones()
        {
            return _context.TimeZones.ToList();
        }

        public IEnumerable<GetClockClientNoteListResultDto> GetClockClientNoteList(GetClockClientNoteListArgsDto args)
        {
            return new GetClockClientNoteListSproc(_context.ConnectionString, args).Execute();
        }

        #endregion

        public IClientMachineQuery QueryClientMachines()
        {
            return new ClientMachineQuery(_context.ClientMachines, QueryResultFactory);
        }
    }
}