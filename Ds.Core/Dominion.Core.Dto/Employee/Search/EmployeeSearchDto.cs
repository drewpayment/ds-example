using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Contact.Search;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Performance;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSearchDto : ContactSearchDto
    {
        public int       ClientId             { get; set; }
        public string    EmployeeNumber       { get; set; }
        public string    MiddleInitial        { get; set; }
        public string    EmailAddress         { get; set; }
        public string    HomePhoneNumber      { get; set; }
        public string    CellPhoneNumber      { get; set; }
        public DateTime? HireDate             { get; set; }
        public DateTime? RehireDate           { get; set; }
        public DateTime? SeparationDate       { get; set; }
        public bool      IsActive             { get; set; }
        public bool      IsTemp               { get; set; }
        public string    DirectSupervisor     { get; set; }
        public ClientDivisionDto Division { get; set; }
        public CoreClientDepartmentDto Department { get; set; }
        public string JobTitle { get; set; }
        public User.UserInfoDto Supervisor { get; set; }
        public PayType? PayType { get; set; }
        public int? CompetencyModel { get; set; }
        public EmployeeStatusType? EmployeeStatusType { get; set; }
        public int? JobTitleId { get; set; }
        public int? CostCenter { get; set; }

        /// <summary>
        /// The ReviewTemplateId of every Review that the employee has
        /// </summary>
        public IEnumerable<int> ReviewTemplates { get; set; }

        public IEnumerable<IEmployeeSearchFilterOption> Groups { get; set; }
        public EmployeeAvatarDto EmployeeAvatar { get; set; }
    }
}