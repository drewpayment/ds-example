using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
//using Dominion.Core.Dto.Interfaces.Client;
//using Dominion.Core.Dto.Interfaces.Email;
//using Dominion.Core.Dto.Interfaces.Employee;
//using Dominion.Core.Dto.Interfaces.Employee.Composite;
//using Dominion.Core.Dto.Interfaces.Employee.Dates;
//using Dominion.Core.Dto.Interfaces.User;
//using Dominion.Core.Dto.Interfaces.User.Composite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dominion.Utility.Msg.Specific;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.Yardi
{
    /// <summary>
    /// Intermediate dto for YardiLearningExport.
    /// </summary>
    public class YardiLearningEmployeeDto
        //: IEmployeeBasicDto
        //, IHasEmail
        //, IHasJobTitle
        //, IHasNullableEmployeeStatusIdEnum
        //, IHasNullableHireDate, IHasNullableSeparationDate, IHasNullableRehireDate
    {
        #region IEmployeeBasicDto
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public int? ClientId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? JobProfileId { get; set; }
        #endregion

        #region Misc
        public string Email { get; set; }
        public EmployeeStatusType? EmployeeStatusIdEnum { get; set; }
        public bool? IsInactiveEmployeeStatusType => EmployeeStatusIdEnum.HasValue 
            ? (EmployeeStatusIdEnum.Value.IsInactiveEmployeeStatusType() as bool?) 
            : null
            ;
        #endregion

        #region EmployeeDates
        public DateTime? HireDate { get; set; }
        public DateTime? SeparationDate { get; set; }
        public DateTime? RehireDate { get; set; }
        #endregion

        #region DirectSupervisor
        public int? DirectSupervisorUserId { get; set; }
        #endregion

        
    }
}
