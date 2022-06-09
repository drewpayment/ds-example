using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeNotesQuery : IQuery<EmployeeNotes, IEmployeeNotesQuery>
    {
        IEmployeeNotesQuery ByEmployeeId(int employeeId);
        IEmployeeNotesQuery ByRemarkId(int RemarkId);
        IEmployeeNotesQuery ByIsArchived(bool value);
        IEmployeeNotesQuery ByAddedBy(int AddedBy);
        IEmployeeNotesQuery BySecurityLevel(int userId, UserType userType, int supervisor);
    }
}
