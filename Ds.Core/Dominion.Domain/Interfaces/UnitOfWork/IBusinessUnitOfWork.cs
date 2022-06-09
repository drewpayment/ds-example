using System;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Utility.OpResult;

namespace Dominion.Domain.Interfaces.UnitOfWork
{
    /// <summary>
    /// The business unit of work is a once stop shop for all your data store needs.
    /// It's a facade to any data store needed to complete all of you business needsd.
    /// </summary>
    public interface IBusinessUnitOfWork : IDataStoreUnitOfWork
    {

        IApiRepository ApiRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Benefit Entity objects.
        /// </summary>
        IBenefitRepository BenefitRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Client Entity objects.
        /// </summary>
        IClientRepository ClientRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Employee Entity objects.
        /// </summary>
        IEmployeeRepository EmployeeRepository { get; }

        IReportRepository ReportRepository { get; }

        /// <summary>
        /// Repository used to manipulate various User Entity objects.
        /// </summary>
        IUserRepository UserRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Miscellaneous Entity objects.
        /// </summary>
        IMiscRepository MiscRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Payroll Entity objects.
        /// </summary>
        IPayrollRepository PayrollRepository { get; }

        /// <summary>
        /// Repository providing query access to various security objects.
        /// </summary>
        ISecurityRepository SecurityRepository { get; }

        /// <summary>
        /// Repository used to manipulate various User Entity objects.
        /// </summary>
        ITaxRepository TaxRepository { get; }

        /// <summary>
        /// Repository used to access various Legacy Tax entities.
        /// </summary>
        ILegacyTaxRepository LegacyTaxRepository { get; }

        /// <summary>
        /// Repository used to manipulate various User Entity objects.
        /// </summary>
        ITaxesRepository TaxesRepository { get; }

        /// <summary>
        /// Repository used to manipulate various TimeClock Entity objects.
        /// </summary>
        ITimeClockRepository TimeClockRepository { get; }

        /// <summary>
        /// Repository used to get data about locations.
        /// Country, state, address, etc.
        /// </summary>
        ILocationRepository LocationRepository { get; }

        /// <summary>
        /// Used to access data related to the leave management functionality.
        /// </summary>
        ILeaveManagementRepository LeaveManagementRepository { get; }

        /// <summary>
        /// Used to access data related to labor management.
        /// </summary>
        ILaborManagementRepository LaborManagementRepository { get; }

        /// <summary>
        /// Used to access data related to ApplicantTracking
        /// </summary>
        IApplicantTrackingRepository ApplicantTrackingRepository { get; }

        /// <summary>
        /// Used to access ACA data.
        /// </summary>
        IAcaRepository AcaRepository { get; }

        /// <summary>
        /// Used to access client features to retrieve specific clients.
        /// </summary>
        IClientAccountFeatureRepository ClientAccountFeatureRepository { get; }
        
        /// <summary>
        /// Used to access billing related data.
        /// </summary>
        IOnboardingRepository OnboardingRepository { get; }
        
        /// <summary>
        /// Used to access billing related data.
        /// </summary>
        IBillingRepository BillingRepository { get; }

        /// <summary>
        /// Repository providing query access to core elements of the application.
        /// </summary>
        ICoreRepository CoreRepository { get; }

        IEEOCRepository EEOCRepository { get; }

        IContactRepository ContactRepository { get; }

        /// <summary>
        /// Repository providing access to W2 related database entities.
        /// </summary>
        IW2Repository W2Repository { get; }

        /// <summary>
        /// Repository providing access to Job Costing database entities.
        /// </summary>
        IJobCostingRepository JobCostingRepository { get; }

        /// <summary>
        /// Repository providing access to Client Check Stock Order database entities </summary>
        ICheckStockOrderRepository CheckStockOrderRepository { get; }

        /// <summary>
        /// Repository providing access to Client Check Stock Billing database entities
        /// </summary>
        ICheckStockBillingRepository CheckStockBillingRepository { get; }

        /// <summary>
        /// Repository providing access to Accounting-type functionality that isn't specifically for Payroll.
        /// </summary>
        IAccountingRepository AccountingRepository { get; }

        /// <summary>
        /// Repository providing access to Notification related entities
        /// </summary>
        INotificationRepository NotificationRepository { get; }

        /// <summary>
        /// Repository providing access to Performance Management/Review related entities.
        /// </summary>
        IPerformanceRepository PerformanceRepository { get; }

        INpsRepository NpsRepository { get; }

        IArRepository ArRepository { get; }

        /// <summary>
        /// Respository providing access to application configuration data such as Web Resources and Menus.
        /// </summary>
        IApplicationConfigurationRepository ApplicationConfigurationRepository { get; }
        IPushRepository PushRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Payroll Entity objects.
        /// </summary>
        IGeofenceRepository GeofenceRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Payroll Entity objects.
        /// </summary>
        IClientNotesRepository ClientNotesRepository { get; }

        /// <summary>
        /// Repository used to manipulate various Payroll Entity objects.
        /// </summary>
        IClientFeaturesRepository ClientFeaturesRepository { get; }

        /// <summary>
        /// Set the context so that it does not track changes.
        /// </summary>
        void NoChangeTracking();
        
        /// <summary>
        /// Registers an action that will be executed after a successful commit. 
        /// </summary>
        /// <param name="action">Action to perform after a successful commit.</param>
        /// <param name="mergeResultIntoCommitResult">If true, the action's op-result will be merged into the commit's 
        /// result. Be careful, this could cause a commit to appear to have failed even when the data has been successfully 
        /// persisted.</param>
        void RegisterPostCommitAction(Func<IOpResult> action, bool mergeResultIntoCommitResult = false);

        /// <summary>
        /// Registers an action that will be executed after a successful commit. 
        /// </summary>
        /// <param name="action">Action to perform after a successful commit.</param>
        /// <param name="mergeResultIntoCommitResult">If true, the action's op-result will be merged into the commit's 
        /// result. Be careful, this could cause a commit to appear to have failed even when the data has been successfully 
        /// persisted.</param>
        void RegisterPostCommitAction(Action action, bool mergeResultIntoCommitResult = false);
    }
}