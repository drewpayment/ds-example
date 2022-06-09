using Dominion.Core.Dto.Interfaces;
//using Dominion.Core.Dto.Interfaces.Client;
//using Dominion.Core.Dto.Interfaces.Email;
//using Dominion.Core.Dto.Interfaces.Employee;
//using Dominion.Core.Dto.Interfaces.Employee.Composite;
//using Dominion.Core.Dto.Interfaces.Employee.Dates;
//using Dominion.Core.Dto.Interfaces.User;
//using Dominion.Core.Dto.Interfaces.User.Composite;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.Yardi
{
    public class YardiLearningSupervisorInfoDto
        : IHasFirstMiddleInitialLast
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string UserEmailAddress { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public int? EmployeeId { get; set; }
    }
}
