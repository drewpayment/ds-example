using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class ConvertToPdfDto
    {
        public string FileName { get; set; }
        public string HtmlContent { get; set; }
    }
}