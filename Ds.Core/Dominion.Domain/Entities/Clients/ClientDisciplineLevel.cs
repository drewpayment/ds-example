using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ClientDisciplineLevel: Entity<ClientDisciplineLevel>, IHasModifiedData
    {
        public virtual int DisciplineLevelId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Name { get; set; }
        public virtual double PointLevel { get; set; }
        public virtual bool IsNotify { get; set; }
        public virtual int SortOrder { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
    }
}
