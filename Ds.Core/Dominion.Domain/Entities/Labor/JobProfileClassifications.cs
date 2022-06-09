using System.Collections.Generic;
using Dominion.Core.Dto.EEOC;
using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.EEOC;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Labor
{
    public partial class JobProfileClassifications : Entity<JobProfileClassifications>
    {
        public virtual int JobProfileId { get; set; }
        public virtual int? ClientDivisionId { get; set; }
        public virtual int? ClientDepartmentId { get; set; }
        public virtual int? ClientGroupId { get; set; }
        public virtual EmployeeStatusType? EmployeeStatusId { get; set; }
        public virtual int? ApplicantPostingCategoryId { get; set; }
        public virtual string JobClass { get; set; }
        public virtual int? EeocLocationId { get; set; }
        public virtual int? EeocJobCategoryId { get; set; }
        public virtual int? ClientWorkersCompId { get; set; }
        public virtual int? ClientCostCenterId { get; set; }
        public virtual int? ClientShiftId { get; set; }
        public virtual int? DirectSupervisorId { get; set; }
        public virtual JobProfile JobProfile { get; set; }
        public virtual ClientDivision ClientDivision { get; set; }
        public virtual ClientDepartment ClientDepartment { get; set; }
        public virtual ClientGroup ClientGroup { get; set; }
        public virtual EEOCJobCategories EEOCJobCategory { get; set; }
        public virtual EEOCLocation EEOCLocation { get; set; }
        public virtual ClientWorkersComp ClientWorkersComp { get; set; }
        public virtual ClientCostCenter ClientCostCenter { get; set; }
        public virtual ClientShift ClientShift { get; set; }
        public virtual EmployeeStatus EmployeeStatus { get; set; }
        public virtual User.User DirectSupervisor { get; set; }


        public virtual ICollection<SftpTypeEntity> SftpTypeEntities { get; set; }
    }
}
