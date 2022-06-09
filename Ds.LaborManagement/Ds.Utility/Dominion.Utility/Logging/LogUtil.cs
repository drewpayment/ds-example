using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Logging
{
    public class LogUtil
    {
        public static string[] DevMachineNames = new string[]
        {
            "DOM-WS072",
            "DUCATI",
            "DUCATI2",
            "ZMONSTER",
        };

        public static bool IsDevMachine()
        {
            return DevMachineNames.Contains(Environment.MachineName);
        }
    }
}
