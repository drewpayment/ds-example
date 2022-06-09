using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Employee.Search;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Utility.Dto;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Query.Employee;
using Dominion.Domain.Interfaces.Query.Employee.Search;
using Dominion.Utility.OpResult;
using Dominion.Domain.Interfaces.Query.Benefits;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Core.Dto.Sprocs;
using Dominion.Domain.Interfaces.Query.ExitInterview;
using Dominion.Core.Dto.Notification;
using Dominion.Core.Dto.Accruals;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface to a repository used to manipulate various Employee related Entities in a data store.
    /// </summary>
    public interface IEmployeeRepository : IRepository, IDisposable
    {
        #region EMPLOYEE

        /// <summary>
        /// Gets the Employee with the specified ID or null if no Employee was found.
        /// </summary>
        /// <param name="id">ID of the Employee to be found.</param>
        /// <returns>Employee with the specified ID or null if no Employee was found.</returns>
        Employee GetEmployee(int id);

        IEmployeeStatusQuery GetEmployeeStatusList();
        /// <summary>
        /// Gets the specified employee.
        /// //todo: jay: need to write a test for this
        /// </summary>
        /// <typeparam name="TResult">Type of object the resulting entity will be translated into.</typeparam>
        /// <param name="employeeDependentId">ID of the employee to retrieve.</param>
        /// <param name="selector">Expression describing how to translate the employee entity into the desired type.</param>
        /// <returns>An employee entity with the specified ID.</returns>
        TResult GetEmployee<TResult>(int employeeId, Expression<Func<Employee, TResult>> selector) where TResult : class;

        /// <summary>
        /// Returns the Employees that satisfy the provided base query.
        /// </summary>
        /// <typeparam name="TResult">A strongly-typed object containing the desired Employee properties to be returned.</typeparam>
        /// <param name="query">The query parameters to apply to the employee  result set including the object type to translate the results to.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployees<TResult>(QueryBuilder<Employee, TResult> query) where TResult : class;

        /// <summary>
        /// Constructs a new query on <see cref="Employee"/> data.
        /// </summary>
        /// <returns></returns>
        IEmployeeQuery QueryEmployees();

        #endregion // region EMPLOYEE

        #region EMPLOYEE DEPENDENT

        /// <summary>
        /// Gets the specified employee dependent.
        /// </summary>
        /// <param name="employeeDependentId">ID of the employee dependent to retrieve.</param>
        /// <returns>An employee dependent entity with the specified ID.</returns>
        EmployeeDependent GetEmployeeDependent(int employeeDependentId);

        /// <summary>
        /// Gets the specified employee dependent.
        /// </summary>
        /// <typeparam name="TResult">Type of object the resulting entity will be translated into.</typeparam>
        /// <param name="employeeDependentId">ID of the employee dependent to retrieve.</param>
        /// <param name="selector">Expression describing how to translate the dependent entity into the desired type.</param>
        /// <returns>An employee dependent entity with the specified ID.</returns>
        TResult GetEmployeeDependent<TResult>(int employeeDependentId,
            Expression<Func<EmployeeDependent, TResult>> selector) where TResult : class;

        /// <summary>
        /// Returns the Employee Dependents for the given employee that satisfy the provided base query.
        /// </summary>
        /// <typeparam name="TResult">A strongly-typed object containing the desired Employee Dependent properties to be returned.</typeparam>
        /// <param name="employeeId">The ID of the employee to get dependents for.</param>
        /// <param name="query">The query parameters to apply to the employee dependent result set including the object type to translate the results to.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployeeDependents<TResult>(int employeeId,
            QueryBuilder<EmployeeDependent, TResult> query) where TResult : class;

        /// <summary>
        /// Returns the Employee Dependents that satisfy the provided base query.
        /// </summary>
        /// <typeparam name="TResult">A strongly-typed object containing the desired Employee Dependent properties to be returned.</typeparam>
        /// <param name="query">The query parameters to apply to the employee dependent result set including the object type to translate the results to.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployeeDependents<TResult>(QueryBuilder<EmployeeDependent, TResult> query)
            where TResult : class;

        /// <summary>
        /// Returns a new query on employee dependents.
        /// </summary>
        /// <returns></returns>
        IEmployeeDependentQuery QueryDependents();

        #endregion // region EMPLOYEE DEPENDENT

        #region EMPLOYEE EMERGENCY CONTACT

        /// <summary>
        /// Get a single emergency contact.
        /// </summary>
        /// <param name="Id">ID of the contact.</param>
        /// <returns>The contact.</returns>
        EmployeeEmergencyContact GetEmployeeEmergencyContact(int id);
        EmployeeEmergencyContact GetEmployeeEmergencyContactFullName(int id);

        /// <summary>
        /// Returns the Employee Emergency Contacts for the given employee that satisfy the provided base query.
        /// </summary>
        /// <typeparam name="TResult">A strongly-typed object containing the desired Employee Emergency Contact properties to be returned.</typeparam>
        /// <param name="employeeId">The ID of the employee to get emergency contacts for.</param>
        /// <param name="query">The query parameters to apply to the employee emergency contact result set including the object type to translate the results to.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployeeEmergencyContacts<TResult>(QueryBuilder<EmployeeEmergencyContact, TResult> query)
            where TResult : class;

        /// <summary>
        /// Returns a new query on employee emergency contacts.
        /// </summary>
        /// <returns></returns>
        IEmployeeEmergencyContactQuery EmployeeEmergencyContactQuery();

        #endregion

        EmployeeOnboarding GetEmployeeOnboarding(int empId);

        EmployeeOnboardingWorkflow GetEmployeeFinalizePageStatus(int empId);

        EmployeeOnboardingWorkflow GetEmployeePageIsCompleteStatus(int empId, int workFlowTask);

        #region EMPLOYEE DEDUCTION

        /// <summary>
        /// Creates a new <see cref="IEmployeeDeductionQuery"/> used to query <see cref="EmployeeDeduction"/> entities.
        /// </summary>
        /// <returns></returns>
        IEmployeeDeductionQuery EmployeeDeductionQuery();

        #endregion

        #region Employee Bank

        IEmployeeBankQuery EmployeeBankQuery();

        #endregion

        W4TotalExemptionsDto GetW4TotalExemptions(int empId);

        IEmployeeClientRateQuery EmployeeClientRateQuery();

        IEmployeeAccrualQuery EmployeeAccrualQuery();
        IEmployeeAvatarQuery EmployeeAvatarQuery();

        IPreviewEmployeeBenefitImportQuery QueryPreviewEmployeeDeductions();
        /// <summary>
        /// Clears the EmployeePreviewBenefitImport table.
        /// </summary>
        /// <returns></returns>
        IOpResult<int> ClearEmployeePreviewBenefitImport();

        IClientGroupQuery QueryClientGroups();
        IEmployeeClientRateCostCenterQuery EmployeeClientRateCostCenterQuery();
        IEmployeeCostCenterPercentageQuery EmployeeCostCenterPercentageQuery();

        /// <summary>
        /// Query on <see cref="LegacyEmployeeSearchSetting"/> data.
        /// </summary>
        /// <returns></returns>
        ILegacyEmployeeSearchSettingQuery QueryLegacyEmployeeSearchSettings();
        IEmployeeEventQuery GetEmployeeEvents();
        IEmployeePersonalInfoQuery EmployeePersonalInfoQuery();
        IEmployeePointsQuery EmployeePointsQuery();
        IEmployeeBenefitInfoQuery EmployeeBenefitInfoQuery();
        string GetNextEmployeeNumberForClient(int clientId);
        EmployeePayBasicDto GetEmployeePayInfo(int employeeId);
        IEnumerable<EmployeeChangesDto> GetEmployeeChangesInfo(int clientId, int userId, EmployeeChangesParametersDto dto);
        IEmployeeDriversLicenseQuery EmployeeDriversLicenseQuery();
		IEmployeeNotesQuery EmployeeNotesQuery();
        IEmployeeNotesActivityIdQuery EmployeeNotesActivityIdQuery();
        IEmployeeNoteTagsQuery EmployeeNoteTagsQuery();
        IEmployeeNoteTagsDetailsQuery EmployeeNoteTagsDetailsQuery();
		IEmployeeNoteAttachmentsQuery EmployeeNoteAttachmentsQuery();
        IEmployeePayTypeInfoQuery EmployeePayTypeInfoQuery();

        IEmployeeTerminationReasonQuery GetEmployeeTerminationReasons();
		IEmployeeStatusQuery EmployeeStatusQuery();
		IClockEmployeeApproveHoursSettingsQuery ClockEmployeeApproveHoursSettingsQuery();
        IEmployeeExitInterviewRequestQuery EmployeeExitInterviewRequestQuery();
        //IReviewRemarkQuery ReviewRemarkQuery();
        IEnumerable<GetClockEmployeeExceptionHistoryByEmployeeIDDto> GetClockEmployeeExceptionHistoryByEmployeeID(GetClockEmployeeExceptionHistoryByEmployeeIDArgsDto args);
        IEnumerable<GetClockEmployeeExceptionHistoryByEmployeeIDListDto> GetClockEmployeeExceptionHistoryByEmployeeIDList(GetClockEmployeeExceptionHistoryByEmployeeIDListArgsDto args);
        IEnumerable<GetReportClockEmployeeOnSiteDto> GetReportClockEmployeeOnSite(GetReportClockEmployeeOnSiteArgsDto args);
        IEnumerable<GetClockEmployeeHoursComparisonDto> GetClockEmployeeHoursComparisonSproc(GetClockEmployeeHoursComparisonArgsDto args);

        IEmployeeHRInfoQuery EmployeeHRInfoQuery();
        IEnumerable<EmployeeAccrualListDto> GetEmployeeAccrualList(int employeeId);
        IEnumerable<EmployeePaycheckAccrualHistoryDto> GetMostRecentPaycheckAccrualHistoryListByEmployeeID(int clientId, int employeeId);
        IEnumerable<GenPayCheckAccrualHistoryDto> GetGenPayCheckAccrualHistoryByEmployeeAdjustments(int employeeId, int clientAccrualId, int payrollId);

    } // interface IEmployeeRepository
}