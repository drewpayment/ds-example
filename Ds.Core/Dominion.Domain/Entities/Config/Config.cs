using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Configs
{


    /// <summary>
    /// Entity for a person in the contact schema
    /// </summary>
    public class Config : Entity<Config>
    {
        public virtual int    ConfigId { get; set; }
        public virtual string ConfigName { get; set; }

    }
}