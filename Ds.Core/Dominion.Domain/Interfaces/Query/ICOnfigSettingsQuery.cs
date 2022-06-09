using Dominion.Core.Dto.Config;
using Dominion.Domain.Entities.Configs;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Query on <see cref="ConfigSetting"/> data.
/// </summary>
namespace Dominion.Domain.Interfaces.Query
{
    public interface IConfigSettingsQuery : IQuery<ConfigSetting, IConfigSettingsQuery>
    {
        IConfigSettingsQuery ByConfigType(ConfigType configId);
    }
}
