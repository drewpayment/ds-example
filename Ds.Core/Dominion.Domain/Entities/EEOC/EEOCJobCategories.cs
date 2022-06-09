using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dominion.Domain.Entities.EEOC
{
    public class EEOCJobCategories
    {
        public virtual int JobCategoryId { get; set; }
        public virtual string Description { get; set; }
    }
}
