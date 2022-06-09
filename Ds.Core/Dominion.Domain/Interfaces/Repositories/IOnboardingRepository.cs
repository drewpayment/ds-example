using Dominion.Authentication.Dto;
using Dominion.Core.Dto.Sprocs;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Forms;
using Dominion.Utility.OpResult;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IOnboardingRepository
    {
        IOnboardingW4AssistQuery OnboardingW4AssistQuery();
        II9DocumentQuery GetI9DocumentQuery();
        IOnboardingI9Query OnboardingI9Query();
        IOnboardingW4Query OnboardingW4Query();
 		IEmergencyContactRelationshipQuery GetEmergencyContactRelationshipQuery();
        IEmployeeDependentRelationshipsQuery GetEmployeeDependentRelationshipsQuery();
        IEmployeeOnboardingEmergencyContactQuery GetEmployeeOnboardingEmergencyContactQuery();

        IEmployeeW2ConsentQuery GetEmployeeW2ConsentQuery();
        IEmployee1095CConsentQuery GetEmployee1095CConsentQuery();

        IOnboardingMenuClientQuery OnboardingMenuClientQuery();

        IOnboardingWorkflowTaskQuery OnboardingWorkflowTaskQuery();
        IOnboardingWorkflowResourcesQuery OnboardingWorkflowResourcesQuery();
        IOnboardingWorkflowQuery OnboardingWorkflowClientQuery();
        IOnboardingAdminTaskListQuery OnboardingAdminTaskListQuery();
        IOnboardingAdminTaskQuery OnboardingAdminTaskQuery();
        IEmployeeOnboardingWorkflowQuery EmployeeOnboardingWorkflowQuery();
        IEmployeeOnboardingQuery EmployeeOnboardingQuery();
        IEmployeeOnboardingTasksQuery EmployeeOnboardingTasksQuery();
        int InsertClockEmployeeBenefitHolidays(ClockEmployeeBenefitHolidayArgsDto args);

        IOnboardingI9DocumentQuery OnboardingI9DocumentQuery();
        int InsertUserSite(string connStr, UserSiteArgsDto args);

        IEeocRaceEthnicCategoriesQuery GetEeocRaceEthnicCategoriesQuery();

        IEmployeeOnboardingFieldQuery EmployeeOnboardingFieldQuery();
        ISchoolDistrictQuery SchoolDistrictQuery();
        IOpResult<UserIdDto> RemoveEmployeeFromESS(int employeeId, int modifiedBy);
    }
}
