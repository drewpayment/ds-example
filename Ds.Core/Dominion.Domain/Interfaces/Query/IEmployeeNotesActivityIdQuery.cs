using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeNotesActivityIdQuery : IQuery<EmployeeNotesActivityId, IEmployeeNotesActivityIdQuery>
    { 
        IEmployeeNotesActivityIdQuery ByRemarkId(int RemarkId);
    }
}
