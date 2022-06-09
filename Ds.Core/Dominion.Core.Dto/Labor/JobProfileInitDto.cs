using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Benefits.Packages;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.EEOC;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Core.Dto.Onboarding;
using Dominion.Core.Dto.Payroll;


namespace Dominion.Core.Dto.Labor
{
    public class JobProfileInitDto
    {
        public JobProfileDto JobProfileDto { get; set; }
        public IEnumerable<JobSkillsDto> Skills { get; set; }
        public IEnumerable<JobResponsibilitiesDto> Responsibilities { get; set; }
        public IEnumerable<ClientDivisionDto> ClientDivisionList { get; set; }
        public IEnumerable<CoreClientCostCenterDto> ClientCostCenterList { get; set; }
        public IEnumerable<ClientGroupDto> ClientGroupList { get; set; }
        public IEnumerable<ClientWorkersCompDto> ClientWorkersCompList { get; set; }
        public IEnumerable<EEOCLocationDto> EEOCLocationList { get; set; }
        public IEnumerable<EEOCJobCategoryDto> EEOCJobCategoryList { get; set; }
        public IEnumerable<EmployeeStatusDto> EmployeeStatusList { get; set; }
        public IEnumerable<ApplicantPostingCategoryDto> PostingCategoryList { get; set; }
        public IEnumerable<PayFrequencyListDto> PayFrequencyList { get; set; }
        public IEnumerable<ClientShiftDto> ClientShiftList { get; set; }
        public IEnumerable<BenefitPackageDto> BenefitPackageList { get; set; }
        public IEnumerable<SalaryDeterminationMethodDto> SalaryDeterminationMethodList { get; set; }
        public ICollection<UserNameDto> SupervisorAndCompAdminList { get; set; }
        public IEnumerable<ClientAccrualDto> ClientAccrualList { get; set; }
        public IEnumerable<OnboardingAdminTaskListDto> OnboardingAdminTaskList { get; set; }

    }
}
