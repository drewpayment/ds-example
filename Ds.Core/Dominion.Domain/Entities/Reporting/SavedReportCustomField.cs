using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Reporting
{
    public partial class SavedReportCustomField : Entity<SavedReportCustomField>, IHasModifiedOptionalData
    {
        public virtual int SavedReportCustomFieldId { get; set; }
        public virtual string Description { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string CustomOperator { get; set; }
        public virtual double? CustomValue { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
    }
}
