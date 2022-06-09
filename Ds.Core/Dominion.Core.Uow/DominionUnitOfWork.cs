using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Transactions;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Domain.Interfaces.UnitOfWork;
using Dominion.Utility.Containers;
using Dominion.Utility.Messaging;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Ninject;
using Dominion.Core.EF.Repositories;
using Serilog;

namespace Dominion.Core.Uow
{
    /// <summary>
    /// This is the only concrete implementation of the interface implemented.
    /// The business unit of work is a once stop shop for all your data store needs.
    /// It's a facade to any data store needed to complete all of you business needed.
    /// </summary>
    public class DominionUnitOfWork : IBusinessUnitOfWork
    {
        internal class PostCommitAction
        {
            public bool ShouldMergeResult { get; set; }
            public Func<IOpResult> Action { get; set; }
        }

        private readonly List<PostCommitAction> _postCommitActions = new List<PostCommitAction>();

        private readonly ILogger _logger;

        #region Variables and Properties

        /// <summary>
        /// Access to any number of units of work.
        /// </summary>
        private IDataStoreUnitOfWorkProvider _dataStoreUnitOfWorkProvider;

        public IApiRepository ApiRepository { get; private set; }

        /// <summary>
        /// Repository used to manipulate various Benefit Entity objects.
        /// </summary>
        public IBenefitRepository BenefitRepository { get; private set; }

        /// <summary>
        /// Repository used to manipulate various Client Entity objects.
        /// </summary>
        public IClientRepository ClientRepository { get; private set; }

        /// <summary>
        /// Repository used to manipulate various Employee Entity objects.
        /// </summary>
        public IEmployeeRepository EmployeeRepository { get; private set; }

        /// <summary>
        /// Repository used to manipulate various User Entity objects.
        /// </summary>
        public IUserRepository UserRepository { get; private set; }

        /// <summary>
        /// Misc Repository.
        /// States, Country, SecretQuestion, and more.
        /// </summary>
        public IMiscRepository MiscRepository { get; private set; }

        /// <summary>
        /// Payroll Repository.
        /// </summary>
        public IPayrollRepository PayrollRepository { get; private set; }

        /// <summary>
        /// Tax Repository.
        /// </summary>
        public ITaxRepository TaxRepository { get; private set; }

        /// <summary>
        /// Legacy Tax Repository.
        /// </summary>
        public ILegacyTaxRepository LegacyTaxRepository { get; private set; }

        /// <summary>
        /// New Taxes Repository.
        /// </summary>
        public ITaxesRepository TaxesRepository { get; private set; }

        /// <summary>
        /// TimeClock Repository.
        /// </summary>
        public ITimeClockRepository TimeClockRepository { get; private set; }

        /// <summary>
        /// Security Repository.
        /// </summary>
        public ISecurityRepository SecurityRepository { get; private set; }

        /// <summary>
        /// Repository used to get data about locations.
        /// Country, state, address, etc.
        /// </summary>
        public ILocationRepository LocationRepository { get; private set; }

        /// <summary>
        /// Used to access data related to the leave management functionality.
        /// </summary>
        public ILeaveManagementRepository LeaveManagementRepository { get; private set; }

        /// <summary>
        /// Used to access data related to labor management.
        /// </summary>
        public ILaborManagementRepository LaborManagementRepository { get; private set; }

        /// <summary>
        /// Used to access data related to Applicant Tracking.
        /// </summary>
        public IApplicantTrackingRepository ApplicantTrackingRepository { get; private set; }

        /// <summary>
        /// Used to access data related to ACA.
        /// </summary>
        public IAcaRepository AcaRepository { get; private set; }

        /// <summary>
        /// Used to access client features to retrieve specific clients.
        /// </summary>
        public IClientAccountFeatureRepository ClientAccountFeatureRepository { get; private set; }

        /// <summary>
        /// Used to access billing related data.
        /// </summary>
        public IBillingRepository BillingRepository { get; private set; }

        /// <summary>
        /// Provides query access to core elements of the application.
        /// </summary>
        public ICoreRepository CoreRepository { get; private set; }
        /// Used to access billing related data.
        /// </summary>
        public IOnboardingRepository OnboardingRepository { get; private set; }
        public IEEOCRepository EEOCRepository { get; private set; }
        public IContactRepository ContactRepository { get; private set; }

        /// <summary>
        /// Repository providing access to W2 related database entities.
        /// </summary>
        public IW2Repository W2Repository { get; private set; }

        /// <summary>
        /// Repository providing access to Job Costing database entities.
        /// </summary>
        public IJobCostingRepository JobCostingRepository { get; private set; }

        /// <inheritdoc />
        public IAccountingRepository AccountingRepository { get; private set; }

        /// <summary>
        /// Repository providing access to Check Stock Order entities
        /// </summary>
        public ICheckStockOrderRepository CheckStockOrderRepository { get; private set; }

        /// <summary>
        /// Repository providing access to Check Stock Billing entities
        /// </summary>
        public ICheckStockBillingRepository CheckStockBillingRepository { get; private set; }

        public IReportRepository ReportRepository { get; private set; }
        
        /// <summary>
        /// Repository providing access to Notification related entities
        /// </summary>
        public INotificationRepository NotificationRepository { get; private set; }

        /// <summary>
        /// Repository providing access to Performance Management/Review related entities.
        /// </summary>
        public IPerformanceRepository PerformanceRepository { get; private set; }

        public INpsRepository NpsRepository { get; private set; }

        /// <summary>
        /// Respository providing access to application configuration data such as Web Resources and Menus.
        /// </summary>
        public IApplicationConfigurationRepository ApplicationConfigurationRepository { get; private set; }
        public IGeofenceRepository GeofenceRepository { get; private set; }
        public IClientNotesRepository ClientNotesRepository { get; private set; }

        public IArRepository ArRepository { get; private set; }

        public bool AutoDetectChangesEnabled { get; private set; }
        public IClientFeaturesRepository ClientFeaturesRepository { get; private set; }

        public IPushRepository PushRepository { get; private set; }

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// Constructor for injection.
        /// The Unit Of Work provider and repositories for query.
        /// </summary>
        /// <param name="dataStoreUnitOfWorkProvider">The Unit of Work provider.</param>
        /// <param name="clientRepo">Client Repo.</param>
        /// <param name="employeeRepo">Employee Repo.</param>
        /// <param name="userRepo">User Repo.</param>
        /// <param name="miscRepo">Misc Repo.</param>
        /// <param name="onboardingRepository"></param>
        /// <param name="reportRepository"></param>
        [Inject]
        public DominionUnitOfWork(
            IDataStoreUnitOfWorkProvider dataStoreUnitOfWorkProvider, 
            IApiRepository apiRepo,
            IClientRepository clientRepo, 
            IEmployeeRepository employeeRepo, 
            IUserRepository userRepo, 
            IMiscRepository miscRepo, 
            ITaxRepository taxRepo, 
            ITaxesRepository taxesRepo, 
            IBenefitRepository benefitRepo, 
            IPayrollRepository payrollRepo, 
            ITimeClockRepository timeClockRepo, 
            ISecurityRepository securityRepo,
            ILegacyTaxRepository legacyTaxRepo,
            ILocationRepository locationRepository, 
            ILeaveManagementRepository leaveManagementRepository,
            ILaborManagementRepository laborManagementRepository,
            IApplicantTrackingRepository applicantTrackingRepository,
            IAcaRepository acaRepository,
            IClientAccountFeatureRepository clientAccountFeatureRepository,
            IBillingRepository billingRepository,
            ICoreRepository coreRepository,
            IOnboardingRepository onboardingRepository,
            IContactRepository contactRepository,
            IEEOCRepository eeocRepository,
            IW2Repository w2Repository,
            IJobCostingRepository jobCostingRepository,
            IAccountingRepository accountingRepository,
            ICheckStockOrderRepository checkStockOrderRepository,
            ICheckStockBillingRepository checkStockBillingRepository,
            IReportRepository reportRepository,
            INotificationRepository notificationRepository,
            IPerformanceRepository performanceRepository,
            INpsRepository npsRepository,
            IApplicationConfigurationRepository appConfigRepo,
            IGeofenceRepository geofenceRepository,
            IClientNotesRepository clientNotesRepository,
            IClientFeaturesRepository clientFeaturesRepository,
            ILogger logger,
            IArRepository arRepository,
            IPushRepository pushRepository)
        {
            _dataStoreUnitOfWorkProvider = dataStoreUnitOfWorkProvider;

            // repository initialization
            ApiRepository = apiRepo;
            ClientRepository = clientRepo;
            EmployeeRepository = employeeRepo;
            UserRepository = userRepo;
            MiscRepository = miscRepo;
            TaxRepository = taxRepo;
            TaxesRepository = taxesRepo;
            BenefitRepository = benefitRepo;
            PayrollRepository = payrollRepo;
            TimeClockRepository = timeClockRepo;
            SecurityRepository = securityRepo;
            LegacyTaxRepository = legacyTaxRepo;
            LocationRepository = locationRepository;
            LeaveManagementRepository = leaveManagementRepository;
            LaborManagementRepository = laborManagementRepository;
            ApplicantTrackingRepository = applicantTrackingRepository;
            AcaRepository = acaRepository;
            ClientAccountFeatureRepository = clientAccountFeatureRepository;
            BillingRepository = billingRepository;
            CoreRepository = coreRepository;
            OnboardingRepository = onboardingRepository;
            ContactRepository = contactRepository;
            EEOCRepository = eeocRepository;
            W2Repository = w2Repository;
            JobCostingRepository = jobCostingRepository;
            AccountingRepository = accountingRepository;
            CheckStockOrderRepository = checkStockOrderRepository;
            CheckStockBillingRepository = checkStockBillingRepository;
            ReportRepository = reportRepository;
            NotificationRepository = notificationRepository;
            PerformanceRepository = performanceRepository;
            NpsRepository = npsRepository;
            GeofenceRepository = geofenceRepository;
            ClientNotesRepository = clientNotesRepository;
            ClientFeaturesRepository = clientFeaturesRepository;
            ApplicationConfigurationRepository = appConfigRepo;
            _logger = logger;
            ArRepository = arRepository;
            PushRepository = pushRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Commit on all of the registered Units of Work in the provider.
        /// </summary>
        /// <param name="messages">Messages for holding all messages resulting from commits each unit of work.</param>
        /// <returns>True if all were successful.</returns>
        public bool Commit(IValidationStatusMessageList messages)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    foreach (IDataStoreUnitOfWork unitOfWork in _dataStoreUnitOfWorkProvider.GetUnitsOfWork())
                    {
                        if (AutoDetectChangesEnabled == false)  //Turn back on if disabled.
                        {
                            unitOfWork.AutoDetectChanges(true);
                            AutoDetectChangesEnabled = true;
                        }

                        bool successful = unitOfWork.Commit(messages);
                        if (!successful)
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    if (messages != null)
                    {
                        messages.Add(
                            "Exception was thrown while attempting to commit data.", 
                            ValidationStatusMessageType.General, 
                            null, 
                            StatusMessageLevelType.Error, 
                            ex);

                        _logger.Error("Exception was thrown while attempting to commit data: {ex}", ex);
                    }

                    return false;
                }

                scope.Complete(); // commit transaction (at this time this is only pertinent to Entity Framework)
            }

            return true;
        }

        /// <summary>
        /// This will commit all the UOWs registered on the provider.
        /// </summary>
        /// <returns>Results of commit.</returns>
        public IOpResult Commit()
        {
            var ret = new OpResult();

            using (var scope = new TransactionScope())
            {
                try
                {
                    // loop through all the providers.
                    foreach (var unitOfWork in _dataStoreUnitOfWorkProvider.GetUnitsOfWork())
                    {
                        if (AutoDetectChangesEnabled == false) //Turn back on if disabled.
                        {
                            unitOfWork.AutoDetectChanges(true);
                            AutoDetectChangesEnabled = true;
                        }

                        if (ret.CombineSuccessAndMessages(unitOfWork.Commit()).HasError)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // record the exception
                    ret.SetToFail().AddMessage(new BasicExceptionMsg(ex));
                }

                //if there are no errors complete the transaction
                if(ret.HasNoError)
                    scope.Complete(); // commit transaction (at this time this is only pertinent to Entity Framework)
            }

            //perform any post-commit actions
            if(ret.HasNoError && _postCommitActions.Any())
            {
                //Set post commit actions to a new list and clear them out before performing the actions.
                //Prevents an infinite loop if one of the post commit actions also has a commit in it.
                var currentActions = new List<PostCommitAction>(_postCommitActions);
                _postCommitActions.Clear();

                foreach (var action in currentActions)
                {
                    var actionResult = action.Action();
                    if (action.ShouldMergeResult)
                        actionResult.MergeInto(ret);
                }

            }

            return ret;
        }

        /// <summary>
        /// Not implemented here.
        /// </summary>
        public void AutoDetectChanges(bool value)
        {
            throw new NotImplementedException();
        }

        public List<string> LogSql()
        {
            var log = default(List<string>);
            foreach (IDataStoreUnitOfWork unitOfWork in _dataStoreUnitOfWorkProvider.GetUnitsOfWork())
            {
                try
                {
                    if (log == null)
                    {
                        log = unitOfWork.LogSql();
                    }
                }
                catch (Exception ex)
                {
                    // succckkkkaaaaaa
                }
            }

            return log;
        }


        /// <summary>
        /// Forces a RECOMPILE on the specified EF query.
        /// SHOULD ONLY BE USED WHEN DEBUGGING LOCALLY.  SHOULD NEVER BE LEFT
        /// ON IN PRODUCTION.
        /// See:https://stackoverflow.com/a/40387038
        /// </summary>
        /// <param name="query"></param>
        public void RecompileSqlQuery(Action query)
        {
            foreach (IDataStoreUnitOfWork unitOfWork in _dataStoreUnitOfWorkProvider.GetUnitsOfWork())
            {
                unitOfWork.RecompileSqlQuery(query);
            }
        }

        /// <summary>
        /// Turns off the context entity change tracking until a commit is performed..
        /// </summary>
        public void NoChangeTracking()
        {
            foreach (IDataStoreUnitOfWork unitOfWork in _dataStoreUnitOfWorkProvider.GetUnitsOfWork())
            {
                AutoDetectChangesEnabled = false;
                unitOfWork.AutoDetectChanges(false);
            }

        }

        /// <summary>
        /// Registers an action that will be executed after a successful commit. 
        /// </summary>
        /// <param name="action">Action to perform after a successful commit.</param>
        /// <param name="mergeResultIntoCommitResult">If true, the action's op-result will be merged into the commit's 
        /// result. Be careful, this could cause a commit to appear to have failed even when the data has been successfully 
        /// persisted.</param>
        public void RegisterPostCommitAction(Func<IOpResult> action, bool mergeResultIntoCommitResult = false)
        {
            _postCommitActions.Add(new PostCommitAction { Action = action, ShouldMergeResult = mergeResultIntoCommitResult });
        }

        /// <summary>
        /// Registers an action that will be executed after a successful commit. 
        /// </summary>
        /// <param name="action">Action to perform after a successful commit.</param>
        /// <param name="mergeResultIntoCommitResult">If true, the action's op-result will be merged into the commit's 
        /// result. Be careful, this could cause a commit to appear to have failed even when the data has been successfully 
        /// persisted.</param>
        public void RegisterPostCommitAction(Action action, bool mergeResultIntoCommitResult = false)
        {
            RegisterPostCommitAction(() => new OpResult().TryCatch(action), mergeResultIntoCommitResult);
        }

        /// <summary>
        /// Register an entity to the correlating Unit of Work.
        /// </summary>
        /// An entity type.
        /// <param name="obj">The entity.</param>
        public void Register<T>(T obj) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            unitOfWork.Register(obj);
        }

        /// <summary>
        /// Unregister an entity on the correlating Unit of Work.
        /// </summary>
        /// An entity type.
        /// <param name="obj">The entity.</param>
        public void UnRegister<T>(T obj) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            unitOfWork.UnRegister(obj);
        }

        /// <summary>
        /// Register  an entity on the correlating Unit of Work New (insert).
        /// </summary>
        /// An entity type.
        /// <param name="obj">The entity.</param>
        public void RegisterNew<T>(T obj) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            unitOfWork.RegisterNew(obj);
    
        }

        /// <summary>
        /// Register an entity on the correlating Unit of Work modified (update).
        /// </summary>
        /// An entity type.
        /// <param name="obj">The entity.</param>
        public void RegisterModified<T>(T obj, PropertyList<T> modifiedProperties = null) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            unitOfWork.RegisterModified(obj, modifiedProperties);
        }

        /// <summary>
        /// Register an entity on the correlating Unit of Work deleted (ditto).
        /// </summary>
        /// <typeparam name="T">An entity type.</typeparam>
        /// <param name="obj">The entity.</param>
        public void RegisterDeleted<T>(T obj) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            unitOfWork.RegisterDeleted(obj);
        }

        public void RegisterNewOrExisting<T>(T obj, Func<T, bool> isNewCheck) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            if(isNewCheck(obj))
                    unitOfWork.RegisterNew(obj);
                else
                    unitOfWork.Register(obj);
        }

        public void RegisterNewOrUpdate<T>(T obj, Func<T, bool> isNewCheck) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            if(isNewCheck(obj))
                    unitOfWork.RegisterNew(obj);
                else
                    unitOfWork.Register(obj);
        }

        public void RegisterNewOrModified<T>(T obj, Func<T, bool> isNewCheck) where T : class, IEntity<T>
        {
            var unitOfWork = _dataStoreUnitOfWorkProvider.GetUnitOfWork(obj);
            if(isNewCheck(obj))
                    unitOfWork.RegisterNew(obj);
            else
                unitOfWork.RegisterModified(obj);
        }

        /// <summary>
        /// IDisposable dispose.
        /// </summary>
        public void Dispose()
        {
            Debug.WriteLine("DominionUnitOfWork.Dispose()");

            if (ClientRepository != null)
                ClientRepository = null;

            if (EmployeeRepository != null)
                EmployeeRepository = null;

            if (UserRepository != null)
                UserRepository = null;

            if (MiscRepository != null)
                MiscRepository = null;

            if (BenefitRepository != null)
                BenefitRepository = null;

            _dataStoreUnitOfWorkProvider = null;
        }

        #endregion

        private const string IDENTIFIER = "MasterUow";

        public string UniqueIdentifier
        {
            get { return IDENTIFIER; }
        }
    }
}
