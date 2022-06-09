using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeNoteTagsQuery : IQuery<EmployeeNoteTags, IEmployeeNoteTagsQuery>
    {
        IEmployeeNoteTagsQuery ByNoteTagID(int id);
        IEmployeeNoteTagsQuery ByRemarkId(int RemarkId);

    }
}
