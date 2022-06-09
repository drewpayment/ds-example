using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Security
{
    public interface IUserAccessEmployeeInfo
    {
        int EmployeeId { get; }
        PayType? PayType { get; }
        EmployeeStatusType? EmployeeStatus { get; }
        int? ClientDepartmentId { get; }
        string ClientDepartmentCode { get; }
    }
}