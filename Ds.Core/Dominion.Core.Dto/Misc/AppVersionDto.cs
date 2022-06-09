using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Misc
{
    public class AppVersionDto
    {
        public string AppName { get; set; }
        public string Version { get; set; }
        public string PackageName { get; set; }
        public string BuildNumber { get; set; }
        public AppVersionType OsType { get; set; }
    }

    public enum AppVersionType
    {
        ios,
        android
    }
}