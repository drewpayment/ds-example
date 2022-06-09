using System;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Security
{
    /// <summary>
    /// Application-wide security setting defaults.
    /// </summary>
    public enum SecuritySetting
    {
        /// <summary>
        /// The amount of days that a user's password will expire if their SecuritySettings flag is set to true.
        /// </summary>
        ExpireDays = 1, 

        /// <summary>
        /// The amount of previous passwords checked when changing a users password to a new one. 
        /// Note: -1: No History is Checked | 0: All History is Checked | otherwise: # = Number of History Entries Checked
        /// </summary>
        HistoryCount = 2, 

        /// <summary>
        /// How many failed attempts on a login before the user is disabled. 
        /// Note: 0: Lockout is off
        /// </summary>
        LockOut = 3
    }

    public class SecuritySettingInfo : Entity<SecuritySettingInfo>
    {
        public virtual SecuritySetting SecuritySettingId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }
        public virtual string Description { get; set; }

        #region Filters

        /// <summary>
        /// Expression filtering by SecuritySetting type.
        /// </summary>
        /// <param name="setting">Type of security setting to filter by.</param>
        /// <returns></returns>
        public static Expression<Func<SecuritySettingInfo, bool>> ForSetting(SecuritySetting setting)
        {
            return info => info.SecuritySettingId == setting;
        }

        #endregion
    }
}