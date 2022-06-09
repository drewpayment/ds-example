using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    /// <summary>
    /// Folder group permission entry.
    /// </summary>
    public partial class EmployeeFolderGroup : Entity<EmployeeFolderGroup>
    {
        public virtual int                     FolderId     { get; set; }
        public virtual EmployeeFolderGroupType GroupType    { get; set; }
        public virtual int                     GroupId      { get; set; }

        public virtual EmployeeAttachmentFolder Folder { get; set; }
    }
}
