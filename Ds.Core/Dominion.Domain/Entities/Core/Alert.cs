using Dominion.Domain.Entities.Base;
using System;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Core
{
    public partial class Alert : Entity<Alert>
    {
        public virtual int AlertId { get; set; }
        public virtual DateTime DatePosted { get; set; }
        public virtual string AlertText { get; set; }
        public virtual string AlertLink { get; set; }
        public virtual DateTime DateStartDisplay { get; set; }
        public virtual DateTime DateEndDisplay { get; set; }
        public virtual int AlertType { get; set; }
        public virtual byte SecurityLevel { get; set; }
        public virtual int? ClientId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual byte? AlertCategoryId { get; set; }
        public virtual string Title { get; set; }
        public virtual Client Client { get; set; }
        public virtual int? ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
        public Alert()
        {
        }
    }
}
