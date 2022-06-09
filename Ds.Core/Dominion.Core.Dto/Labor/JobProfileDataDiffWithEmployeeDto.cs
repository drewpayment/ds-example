using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileDataDiffWithEmployeeDto
    {
        public int? JobProfileId { get; set; }
        public string JobTitle { get; set; }
        public bool HasJobTitleChanged { get; set; }
        public int ClientId { get; set; }
        public int? ClientDivisionId { get; set; }
        public string ClientDivision { get; set; }
        public bool HasClientDivisionChanged { get; set; }
        public int? ClientDepartmentId { get; set; }
        public string ClientDepartment { get; set; }
        public bool HasClientDepartmentChanged { get; set; }
        public int? ClientGroupId { get; set; }
        public string ClientGroup { get; set; }
        public bool HasClientGroupChanged { get; set; }
        public int? ApplicantPostingCategoryId { get; set; }
        public string ApplicantPostingCategory { get; set; }
        public bool HasApplicantPostingCategoryChanged { get; set; }
        public string JobClass { get; set; }
        public bool HasJobClassChanged { get; set; }
        public int? EeocLocationId { get; set; }
        public string EeocLocation { get; set; }
        public bool HasEeocLocationChanged { get; set; }
        public int? EeocJobCategoryId { get; set; }
        public string EeocJobCategory { get; set; }
        public bool HasEeocJobCategoryChanged { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public string ClientWorkersComp { get; set; }
        public bool HasClientWorkersCompChanged { get; set; }
        public int? ClientCostCenterId { get; set; }
        public string ClientCostCenter { get; set; }
        public bool HasClientCostCenterChanged { get; set; }
        public int? CompetencyModelId { get; set; }
        public string CompetencyModel { get; set; }
        public bool HasCompetencyModelChanged { get; set; }
        public int? DirectSupervisorId { get; set; }
        public string DirectSupervisor { get; set; }
        public bool HasDirectSupervisorChanged { get; set; }

        public DiffEmployeePayDto DiffEmployeePayData { get; set; }
        public IEnumerable<ClientAccrualDto> ClientAccruals { get; set; }
        public bool HasEmployeeAccrualChanged { get; set; }
    }

    public class DiffEmployeePayDto
    {
        public int? ClientShiftId { get; set; }
        public string ClientShift { get; set; }
        public bool HasClientShiftChanged { get; set; }
        public PayFrequencyType? PayFrequencyId { get; set; }
        public string PayFrequency { get; set; }
        public bool HasPayFrequencyChanged { get; set; }
        public EmployeeStatusType? EmployeeStatusId { get; set; }
        public string EmployeeStatus { get; set; }
        public bool HasEmployeeStatusChanged { get; set; }
        public PayType? PayTypeId { get; set; }
        //public string PayType { get; set; }
        public bool HasPayTypeChanged { get; set; }
    }
}
