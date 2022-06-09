using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.Configs;

namespace Dominion.Core.Dto.Core
{
    /// <summary>
    /// Enum to show which azure account we are using
    /// </summary>
    public enum AzureAccountName : byte
    {
        AzureFile = 1,
        AzureProfileImage = 2
    }

    public static class AzureAccountNameExtensions
    {
        /// <summary>
        /// Gets the storage account of the instance enum
        /// </summary>
        /// <param name="acctName"></param>
        /// <returns></returns>
        public static String GetStorageAccountString(this AzureAccountName acctName)
        {
            switch (acctName)
            {
                case AzureAccountName.AzureFile:
                    return ConfigValues.AzureFile;
                case AzureAccountName.AzureProfileImage:
                    return ConfigValues.AzureProfileImage;
                default:
                    return ConfigValues.AzureFile;
            }
        }
    }
}
