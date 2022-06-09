using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Forms
{
    public class FormFields : Entity<FormFields>
    {
        public virtual int FieldId { get; set; }
        public virtual string Description { get; set; }
        public virtual ICollection<FormDefinitionFields> FormDefinitionFields { get; set; }
    }
}
