using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.SftpConfiguration;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Core.Dto.Labor
{
    public class JobProfileClassificationsDto
    {
        public int JobProfileId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientGroupId { get; set; }
        public EmployeeStatusType? EmployeeStatusId { get; set; }
        public int? ApplicantPostingCategoryId { get; set; }
        public string JobClass { get; set; }
        public int? EeocLocationId { get; set; }
        public int? EeocJobCategoryId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientShiftId { get; set; }
        public int? DirectSupervisorId { get; set; }

        /// <summary>
        /// Mapped to/from <see cref="SftpTypeIds"/>.
        /// </summary>
        public bool? IsIncludedInYardiExport { 
            get => SftpTypeIds?.Contains(SftpType.YardiExport);
            set 
            {
                if (SftpTypeIds is null)
                {
                    SftpTypeIds = new List<SftpType>();
                }

                if (value.HasValue && value.Value)
                {
                    if (!(IsIncludedInYardiExport ?? true))
                    {
                        SftpTypeIds.Add(SftpType.YardiExport);
                    }
                }
                else
                {
                    SftpTypeIds.RemoveWhere(x => x == SftpType.YardiExport);
                }
            }
        }

        /// <summary>
        /// Excluded from json/xml serialization using <see cref="IgnoreDataMember"/>.
        /// Current use-case for frontend is covered by <see cref="IsIncludedInYardiExport"/>.
        /// </summary>
        /// <remarks>
        /// If later on, it is necessary to tie this additional <see cref="SftpType"/>s,
        /// could simply refactor frontend to use a dropdown-list (instead of the current checkbox)
        /// and instead expose <see cref="SftpTypeIds"/> by removing the <see cref="IgnoreDataMember"/> annotation.
        /// Or alternatively, just add another <c>IsIncludedIn{SomeOtherSftpType}</c> here.
        /// </remarks>
        [IgnoreDataMember]
        public IList<SftpType> SftpTypeIds { get; set; }

        public IList<CoreClientDepartmentDto> Departments { get; set; }

        public IList<JobResponsibilitiesDto> JobResponsibilities { get; set; }
        public IList<JobSkillsDto> JobSkills { get; set; }
    }
}
