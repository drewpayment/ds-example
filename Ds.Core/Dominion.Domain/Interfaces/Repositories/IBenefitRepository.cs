using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Benefits;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IBenefitRepository : IRepository, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TResult> GetOpenEnrollmentPlans<TResult>(QueryBuilder<OpenEnrollment, TResult> query)
            where TResult : class;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="planOptionId"></param>
        /// <returns></returns>
        PlanOption GetPlanOptionById(int planOptionId);
        
        /// <summary>
        /// Provides a way to get open enrollment that meet the specified query criteria.
        /// </summary>
        /// <typeparam name="TResult">Type of object to return.</typeparam>
        /// <param name="query">Filter/selection criteria to apply to the result set.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetOpenEnrollments<TResult>(QueryBuilder<OpenEnrollment, TResult> query)
            where TResult : class;

        IEnumerable<TResult> EmployeeOpenEnrollments<TResult>(QueryBuilder<EmployeeOpenEnrollment, TResult> query)
            where TResult : class;

        IEnumerable<TResult> GetEmployeeOpenEnrollment<TResult>(QueryBuilder<EmployeeOpenEnrollment, TResult> query)
            where TResult : class;

        IEnumerable<TResult> EmployeeOpenEnrollmentSelections<TResult>(
            QueryBuilder<EmployeeOpenEnrollmentSelection, TResult> query) where TResult : class;

        /// <summary>
        /// Provides a way to get employee selection dependents that meet the specified query criteria.
        /// </summary>
        /// <typeparam name="TResult">Type of object to return.</typeparam>
        /// <param name="query">Filter/selection criteria to apply to the result set.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployeeSelectionDependents<TResult>(
            QueryBuilder<EmployeeSelectionDependent, TResult> query) where TResult : class;

        /// <summary>
        /// Provides a way to get a list of Life Event Options.
        /// </summary>
        /// <typeparam name="TResult">Type of object to return.</typeparam>
        /// <param name="query">Filter/selection criteria to apply to the result set.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetLifeEventList<TResult>(QueryBuilder<LifeEventReasonInfo, TResult> query) where TResult : class;

        /// <summary>
        /// Provides a way to get the list of life event approval status types
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        IEnumerable<TResult> GetLifeEventApprovalStatusTypes<TResult>(QueryBuilder<LifeEventApprovalStatusType, TResult> query) where TResult : class;

        IEnumerable<TResult> GetPlanWaiveReasons<TResult>(QueryBuilder<PlanWaiveReason, TResult> query) where TResult : class;

        IEmployeePcpQuery QueryEmployeePcps();

        /// <summary>
        /// Creates a new query on <see cref="OpenEnrollment"/> data.
        /// </summary>
        /// <returns></returns>
        IOpenEnrollmentQuery QueryOpenEnrollments();

        /// <summary>
        /// Creates a new query on <see cref="PlanProvider"/> data.
        /// </summary>
        /// <returns></returns>
        IPlanProviderQuery QueryPlanProviders();

        /// <summary>
        /// Create a new query on <see cref="PlanCategory"/> data.
        /// </summary>
        /// <returns></returns>
        IPlanCategoryQuery QueryPlanCategories();

        /// <summary>
        /// Creates a new query on benefit <see cref="BenefitResource"/> data.
        /// </summary>
        /// <returns></returns>
        IBenefitResourceQuery QueryResources();

        /// <summary>
        /// Creates a new query on benefit <see cref="Plan"/> data.
        /// </summary>
        /// <returns></returns>
        IPlanQuery QueryPlans();

        /// <summary>
        /// Creates a new query on benefit <see cref="PlanOptionCost"/> data.
        /// </summary>
        /// <returns></returns>
        IPlanOptionCostQuery QueryPlanOptionCosts();

        /// <summary>
        /// Creates a new query on <see cref="DependentCoverageOptionTypeInfo"/> data.
        /// </summary>
        /// <returns></returns>
        IDependentCoverageOptionQuery QueryDependentCoverageOptions();

        /// <summary>
        /// Creates a new query on <see cref="PlanOption"/> data.
        /// </summary>
        /// <returns></returns>
        IPlanOptionQuery QueryPlanOptions();

        /// <summary>
        /// Creates a new query on <see cref="LifeEventReasonInfo"/>(s).
        /// </summary>
        /// <returns></returns>
        ILifeEventReasonQuery QueryLifeEventReasons();

        /// <summary>
        /// Creates a new query on <see cref="LifeEventApprovalStatusType"/>(s). 
        /// </summary>
        /// <returns></returns>
        ILifeEventApprovalStatusTypeQuery QueryLifeEventApprovalStatusTypes();

        /// <summary>
        /// Creates a new query on <see cref="PlanType"/>
        /// </summary>
        /// <returns></returns>
        IPlanTypeQuery QueryPlanTypes();

        /// <summary>
        /// Creates a new query on <see cref="PlanOptionPlan"/>
        /// </summary>
        /// <returns></returns>
        IPlanOptionPlanQuery QueryPlanOptionPlans();

        /// <summary>
        /// Creates a new query on <see cref="PlanTypePlanOption"/>
        /// </summary>
        /// <returns></returns>
        IPlanTypePlanOptionQuery QueryPlanTypePlanOptions();

        /// <summary>
        /// Creates a new query on cost factors.
        /// </summary>
        /// <returns></returns>
        ICostFactorQuery QueryCostFactors();

        /// <summary>
        /// Queries cost factor configurations for a plan option.
        /// </summary>
        /// <returns></returns>
        IPlanOptionCostFactorQuery QueryPlanOptionCostFactorConfigs();

        /// <summary>
        /// Creates a new query on <see cref="BenefitAmount"/>
        /// </summary>
        /// <returns></returns>
        IBenefitAmountQuery QueryBenefitAmounts();

        /// <summary>
        /// Creates a new query on <see cref="EmployeeOpenEnrollmentSelection"/>(s).
        /// </summary>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery QueryEmployeeOpenEnrollmentSelections();

        /// <summary>
        /// Creates a new query on <see cref="CoverageType"/>(s).
        /// </summary>
        /// <returns></returns>
        ICoverageTypeQuery QueryCoverageTypes();

        /// <summary>
        /// Creates a new query on <see cref="EmployeeOpenEnrollment"/> data.
        /// </summary>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery QueryEmployeeOpenEnrollmentInfo();

        /// <summary>
        /// Creates a new query on <see cref="AgeDeterminationType"/> data.
        /// </summary>
        /// <returns></returns>
        IAgeDeterminationTypeQuery QueryAgeDeterminationTypes();
        
        /// <summary>
        /// Creates a new query on <see cref="BenefitPackage"/>(s).
        /// </summary>
        /// <returns></returns>
        IBenefitPackageQuery QueryBenefitPackages();

        /// <summary>
        /// Creates a new query on <see cref="SalaryMethodTypeInfo"/> data.
        /// </summary>
        /// <returns></returns>
        ISalaryMethodInfoQuery QuerySalaryMethodInformation();

        /// <summary>
        /// Creates a new query on <see cref="BenefitAmountSelectionTypeInfo"/>
        /// </summary>
        /// <returns></returns>
        IBenefitAmountSelectionTypeQuery QueryBenefitAmountSelectionTypes();
        /// <summary>
        /// Creates a new query on <see cref="BenefitImportExportFormat"/> data.
        /// </summary>
        /// <returns></returns>
        IBenefitImportExportFormatQuery QueryBenefitImportExportFormats();

        /// <summary>
        /// Creates a new query on <see cref="CustomBenefitField"/> data.
        /// </summary>
        /// <returns></returns>
        ICustomBenefitFieldQuery QueryCustomBenefitFields();

        /// <summary>
        /// Creates a new query on <see cref="CustomBenefitFieldValue"/> data.
        /// </summary>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery QueryCustomBenefitFieldValues();
    }
}