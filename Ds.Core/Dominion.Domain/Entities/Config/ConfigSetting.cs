using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Configs
{


    /// <summary>
    /// Entity for a person in the contact schema
    /// </summary>
    public class ConfigSetting : Entity<ConfigSetting>
    {
        public virtual int ConfigSettingId { get; set; }
        public virtual int ConfigId { get; set; }
        public virtual string ConfigSettingKey { get; set; }
        public virtual string ConfigSettingValue { get; set; }

        public virtual Config  Config { get; set; }

    }
}