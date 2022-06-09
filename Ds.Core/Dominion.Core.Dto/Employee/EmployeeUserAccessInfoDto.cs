using Dominion.Core.Dto.Security;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeUserAccessInfoDto : IUserAccessEmployeeInfo
    {
        public int                 EmployeeId           { get; set; }
        public int                 ClientId             { get; set; }
        public PayType?            PayType              { get; set; }
        public EmployeeStatusType? EmployeeStatus       { get; set; }
        public int?                ClientDepartmentId   { get; set; }
        public string              ClientDepartmentCode { get; set; }
    }
}
