using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.Security;

namespace Dominion.Core.Dto.Onboarding
{   
    [Serializable]
    public class EmployeeOnboardingDto : IUserAccessEmployeeInfo
    {
        public int       EmployeeId                  { get; set; } 
        public int       ClientId                    { get; set; } 
        public DateTime  OnboardingInitiated         { get; set; } 
        public DateTime? EmployeeStarted             { get; set; } 
        public DateTime? EmployeeSignoff             { get; set; }
        public DateTime? ESSActivated                { get; set; }
        public DateTime? OnboardingEnd               { get; set; } 
        public DateTime? InvitationSent              { get; set; }
        public Boolean?  IsI9AdminComplete           { get; set; }
        public Boolean?  IsI9Required                { get; set; }
        public Boolean?  IsWorkflowComplete          { get; set; }
        public int       PctComplete                 { get; set; }
        public int       AdminPctComplete            { get; set; }
        public int       ModifiedBy                  { get; set; } 
        public DateTime  Modified                    { get; set; } 
        public DateTime? HireDate                    { get; set; }
        public DateTime? RehireDate                  { get; set; }
        public string    EmployeeNumber              { get; set; }
        public string    EmployeeName                { get; set; }
        public string    EmployeeFirstName           { get; set; }
        public string    EmployeeLastName            { get; set; }
        public string   EmployeeMiddleName           { get; set; }
        public string   EmployeeInitial              { get; set; }
        public bool     IsInOnboarding               { get; set; }
        public bool     IsOnboardingCompleted        { get; set; }
        public int      UserId                       { get; set; }
        public string   UserName                     { get; set; }
        public string   EmailAddress                 { get; set; }
        public bool?    UserAddedDuringOnboarding    { get; set; }
        public string   ClientName                   { get; set; }
        public string   HomePhone                    { get; set; }
        public PayType? PayType                      { get; set; }
        
        public string   StatusType                   { get; set; }
        public string   JobTitle                     { get; set; }
        public int?     JobProfileId                 { get; set; }
        public string   JobCategory                  { get; set; }
        public string   City                         { get; set; }
        public string   State                        { get; set; }
        public string   ShortEmployee                { get; set; }
        public string   ShortAdmin                   { get; set; }
        public DateTime? SeparationDate              { get; set; }
        public int? ClientDivisionId { get; set; }
        public string DivisionName   { get; set; }
        public string DepartmentName { get; set; }
        public string Supervisor { get; set; }
        public IEnumerable<EmployeeOnboardingTasksDto> TaskList { get; set; }
        public IEnumerable<EmployeeOnboardingWorkflowDto>  EmployeeWorkflow { get; set; }
        public EmployeeProfileImageDto ProfileImage { get; set; }
        public string AzureSas { get; set; }

        public int? ClientDepartmentId { get; set; }
        public string ClientDepartmentCode { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientGroupId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public int? EeocLocationId { get; set; }
        public int? EeocJobCategoryId { get; set; }
        public EmployeeStatusType? EmployeeStatus { get; set; }

        //public double?            SalaryAmount                { get; set; }
        //public DateTime?          SalaryAmountEffectiveDate   { get; set; }
        //public double?            Hours                       { get; set; }
        //public PayFrequencyType   PayFrequency                { get; set; }
        public int?               DirectSupervisorID          { get; set; }
        //public int                ClientRateId                { get; set; }
        //public double             Rate                        { get; set; }

        public ICollection<EmployeePayDto>        EmployeePayInfo     { get; set; }
        public ICollection<EmployeeClientRateDto> EmployeeClientRates { get; set; }
        public EmployeeAvatarDto EmployeeAvatar { get; set; }
    }
}
