using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientNotes : Entity<ClientNotes>
    {
        public virtual int ClientNoteID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual ClientNoteSubjectType ClientNoteSubjectID { get; set; }
        public virtual int? RemarkID { get; set; }
    }
}
