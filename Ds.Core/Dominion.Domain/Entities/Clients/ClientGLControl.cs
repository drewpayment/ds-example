using Dominion.Domain.Entities.Base;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientGLControl : Entity<ClientGLControl>
    {
        public virtual int  ClientGLControlId  { get; set; }
        public virtual int  ClientId           { get; set; }
        public virtual bool IncludeAccrual     { get; set; }
        public virtual bool IncludeProject     { get; set; }
        public virtual bool IncludeSequence    { get; set; }
        public virtual bool IncludeClassGroups { get; set; }
        public virtual bool IncludeOffset      { get; set; }
        public virtual bool IncludeDetail      { get; set; }

        public virtual ICollection<ClientGLControlItem> ClientGLControlItems { get; set; }
        public virtual Client Client { get; set; }
    }
}
