using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.EEOC
{
    public enum EEOCJobCategory : byte
    {
        None = 0,
        Execs = 1,
        Managers = 2,
        Professionals = 3,
        Technicians = 4,
        SalesWorkers = 5,
        AdminSupport = 6,
        CraftWorkers = 7,
        Operatives = 8,
        Laborers = 9,
        ServiceWorkers = 10,

        /// <summary>
        /// For Type 6 Report 6 Only
        /// </summary>
        NotApplicable = 99
    }
}
