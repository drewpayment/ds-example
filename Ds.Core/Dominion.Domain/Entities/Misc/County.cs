using System;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;


namespace Dominion.Domain.Entities.Misc
{
   public class County : Entity<County>
   {
        public virtual int CountyId { get; set; }
        public virtual int StateId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Fips { get; set; }

        public virtual State State {get; set; }

         public static Expression<Func<County, bool>> ForState(int stateId)
        {
            return x => x.StateId == stateId;
        }
    }
}
