using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.ClientFeatures
{
    public class ClientNotes : Entity<ClientNotes>
    {
        public virtual int ClientNoteID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual ClientNoteSubjectType ClientNoteSubjectID { get; set; }
        public virtual int? RemarkID { get; set; }

        // RELATIONSHIPS
        public virtual Remark Remark { get; set; }
    }
}
