using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientNote : Entity<ClockClientNote>, IHasClockClientNoteValidation
    {
        public virtual int    ClockClientNoteId  { get; set; }
        public virtual int    ClientId           { get; set; }
        public virtual string Note               { get; set; }
        public virtual bool   IsHideFromEmployee { get; set; }
        public virtual bool   IsActive           { get; set; }
        public virtual Client Client             { get; set; }
    }
}
