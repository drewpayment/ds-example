using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.Entities.Core
{
    public class RemarkChangeHistory : Entity<RemarkChangeHistory>
    {
        [Key]
        public int      Change_ID           { get; set; }
        public int      RemarkId            { get; set; }
        public String   Description         { get; set; }
        public int      AddedBy             { get; set; }
        public DateTime AddedDate           { get; set; }
        public Boolean  IsSystemGenerated   { get; set; }
        public int      ModifiedBy          { get; set; }
        public DateTime Modified            { get; set; }
        public Boolean  IsArchived          { get; set; }
        public DateTime Change_Date         { get; set; }
        public Byte     Change_Mode         { get; set; }

        public virtual User.User ModifiedByUser { get; set; }
}
}
