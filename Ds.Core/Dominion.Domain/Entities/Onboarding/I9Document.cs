using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Onboarding
{
    public  class I9Document : Entity<I9Document>
    {
        public virtual int       I9DocumentId   { get; set; }
        public virtual string    Name           { get; set; }
        [Column(TypeName = "char")]
        public virtual string    Category       { get; set; }
       

    }
}
