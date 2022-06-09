using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Client
{
    public class ClientNotesDto
    {
        public int ClientNoteID { get; set; }
        public int ClientID { get; set; }
        public ClientNoteSubjectType ClientNoteSubjectID { get; set; }
        public int? RemarkID { get; set; }
        public RemarkDto Remark { get; set; }
    }
}