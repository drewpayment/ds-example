using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientGLControlItem : Entity<ClientGLControlItem>
    {
        public virtual int    ClientGLControlItemId { get; set; }
        public virtual int    ClientGLContolId      { get; set; }
        public virtual int    ClientId              { get; set; }
        public virtual string Description           { get; set; }
        public virtual int    GeneralLedgerTypeId   { get; set; }
        public virtual int?   ForeignKeyId          { get; set; }
        public virtual int    AssignmentMethodId    { get; set; }

        public virtual ClientGLControl ClientGLControl { get; set; }
        public virtual Client Client { get; set; }
    }
}
