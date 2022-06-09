using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Linking object bewteen an <see cref="Aca1095C"/> and a <see cref="Resource"/>.
    /// </summary>
    public partial class Aca1095CResource : Entity<Aca1095CResource>
    {
        public virtual int   EmployeeId { get; set; } 
        public virtual short Year       { get; set; } 
        public virtual int   ResourceId { get; set; } 

        //FOREIGN KEYS
        public virtual Resource Resource { get; set; }
        public virtual Aca1095C Aca1095C { get; set; }
    }
}
