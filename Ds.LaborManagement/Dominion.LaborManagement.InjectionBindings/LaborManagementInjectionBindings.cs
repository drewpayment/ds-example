using Dominion.Core.Services.Api.ClockException;
using Dominion.Core.Services.Export.ExportDefinitions.Geofence;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers.Export.Geofence;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.EF.Repository;
using Dominion.LaborManagement.Service.Api;
using Dominion.LaborManagement.Service.Api.Notification;
using Dominion.LaborManagement.Service.Internal.ClockException;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Internal.Security.Filter;
using Dominion.LaborManagement.Service.Internal.Validation;
using Dominion.Utility.Security;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.WebApi.FilterBindingSyntax;
using System.Web.Http.Filters;

namespace Dominion.LaborManagement.InjectionBindings
{
    public class LaborManagementInjectionBindings: NinjectModule
    {
        public override void Load()
        {
            this.Bind<IActionTypeRegulator>()
                .To<LaborManagementActionTypeRegulator>()
                .InRequestScope();

            this.Bind<IActionTypeRegulator>()
                .To<ApplicantTrackingActionTypeRegulator>()
                .InRequestScope();

            this.Bind<ILaborManagementRepository>()
                .To<LaborManagementRepository>()
                .InRequestScope();

            this.Bind<IApplicantTrackingRepository>()
                .To<ApplicantTrackingRepository>()
                .InRequestScope();

            this.Bind<IGroupScheduleProvider>()
                .To<GroupScheduleProvider>()
                .InRequestScope();

            this.Bind<IScheduleGroupProvider>()
                .To<ScheduleGroupProvider>()
                .InRequestScope();

            this.Bind<IGroupScheduleService>()
                .To<GroupScheduleService>()
                .InRequestScope();

            this.Bind<ISchedulingProvider>()
                .To<SchedulingProvider>();

            this.Bind<IEmployeeLaborManagementService>()
                .To<EmployeeLaborManagementService>();
				
			this.Bind<IEmployeePunchProvider>()
                .To<EmployeePunchProvider>();

            this.Bind<IClockService>()
                .To<ClockService>();

            this.Bind<IClockSyncService>()
                .To<ClockSyncService>()
                .InRequestScope();

            this.Bind<ITimeClockRepository>()
                .To<TimeClockRepository>()
                .InRequestScope();

            this.Bind<IJobCostingRepository>()
                .To<JobCostingRepository>()
                .InRequestScope();

            this.Bind<IJobCostingProvider>()
                .To<JobCostingProvider>();

            this.Bind<IActionTypeRegulator>()
                .To<BudgetingActionTypeRegulator>()
                .InRequestScope();

            Bind<IActionTypeRegulator>()
                .To<ClockActionTypeRegulator>()
                .InRequestScope();

            //this.Bind<IApplicantPostingCategoriesService>()
            //    .To<ApplicantPostingCategoriesService>()
            //    .InRequestScope();

            this.Bind<IApplicantTrackingService>()
                .To<ApplicantTrackingService>()
                .InRequestScope();

            this.Bind<IApprovalStatusService>()
                .To<ApprovalStatusService>()
                .InRequestScope();

            this.Bind<ILaborManagementService>()
                .To<LaborManagementService>()
                .InRequestScope();

            this.Bind<ILaborManagementProvider>()
                .To<LaborManagementProvider>()
                .InRequestScope();
            
            this.Bind<ILaborManagementValidationProvider>()
                .To<LaborManagementValidationProvider>()
                .InRequestScope();

            this.Bind<IApplicantAppHistoryService>()
                .To<ApplicantAppHistoryService>()
                .InRequestScope();

            Bind<IApplicationDisclaimerService>()
                .To<ApplicationDisclaimerService>()
                .InRequestScope();

            Bind<IApplicantTrackingHomeService>()
                .To<ApplicantTrackingHomeService>()
                .InRequestScope();
				
            Bind<IApplicantTrackingAuthProvider>()
                .To<ApplicantTrackingAuthProvider>()
                .InRequestScope();
				
            Bind<IApplicantTrackingHomeProvider>()
                .To<ApplicantTrackingHomeProvider>()
                .InRequestScope();
				
			Bind<IApplicantTrackingNotificationService>()
                .To<ApplicantTrackingNotificationService>()
                .InRequestScope();

            Bind<IXMLDataService>()
                .To<IndeedXMLDataService>()
                .InRequestScope();

            Bind<IXMLGeneratorService>()
                .To<IndeedXMLGeneratorService>()
                .InRequestScope();

            Bind<IEmployeeLaborManagementProvider>()
                .To<EmployeeLaborManagementProvider>()
                .InRequestScope();

            Bind<IIndeedXMLService>()
                .To<IndeedXMLService>()
                .InRequestScope();

            Bind<IIndeedApplicationProvider>()
                .To<IndeedApplicationProvider>()
                .InRequestScope();

            Bind<IApplicantTrackingProvider>()
                .To<ApplicantTrackingProvider>()
                .InRequestScope();



             this.BindHttpFilter<IndeedAuthorizationFilter>(FilterScope.Controller);

            Bind<ITimeCardAuthorizationService>()
                .To<TimeCardAuthorizationService>()
                .InRequestScope();

            Bind<ITimeCardAuthorizationProvider>()
                .To<TimeCardAuthorizationProvider>()
                .InRequestScope();

            Bind<IGeoService>()
                .To<GeoService>()
                .InRequestScope();

            Bind<IGeoProvider>()
                .To<GeoProvider>()
                .InRequestScope();

            this.Bind<IClockSyncProvider>()
                .To<ClockSyncProvider>()
                .InRequestScope();

            Bind<ITimeClockHardwareService>()
                .To<TimeClockHardwareService>()
                .InRequestScope();

            Bind<IClockEmployeeExceptionService>()
                .To<ClockEmployeeExceptionService>()
                .InRequestScope();

            Bind<IClockEmployeeExceptionProvider>()
                .To<ClockEmployeeExceptionProvider>()
                .InRequestScope();

            Bind<IGeoBillingService>()
                .To<GeoBillingService>()
                .InRequestScope();

            Bind<IGeoBillingProvider>()
                .To<GeoBillingProvider>()
                .InRequestScope();
        }
    }
}