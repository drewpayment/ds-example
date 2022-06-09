using System;

namespace Dominion.Core.Dto.Core
{
    public class RemarkDto
    {
        public int      RemarkId          { get; set; }
        public string   Description       { get; set; }
        public int      AddedBy           { get; set; }
        public DateTime AddedDate         { get; set; }
        public int      ModifiedBy        { get; set; }
        public DateTime Modified          { get; set; }
        public bool     IsSystemGenerated { get; set; }
        public bool     IsArchived        { get; set; }

        // RELATIONSHIPS
        public virtual User.UserInfoDto User { get; set; }
    }
}
