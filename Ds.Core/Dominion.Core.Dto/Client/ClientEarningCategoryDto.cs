using Dominion.Core.Dto.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientEarningCategoryDto
    {
        public ClientEarningCategory Id            { get; set; }
        //public int    Id            { get; set; }
        public string Description   { get; set; }
        public int?   Sequence      { get; set; }
        public bool   IsAdjustToNet { get; set; }
    }
}
