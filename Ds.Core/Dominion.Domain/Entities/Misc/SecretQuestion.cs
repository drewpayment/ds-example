using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Misc
{
    /// <summary>
    /// Container class for secret question info.
    /// </summary>
    public class SecretQuestion : Entity<SecretQuestion>
    {
        public virtual int SecretQuestionId { get; set; }
        public virtual string Text { get; set; }
    }
}