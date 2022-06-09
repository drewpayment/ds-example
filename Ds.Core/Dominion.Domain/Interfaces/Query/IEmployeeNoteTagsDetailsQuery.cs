using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeNoteTagsDetailsQuery : IQuery<EmployeeNoteTagsDetails, IEmployeeNoteTagsDetailsQuery>
    {
        IEmployeeNoteTagsDetailsQuery ByTagID(int id);
        IEmployeeNoteTagsDetailsQuery ByClientID(int id);

    }
}
