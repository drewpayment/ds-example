using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Info entity for the <see cref="DependentCoverageOptionType"/> enum. 
    /// Relates to the [dbo].[bpDependentCoverageOption] table.
    /// </summary>
    public class DependentCoverageOptionTypeInfo : Entity<DependentCoverageOptionTypeInfo>
    {
        public virtual DependentCoverageOptionType DependentCoverageOptionId   { get; set; }
        public virtual string                      Description                 { get; set; }
        public virtual byte                        Sequence                    { get; set; }
        public virtual bool                        IsEmployeeCovered           { get; set; }
        public virtual bool                        IsSpouseCovered             { get; set; }
        public virtual bool                        IsChildCovered              { get; set; }
        public virtual byte?                       MinNoOfDependents           { get; set; }
        public virtual byte?                       MaxNoOfDependents           { get; set; }
    }
}
