using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeNoteAttachmentsQuery : IQuery<EmployeeNoteAttachments, IEmployeeNoteAttachmentsQuery>
    {
        IEmployeeNoteAttachmentsQuery ByEmployeeNoteRemarkId(int RemarkId);
        IEmployeeNoteAttachmentsQuery ByEmployeeNoteResourceId(int ResourceId);
    }
}
